using System;

namespace Beryl
{
    public class Demangler
    {
        private static bool IsDigitOrLetter(char value)
        {
            if (value >= 'a' && value <= 'z')
                return true;
            if (value >= 'A' && value <= 'Z')
                return true;
            if (value >= '0' && value <= '9')
                return true;

            return false;
        }

        public static string Decode(string symbol)
        {
            System.Text.StringBuilder result = new System.Text.StringBuilder(64);

            int index = 0;
            if (index == symbol.Length)
                throw new BerylError("Attempt of demangling empty string");

            if (symbol[index] != '$')
                throw new BerylError("Attempt of demangling unmangled name: " + symbol);
            index += 1;

            if (index == symbol.Length)
                throw new BerylError("Malformed symbol: " + symbol);
            bool @operator = false;
            if (symbol[index] == '$')
            {
                @operator = true;
                index += 1;
            }

            int start = index;
            while (index < symbol.Length && IsDigitOrLetter(symbol[index]))
                index += 1;
            if (index == symbol.Length)
                throw new BerylError("Missing parameters in symbol: " + symbol);

            int length = index - start;
            string name = symbol.Substring(start, length);
            if (@operator)
            {
                if (length > 3)
                    throw new BerylError("Malformed operator in symbol: " + symbol);
                switch (name)
                {
                    case "add": name = "+"; break;
                    case "sub": name = "-"; break;
                    case "mul": name = "*"; break;
                    case "div": name = "/"; break;
                    case "mod": name = "//"; break;
                    case "eq" : name = "="; break;
                    case "ne" : name = "\\="; break;
                    case "lt" : name = "<"; break;
                    case "le" : name = "<="; break;
                    case "gt" : name = ">"; break;
                    case "ge" : name = ">="; break;
                    case "and": name = "/\\"; break;
                    case "ior": name = "\\/"; break;
                    case "not": name = "\\"; break;
                    default   : throw new BerylError("Invalid operator in symbol: " + symbol);
                }
            }
            result.Append(name);

            result.Append('(');
            if (symbol[index++] != '$')
                throw new BerylError("Invalid parameter list in symbol: " + symbol);
            if (index == symbol.Length)
                throw new BerylError("Truncated parameter list in symbol: " + symbol);
            while (index < symbol.Length - 1)
            {
                switch (symbol[index])
                {
                    case 'b': result.Append("Boolean"); break;
                    case 'i': result.Append("Integer"); break;
                    case 's': result.Append("String"); break;
                    default : throw new BerylError("Invalid parameter type in symbol: " + symbol);
                }
                index += 1;

                if (index < symbol.Length - 1 && symbol[index] != '$')
                    result.Append(", ");
            }
            if (index != symbol.Length -1 || symbol[index] != '$')
                throw new BerylError("Missing terminator in symbol: " + symbol);
            result.Append(')');

            return result.ToString();
        }
    }
}
