using System;
using System.Collections.Generic;
using System.Text;

namespace MongoPie.Models.Entities
{
    public class MongoConnection
    {
        public Guid Id { get; set; }
        public string ConnectionName { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
