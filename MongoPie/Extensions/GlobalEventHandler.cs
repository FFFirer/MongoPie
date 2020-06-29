using MongoPie.Models.CustomEventArgs;
using MongoPie.Models.Entities;
using MongoPie.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoPie.Extensions
{
    /// <summary>
    /// 全局事件处理
    /// </summary>
    public class GlobalEventHandler
    {
        private GlobalEventHandler()
        {

        }

        /// <summary>
        /// 单例
        /// </summary>
        public static GlobalEventHandler Instance { get; } = new GlobalEventHandler();

        public delegate void OpenDatabaseTabHandler(object sender, MongoConnection connection);
        public event OpenDatabaseTabHandler OpenDatabaseTabEvent;
        /// <summary>
        /// 打开数据库
        /// </summary>
        public void OpenDatabaseTab(object sender, MongoConnection connection)
        {
            if(connection == null)
            {
                throw new Exception($"{nameof(connection)} is null");
            }
            else if(connection.Id == Guid.Empty)
            {
                throw new Exception($"{nameof(connection.Id)} is not saved");
            }
            else
            {
                OpenDatabaseTabEvent?.Invoke(sender, connection);
            }
        }
    }
}
