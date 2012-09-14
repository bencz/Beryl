using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl
{
    public class Symbol
    {
        private Position _position;
        public Position Position
        {
            get { return _position; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
        }

        private AST.Type _type;
        public AST.Type Type
        {
            get { return _type; }
        }

        private int _value;
        public int Value
        {
            get { return _value; }
        }

        /* Not used by the MSIL backend. */
        private ulong _address;
        public ulong Address
        {
            get { return _address; }
        }

        public Symbol(Position position, string name, AST.Type type, int value)
        {
            _position = position;
            _name = name;
            _type = type;
            _value = value;
        }

        public Symbol(Position position, string name, AST.Type type, int value, ulong address = 0)
        {
            _position = position;
            _name = name;
            _type = type;
            _value = value;
            _address = address;
        }
    }
}
