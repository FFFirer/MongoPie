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
            window.ShowDialog();
            if (window.IsSave)
            {
                AddConnection(window.ConnectionInfo);
            }
        }

        private void AddConnectionControl(ConnectionViewModel connection)
        {
            // 展现到界面上
            ConnectionPanelControl connectionPanel = new ConnectionPanelControl();
            connectionPanel.SetViewModel(connection);
            wpConnectionPanels.RegisterName(connection.ConnectionName, connectionPanel);
        }

        private void AddConnection(ConnectionViewModel connection)
        {
            // 保存到数据里
            if (LocalConnectionInfoManger.Instance.Add(connection))
            {
                AddConnectionControl(connection);
            }
        }

        private void RemoveConnection(ConnectionViewModel connection)
        {
            if (LocalConnectionInfoManger.Instance.Remove(connection.ConnectionName))
            {
                RemoveConnectionControl(connection.ConnectionName)
            }
        }

        private void RemoveConnectionControl(string controlName)
        {
            wpConnectionPanels.UnregisterName(controlName);
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
