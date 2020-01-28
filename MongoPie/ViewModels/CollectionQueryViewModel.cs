using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoPie.ViewModels
{
    public class CollectionQueryViewModel : INotifyPropertyChanged
    {
        public CollectionQueryViewModel()
        {
            Results = new ObservableCollection<ResultItem>();
            Query = "{}";
            Limit = 1000;
            Skip = 0;
            Projection = "{}";
            Paging = new PagingInfo();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        public DataBaseContext Context { get; set; }

        // query
        private string _query { get; set; }
        public string Query
        {
            get { return _query; }
            set
            {
                if (_query == value) return;
                _query = value;
                NotifyPropertyChanged();
            }
        }

        //skip
        private int _skip { get; set; }
        public int Skip
        {
            get { return _skip; }
            set
            {
                if (_skip == value) return;
                _skip = value;
                NotifyPropertyChanged();
            }
        }

        // limit
        private int _limit { get; set; }
        public int Limit
        {
            get { return _limit; }
            set
            {
                if (_limit == value) return;
                _limit = value;
                NotifyPropertyChanged();
            }
        }

        private string _projection { get; set; }
        public string Projection
        {
            get { return _projection; }
            set
            {
                if (_projection == value) return;
                _projection = value;
                NotifyPropertyChanged();
            }
        }

        // message
        private string _message { get; set; }
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if (_message == value) return;
                _message = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<ResultItem> _results { get; set; }
        public ObservableCollection<ResultItem> Results
        {
            get { return _results; }
            set
            {
                if (_results == value) return;
                _results = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<ResultItem> ResultsCache { get; set; }

        private PagingInfo _paging { get; set; }
        public PagingInfo Paging
        {
            get { return _paging; }
            set
            {
                if (_paging == value) return;
                _paging = value;
                NotifyPropertyChanged();
            }
        }
    }

    public class DataBaseContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        private string _dbname { get; set; }
        public string DbName
        {
            get { return _dbname; }
            set
            {
                if (_dbname == value) return;
                _dbname = value;
                NotifyPropertyChanged();
            }
        }
        private string _collectioname { get; set; }
        public string CollectionName
        {
            get { return _collectioname; }
            set
            {
                if (_collectioname == value) return;
                _collectioname = value;
                NotifyPropertyChanged();
            }
        }
        private string _clientKey { get; set; }
        public string ClientKey
        {
            get { return _clientKey; }
            set
            {
                if (_clientKey == value) return;
                _clientKey = value;
                NotifyPropertyChanged();
            }
        }
    }

    public class ResultItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        private string _result { get; set; }
        public string Result
        {
            get { return _result; }
            set
            {
                if (_result == value) return;
                _result = value;
                NotifyPropertyChanged();
            }
        }

    }

    public class PagingInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        public PagingInfo()
        {
            CurrentPage = 1;
            TotalCount = 0;
            CountPerPage = 50;
            StartIndex = 0;
            EndIndex = 0;
        }

        private int _currentpage { get; set; }
        public int CurrentPage
        {
            get
            {
                return _currentpage;
            }
            set
            {
                if (_currentpage == value) return;
                _currentpage = value;
                NotifyPropertyChanged();
            }
        }

        private int _totalcount { get; set; }
        public int TotalCount
        {
            get
            {
                return _totalcount;
            }
            set
            {
                if (_totalcount == value) return;
                _totalcount = value;
                NotifyPropertyChanged();
            }
        }

        private int _countperpage { get; set; }
        public int CountPerPage
        {
            get
            {
                return _countperpage;
            }
            set
            {
                if (_countperpage == value) return;
                _countperpage = value;
                NotifyPropertyChanged();
            }
        }

        private int _startindex { get; set; }
        public int StartIndex
        {
            get { return _startindex; }
            set
            {
                if (_startindex == value) return;
                _startindex = value;
                NotifyPropertyChanged();
            }
        }

        private int _endindex { get; set; }
        public int EndIndex
        {
            get { return _endindex; }
            set
            {
                if (_endindex == value) return;
                _endindex = value;
                NotifyPropertyChanged();
            }
        }

    }
}
