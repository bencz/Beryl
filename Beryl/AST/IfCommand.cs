using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class IfCommand: Command
    {
        Expression _expression;
        Command _if;
        Command _else;

        public IfCommand(Expression expression, Command @if, Command @else)
        {
            _expression = expression;
            _expression.Parent = this;
            _if = @if;
            _if.Parent = this;
            _else = @else;
            _else.Parent = this;
        }
    }
}
