using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beryl
{
    public enum TokenKind
    {
        None,
        Assignment,                     // ,=
        Asterisk,                       // *
        Backslash,                      // \
        Colon,                          // :
		Comma,							// ,
        EndOfFile,
        Equal,                          // = 
        GreaterThan,                    // >
        Identifier,
        Keyword_Begin,
        Keyword_Const,
        Keyword_Do,
        Keyword_Else,
        Keyword_End,
        Keyword_Func,
        Keyword_If,
        Keyword_In,
        Keyword_Let,
        Keyword_Then,
        Keyword_Var,
        Keyword_While,
        LeftParenthesis,                // (
        LessThan,                       // <
        Literal_Integer,
        Minus,                          // -
        Plus,                           // +
        RightParenthesis,               // )
        Semicolon,                      // ;
        Slash,                          // /
        Tilde                           // ~
    }
}

