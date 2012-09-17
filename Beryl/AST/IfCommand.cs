using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class IfCommand: Command
    {
        private Expression _expression;
        public Expression Expression
        {
            get { return _expression; }
        }

        private Command _if;
        public Command If
        {
            get { return _if; }
        }

        private Command _else;
        public Command Else
        {
            get { return _else; }
        }

        public IfCommand(Position position, Expression expression, Command @if, Command @else):
            base(position)
        {
            _expression = expression;
            _expression.Parent = this;

            _if = @if;
            _if.Parent = this;

            _else = @else;
            _else.Parent = this;
        }

        public override void DumpFields(Indenter stream)
        {
            stream.WriteLine("Expression = {0,4:D4}", _expression.Id);
            stream.WriteLine("If = {0,4:D4}", _if.Id);
            stream.WriteLine("Else = {0,4:D4}", _else.Id);
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}
