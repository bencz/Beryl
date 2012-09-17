using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public abstract class Node
    {
        /** The number of nodes created so far. */
        private static int _count;

        /** A global dictionary of all the nodes ever created. */
        private static Dictionary<int, Node> _index = new Dictionary<int, Node>();

        /** The unique little integer number that identifies this node. */
        private int _id;
        public int Id
        {
            get { return _id; }
        }

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
            _id = _count++;
            _position = new Position(position);     // make a DEEP copy so as to avoid unwanted side effects

            _index[_id] = this;
        }

        public abstract void visit(Visitor that);

        public void Dump(Indenter stream)
        {
            // display the nodes in reverse order of creation as we create the tree bottom-up so the last are the topmost nodes
            for (int i = _count - 1; i >= 0; i--)
            {
                Node node = _index[i];
                stream.WriteLine("{0,4:D4} {1} {2}:", node.Id, node.ToString(), node.Position.ToString());
                stream.Indent();
                node.DumpFields(stream);
                stream.Dedent();
                stream.WriteLine();
            }
        }

        public abstract void DumpFields(Indenter stream);
    }
}
