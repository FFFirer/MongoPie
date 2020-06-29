using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MongoPie.ViewModels
{
    public class ConnectionViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 唯一ID，由IDbConnectionManager的具体实现负责赋值
        /// </summary>
        public Guid Id { get; set; }

        #region 登录信息
        private string _connectionname { get; set; }
        /// <summary>
        /// 连接名称
        /// </summary>
        public string ConnectionName
        {
            get
            {
                return _connectionname;
            }
            set
            {
                if (_connectionname == value) return;
                _connectionname = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// 服务器地址
        /// </summary>
        public string ServerAddress
        {
            get
            {
                return $"{_host}:{_port}";
            }
        }

        private string _host { get; set; }
        /// <summary>
        /// IP 地址
        /// </summary>
        public string Host
        {
            get
            {
                return _host;
            }
            set
            {
                if (_host == value) return;
                _host = value;
                NotifyPropertyChanged();
            }
        }

        private int _port { get; set; } 
        /// <summary>
        /// 服务器端口
        /// </summary>
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

        private string _databasename { get; set; }
        /// <summary>
        /// 数据库名称（可选）
        /// </summary>
        public string DatabaseName
        {
            get
            {
                return _databasename;
            }
            set
            {
                if (_databasename == value) return;
                _databasename = value;
                NotifyPropertyChanged();
            }
        }

        private string _username { get; set; }
        /// <summary>
        /// 数据库用户名（可选）
        /// </summary>
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

        private string _password { get; set; }
        /// <summary>
        /// 数据库密码（可选）
        /// </summary>
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (_password == value) return;
                _password = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
    }
}
