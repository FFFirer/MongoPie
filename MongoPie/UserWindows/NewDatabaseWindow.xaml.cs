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
    /// NewDatabaseWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NewDatabaseWindow : Window
    {
        public NewDatabaseWindow()
        {
            InitializeComponent();
        }

        public NewDatabaseWindow(NodeInfo nodeInfo)
        {
            InitializeComponent();
            this.nodeInfo = nodeInfo;
        }

        public string DatabaseName { get; set; }

        public Action<string> CreateDatabaseAction;

        private NodeInfo nodeInfo { get; set; }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtDatabaseName.Text.Trim()))
            {
                MessageBox.Show("请填写数据库名称");
                return;
            }

            try
            {
                CreateDatabaseAction(txtDatabaseName.Text.Trim());
                MessageBox.Show("创建成功");
                this.Close();
            }
            catch (Exception ex)
            {
                GlobalExceptionHelper.Display(ex, "创建失败", true);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
