using MongoPie.Services;
using MongoPie.UserWindows;
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

namespace MongoPie.UserControls
{
    /// <summary>
    /// Interaction logic for ConnectionPanelControl.xaml
    /// </summary>
    public partial class ConnectionPanelControl : UserControl
    {
        public ConnectionViewModel viewModel { get; set; }

        

        public ConnectionPanelControl()
        {
            InitializeComponent();
            BindProperty();
        }

        public void SetViewModel(ConnectionViewModel viewModel)
        {
            this.viewModel = viewModel;
            BindProperty();
        }

        public void BindProperty()
        {
            this.txbConnectionName.SetBinding(TextBlock.TextProperty, new Binding("ConnectionName") { Source = viewModel });
            this.txbUserName.SetBinding(TextBlock.TextProperty, new Binding("UserName") { Source = viewModel });
            this.txbServerAddress.SetBinding(TextBlock.TextProperty, new Binding("ServerHost") { Source = viewModel });
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("打开连接");
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("打开设置");
            var connection = LocalConnectionInfoManger.Instance.Query(this.viewModel.ConnectionName);
            if(connection != null)
            {
                ConnectionModifyWindow modifyWindow = new ConnectionModifyWindow();
                modifyWindow.Owner = App.Current.MainWindow;
                modifyWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                modifyWindow.IsUpdate = true;
                modifyWindow.ConnectionInfo = connection;
                modifyWindow.ShowDialog();
                if (modifyWindow.IsSave)
                {
                    LocalConnectionInfoManger.Instance.Update(modifyWindow.ConnectionInfo);
                    SetViewModel(modifyWindow.ConnectionInfo);
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show(App.Current.MainWindow, "将要删除连接？", "注意", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                RemoveConnectionEventHandler?.Invoke(this, null);
            }
        }

        public event EventHandler RemoveConnectionEventHandler;
    }
}
