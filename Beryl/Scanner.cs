using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Beryl
{
    public class Scanner
    {
        const char EOF = '\uFFFF';

        private TextReader _source;
        private Position _cursor;
        private int _tabsize;
        private char _nextChar;
        private Token _nextToken;
        private StringBuilder _text = new StringBuilder(132);
        private Dictionary<string, TokenKind> _keywords = new Dictionary<string, TokenKind>();

        public Scanner(string filename, TextReader source, int tabsize)
        {
            // set up keyword map to TokenKind
            dictPop();

            _source = source;
            _cursor = new Position(filename, 1, 0);  // filename could be "(console)" if doing interactive interpretation
            _tabsize = tabsize;

            ReadChar();    // fetch initial lookahead character
            _nextToken = ScanToken();  // fetch initial lookahead token
        }

        public void dictPop()
        {
            _keywords["begin"] = TokenKind.Keyword_Begin;
            _keywords["const"] = TokenKind.Keyword_Const;
            _keywords["do"] = TokenKind.Keyword_Do;
            _keywords["else"] = TokenKind.Keyword_Else;
            _keywords["func"] = TokenKind.Keyword_Func;
            _keywords["end"] = TokenKind.Keyword_End;
            _keywords["if"] = TokenKind.Keyword_If;
            _keywords["in"] = TokenKind.Keyword_In;
            _keywords["let"] = TokenKind.Keyword_Let;
            _keywords["then"] = TokenKind.Keyword_Then;
            _keywords["var"] = TokenKind.Keyword_Var;
            _keywords["while"] = TokenKind.Keyword_While;
        }

        private char ReadCharPrim()
        {
            int ch = _source.Read();
            return (ch == -1 ? EOF : (char)ch);
        }

        private char ReadChar()
        {
            char result = _nextChar;
            _nextChar = ReadCharPrim();

            _cursor.Char += 1;
            if (result == '\n')
            {
                _cursor.Line += 1;
                _cursor.Char = 1;
            }
            else if (result == '\t')
            {
                // expand tabs into spaces for proper error reporting (otherwise the character number goes amok on tabs)
                int width = _tabsize - (_cursor.Char % _tabsize) + 1;
                _cursor.Char += width;
            }

            return result;
        }

        /* in the parser:
         *
         * if (PeekToken() == TokenKind.Keyword_if)
         *     ParseifStatement();
         *
         * and:
         *
         * switch (PeekToken())
         * {
         *     ...
         * }
         */
        public Token PeekToken()
        {
            return _nextToken;
        }

        /* ReadToken() scans a new token and returns the current lookahead token. */
        public Token ReadToken()
        {
            Token result = _nextToken;
            _nextToken = ScanToken();
            return result;
        }

        private Token ScanToken()
        {
            Token result = new Token(_cursor);
            _text.Length = 0;

            char ch = ReadChar();
            switch (ch)
            {
                // discard whitespace
                case ' ':
                case '\t':
                case '\n':
                case '\r':
                    while (char.IsWhiteSpace(_nextChar))
                        ReadChar();
                    return ScanToken();

                case ';':
                    result.Kind = TokenKind.Semicolon;
                    break;

                case ':':
                    if (_nextChar == '=')
                    {
                        ReadChar();
                        result.Kind = TokenKind.Assignment;
                        break;
                    }

                    result.Kind = TokenKind.Colon;
                    break;

                case ',':
                    result.Kind = TokenKind.Comma;
                    break;

                case '~':
                    result.Kind = TokenKind.Tilde;
                    break;

                case '(':
                    result.Kind = TokenKind.LeftParenthesis;
                    break;

                case ')':
                    result.Kind = TokenKind.RightParenthesis;
                    break;

                case '+':
                    result.Kind = TokenKind.Plus;
                    break;

                case '-':
                    result.Kind = TokenKind.Minus;
                    break;

                case '*':
                    result.Kind = TokenKind.Asterisk;
                    break;

                case '/':
                    result.Kind = TokenKind.Slash;
                    break;

                case '<':
                    result.Kind = TokenKind.LessThan;
                    break;

                case '>':
                    result.Kind = TokenKind.GreaterThan;
                    break;

                case '=':
                    result.Kind = TokenKind.Equal;
                    break;

                case '\\':
                    result.Kind = TokenKind.Backslash;
                    break;

                case EOF:
                    result.Kind = TokenKind.EndOfFile;
                    break;

                case '"':
                    while (_nextChar != '"')
                    {
                        if (_nextChar == '\n')
                            throw new ScannerError(_cursor, "End of line in string literal");
                        else if (_nextChar == EOF)
                            throw new ScannerError(_cursor, "End of file in string literal");

                        _text.Append(ReadChar());
                    }
                    ReadChar();         // skip closing quote

                    result.Kind = TokenKind.Literal_String;
                    result.Text = _text.ToString();
                    break;

                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    _text.Append(ch);
                    while (_nextChar >= '0' && _nextChar <= '9')
                    {
                        _text.Append(_nextChar);
                        ReadChar();
                    }
                    result.Kind = TokenKind.Literal_Integer;
                    result.Text = _text.ToString();
                    break;

                default:
                    if (!Char.IsLetter(ch))
                        throw new ScannerError(_cursor, "Invalid character");
                    _text.Append(ch);

                    while (char.IsLetterOrDigit(_nextChar))
                    {
                        _text.Append(_nextChar);
                        ReadChar();
                    }

                    // check if the identifier is a keyword
                    result.Text = _text.ToString();
                    if (_keywords.ContainsKey(result.Text))
                        result.Kind = _keywords[result.Text];
                    else
                        result.Kind = TokenKind.Identifier;

                    break;
            }

            return result;
        }

    }
}
