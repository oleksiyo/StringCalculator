using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CalculatorKata;
using NSubstitute;
using Xunit;
using FluentAssertions;

namespace CalculatorKata
{
    public class DelimetersServices : IDelimetersServices
    {
        List<string> delimeters = new List<string> { "\n", ",", ";" };
        private readonly int partOfRegularExpression = 1;

        public List<string> FillDelimeters(string input)
        {
            var stringOfDelimeters = GetStringOfDelimeters(input);

            if (string.IsNullOrEmpty(stringOfDelimeters))
                return delimeters;

            if (!Regex.Match(stringOfDelimeters, Configuration.RegexForDelimeter.ToString()).Success && !delimeters.Contains(stringOfDelimeters))
                delimeters.Add(stringOfDelimeters);

            delimeters.AddRange(GetListSubStringsByRegex(stringOfDelimeters, Configuration.RegexForDelimeter));

            return delimeters;
        }

        public string GetStringOfDelimeters(string input)
        {
            return GetListSubStringsByRegex(input, Configuration.RegexForSubString).Count > 0
                ? GetListSubStringsByRegex(input, Configuration.RegexForSubString)[0]
                : "";
        }

        private List<string> GetListSubStringsByRegex(string input, Regex regex)
        {
            var matches = regex.Matches(input);
            var listSubStrings = new List<string>();
            listSubStrings.AddRange(from Match match in matches select match.Groups[partOfRegularExpression].Value);
            return listSubStrings;
        }
    }
}

public class DelimetersServicesTest
{
    [Fact]
    public void should_get_one_delimiter_with_any_length()
    {
        const string input = "//[***]\n1***2***3";
        var delimetersServices = new DelimetersServices();
        var delimeters = delimetersServices.GetStringOfDelimeters(input);
        delimeters.Should().Be("[***]");
    }

    [Fact]
    public void should_get_more_then_one_delimeters()
    {
        const string input = "//[***][!!!]\n1***2!!!3";

        var delimetersServices = new DelimetersServices();
        var delimeters = delimetersServices.GetStringOfDelimeters(input);
        delimeters.Should().Be("[***][!!!]");
    }

    [Fact]
    public void should_not_get_const_delimeters()
    {
        const string input = "1,2,3";

        var delimetersServices = new DelimetersServices();
        var delimeters = delimetersServices.GetStringOfDelimeters(input);
        delimeters.Should().Be("");


    }

    [Fact]
    public void should_get_delimeter_without_scope()
    {
        const string input = "//;\n1;-2;3";

        var delimetersServices = new DelimetersServices();
        var delimeters = delimetersServices.GetStringOfDelimeters(input);
        delimeters.Should().Be(";");
    }

    [Fact]
    public void should_split_one_delimeter_with_any_length()
    {
        const string input = "//[***]\n1***2***3";
        var delimetersServices = new DelimetersServices();
        var listDelimeters = delimetersServices.FillDelimeters(input);
        listDelimeters.Count.Should().Be(4);
    }

    [Fact]
    public void should_split_more_then_one_delimeter()
    {
        const string input = "//[***][!!!]\n1***2!!!3";

        var delimetersServices = new DelimetersServices();
        var listDelimeters = delimetersServices.FillDelimeters(input);
        listDelimeters.Count.Should().Be(5);
    }

    [Fact]
    public void should_not_split_string_without_delimeters()
    {
        const string input = "";

        var delimetersServices = new DelimetersServices();
        var listDelimeters = delimetersServices.FillDelimeters(input);
        listDelimeters.Count.Should().Be(3);
    }

    [Fact]
    public void should_not_split_const_delimeter()
    {
        const string input = ";";

        var delimetersServices = new DelimetersServices();
        var listDelimeters = delimetersServices.FillDelimeters(input);
        listDelimeters.Count.Should().Be(3);
    }
}
