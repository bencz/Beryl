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
                // expand wildcards
                string[] found = Toolbox.Find(args, false);

                // process each input file in turn
                foreach (string arg in found)
                {
                    SymbolTable symbols = new SymbolTable();
                    StreamReader reader = new StreamReader(arg);
                    Scanner scanner = new Scanner(arg, reader, 4);
                    Parser parser = new Parser(symbols, scanner);
                    AST.Program program = parser.ParseProgram();
                    Checker checker = new Checker(symbols, program);
                    Indenter indenter = new Indenter(arg + ".log", System.Text.Encoding.ASCII);
                    try
                    {
                        program.Dump(indenter);
                    }
                    finally
                    {
                        indenter.Close();
                    }
                    new CodeGen(symbols, program);
                    Console.WriteLine("Press ENTER");
                    Console.ReadLine();
                }
#endif
            }
            catch (BerylError that)
            {
                if (that.Position == null)
                    Console.WriteLine("Error: {0}", that.Message);
                else
                    Console.WriteLine("{0} Error: {1}", that.Position.ToString(), that.Message);
                result = 1;
            }
#if false
            catch (System.Exception that)
            {
                Console.WriteLine("Error: {0}", that.Message);
                result = 1;
            }
#endif

            return result;
        }
    }
}