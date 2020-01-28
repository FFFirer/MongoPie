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

namespace MongoPie.UserControls.QueryTool
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public List<string> test = new List<string>();
        public UserControl1()
        {
            InitializeComponent();
            this.Loaded += UserControl1_Loaded;
        }

        private void UserControl1_Loaded(object sender, RoutedEventArgs e)
        {
            test = new List<string>
            {
                "test1",
                "test2",
                "test3",
                "test4",
                "test5",
            };
            this.lvResult.DataContext = test;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.test.Add(DateTime.Now.ToLongTimeString());
        }
    }
}
