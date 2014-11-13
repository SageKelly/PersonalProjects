using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_XML_Reader
{
    public class Node
    {
        /// <summary>
        /// The name of the node
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Dictates whether or not the Node has children
        /// </summary>
        public bool HasChildren
        {
            get;
            private set;
        }

        /// <summary>
        /// Dictates whether or not the Node has siblings
        /// </summary>
        public bool HasSiblings
        {
            get;
            private set;
        }

        /// <summary>
        /// The Node of which this Node is a child
        /// </summary>
        public Node Parent;

        public Dictionary<string, string> Attributes;

        /// <summary>
        /// The collection of the sibling nodes on the search tree.
        /// </summary>
        public List<Node> Siblings;

        /// <summary>
        /// The collection of the child nodes on the search tree.
        /// </summary>
        public List<Node> Children;

        /// <summary>
        /// Collection of children on the set the depth as this Node
        /// </summary>
        public List<Node> Cousins;

        public Node(string name, bool IsRoot = false)
        {
            Name = name;
            HasChildren = false;
            Attributes = new Dictionary<string, string>();
            HasSiblings = false;
            Children = new List<Node>();
            if (!IsRoot)
            {
                Siblings = new List<Node>();
                Cousins = new List<Node>();
            }
        }

        public void AddChild(Node child)
        {
            Children.Add(child);
            HasChildren = true;
        }

        public void AddSibling(Node sibling)
        {
            Siblings.Add(sibling);
            HasSiblings = true;
        }
    }
}
