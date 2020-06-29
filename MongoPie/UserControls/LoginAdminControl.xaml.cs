using System;
using System.Collections.Generic;
using System.Linq;
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
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver.Core.Events;
using MongoPie.Extensions;
using MongoPie.Models;
using MongoPie.Models.Entities;
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
        private IMongoConnectionManager _manager;

        public LoginAdminControl()
        {
            InitializeComponent();
            _manager = GlobalIOC.Instance.serviceProvider.GetRequiredService<IMongoConnectionManager>();
            InitConnectionPanels();
        }

        /// <summary>
        /// 添加新连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewConnection_Click(object sender, RoutedEventArgs e)
        {
            ConnectionModifyWindow window = new ConnectionModifyWindow();
            window.Owner = App.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
            if (window.IsSave)
            {
                //SaveConnection(window.ConnectionInfo);
                AddConnectionControl(window.ConnectionInfo);
            }
        }

        /// <summary>
        /// 从界面上移除连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveConnectionHandler(object sender, EventArgs e)
        {
            ConnectionPanelControl connectionPanel = (ConnectionPanelControl)sender;
            if (_manager.Remove(connectionPanel.viewModel.Id))
            {
                wpConnectionPanels.Children.Remove(connectionPanel);
            }
            MessageBox.Show("删除成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 添加连接到界面上
        /// </summary>
        /// <param name="connection"></param>
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
        }

        /// <summary>
        /// 保存连接
        /// </summary>
        /// <param name="connection"></param>
        private void SaveConnection(ConnectionViewModel connection)
        {
            
            // 保存到数据里
            if (_manager.Save(ModelConverter.ToMongoConnection(connection)))
            {
                AddConnectionControl(connection);
            }
        }

        /// <summary>
        /// 初始化已有的连接
        /// </summary>
        private void InitConnectionPanels()
        {
            var connections = _manager.GetAllConnections().ToList();
            foreach (var connection in connections)
            {
                AddConnectionControl(ModelConverter.ToConnectionViewModel(connection));
            }
        }
    }
}
