using Microsoft.Extensions.DependencyInjection;
using MongoPie.Extensions;
using MongoPie.Models;
using MongoPie.Models.Entities;
using MongoPie.Services;
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
using System.Windows.Shapes;

namespace MongoPie.UserWindows
{
    /// <summary>
    /// Interaction logic for ConnectionModifyWindow.xaml
    /// </summary>
    public partial class ConnectionModifyWindow : Window
    {
        private ConnectionViewModel _connection { get; set; }
        public ConnectionViewModel ConnectionInfo
        {
            get
            {
                return _connection;
            }
            set
            {
                if (_connection == value) return;
                _connection = value;
                BindProperty();
            }
        }

        public bool IsUpdate { get; set; } = false;
        public bool IsSave { get; set; }
        private IMongoConnectionManager _manager { get; set; }

        public ConnectionModifyWindow()
        {
            InitializeComponent();
            _manager = GlobalIOC.Instance.serviceProvider.GetRequiredService<IMongoConnectionManager>();
            ConnectionInfo = new ConnectionViewModel
            {
                Port = 27017,
                Host = "127.0.0.1"
            };
            LoginInfo.DataContext = ConnectionInfo;
            BindProperty();
            this.txtConnectinName.Focus();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(ValidateConnectionInfo())
            {
                MongoConnection connection = ModelConverter.ToMongoConnection(ConnectionInfo);
                if (_manager.Save(connection))
                {
                    IsSave = true;
                    ConnectionInfo = ModelConverter.ToConnectionViewModel(connection);
                    //MessageBox.Show("保存成功！");
                    this.Close();
                }
                else
                {
                    IsSave = false;
                    MessageBox.Show("保存失败！");
                }
            }
        }

        /// <summary>
        /// 设置初始化连接
        /// </summary>
        /// <param name="connection"></param>
        public void SetConnection(ConnectionViewModel connection)
        {
            this.ConnectionInfo = connection;
        }

        /// <summary>
        /// 设置连接
        /// </summary>
        /// <param name="guid"></param>
        public void SetConnection(Guid guid)
        {
            this.ConnectionInfo = ModelConverter.ToConnectionViewModel(_manager.Query(guid));
        }

        /// <summary>
        /// 取消修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            IsSave = false;
            this.Close();
        }

        /// <summary>
        /// 测试连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTestLogin_Click(object sender, RoutedEventArgs e)
        {
            if(ValidateConnectionInfo())
            {
                MessageBox.Show(App.Current.MainWindow, "验证成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// 数据绑定
        /// </summary>
        private void BindProperty()
        {
            this.txtConnectinName.SetBinding(TextBox.TextProperty, new Binding("ConnectionName") { Source = ConnectionInfo });

            this.txtHost.SetBinding(TextBox.TextProperty, new Binding("Host") { Source = ConnectionInfo });

            this.txtPort.SetBinding(TextBox.TextProperty, new Binding("Port") { Source = ConnectionInfo });

            this.txtDatabaseName.SetBinding(TextBox.TextProperty, new Binding("DatabaseName") { Source = ConnectionInfo });

            this.txtUserName.SetBinding(TextBox.TextProperty, new Binding("UserName") { Source = ConnectionInfo });

            this.txtPassword.SetBinding(PasswordBoxHelper.PasswordProperty, new Binding("Password") { Source = ConnectionInfo, Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        }

        /// <summary>
        /// 验证连接正确性
        /// </summary>
        /// <returns></returns>
        private bool ValidateConnectionInfo()
        {
            StringBuilder builder = new StringBuilder();

            if(string.IsNullOrEmpty(ConnectionInfo.ConnectionName))
            {
                builder.AppendLine("请填写连接名称！");
            }

            else if (_manager.ExistConnectionName(ConnectionInfo.ConnectionName) && !IsUpdate)
            {
                builder.AppendLine("连接名称已存在！");
            }

            if (string.IsNullOrEmpty(ConnectionInfo.ServerAddress))
            {
                builder.AppendLine("请填写服务器地址！");
            }

            if(ConnectionInfo.Port > 0)
            {

            }
            else
            {
                builder.AppendLine("请填写服务器端口！");
            }

            if(string.IsNullOrEmpty(ConnectionInfo.DatabaseName) && string.IsNullOrEmpty(ConnectionInfo.UserName) && string.IsNullOrEmpty(ConnectionInfo.Password))
            {
                
            }
            else if (!string.IsNullOrEmpty(ConnectionInfo.DatabaseName) && !string.IsNullOrEmpty(ConnectionInfo.UserName) && !string.IsNullOrEmpty(ConnectionInfo.Password))
            {

            }
            else
            {
                builder.AppendLine("数据库名称，用户名，密码，全部填写或全部不填写！");
            }

            if(builder.ToString().Length > 0)
            {
                MessageBox.Show(App.Current.MainWindow, builder.ToString(), "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
