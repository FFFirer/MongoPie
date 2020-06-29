using MongoPie.Models.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace MongoPie.Services
{
    public class LocalRepository
    {
        public static LocalRepository Instance { get; } = new LocalRepository();

        private LocalRepository()
        {
            Connections = new ConcurrentDictionary<Guid, MongoConnection>();
            MongoConnection connection = new MongoConnection
            {
                Id = Guid.NewGuid(),
                ConnectionName = "test",
                Host = "127.0.0.1",
                Port = 27017
            };
            Connections.TryAdd(connection.Id, connection);
        }

        public ConcurrentDictionary<Guid, MongoConnection> Connections { get; set; }
    }
}
