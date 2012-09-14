using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class WhileCommand: Command
    {
        Expression _expression;
        public Expression Expression
        {
            get { return _expression; }
        }

        Command _command;
        public Command Command
        {
            get { return _command; }
        }

        public WhileCommand(Position position, Expression expression, Command command):
			base(position)
        {
            _expression = expression;
            _expression.Parent = this;
            _command = command;
            _command.Parent = this;
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}
