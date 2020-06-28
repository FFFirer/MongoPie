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
        }

        public void BindProperty()
        {
            this.txbConnectionName.SetBinding(TextBlock.TextProperty, new Binding("ConnectionName") { Source = viewModel });
            this.txbUserName.SetBinding(TextBlock.TextProperty, new Binding("UserName") { Source = viewModel });
            this.txbServerAddress.SetBinding(TextBlock.TextProperty, new Binding("Password") { Source = viewModel });

        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("打开连接");
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("打开设置");
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("删除连接");
        }
    }
}
