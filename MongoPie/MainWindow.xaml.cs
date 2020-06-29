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
        public MainViewModel viewmodel { get; set; }

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
            viewmodel = new MainViewModel() {
                Port = 27017,
                ServerAddress = "127.0.0.1",
                UserName = "",
                Pwd = "",
                DbName = "",
                Dbs = new List<NodeInfo>()
            };
            this.Loaded += (s, e) =>
            {
                this.DataContext = viewmodel;
                spStatus.Visibility = Visibility.Hidden;
                spLogin.Visibility = Visibility.Visible;
                BindProperty();
            };

            GlobalEventHandler.Instance.OpenDatabaseTabEvent += AddDatabaseTab;
        }

        /// <summary>
        /// [废弃]登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(viewmodel.ServerAddress))
            {
                MessageBox.Show("请填写服务器的地址");
                return;
            }

            if ((string.IsNullOrEmpty(viewmodel.UserName) || string.IsNullOrEmpty(viewmodel.Pwd) || string.IsNullOrEmpty(viewmodel.DbName)) && !(string.IsNullOrEmpty(viewmodel.UserName) && string.IsNullOrEmpty(viewmodel.Pwd) && string.IsNullOrEmpty(viewmodel.DbName)))
            {
                MessageBox.Show("用户名，密码，数据库名称，全部填写或全部不填写");
                return;
            }

            MongoCredential credential = MongoCredential.CreateCredential(viewmodel.DbName, viewmodel.UserName, viewmodel.Pwd);
            if (string.IsNullOrEmpty(viewmodel.DbName) && string.IsNullOrEmpty(viewmodel.UserName) && string.IsNullOrEmpty(viewmodel.Pwd))
            {
                credential = null;
            }

            MongoClientService.Instance.AddClient(viewmodel.DbKey, credential, new MongoServerAddress(viewmodel.ServerAddress, viewmodel.Port));

            this.spLogin.Visibility = Visibility.Hidden;
            this.spStatus.Visibility = Visibility.Visible;

            GetDbInfo();
        }

        /// <summary>
        /// [废弃]登出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            this.spStatus.Visibility = Visibility.Hidden;
            this.spLogin.Visibility = Visibility.Visible;
            viewmodel.Dbs = new List<NodeInfo>();
        }

        /// <summary>
        /// [废弃]绑定
        /// </summary>
        private void BindProperty()
        {
            this.txtServerAddress.SetBinding(TextBox.TextProperty, new Binding("ServerAddress") { Source = viewmodel });

            this.txtServerPort.SetBinding(TextBox.TextProperty, new Binding("Port") { Source = viewmodel });

            this.txtDbName.SetBinding(TextBox.TextProperty, new Binding("DbName") { Source = viewmodel });

            this.txtUserName.SetBinding(TextBox.TextProperty, new Binding("UserName") { Source = viewmodel });

            this.txtPasswd.SetBinding(PasswordBoxHelper.PasswordProperty, new Binding("Pwd") { Source = viewmodel, Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        }

        /// <summary>
        /// [废弃]获取数据库所有database和collection
        /// </summary>
        private void GetDbInfo()
        {
            viewmodel.Dbs = MongoClientService.Instance.GetDbInfo(viewmodel.DbKey);

            this.treeStatus.ItemsSource = viewmodel.Dbs;
        }

        /// <summary>
        /// [废弃]添加标签页
        /// </summary>
        /// <param name="Header"></param>
        public void AddItem(NodeInfo node)
        {
            foreach (TabItem item in tcContainer.Items)
            {

                if (((TextBlock)item.Header).Text.ToString() == node.NodeName)
                {
                    item.IsSelected = true;
                    return;
                }
            }

            TabItem item2 = new TabItem()
            {
                Header = new TextBlock() { Text = node.NodeName, ToolTip = "右击选项卡关闭" },
                Content = new UserControls.CollectionQueryControl()
                {
                    viewmodel = new CollectionQueryViewModel()
                    {
                        Context = new DataBaseContext()
                        {
                            DbName = node.DbName,
                            CollectionName = node.NodeName,
                            ClientKey = viewmodel.DbKey
                        }
                    }
                }
            };

            item2.MouseRightButtonDown += tcContainerTabMouseRightDown;
            this.tcContainer.Items.Add(item2);
            item2.IsSelected = true;
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
        /// [废弃]右击关闭标签页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tab_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            TabItem item = sender as TabItem;
            this.mainTab.Items.Remove(item);
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

        /// <summary>
        /// [废弃]TabControl右键关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tcContainerTabMouseRightDown(object sender, EventArgs e)
        {
            TabItem item = sender as TabItem;
            this.tcContainer.Items.Remove(item);
        }

        /// <summary>
        /// TabControl添加Tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tcContainer_AddTab(object sender, MouseButtonEventArgs e)
        {
            var tree = sender as TreeView;
            var item = (NodeInfo)tree.SelectedItem;
            if (item == null) return;
            if (item.NodeType == NodeType.Collection)
            {
                AddItem(item);
            }
        }

        /// <summary>
        /// [废弃]刷新当前数据库信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FreshDB_Click(object sender, RoutedEventArgs e)
        {
            GetDbInfo();
        }
    }
}
