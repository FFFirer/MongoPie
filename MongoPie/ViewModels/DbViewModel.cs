using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace MongoPie.ViewModels
{
    public class DbViewModel : INotifyPropertyChanged
    {
        //public MainViewModel(Type ownerType)
        //{
        //    PasswordProperty = DependencyProperty.Register("Password", typeof(string), ownerType, new PropertyMetadata(""));
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region 数据库信息
        private List<NodeInfo> _dbs { get; set; }
        public List<NodeInfo> Dbs
        {
            get
            {
                return _dbs;
            }
            set
            {
                if (_dbs == value) return;
                _dbs = value;
                NotifyPropertyChanged();
            }
        }

        #endregion
    }
}
