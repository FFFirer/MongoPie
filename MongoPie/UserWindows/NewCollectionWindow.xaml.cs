using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MongoPie.ViewModels;

namespace MongoPie.UserWindows
{
    /// <summary>
    /// NewCollectionWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NewCollectionWindow : Window
    {
        public NewCollectionWindow()
        {
            InitializeComponent();
        }

        public NewCollectionWindow(List<string> currentDatabases, string databaseName, Action<string, string> CreateCollection)
        {
            InitializeComponent();
            viewModel = new NewCollectionViewModel()
            {
                Databases = currentDatabases,
                DatabaseName = databaseName,
                CollectionName = string.Empty,
            };
            this.DataContext = viewModel;
            CreateCollectionAction = CreateCollection;
        }

        private NewCollectionViewModel viewModel;

        private Action<string, string> CreateCollectionAction;

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(viewModel.DatabaseName) && !string.IsNullOrEmpty(viewModel.CollectionName))
            {
                try
                {
                    CreateCollectionAction(viewModel.DatabaseName, viewModel.CollectionName);
                    MessageBox.Show("创建成功");
                    this.Close();
                }
                catch (Exception ex)
                {
                    GlobalExceptionHelper.Display(ex, "创建Collection失败");
                }
            }
            else
            {
                MessageBox.Show("请完整填写数据库名称和Collection名称");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}
