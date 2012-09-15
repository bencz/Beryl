using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class FunctionType: Type
    {
        private AST.Type _type;
        public AST.Type Type
        {
            get { return _type; }
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

        public FunctionType(Position position, AST.Type type, Parameter[] parameters, Expression body):
            base(position)
        {
            _type = type;
            _type.Parent = this;

            _parameters = parameters;
            foreach (Parameter parameter in _parameters)
                parameter.Parent = this;

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

