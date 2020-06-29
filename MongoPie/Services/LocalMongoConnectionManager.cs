using MongoPie.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoPie.Services
{
    public class LocalMongoConnectionManager : IMongoConnectionManager
    {
        public bool ExistConnectionName(string connectionName)
        {
            return LocalRepository.Instance.Connections.Values.Where(c => c.ConnectionName.Equals(connectionName)).Count() > 0;
        }

        public bool ExistConnection(Guid guid)
        {
            return LocalRepository.Instance.Connections.ContainsKey(guid);
        }

        public IList<MongoConnection> GetAllConnections()
        {
            return LocalRepository.Instance.Connections.Values.ToList();
        }

        public MongoConnection Query(string connectionName)
        {
            MongoConnection connection = LocalRepository.Instance.Connections.Values
                .Where(c => c.ConnectionName.Equals(connectionName)).FirstOrDefault();
            return connection;
        }

        public MongoConnection Query(Guid guid)
        {
            MongoConnection connection = LocalRepository.Instance.Connections[guid];
            return connection;
        }

        public bool Remove(string connectionName)
        {
            MongoConnection connection = LocalRepository.Instance.Connections.Values
                .Where(c => c.ConnectionName.Equals(connectionName)).FirstOrDefault();
            if(connection == null)
            {
                return true;
            }

            return LocalRepository.Instance.Connections.Remove(connection.Id, out connection);
        }

        public bool Remove(Guid guid)
        {
            MongoConnection connection = LocalRepository.Instance.Connections[guid];
            if(connection == null)
            {
                return true;
            }

            return LocalRepository.Instance.Connections.Remove(guid, out connection);
        }

        public bool Save(MongoConnection connection)
        {
            if(connection.Id == null || connection.Id.Equals(Guid.Empty))
            {
                connection.Id = CrateGuid();
                return LocalRepository.Instance.Connections.TryAdd(connection.Id, connection);
            }
            else
            {
                MongoConnection oldConnection = null;
                LocalRepository.Instance.Connections.TryGetValue(connection.Id, out oldConnection);
                return LocalRepository.Instance.Connections.TryUpdate(connection.Id, connection, oldConnection);
            }
        }

        private Guid CrateGuid()
        {
            Guid guid = new Guid();
            guid = Guid.NewGuid();
            return guid;
        }
    }
}
