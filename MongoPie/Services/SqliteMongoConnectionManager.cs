using MongoPie.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data;
using Microsoft.Data.Sqlite;
using System.Linq;

namespace MongoPie.Services
{
    public class SqliteMongoConnectionManager : IMongoConnectionManager
    {
        private MongoDbContext _db;
        public SqliteMongoConnectionManager()
        {
            _db = new MongoDbContext();
        }

        public bool ExistConnection(Guid guid)
        {
            var connection = _db.mongoConnections
                .Where(c => c.Id.Equals(guid))
                .FirstOrDefault();
            if(connection == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ExistConnectionName(string connectionName)
        {
            var connection = _db.mongoConnections
                .Where(c => c.ConnectionName.Equals(connectionName))
                .FirstOrDefault();
            if (connection == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public IList<MongoConnection> GetAllConnections()
        {
            return _db.mongoConnections.ToList();
        }

        public MongoConnection Query(string connectionName)
        {
            return _db.mongoConnections
                .Where(c => c.ConnectionName.Equals(connectionName))
                .FirstOrDefault();
        }

        public MongoConnection Query(Guid guid)
        {
            return _db.mongoConnections
                .Where(c => c.Id.Equals(guid))
                .FirstOrDefault();
        }

        public bool Remove(string connectionName)
        {
            var connection = Query(connectionName);
            if(connection == null)
            {
                return true;
            }
            else
            {
                _db.mongoConnections.Remove(connection);
                int rows = _db.SaveChanges();
                return rows > 0;
            }
        }

        public bool Remove(Guid guid)
        {
            var connection = Query(guid);
            if (connection == null)
            {
                return true;
            }
            else
            {
                _db.mongoConnections.Remove(connection);
                int rows = _db.SaveChanges();
                return rows > 0;
            }
        }

        public bool Save(MongoConnection connection)
        {
            if(connection.Id == Guid.Empty || connection.Id == null)
            {
                connection.Id = CreateGuid();
                _db.mongoConnections.Add(connection);
            }
            else
            {
                _db.mongoConnections.Update(connection);
            }

            int rows = _db.SaveChanges();
            return rows > 0;
        }

        private Guid CreateGuid()
        {
            Guid guid = new Guid();
            guid = Guid.NewGuid();
            return guid;
        }
    }
}
