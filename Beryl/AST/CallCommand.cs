using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class CallCommand: Command
    {
        private string _identifier;
        public string Identifier
        {
            get { return _identifier; }
        }

        private Expression _expression;
        public Expression Expression
        {
            get { return _expression; }
        }

        public CallCommand(string identifier, Expression expression)
        {
            _identifier = identifier;
            _expression = expression;
            _expression.Parent = this;
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}
