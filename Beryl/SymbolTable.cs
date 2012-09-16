using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl
{
    public class SymbolTable
    {
        Stack<Scope> _scopes = new Stack<Scope>();

        public SymbolTable()
        {
            EnterScope();
        }

        public void EnterScope()
        {
            _scopes.Push(new Scope());
        }

        public void LeaveScope()
        {
            if (_scopes.Count > 1)
                _scopes.Pop();
        }

        public void Insert(Position position, string name, Symbol symbol)
        {
            Scope top = _scopes.Peek();
            if (top.Lookup(name) != null)
                throw new ParserError(position, "Symbol '" + name + "' already defined");
            top.Insert(name, symbol);
        }

        public Symbol Lookup(string name)
        {
            foreach (Scope scope in _scopes)
            {
                Symbol symbol = scope.Lookup(name);
                if (symbol != null)
                    return symbol;
            }

            return null;
        }
    }
}
