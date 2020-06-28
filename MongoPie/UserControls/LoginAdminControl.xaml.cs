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
using MongoDB.Driver.Core.Events;
using MongoPie.Services;
using MongoPie.UserWindows;
using MongoPie.ViewModels;

namespace MongoPie.UserControls
{
    /// <summary>
    /// Interaction logic for LoginAdminControl.xaml
    /// </summary>
    public partial class LoginAdminControl : UserControl
    {
        public LoginAdminControl()
        {
            InitializeComponent();

        }

        private void btnNewConnection_Click(object sender, RoutedEventArgs e)
        {
            ConnectionModifyWindow window = new ConnectionModifyWindow();
            window.Owner = App.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
            if (window.IsSave)
            {
                SaveConnection(window.ConnectionInfo);
            }
        }

        private void RemoveConnectionHandler(object sender, EventArgs e)
        {
            ConnectionPanelControl connectionPanel = (ConnectionPanelControl)sender;
            if (LocalConnectionInfoManger.Instance.Remove(connectionPanel.viewModel.ConnectionName))
            {
                wpConnectionPanels.Children.Remove(connectionPanel);
            }
            MessageBox.Show("删除成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AddConnectionControl(ConnectionViewModel connection)
        {
            // 展现到界面上
            ConnectionPanelControl connectionPanel = new ConnectionPanelControl();
            connectionPanel.Margin = new Thickness(5, 5, 0, 0);
            connectionPanel.Width = 150.0;
            connectionPanel.Height = 100.0;
            connectionPanel.SetViewModel(connection);
            connectionPanel.RemoveConnectionEventHandler += RemoveConnectionHandler;
            wpConnectionPanels.Children.Add(connectionPanel);
            //wpConnectionPanels.RegisterName(connection.ConnectionName, connectionPanel);

        }

        private void SaveConnection(ConnectionViewModel connection)
        {
            // 保存到数据里
            if (LocalConnectionInfoManger.Instance.Save(connection))
            {
                AddConnectionControl(connection);
            }
        }

        private void InitConnectionPanels()
        {
            var connections = LocalConnectionInfoManger.Instance.GetAllConnections();
            foreach (var connection in connections)
            {
                AddConnectionControl(connection);
            }
        }
    }
}
