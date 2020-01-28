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
            int Count = viewmodel.Results.Count;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"当前数据共{Count}条");
            foreach (var g in viewmodel.Results)
            {
                builder.AppendLine(g.Result);
            }
            MessageBox.Show(builder.ToString());

            // 创建约束生成器
            FilterDefinitionBuilder<BsonDocument> filter = Builders<BsonDocument>.Filter;

            var results = MongoClientService.Instance.Query(viewmodel.Context, filter.ToBsonDocument());

            viewmodel.Results = new System.Collections.ObjectModel.ObservableCollection<ResultItem>();
            foreach (var r in results)
            {
                viewmodel.Results.Add(new ResultItem()
                {
                    Result = r.ToJson()
                });
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
    }
}
