using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;


namespace MongoPie.ViewModels
{
    public class TestControlViewModel : INotifyPropertyChanged
    {
        public TestControlViewModel()
        {
            Goods = new ObservableCollection<GoodItem>();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string PropName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropName));
        }
        private ObservableCollection<GoodItem> _goods { get; set; }
        public ObservableCollection<GoodItem> Goods
        {
            get { return _goods; }
            set
            {
                if (_goods == value) return;
                _goods = value;
                NotifyPropertyChanged();
            }
        }

        private string _key { get; set; }
        public string Key
        {
            get { return _key; }
            set
            {
                if (_key == value) return;
                _key = value;
                NotifyPropertyChanged();
            }
        }
    }

    public class GoodItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName]string PropName="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropName));
        }

        private string _description { get; set; }
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (_description == value) return;
                _description = value;
                NotifyPropertyChanged();
            }
        }
    }
}
