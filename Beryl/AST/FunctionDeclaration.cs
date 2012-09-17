using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class FunctionDeclaration: Declaration
    {
        private ParameterDeclaration[] _parameters;
        public ParameterDeclaration[] Parameters
        {
            get { return _parameters; }
        }

        private Expression _body;
        public Expression Body
        {
            get { return _body; }
        }

        public FunctionDeclaration(Position position, string name, AST.Type type, ParameterDeclaration[] parameters, Expression body) :
            base(position, name, SymbolKind.Function, type)
        {
            _parameters = parameters;
            foreach (ParameterDeclaration parameter in _parameters)
                parameter.Parent = this;

            // body MAY be null for predefined functions
            _body = body;
            if (_body != null)
                _body.Parent = this;
        }

        public override void DumpFields(Indenter stream)
        {
            base.DumpFields(stream);

            System.Text.StringBuilder mangled = new System.Text.StringBuilder(64);
            this.Encode(mangled);
            stream.WriteLine("Mangled = {0}", mangled.ToString());
            stream.WriteLine("Demangled = {0}", Demangler.Decode(mangled.ToString()));

            foreach (ParameterDeclaration parameter in _parameters)
                stream.WriteLine("Parameter = {0,4:D4}", parameter.Id);
            stream.WriteLine("Body = {0,4:D4}", (_body == null) ? "null" : _body.Id.ToString("D4"));
        }

        public override void Encode(System.Text.StringBuilder result)
        {
            if (this.Name[0] == '$')
                throw new BerylError("Symbol already mangled: " + this.Name);

            // output function or operator name
            result.Append('$');
            result.Append(Mangler.EncodeNamePart(this.Name));

            // output parameter types
            result.Append('$');
            foreach (ParameterDeclaration parameter in _parameters)
                parameter.Encode(result);

            // output terminating dollar sign ($)
            result.Append('$');
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}

