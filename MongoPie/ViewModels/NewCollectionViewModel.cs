using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MongoPie.ViewModels
{
    public class NewCollectionViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        public List<string> Databases { get; set; }

        private string _databaseName { get; set; }
        public string DatabaseName {
            get { return _databaseName; }
            set
            {
                if (_databaseName == value) return;
                _databaseName = value;
                NotifyPropertyChanged();
            }
        }

        private string _collectionName { get; set; }
        public string CollectionName
        {
            get { return _collectionName; }
            set
            {
                if (_collectionName == value) return;
                _collectionName = value;
                NotifyPropertyChanged();
            }
        }
    }
}
