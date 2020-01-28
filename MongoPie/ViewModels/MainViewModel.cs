using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace MongoPie.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
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

        #region 登录信息
        public string DbKey
        {
            get
            {
                return $"{_serveraddress}@{_port}";
            }
        }
        private string _serveraddress { get; set; }
        public string ServerAddress
        {
            get
            {
                return _serveraddress;
            }
            set
            {
                if (_serveraddress == value) return;
                _serveraddress = value;
                NotifyPropertyChanged();
            }
        }

        private int _port { get; set; }
        public int Port
        {
            get
            {
                return _port;
            }
            set
            {
                if (_port == value) return;
                _port = value;
                NotifyPropertyChanged();
            }
        }

        private string _dbname { get; set; }
        public string DbName
        {
            get
            {
                return _dbname;
            }
            set
            {
                if (_dbname == value) return;
                _dbname = value;
                NotifyPropertyChanged();
            }
        }

        private string _username { get; set; }
        public string UserName
        {
            get
            {
                return _username;
            }
            set
            {
                if (_username == value) return;
                _username = value;
                NotifyPropertyChanged();
            }
        }

        private string _pwd { get; set; }
        public string Pwd
        {
            get
            {
                return _pwd;
            }
            set
            {
                if (_pwd == value) return;
                _pwd = value;
                NotifyPropertyChanged();
            }
        }
        //public static DependencyProperty PasswordProperty { get; set; }
        #endregion

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

    /// <summary>
    /// 树形菜单节点
    /// </summary>
    public class NodeInfo
    {
        public int Id { get; set; }
        public string DbName { get; set; }
        private string _nodename { get; set; }
        public string NodeName { get; set; }
        public NodeType NodeType { get; set; }
        public string NodeDesc
        {
            get
            {
                if (NodeType == NodeType.Collection) return NodeName;
                return $"{NodeName}({Nodes.Count})";
            }
        }
        public int ParentId { get; set; }
        public List<NodeInfo> Nodes { get; set; }
        
    }

    public enum NodeType
    {
        Databases = 0,
        Database = 1,
        Collections = 2,
        Collection = 3,
    }
}
