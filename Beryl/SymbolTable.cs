using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl
{
    public class SymbolTable
    {
        private Dictionary<string, Symbol> _symbols = new Dictionary<string, Symbol>();

        public SymbolTable()
        {
        }

        public void Insert(Position position, string name, Symbol symbol)
        {
            if (_symbols.ContainsKey(name))
                throw new ParserError(position, "Symbol already defined '" + name + "'");
            _symbols[name] = symbol;
        }

        public Symbol Lookup(Position position, string name)
        {
            Symbol result;
            if (!_symbols.TryGetValue(name, out result))
                throw new ParserError(position, "Symbol not defined '" + name + "'");
            return result;
        }
    }
}
