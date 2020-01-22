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
                ServerAddress = "115.159.223.241",
                UserName= "AdminUser",
                Pwd = "1qaz@WSX3edc",
                DbName = "admin",
                Dbs = new List<MainViewModel.NodeInfo>()
            };
            this.Loaded += (s, e) =>
            {
                this.DataContext = viewmodel;
                spStatus.Visibility = Visibility.Hidden;
                spLogin.Visibility = Visibility.Visible;
                BindProperty();
            };
        }

        /// <summary>
        /// 登录
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
            MongoClientSettings setting = new MongoClientSettings()
            {
                Credential = credential,
                Server = new MongoServerAddress(viewmodel.ServerAddress, viewmodel.Port)
            };

            Client = new MongoClient(setting);

            this.spLogin.Visibility = Visibility.Hidden;
            this.spStatus.Visibility = Visibility.Visible;

            GetDbInfo();
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            this.spStatus.Visibility = Visibility.Hidden;
            this.spLogin.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 绑定
        /// </summary>
        private void BindProperty()
        {
            this.txtServerAddress.SetBinding(TextBox.TextProperty, new Binding("ServerAddress") { Source = viewmodel });

            this.txtServerPort.SetBinding(TextBox.TextProperty, new Binding("Port") { Source = viewmodel });

            this.txtDbName.SetBinding(TextBox.TextProperty, new Binding("DbName") { Source = viewmodel });

            this.txtUserName.SetBinding(TextBox.TextProperty, new Binding("UserName") { Source = viewmodel });

            this.txtPasswd.SetBinding(PasswordBoxHelper.PasswordProperty, new Binding("Pwd") { Source = viewmodel, Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged});
        }

        private void GetDbInfo()
        {
            if(Client == null)
            {
                throw new NullReferenceException(nameof(Client));
            }

            // 获取所有database
            using(var cursor = Client.ListDatabaseNames())
            {
                List<string> databasenames = cursor.ToList();
                int count = databasenames.Count();
                viewmodel.Dbs.Add(new MainViewModel.NodeInfo()
                {
                    Id = 0,
                    NodeName = "databases",
                    ParentId = -1,
                    Nodes = new List<MainViewModel.NodeInfo>(0)
                });
                for (int i = 1; i < count; i++)
                {
                    viewmodel.Dbs[0].Nodes.Add(new MainViewModel.NodeInfo()
                    {
                        Id = i,
                        NodeName = databasenames[i],
                        ParentId = 0,
                        Nodes = new List<MainViewModel.NodeInfo>()
                    });
                }
            }

            this.treeStatus.ItemsSource = viewmodel.Dbs;
        }
    }
}
