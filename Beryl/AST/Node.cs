using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class Node
    {
        private Node _parent;
        public Node Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public Node()
        {
        }
    }
}
