namespace Beryl
{
    public class Mangler
    {
        public static string EncodeNamePart(string name)
        {
            string result;
            switch (name)
            {
                case "\\" : result = "$not"; break;
                case "/\\": result = "$and"; break;
                case "\\/": result = "$ior"; break;
                case "+"  : result = "$add"; break;
                case "-"  : result = "$sub"; break;
                case "*"  : result = "$mul"; break;
                case "/"  : result = "$div"; break;
                case "//" : result = "$mod"; break;
                case "<"  : result = "$lt"; break;
                case "<=" : result = "$le"; break;
                case ">"  : result = "$gt"; break;
                case ">=" : result = "$ge"; break;
                case "="  : result = "$eq"; break;
                case "\\=": result = "$ne"; break;
                default   : result = name; break;
            }

            return result;
        }
    }
}

