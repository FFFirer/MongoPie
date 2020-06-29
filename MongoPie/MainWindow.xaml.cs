using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using MongoPie;
using MongoPie.UserControls;
using MongoPie.Extensions;
using MongoPie.Models.Entities;
using System.Net.NetworkInformation;

namespace MongoPie
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region MongoClient, database, collection
        private IMongoClient _client { get; set; }
        public IMongoClient Client
        {
            get
            {
                return _client;
            }
            set
            {
                if (_client == value) return;
                _client = value;
            }
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            GlobalEventHandler.Instance.OpenDatabaseTabEvent += AddDatabaseTab;
        }

        /// <summary>
        /// 添加数据库标签页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="tabName">Connection的GUID的无短划线字符串</param>
        public void AddDatabaseTab(object sender, MongoConnection connection)
        {
            // 查看数据库对应的Tab是否已经打开
            foreach (TabItem item in mainTab.Items)
            {
                if (item.Name == connection.Id.ToString("N") || item.Name == connection.ConnectionName)
                {
                    item.IsSelected = true;
                    return;
                }
            }

            string tabName = connection.Id == Guid.Empty || connection.Id == null ? $"tab{connection.ConnectionName}" : $"tab{connection.Id.ToString("N")}";

            // 添加
            TabItem dbTab = new TabItem()
            {
                Name = tabName,
                Header = CreateTabGrid(tabName, connection.ConnectionName),
                Content = new DatabaseControl()
                {
                    CurrentConnection = connection
                }
            };

            //dbTab.MouseRightButtonUp += Tab_MouseRightButtonUp;
            this.mainTab.Items.Add(dbTab);
            dbTab.IsSelected = true;
        }

        /// <summary>
        /// 创建tabheader的组件
        /// </summary>
        /// <param name="tabName"></param>
        /// <returns></returns>
        private Grid CreateTabGrid(string tabName, string tabHeader)
        {
            Grid tabGrid = new Grid();
            tabGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            tabGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(20.0) });
            TextBlock tb = new TextBlock() { Text = tabHeader };
            Grid.SetColumn(tb, 0);
            Button btnClose = new Button()
            {
                Name = $"btn{tabName}",
                Padding = new Thickness(3,0,3,0),
                Content = "X",
                Background = null,
                BorderThickness = new Thickness(0),
                HorizontalAlignment = HorizontalAlignment.Right
            };
            btnClose.Click += BtnClose_Click;
            Grid.SetColumn(btnClose, 1);
            tabGrid.Children.Add(tb);
            tabGrid.Children.Add(btnClose);

            return tabGrid;
        }

        /// <summary>
        /// 点击关闭按钮移除对应Tab页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Button ClickedBtn = sender as Button;
            string tabName = ClickedBtn.Name.Substring(3, ClickedBtn.Name.Length - 3);

            var tab = ((System.Windows.FrameworkElement)VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(ClickedBtn))).TemplatedParent;
            if(tab.GetType() == typeof(TabItem))
            {
                this.mainTab.Items.Remove((TabItem)tab);
                mainTab.SelectedIndex = 0;
            }

            //MessageBox.Show("未找到TabItem");

            //foreach (var item in mainTab.Items)
            //{
            //    TabItem tab = item as TabItem;
            //    if (tab.Name == tabName)
            //    {
            //        this.mainTab.Items.Remove(tab);
            //        mainTab.SelectedIndex = 0;
            //        return;
            //    }
            //}
        }
    }
}
