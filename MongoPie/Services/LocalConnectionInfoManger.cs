using MongoPie.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoPie.Services
{
    public class LocalConnectionInfoManger
    {
        public static LocalConnectionInfoManger Instance { get; } = new LocalConnectionInfoManger();

        private Dictionary<string, ConnectionViewModel> connections { get; set; }
        
        private LocalConnectionInfoManger()
        {
            connections = new Dictionary<string, ConnectionViewModel>();
        }

        /// <summary>
        /// 确认连接是否合法
        /// </summary>
        /// <param name="connection"></param>
        public bool ExistConnectionName(string ConnectionName)
        {
            return connections.ContainsKey(ConnectionName);
        }

        public bool Add(ConnectionViewModel connection)
        {
            if(!connections.ContainsKey(connection.ConnectionName))
            {
                connections.Add(connection.ConnectionName, connection);
                return true;
            }
            else
            {
                GlobalExceptionHelper.Display(null, "已存在相同的连接名称");
                return false;
            }
        }

        public bool Remove(string connectionKey)
        {
            if(connections.ContainsKey(connectionKey))
            {
                return connections.Remove(connectionKey);
            }
            else
            {
                return true;
            }
        }

        internal List<ConnectionViewModel> GetAllConnections()
        {
            return connections.Values.ToList();
        }
    }
}
