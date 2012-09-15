using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class ConstDeclaration: Declaration
    {
        private string _identifier;
        public string Identifier
        {
            get { return _identifier; }
        }

        private AST.Type _type;
        public AST.Type Type
        {
            get { return _type; }
        }

        private Expression _expression;
        public Expression Expression
        {
            get { return _expression; }
        }

        public ConstDeclaration(Position position, string identifier, AST.Type type, Expression expression):
            base(position)
        {
            _identifier = identifier;

            _type = type;
            _type.Parent = this;

            _expression = expression;
            _expression.Parent = this;
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}

