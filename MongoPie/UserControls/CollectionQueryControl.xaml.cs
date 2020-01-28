using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MongoPie.ViewModels;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using System.Linq;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace MongoPie.UserControls
{
    /// <summary>
    /// Interaction logic for CollectionQueryControl.xaml
    /// </summary>
    public partial class CollectionQueryControl : UserControl
    {
        //public TestControlViewModel viewmodel { get; set; } = new TestControlViewModel();
        public CollectionQueryViewModel viewmodel { get; set; } = new CollectionQueryViewModel();

        public CollectionQueryControl()
        {
            InitializeComponent();
            this.Loaded += (s, e) =>
            {
                this.DataContext = viewmodel;
            };
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(viewmodel.Query))
            {
                viewmodel.Query = "{}";
            }

            if (string.IsNullOrEmpty(viewmodel.Projection))
            {
                viewmodel.Projection = "{}";
            }
            try
            {
                ShowMessage("开始查询。");

                BsonDocument filter = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(viewmodel.Query);
                BsonDocument projections = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(viewmodel.Projection);

                var countoptions = new CountOptions();
                FindOptions<BsonDocument> options = new FindOptions<BsonDocument>();

                if (viewmodel.Skip > 0)
                {
                    options.Skip = viewmodel.Skip;
                    countoptions.Skip = viewmodel.Skip;
                }

                if (viewmodel.Limit > 0)
                {
                    options.Limit = viewmodel.Limit;
                    countoptions.Limit = viewmodel.Limit;
                }

                if (projections.ElementCount > 0)
                {
                    options.Projection = projections;
                }

                var count = MongoClientService.Instance.GetCount(viewmodel.Context, filter, countoptions);

                // 计算分页信息
                viewmodel.Paging.CurrentPage = 1;
                viewmodel.Paging.TotalCount = count;
                viewmodel.Paging.StartIndex = (count > 0 && viewmodel.Paging.StartIndex <= count) ? (viewmodel.Paging.CurrentPage - 1) * viewmodel.Paging.CountPerPage + 1 : viewmodel.Paging.StartIndex;

                // 查询
                var results = MongoClientService.Instance.Query(viewmodel.Context, filter, options);
                ShowMessage($"查询成功，共{results.Count()}条。");

                // 处理
                viewmodel.ResultsCache = new System.Collections.ObjectModel.ObservableCollection<ResultItem>();

                foreach (var r in results)
                {
                    viewmodel.ResultsCache.Add(new ResultItem()
                    {
                        Result = r.ToJson()
                    });
                }

                // end
                viewmodel.Results = new ObservableCollection<ResultItem>(viewmodel.ResultsCache.Skip(viewmodel.Paging.StartIndex - 1).Take(viewmodel.Paging.CountPerPage));
                viewmodel.Paging.EndIndex = viewmodel.Paging.StartIndex - 1 + viewmodel.Results.Count();
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
                MessageBox.Show(ex.Message);
            }
            
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(viewmodel.Query))
            {
                MessageBox.Show($"{nameof(viewmodel.Query)}不可为空");
                return;
            }

            viewmodel.Results.Add(new ResultItem()
            {
                Result = viewmodel.Query
            });
        }
        
        /// <summary>
        /// 展示操作信息
        /// </summary>
        /// <param name="message"></param>
        private void ShowMessage(string message)
        {
            viewmodel.Message = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff") + " " + message + "\n" + viewmodel.Message;
        }

        /// <summary>
        /// 跳转到前一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMovePrevious_Click(object sender, RoutedEventArgs e)
        {
            if(viewmodel.Paging.CurrentPage > 1)
            {
                viewmodel.Paging.CurrentPage -= 1;
                viewmodel.Paging.StartIndex = viewmodel.Paging.StartIndex - viewmodel.Paging.CountPerPage <= 0 ? 1 : viewmodel.Paging.StartIndex - viewmodel.Paging.CountPerPage;
                viewmodel.Results = new ObservableCollection<ResultItem>(viewmodel.ResultsCache.Skip(viewmodel.Paging.StartIndex - 1).Take(viewmodel.Paging.CountPerPage));
                viewmodel.Paging.EndIndex = viewmodel.Paging.StartIndex - 1 + viewmodel.Results.Count();
            }
            else
            {
                MessageBox.Show("已经是第一页了");
            }
        }

        /// <summary>
        /// 跳转到后一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMoveNext_Click(object sender, RoutedEventArgs e)
        {
            if(viewmodel.Paging.EndIndex < viewmodel.Paging.TotalCount)
            {
                viewmodel.Paging.CurrentPage += 1;
                viewmodel.Paging.StartIndex = viewmodel.Paging.EndIndex + 1 < viewmodel.Paging.TotalCount ? viewmodel.Paging.EndIndex + 1 : viewmodel.Paging.StartIndex;
                viewmodel.Results = new ObservableCollection<ResultItem>(viewmodel.ResultsCache.Skip(viewmodel.Paging.StartIndex - 1).Take(viewmodel.Paging.CountPerPage));
                viewmodel.Paging.EndIndex = viewmodel.Paging.StartIndex - 1 + viewmodel.Results.Count();

            }
            else
            {
                MessageBox.Show("已经是最后一页了");
            }
        }

        /// <summary>
        /// 跳转到最后一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMoveLast_Click(object sender, RoutedEventArgs e)
        {
            if(viewmodel.Paging.EndIndex < viewmodel.Paging.TotalCount)
            {
                viewmodel.Paging.CurrentPage = viewmodel.Paging.TotalCount / viewmodel.Paging.CountPerPage;
                viewmodel.Paging.StartIndex = (viewmodel.Paging.CurrentPage * viewmodel.Paging.CountPerPage + 1) > viewmodel.Paging.TotalCount 
                    ? (viewmodel.Paging.CurrentPage - 1) * viewmodel.Paging.CountPerPage + 1 
                    : viewmodel.Paging.CurrentPage * viewmodel.Paging.CountPerPage + 1;
                viewmodel.Results = new ObservableCollection<ResultItem>(viewmodel.ResultsCache.Skip(viewmodel.Paging.StartIndex - 1).Take(viewmodel.Paging.CountPerPage));
                viewmodel.Paging.EndIndex = viewmodel.Paging.StartIndex - 1 + viewmodel.Results.Count();
            }
            else
            {
                MessageBox.Show("已经是最后一页了");
            }
        }

        /// <summary>
        /// 跳转到第一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMoveFirst_Click(object sender, RoutedEventArgs e)
        {
            if(viewmodel.Paging.CurrentPage > 1)
            {
                viewmodel.Paging.CurrentPage = 1;
                viewmodel.Paging.StartIndex = 1;
                viewmodel.Results = new ObservableCollection<ResultItem>(viewmodel.ResultsCache.Skip(viewmodel.Paging.StartIndex - 1).Take(viewmodel.Paging.CountPerPage));
                viewmodel.Paging.EndIndex = viewmodel.Paging.StartIndex - 1 + viewmodel.Results.Count();
            }
            else
            {
                MessageBox.Show("已经是第一页了");
            }
        }
    }
}
