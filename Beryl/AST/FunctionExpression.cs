using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class FunctionExpression: Expression
    {
        private string _name;
        public string Name
        {
            get { return _name; }
        }

        private Expression[] _arguments;
        public Expression[] Arguments
        {
            get { return _arguments; }
        }

        public FunctionExpression(Position position, string name, Expression[] arguments):
			base(position)
        {
            _name = name;
            _arguments = arguments;
            foreach (Expression argument in _arguments)
                argument.Parent = this;
        }

		public override int Evaluate(SymbolTable symbols)
		{
			throw new CheckerError(this.Position, "Functions cannot be part of a constant expression: " + _name);
		}

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}

