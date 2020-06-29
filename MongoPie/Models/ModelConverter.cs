using MongoPie.Models.Entities;
using MongoPie.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoPie.Models
{
    public class ModelConverter
    {
        public static MongoConnection ToMongoConnection(ConnectionViewModel model)
        {
            return new MongoConnection()
            {
                Id = model.Id,
                ConnectionName = model.ConnectionName,
                Host = model.Host,
                Port = model.Port,
                UserName = model.UserName,
                Password = model.Password,
                DatabaseName = model.DatabaseName
            };
        }

        public static ConnectionViewModel ToConnectionViewModel(MongoConnection connection)
        {
            return new ConnectionViewModel()
            {
                Id = connection.Id,
                ConnectionName = connection.ConnectionName,
                Host = connection.Host,
                Port = connection.Port,
                UserName = connection.UserName,
                Password = connection.Password,
                DatabaseName = connection.DatabaseName
            };
        }
    }
}
