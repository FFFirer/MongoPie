using MongoPie.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoPie.Services
{
    /// <summary>
    /// 数据库连接管理器，接口
    /// </summary>
    interface IMongoConnectionManager
    {
        /// <summary>
        /// 保存连接，新增/更新，
        /// </summary>
        /// <param name="connection">要保存的连接</param>
        /// <returns>成功返回true，失败返回false</returns>
        bool Save(MongoConnection connection);

        /// <summary>
        /// 判断连接是否存在
        /// </summary>
        /// <param name="connectionName">连接名称</param>
        /// <returns>存在:true,不存在:false</returns>
        bool ExistConnectionName(string connectionName);

        /// <summary>
        /// 判断连接是否存在
        /// </summary>
        /// <param name="guid">连接的GUID</param>
        /// <returns>存在:true,不存在:false</returns>
        bool ExistConnection(Guid guid);

        /// <summary>
        /// 移除连接
        /// </summary>
        /// <param name="connectionName">连接名称</param>
        /// <returns>连接已不存在：true，连接仍存在/移除失败：false</returns>
        bool Remove(string connectionName);

        /// <summary>
        /// 移除连接
        /// </summary>
        /// <param name="guid">GUID</param>
        /// <returns>连接已不存在：true，连接仍存在/移除失败：false</returns>
        bool Remove(Guid guid);

        /// <summary>
        /// 获取所有连接
        /// </summary>
        /// <returns>所有连接</returns>
        IList<MongoConnection> GetAllConnections();

        /// <summary>
        /// 查询连接
        /// </summary>
        /// <param name="connectionName">连接名称</param>
        /// <returns>查询结果</returns>
        MongoConnection Query(string connectionName);

        /// <summary>
        /// 查询连接
        /// </summary>
        /// <param name="guid">GUID</param>
        /// <returns>查询结果</returns>
        MongoConnection Query(Guid guid);
    }
}
