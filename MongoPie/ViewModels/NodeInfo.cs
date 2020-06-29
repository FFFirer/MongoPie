using System;
using System.Collections.Generic;
using System.Text;

namespace MongoPie.ViewModels
{
    /// <summary>
    /// 树形菜单节点
    /// </summary>
    public class NodeInfo
    {
        public int Id { get; set; }
        public string DbName { get; set; }
        private string _nodename { get; set; }
        public string NodeName { get; set; }
        public NodeType NodeType { get; set; }
        public string NodeDesc
        {
            get
            {
                if (NodeType == NodeType.Collection) return NodeName;
                return $"{NodeName}({Nodes.Count})";
            }
        }
        public int ParentId { get; set; }
        public List<NodeInfo> Nodes { get; set; }

    }

    public enum NodeType
    {
        Databases = 0,
        Database = 1,
        Collections = 2,
        Collection = 3,
    }
}
