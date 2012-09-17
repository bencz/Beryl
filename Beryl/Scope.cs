using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beryl.AST;

namespace Beryl
{
    public class Scope
    {
        private Dictionary<string, Declaration> _symbols = new Dictionary<string, Declaration>();

        private string _name;
        public string Name
        {
            get { return _name; }
        }

        public Scope(string name)
        {
            _name = name;
        }

        public void Insert(string name, Declaration declaration)
        {
            _symbols[name] = declaration;
        }

        public Declaration Lookup(string name)
        {
            Declaration result;
            _symbols.TryGetValue(name, out result);
            return result;
        }
    }
}

