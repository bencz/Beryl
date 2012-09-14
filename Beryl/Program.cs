/* Syntax:
 * Program = Single-Command .
 * Command = Single-Command | Command ";" Single-Command .
 * Single-Command = 
 *      V-name ":=" Expression | 
 *      Identifier "(" Expression ")" |
 *      "if" Expression "then" Single-Command "else" Single-Command |
 *      "while" Expression "do" Single-Command |
 *      "let" Declaration "in" Single-Command |
 *      "begin" Command "end" |
 *      "func" Identifier "(" Parameters ")" ":" TypeDenoter "~" Expression .
 * Expression = Primary-Expression | Expression Operator Primary-Expression .
 * Primary-Expression = Integer-Literal | V-name | Operator Primary-Expression | "(" Expression ")" .
 * V-name             = Identifier .
 * Declaration        = Single-Declaration | Declaration ";" Single-Declaration .
 * Single-Declaration = "const" Identifier "~" Expression | "var" Identifier ":" Type-denoter .
 * Type-denoter       = Identifer .
 * Operator           = "+" | "-" | "*" | "/" | "<" | ">" | "=" | "\" .
 * Identifier         = Letter | Identifier Letter | Identifier Digit .
 * Integer-Literal    = Digit | Integer-Literal Digit .
 * Comment            = "!" Graphics EOL .
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Beryl
{
    class Program
    {
        static int Main(string[] args)
        {
			int result = 0;

			try
			{
#if false
				StreamReader sr = new StreamReader(args[0]);
				Scanner scanner = new Scanner(args[0], sr, 4);
				for (; ; )
				{
					Token token = scanner.ReadToken();
					Console.WriteLine(token.Position.ToString() + ":" + token.ToString());

					if (token.Kind == TokenKind.EndOfFile)
						break;
				}
				sr.Close();
				Console.ReadLine();
#else
				SymbolTable symbols = new SymbolTable();
				StreamReader reader = new StreamReader(args[0]);
				Scanner scanner = new Scanner(args[0], reader, 4);
				Parser parser = new Parser(symbols, scanner);
				AST.Program program = parser.ParseProgram();
				new CodeGen(symbols, program);
				Console.WriteLine("Press ENTER");
				Console.ReadLine();
#endif
			}
			catch (CoderError that)
			{
				Console.WriteLine("{0} Error: {1}", that.Position.ToString(), that.Message);
				result = 1;
			}
			catch (ParserError that)
			{
				Console.WriteLine("{0} Error: {1}", that.Position.ToString(), that.Message);
				result = 1;
			}
			catch (ScannerError that)
			{
				Console.WriteLine("{0} Error: {1}", that.Position.ToString(), that.Message);
				result = 1;
			}

			return result;
        }
    }
}