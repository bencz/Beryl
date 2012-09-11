using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class FunctionDeclaration: Declaration
    {
        private string _name;
        private Parameter[] _parameters;
        private Expression _body;

        public FunctionDeclaration(string name, Parameter[] parameters, Expression body)
        {
            _name = name;
            _parameters = parameters;
            foreach (Parameter parameter in _parameters)
                parameter.Parent = this;
            _body = body;
            _body.Parent = this;
        }
    }
}
