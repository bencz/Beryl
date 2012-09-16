using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beryl.AST;

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

        private Declaration _declaration;
        public Declaration Declaration
        {
            get { return _declaration; }
        }

        /* Not used by the MSIL backend. */
        private ulong _address;
        public ulong Address
        {
            get { return _address; }
            set { _address = value; }
        }

        // var or func declaration (.Address is set by the code generator)
        public Symbol(Position position, string name, Declaration declaration)
        {
            _position = position;
            _name = name;
            _declaration = declaration;
            _address = 0;
        }
    }
}
