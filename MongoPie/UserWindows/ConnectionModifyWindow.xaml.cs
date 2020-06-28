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

        public ConnectionModifyWindow()
        {
            InitializeComponent();
            ConnectionInfo = new ConnectionViewModel
            {
                Port = 27017
            };
            LoginInfo.DataContext = ConnectionInfo;
            BindProperty();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(ValidateConnectionInfo())
            {
                IsSave = true;
                this.Close();
            }
        }

        public void SetConnection(ConnectionViewModel connection)
        {
            this.ConnectionInfo = connection;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            IsSave = false;
            this.Close();
        }

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

            this.txtServerAddress.SetBinding(TextBox.TextProperty, new Binding("ServerAddress") { Source = ConnectionInfo });

            this.txtServerPort.SetBinding(TextBox.TextProperty, new Binding("Port") { Source = ConnectionInfo });

            this.txtDbName.SetBinding(TextBox.TextProperty, new Binding("DatabaseName") { Source = ConnectionInfo });

            this.txtUserName.SetBinding(TextBox.TextProperty, new Binding("UserName") { Source = ConnectionInfo });

            this.txtPasswd.SetBinding(PasswordBoxHelper.PasswordProperty, new Binding("Password") { Source = ConnectionInfo, Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
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

            else if (LocalConnectionInfoManger.Instance.ExistConnectionName(ConnectionInfo.ConnectionName) && !IsUpdate)
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
