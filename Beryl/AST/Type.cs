using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public abstract class Type: Node
    {
		public Type(Position position):
			base(position)
		{
		}
    }
}
