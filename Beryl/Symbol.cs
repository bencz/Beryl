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

        private SymbolKind _kind;
        public SymbolKind Kind
        {
            get { return _kind; }
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
            set { _address = value; }
        }

        // var or func declaration (.Address is set by the code generator)
        public Symbol(Position position, SymbolKind kind, string name, AST.Type type)
        {
            _position = position;
            _kind = kind;
            _name = name;
            _type = type;
            _value = 0;
            _address = 0;
        }

        // const declaration
        public Symbol(Position position, SymbolKind kind, string name, AST.Type type, int value)
        {
            if (kind != SymbolKind.Constant)
                throw new System.Exception("Only constant symbols can have a value");

            _position = position;
            _kind = kind;
            _name = name;
            _type = type;
            _value = value;
        }
    }
}
