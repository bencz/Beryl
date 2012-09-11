using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class AssignCommand: Command
    {
        private string _name;
        private Expression _expression;

        public AssignCommand(string name, Expression expression)
        {
            _name = name;
            _expression = expression;
            _expression.Parent = this;
        }
    }
}
