using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beryl
{
    public class Token
    {
        private TokenKind _kind = TokenKind.None;
        public TokenKind Kind
        {
            get { return _kind; }
            set { _kind = value; }
        }

        private Position _position;
        public Position Position
        {
            get { return _position; }
            set { _position = value; }
        }

        private string _text;
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public Token()
        {
            _position = new Position();
        }

        public Token(Position position)
        {
            _position = new Position(position);  // create a DEEP copy of the position to avoid problems ahead.
        }

        public override string ToString()
        {
            switch (_kind)
            {
                case TokenKind.None:
                    throw new System.Exception("Internal error");

                case TokenKind.Assignment:                     // :=
                    return "operator ':='";

                case TokenKind.Asterisk:                       // *
                    return "operator '*'";

                case TokenKind.Backslash:                      // \
                    return "operator '\'";
                    
                case TokenKind.Colon:                          // :
                    return "symbol ':'";

                case TokenKind.EndOfFile:
                    return "End Of File";

                case TokenKind.Equal:                          // =
                    return "operator '='";

                case TokenKind.GreaterThan:                    // >
                    return "operator '>'";

                case TokenKind.Identifier:
                    return "identifier '" + _text + "'";

                case TokenKind.Keyword_Begin:
                    return "keyword 'begin'";

                case TokenKind.Keyword_Const:
                    return "keyword 'const'";

                case TokenKind.Keyword_Do:
                    return "keyword 'do'";

                case TokenKind.Keyword_Else:
                    return "keyword 'else'";

                case TokenKind.Keyword_End:
                    return "keyword 'end'";

                case TokenKind.Keyword_Func:
                    return "keyword 'func'";

                case TokenKind.Keyword_If:
                    return "keyword 'if'";

                case TokenKind.Keyword_In:
                    return "keyword 'in'";

                case TokenKind.Keyword_Let:
                    return "keyword 'let'";

                case TokenKind.Keyword_Then:
                    return "keyword 'then'";

                case TokenKind.Keyword_Var:
                    return "keyword 'var'";

                case TokenKind.Keyword_While:
                    return "keyword 'while'";

                case TokenKind.LeftParenthesis:                // (
                    return "symbol '('";

                case TokenKind.LessThan:                       // <
                    return "operator '<'";

                case TokenKind.Literal_Integer:
                    return "integer literal '" + _text + "'";

                case TokenKind.Minus:                          // -
                    return "operator '-'";

                case TokenKind.Plus:                           // +
                    return "operator '+'";

                case TokenKind.RightParenthesis:               // )
                    return "symbol ')'";

                case TokenKind.Semicolon:                      // ;
                    return "symbol ';'";

                case TokenKind.Slash:                          // /
                    return "operator '/'";

                case TokenKind.Tilde:                          // ~
                    return "symbol '~'";
            }
            return base.ToString();
        }
    }
}