using System.Text.RegularExpressions;

namespace CalculatorKata
{
    public class Configuration
    {
        public static string startSubString = "//";
        public static string endSubString = "\n";
        const string startDelimeter = "[";
        const string endDelimeter = "]";
        public static string exceptionMsg = "Negatives not allowed:";
        const string regex = "(.*?)";
        public static int startRange = 0;
        public static int endRange = 1000;

        public static Regex RegexForSubString
        {
            get { return new Regex(Regex.Escape(startSubString) + regex + Regex.Escape(endSubString)); }
        }

        public  static Regex RegexForDelimeter
        {
            get { return new Regex(Regex.Escape(startDelimeter) + regex + Regex.Escape(endDelimeter)); }
        }
    }
}
