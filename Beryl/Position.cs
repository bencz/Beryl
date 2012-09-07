using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beryl
{
    public class Position
    {
        private string _file;
        public string File
        {
            get { return _file; }
        }

        private int _line;
        public int Line
        {
            get { return _line; }
            set { _line = value; }
        }

        private int _char;
        public int Char
        {
            get { return _char; }
            set { _char = value; }
        }

        public Position()
        {
        }

        public Position(Position position)
        {
            _file = position.File;
            _line = position.Line;
            _char = position.Char;
        }

        public Position(string file, int line, int @char)
        {
            _file = file;
            _line = line;
            _char = @char;
        }

        public override string ToString()
        {
            return "(" + _file + " " + _line.ToString() + ":" + _char.ToString() + ")";
        }
    }
}
