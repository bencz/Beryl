using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class FunctionDeclaration: Declaration
    {
        private string _name;
        public string Name
        {
            get { return _name; }
        }

        private Parameter[] _parameters;
        public Parameter[] Parameters
        {
            get { return _parameters; }
        }

        private Expression _body;
        public Expression Body
        {
            get { return _body; }
        }

        public FunctionDeclaration(Position position, string name, AST.Type type, Parameter[] parameters, Expression body) :
            base(position, SymbolKind.Function, type)
        {
            _name = name;

            _parameters = parameters;
            foreach (Parameter parameter in _parameters)
                parameter.Parent = this;

            // body MAY be null for predefined functions
            _body = body;
            if (_body != null)
                _body.Parent = this;
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}

