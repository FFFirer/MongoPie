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
using MongoPie.ViewModels;

namespace MongoPie.UserControls
{
    /// <summary>
    /// Interaction logic for TestControl.xaml
    /// </summary>
    public partial class TestControl : UserControl
    {
        public TestControlViewModel viewmodel { get; set; } = new TestControlViewModel();
        public TestControl()
        {
            InitializeComponent();
            this.Loaded += (s, e) =>
            {
                this.DataContext = viewmodel;
            };
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            int Count = viewmodel.Goods.Count;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"当前数据共{Count}条");
            foreach(var g in viewmodel.Goods)
            {
                builder.AppendLine(g.Description);
            }
            MessageBox.Show(builder.ToString());
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(viewmodel.Key))
            {
                MessageBox.Show($"{nameof(viewmodel.Key)}不可为空");
                return;
            }

            viewmodel.Goods.Add(new GoodItem() { Description = viewmodel.Key });
        }
    }
}
