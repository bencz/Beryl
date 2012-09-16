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

        private Expression[] _arguments;
        public Expression[] Arguments
        {
            get { return _arguments; }
        }

        public CallCommand(Position position, string identifier, Expression[] arguments):
            base(position)
        {
            _identifier = identifier;

            _arguments = arguments;
            foreach (Expression argument in _arguments)
                argument.Parent = this;
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}

