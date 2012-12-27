using System.Text.RegularExpressions;

namespace CalculatorKata
{
    public class Conf
    {
        public static string start = "//";
        public static string end = "\n";
        const string dStart = "[";
        const string dEnd = "]";
        public static string exceptionMsg = "Negatives not allowed:";
        const string regex = "(.*?)";

        public static string RegexBetweenTwoStrings
        {
            get { return Regex.Escape(start) + regex + Regex.Escape(end); }
        }

        public static string RegexForDelimeters
        {
            get { return Regex.Escape(dStart) + regex + Regex.Escape(dEnd); }
        }
    }
}
