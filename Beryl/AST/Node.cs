using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public abstract class Node
    {
        private Node _parent;
        public Node Parent
        {
            get { return _parent; }
            set
            {
                if (_parent != null)
                    throw new BerylError("Cannot redefine parent of node");

                _parent = value;
            }
        }

        private Position _position;
        public Position Position
        {
            get { return _position; }
        }

        public Node(Position position)
        {
            _position = position;
        }

        public abstract void visit(Visitor that);
    }
}
