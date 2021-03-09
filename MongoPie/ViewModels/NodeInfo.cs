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
        public string NodeName { get; set; }
        public NodeType NodeType { get; set; }
        public string NodeDesc
        {
            get
            {
                switch (NodeType) {
                    case NodeType.Database:
                        return NodeName;
                    case NodeType.Databases:
                        return $"{NodeName}({Nodes.Count})";
                    case NodeType.Collection:
                        return NodeName;
                    case NodeType.Collections:
                        return $"{NodeName}({Nodes.Count})";
                    default:
                        return NodeName;
                }
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
