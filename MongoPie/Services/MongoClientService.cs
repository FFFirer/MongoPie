using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver.Core;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using MongoPie.ViewModels;

namespace MongoPie
{
    public class MongoClientService
    {
        private static MongoClientService _service { get; } = new MongoClientService();
        public static MongoClientService Instance
        {
            get
            {
                return _service;
            }
        }
        public Dictionary<string, IMongoClient> MongoClients { get; set; } = new Dictionary<string, IMongoClient>();
        public Dictionary<string, IMongoDatabase> MongoDatabases { get; set; } = new Dictionary<string, IMongoDatabase>();
        public Dictionary<string, IMongoCollection<BsonDocument>> MongoCollections { get; set; } = new Dictionary<string, IMongoCollection<BsonDocument>>();

        public void AddClient(string key, string host, int port = 27017)
        {
            if (string.IsNullOrEmpty(host))
            {
                throw new ArgumentNullException(nameof(host));
            }

            MongoServerAddress address = new MongoServerAddress(host, port);
            AddClient(key, null, address);
        }

        public void AddClient(string key, string username, string passwd, string authdb, string host, int port= 27017)
        {
            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(passwd) || string.IsNullOrEmpty(authdb))
            {
                throw new ArgumentNullException($"{nameof(username)},${nameof(passwd)},${nameof(authdb)}必须都填写!");
            }
            MongoCredential credential = MongoCredential.CreateCredential(authdb, username, passwd);
            MongoServerAddress address = new MongoServerAddress(host, port);
            AddClient(key, credential, address);
        }

        public void AddClient(string key, MongoCredential credential, MongoServerAddress address)
        {
            if(address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }
            if (!MongoClients.Keys.Contains(key))
            {
                MongoClientSettings settings = new MongoClientSettings()
                {
                    Server = address
                };

                if (credential != null)
                {
                    settings.Credential = credential;
                }
                var client = new MongoClient(settings);
                MongoClients.Add(key, client);
            }
        }

        public void Remove(string dbkey)
        {
            MongoClients.Remove(dbkey);
        }

        public List<NodeInfo> GetDbInfo(string dbkey)
        {
            int Index = 0;
            var client = MongoClients[dbkey];
            if(client == null)
            {
                throw new NullReferenceException($"{nameof(MongoClients)}中无所指定的client，请先创建");
            }
            List<NodeInfo> nodes = new List<NodeInfo>();
            NodeInfo root = new NodeInfo()
            {
                Id = Index,
                NodeName = "databases",
                NodeType = NodeType.Databases,
                ParentId = -1,
                Nodes = new List<NodeInfo>()
            };
            Index++;
            using (var databasenames = client.ListDatabaseNames())
            {
                foreach (var name in databasenames.ToList())
                {
                    NodeInfo dbnode = new NodeInfo()
                    {
                        Id = Index++,
                        NodeName = name,
                        NodeType = NodeType.Database,
                        ParentId = root.Id,
                        Nodes = new List<NodeInfo>(),
                        DbName = name
                    };
                    NodeInfo collectionsnode = new NodeInfo()
                    {
                        Id = Index++,
                        NodeName = "Collections",
                        NodeType = NodeType.Collections,
                        ParentId = dbnode.Id,
                        Nodes = new List<NodeInfo>(),
                        DbName = name
                    };
                    using(var collections = client.GetDatabase(name).ListCollectionNames())
                    {
                        foreach (var collectioname in collections.ToList())
                        {
                            NodeInfo node = new NodeInfo()
                            {
                                Id = Index++,
                                NodeName = collectioname,
                                NodeType = NodeType.Collection,
                                ParentId = collectionsnode.Id,
                                Nodes = new List<NodeInfo>(),
                                DbName = name
                            };
                            collectionsnode.Nodes.Add(node);
                        }
                    }
                    dbnode.Nodes.Add(collectionsnode);
                    root.Nodes.Add(dbnode);
                }
            }

            nodes.Add(root);
            return nodes;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<BsonDocument> Query(DataBaseContext context, BsonDocument filter, FindOptions<BsonDocument> options = null)
        {
            if (!MongoClients.ContainsKey(context.ClientKey))
            {
                throw new NullReferenceException("请先连接MongoDB!");
            }

            var collection = MongoClients[context.ClientKey].GetDatabase(context.DbName).GetCollection<BsonDocument>(context.CollectionName);

            var cursor = collection.FindAsync<BsonDocument>(filter, options);
            
            var result = cursor.Result.ToList();
            return result;
        }

        public int GetCount(DataBaseContext context, BsonDocument filter, CountOptions options = null)
        {
            if (!MongoClients.ContainsKey(context.ClientKey))
            {
                throw new NullReferenceException("请先连接MongoDB!");
            }

            var collection = MongoClients[context.ClientKey].GetDatabase(context.DbName).GetCollection<BsonDocument>(context.CollectionName);
            var count = collection.CountDocuments(filter, options);
            return (int)count;
        }
    }
}
