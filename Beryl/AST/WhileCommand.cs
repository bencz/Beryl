using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class WhileCommand: Command
    {
        Expression _expression;
        Command _command;

        public WhileCommand(Expression expression, Command command)
        {
            _expression = expression;
            _expression.Parent = this;
            _command = command;
            _command.Parent = this;
        }
    }
}
