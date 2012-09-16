using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl
{
    public class Scope
    {
        private Dictionary<string, Symbol> _symbols = new Dictionary<string, Symbol>();

        public void Insert(string name, Symbol symbol)
        {
            _symbols[name] = symbol;
        }

        public Symbol Lookup(string name)
        {
            Symbol result;
            _symbols.TryGetValue(name, out result);
            return result;
        }
    }
}

