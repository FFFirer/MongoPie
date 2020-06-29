using MongoPie.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoPie.Services
{
    public class SqliteMongoConnectionManager : IMongoConnectionManager
    {
        public bool ExistConnection(Guid guid)
        {
            throw new NotImplementedException();
        }

        public bool ExistConnectionName(string connectionName)
        {
            throw new NotImplementedException();
        }

        public IList<MongoConnection> GetAllConnections()
        {
            throw new NotImplementedException();
        }

        public MongoConnection Query(string connectionName)
        {
            throw new NotImplementedException();
        }

        public MongoConnection Query(Guid guid)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string connectionName)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Guid guid)
        {
            throw new NotImplementedException();
        }

        public bool Save(MongoConnection connection)
        {
            throw new NotImplementedException();
        }
    }
}
