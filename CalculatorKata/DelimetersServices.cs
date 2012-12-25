using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CalculatorKata;
using Xunit;

namespace CalculatorKata
{
    public class DelimetersServices
    {
        static public string GetDelimeters(string input, string start, string end)
        {
            var r = new Regex(Regex.Escape(start) + "(.*?)" + Regex.Escape(end));
            var matches = r.Matches(input);
            return (from Match match in matches select match.Groups[1].Value).FirstOrDefault();
        }

        static public List<string> FillDelimeters(string input, string start, string end)
        {
            var delimeters = new List<string>() { "\n", ",", ";" };
            if (string.IsNullOrEmpty(input))
                return delimeters;

            var strRegex = Regex.Escape(start) + "(.*?)" + Regex.Escape(end);

            if (!Regex.Match(input, strRegex).Success && !delimeters.Contains(input))
                delimeters.Add(input);

            var r = new Regex(strRegex);
            var matches = r.Matches(input);
            delimeters.AddRange(from Match match in matches select match.Groups[1].Value);

            return delimeters;
        }
    }
}

public class DelimetersServicesTest
{
    const string start = "//";
    const string end = "\n";
    const string dStart = "[";
    const string dEnd = "]";


    [Fact]
    public void should_get_one_delimiter_with_any_length()
    {
        const string input = "//[***]\n1***2***3";

        var delimeters = DelimetersServices.GetDelimeters(input, start, end);
        Assert.Equal(delimeters, "[***]");
    }

    [Fact]
    public void should_get_more_then_one_delimeters()
    {
        const string input = "//[***][!!!]\n1***2!!!3";

        var delimeters = DelimetersServices.GetDelimeters(input, start, end);
        Assert.Equal(delimeters, "[***][!!!]");
    }

    [Fact]
    public void should_not_get_const_delimeters()
    {
        const string input = "1,2,3";

        var delimeters = DelimetersServices.GetDelimeters(input, start, end);
        Assert.Equal(delimeters, null);

    }

    [Fact]
    public void should_get_delimeter_without_scope()
    {
        const string input = "//;\n1;-2;3";

        var delimeters = DelimetersServices.GetDelimeters(input, start, end);
        Assert.Equal(delimeters, ";");
    }

    [Fact]
    public void should_split_one_delimeter_with_any_length()
    {
        const string input = "[***]";
        var listDelimeters = DelimetersServices.FillDelimeters(input, dStart, dEnd);
        Assert.Equal(listDelimeters.Count, 4);
    }

    [Fact]
    public void should_split_more_then_one_delimeter()
    {
        const string input = "[***][!!!]";

        var listDelimeters = DelimetersServices.FillDelimeters(input, dStart, dEnd);
        Assert.Equal(listDelimeters.Count, 5);
    }

    [Fact]
    public void should_not_split_string_without_delimeters()
    {
        const string input = "";

        var listDelimeters = DelimetersServices.FillDelimeters(input, dStart, dEnd);
        Assert.Equal(listDelimeters.Count, 3);
    }

    [Fact]
    public void should_not_split_const_delimeter()
    {
        const string input = ";";

        var listDelimeters = DelimetersServices.FillDelimeters(input, dStart, dEnd);
        Assert.Equal(listDelimeters.Count, 3);
    }
}
