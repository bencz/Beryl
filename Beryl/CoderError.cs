using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl
{
    public class CoderError : System.Exception
    {
        private Position _position;
        public Position Position
        {
            get { return _position; }
        }

        public CoderError(string message) :
            base(message)
        {
            _position = null;
        }

        public CoderError(Position position, string message) :
            base(message)
        {
            _position = new Position(position);    // make DEEP copy to avoid nasty side effects
        }
    }
}
