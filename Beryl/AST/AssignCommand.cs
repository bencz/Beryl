using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class AssignCommand: Command
    {
        private string _name;
        public string Name
        {
            get { return _name; }
        }

        private Expression _expression;
        public Expression Expression
        {
            get { return _expression; }
        }

        public AssignCommand(Position position, string name, Expression expression):
            base(position)
        {
            _name = name;
            _expression = expression;
            _expression.Parent = this;
        }

        public override void DumpFields(Indenter stream)
        {
            stream.WriteLine("Name = {0}", _name);
            stream.WriteLine("Expression = {0,4:D4}", _expression.Id);
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}
