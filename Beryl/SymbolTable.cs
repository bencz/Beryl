using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beryl.AST;

namespace Beryl
{
    public class SymbolTable
    {
        Stack<Scope> _scopes = new Stack<Scope>();

        public SymbolTable()
        {
            EnterScope("(global)");
        }

        public void EnterScope(string name)
        {
            _scopes.Push(new Scope(name));
        }

        public void LeaveScope(string name)
        {
            if (_scopes.Count == 0)
                throw new BerylError("Scope stack underflow");

            Scope top = _scopes.Pop();
            if (top.Name != name)
                throw new BerylError("Scope stack mismatch: " + name);
        }

        public void Insert(Position position, string name, Declaration declaration)
        {
            Scope top = _scopes.Peek();
            if (top.Lookup(name) != null)
                throw new ParserError(position, "Symbol '" + name + "' already defined");
            top.Insert(name, declaration);
        }

        public Declaration Lookup(string name)
        {
            foreach (Scope scope in _scopes)
            {
                Declaration declaration = scope.Lookup(name);
                if (declaration != null)
                    return declaration;
            }

            return null;
        }
    }
}
