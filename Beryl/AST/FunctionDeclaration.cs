using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class FunctionDeclaration: Declaration
    {
        private string _name;
        public string Name
        {
            get { return _name; }
        }

        public FunctionDeclaration(Position position, string name, Type type) :
            base(position, SymbolKind.Function, type)
        {
            _name = name;
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}

