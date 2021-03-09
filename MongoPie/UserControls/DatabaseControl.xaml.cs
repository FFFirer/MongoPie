using MongoDB.Driver;
using MongoPie.Models.Entities;
using MongoPie.ViewModels;
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
using MongoPie.UserWindows;
using System.Linq;

namespace MongoPie.UserControls
{
    /// <summary>
    /// Interaction logic for DatabaseControl.xaml
    /// </summary>
    public partial class DatabaseControl : UserControl
    {
        private DbViewModel viewModel { get; set; }
        private MongoConnection _connection { get; set; }
        public MongoConnection CurrentConnection
        {
            get
            {
                return _connection;
            }
            set
            {
                if (_connection == value) return;
                _connection = value;
            }
        }
        public DatabaseControl()
        {
            InitializeComponent();
            viewModel = new DbViewModel()
            {
                Dbs = new List<NodeInfo>()
            };
            this.Loaded += (s, e) =>
            {
                this.DataContext = viewModel;
                InitMongoDb();
            };
        }

        /// <summary>
        /// 刷新数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FreshDB_Click(object sender, RoutedEventArgs e)
        {
            GetDbInfo();
        }

        /// <summary>
        /// 新建查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void queryContainer_AddTab(object sender, MouseButtonEventArgs e)
        {
            var tree = sender as TreeView;
            var item = (NodeInfo)tree.SelectedItem;
            if (item == null) return;
            if(item.NodeType == NodeType.Collection)
            {
                AddTabItem(item);
            }
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        private void InitMongoDb ()
        {
            if(CurrentConnection != null)
            {
                MongoCredential credential = null;

                if (!string.IsNullOrEmpty(CurrentConnection.DatabaseName) && !string.IsNullOrEmpty(CurrentConnection.UserName) && !string.IsNullOrEmpty(CurrentConnection.Password))
                {
                    credential = MongoCredential.CreateCredential(CurrentConnection.DatabaseName, CurrentConnection.UserName, CurrentConnection.Password);
                }

                MongoClientService.Instance.AddClient(CurrentConnection.DatabaseKey, credential, new MongoServerAddress(CurrentConnection.Host, CurrentConnection.Port));
                GetDbInfo();
            }
        }

        /// <summary>
        /// 获取数据库信息
        /// </summary>
        private void GetDbInfo()
        {
            viewModel.Dbs = MongoClientService.Instance.GetDbInfo(CurrentConnection.DatabaseKey);
            this.treeStatus.ItemsSource = viewModel.Dbs;
        }

        /// <summary>
        /// 添加查询标签页
        /// </summary>
        /// <param name="node"></param>
        private void AddTabItem(NodeInfo node)
        {
            string CollectionKey = $"tab_{node.DbName}_{node.NodeName}".Replace('.', '_');
            foreach (TabItem item in queryContainer.Items)
            {
                if(item.Name == CollectionKey)
                {
                    item.IsSelected = true;
                    return;
                }
            }

            TabItem tabItem = new TabItem()
            {
                Name = CollectionKey,
                Header = CreateTabHeader(CollectionKey, node.NodeName),
                Content = new UserControls.CollectionQueryControl()
                {
                    viewmodel = new CollectionQueryViewModel()
                    {
                        Context = new DataBaseContext()
                        {
                            DbName = node.DbName,
                            CollectionName = node.NodeName,
                            ClientKey = CurrentConnection.DatabaseKey
                        }
                    }
                }
            };

            this.queryContainer.Items.Add(tabItem);
            tabItem.IsSelected = true;
        }

        /// <summary>
        /// 创建标签头
        /// </summary>
        /// <param name="tabName"></param>
        /// <param name="tabHeader"></param>
        /// <returns></returns>
        private Grid CreateTabHeader(string tabName, string tabHeader)
        {
            Grid tabGrid = new Grid();
            tabGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            tabGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(20.0) });
            TextBlock tb = new TextBlock() { Text = tabHeader };
            Grid.SetColumn(tb, 0);
            Button btnClose = new Button()
            {
                Name = $"btn{tabName}",
                Padding = new Thickness(3, 0, 3, 0),
                Content = "X",
                Background = null,
                BorderThickness = new Thickness(0),
                HorizontalAlignment = HorizontalAlignment.Right
            };
            btnClose.Click += BtnQueryClose_Click;
            Grid.SetColumn(btnClose, 1);
            tabGrid.Children.Add(tb);
            tabGrid.Children.Add(btnClose);

            return tabGrid;
        }

        /// <summary>
        /// 关闭查询标签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnQueryClose_Click(object sender, RoutedEventArgs e)
        {
            Button ClickedBtn = sender as Button;
            string tabName = ClickedBtn.Name.Substring(3, ClickedBtn.Name.Length - 3);

            var tab = ((System.Windows.FrameworkElement)VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(ClickedBtn))).TemplatedParent;
            if(tab.GetType() == typeof(TabItem))
            {
                this.queryContainer.Items.Remove((TabItem)tab);
            }
        }

        #region database和collection的增删
        /// <summary>
        /// 新建数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddNewDatabase_Click(NodeInfo node)
        {
            if (node.NodeType == NodeType.Database || node.NodeType == NodeType.Databases)
            {
                NewDatabaseWindow newDatabase = new NewDatabaseWindow(node);
                newDatabase.Owner = App.Current.MainWindow;
                newDatabase.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                newDatabase.CreateDatabaseAction = (name) => MongoClientService.Instance.CreateDatabase(CurrentConnection.DatabaseKey, name);
                newDatabase.ShowDialog();
            }
            else
            {
                MessageBox.Show("节点错误");
            }
        }

        /// <summary>
        /// 新建Collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddNewCollection_Click(NodeInfo node)
        {
            if (node.NodeType == NodeType.Collection || node.NodeType == NodeType.Collections)
            {
                var currentDatabaseNames = viewModel.Dbs[0]
                    .Nodes
                    .Select(a => a.DbName)
                    .ToList();
                NewCollectionWindow newCollection = new NewCollectionWindow(currentDatabaseNames, node.DbName, async (db, collection) =>
                {
                    await MongoClientService.Instance.CreateCollection(CurrentConnection.DatabaseKey, db, collection);
                });
                newCollection.Owner = App.Current.MainWindow;
                newCollection.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                newCollection.ShowDialog();
            }
            else
            {
                MessageBox.Show("节点错误");
            }
        }
        
        /// <summary>
        /// 删除Database
        /// </summary>
        /// <param name="node"></param>
        public async void DropDatabase(NodeInfo node)
        {
            if (node.NodeType == NodeType.Database)
            {
                if(MessageBox.Show(App.Current.MainWindow,$"确定要删除Database: {node.DbName}?", "注意", MessageBoxButton.OKCancel, MessageBoxImage.Warning ) == MessageBoxResult.OK)
                {
                    await MongoClientService.Instance.DropDatabase(CurrentConnection.DatabaseKey, node.DbName);
                    MessageBox.Show("删除成功");
                }
            }
            else
            {
                MessageBox.Show("节点错误");
            }
        }

        /// <summary>
        /// 删除Collection
        /// </summary>
        /// <param name="node"></param>
        public async void DropCollection(NodeInfo node)
        {
            if (node.NodeType == NodeType.Collection)
            {
                if (MessageBox.Show(App.Current.MainWindow, $"确定要删除Collection: {node.NodeName}?", "注意", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    await MongoClientService.Instance.DropCollection(CurrentConnection.DatabaseKey, node.DbName, node.NodeName);
                    MessageBox.Show("删除成功");
                }
            }
            else
            {
                MessageBox.Show("节点错误");
            }
        }
        #endregion

        /// <summary>
        /// 树形菜单右键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeStatus_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            DependencyObject source = e.OriginalSource as DependencyObject;
            while (source != null && source.GetType() != typeof(TreeViewItem))
            {
                source = VisualTreeHelper.GetParent(source);
            }

            if(source != null)
            {
                TreeViewItem item = source as TreeViewItem;
                
                CreateContextMenu(item);
            }
        }


        /// <summary>
        /// 创建右键菜单
        /// </summary>
        /// <param name="item"></param>
        private void CreateContextMenu(TreeViewItem item)
        {

            NodeInfo info = (NodeInfo)item.DataContext;

            ContextMenu nodeMenu = new ContextMenu();
            MenuItem freshItem = new MenuItem();
            freshItem.Header = "刷新";
            freshItem.Click += FreshDB_Click;
            nodeMenu.Items.Add(freshItem);

            if (info.NodeType == NodeType.Database || info.NodeType == NodeType.Databases)
            {

                //MenuItem addNewDbItem = new MenuItem();
                //addNewDbItem.Header = "新建Database";
                //addNewDbItem.Click += (sender, e)=> {
                //    AddNewDatabase_Click(info);
                //};
                //nodeMenu.Items.Add(addNewDbItem);

                if(info.NodeType == NodeType.Database)
                {
                    MenuItem dropDatabaseItem = new MenuItem();
                    dropDatabaseItem.Header = "删除Database";
                    dropDatabaseItem.Click += (sender, e) =>
                    {
                        DropDatabase(info);
                    };
                    nodeMenu.Items.Add(dropDatabaseItem);
                }
            }
            if (info.NodeType == NodeType.Collection || info.NodeType == NodeType.Collections)
            {

                MenuItem addNewCollectionItem = new MenuItem();
                addNewCollectionItem.Header = "新建Collection";
                addNewCollectionItem.Click += (sender, e) =>
                {
                    AddNewCollection_Click(info);
                };
                nodeMenu.Items.Add(addNewCollectionItem);

                if(info.NodeType == NodeType.Collection)
                {
                    var dropCollectionItem = new MenuItem();
                    dropCollectionItem.Header = "删除Collection";
                    dropCollectionItem.Click += (sender, e) =>
                    {
                        DropCollection(info);
                    };
                    nodeMenu.Items.Add(dropCollectionItem);
                }
            }

            this.treeStatus.ContextMenu = nodeMenu;
        }
    }
}
