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
using MongoPie.Extensions;
using MongoPie.Models.CustomEventArgs;
using Microsoft.Extensions.DependencyInjection;
using MongoPie.Models;

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
            this.txbServerAddress.SetBinding(TextBlock.TextProperty, new Binding("ServerAddress") { Source = viewModel });
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("打开连接");
            GlobalEventHandler.Instance.OpenDatabaseTab(this, ModelConverter.ToMongoConnection(viewModel));
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            // 传递GUID给设置窗口
            if(viewModel.Id != null)
            {

                ConnectionModifyWindow modifyWindow = new ConnectionModifyWindow();
                modifyWindow.Owner = App.Current.MainWindow;
                modifyWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                modifyWindow.IsUpdate = true;
                if (viewModel.Id == Guid.Empty)
                {
                    MessageBox.Show("此连接未保存");
                    modifyWindow.SetConnection(viewModel);
                }
                else
                {
                    modifyWindow.SetConnection(viewModel.Id);
                }
                modifyWindow.ShowDialog();
                if (modifyWindow.IsSave)
                {
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
