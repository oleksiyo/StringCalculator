using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CalculatorKata;
using Xunit;
using FluentAssertions;

namespace CalculatorKata
{
    public class DelimetersServices : IDelimetersServices
    {
        readonly List<string> delimeters = new List<string> { "\n", ",", ";" };
        private const int partOfRegularExpression = 1;

        public List<string> FillDelimeters(string input)
        {
            var stringOfDelimeters = GetListSubStringsByRegex(input, GetConstantValue.RegexForSubString).FirstOrDefault();

            if (string.IsNullOrEmpty(stringOfDelimeters))
                return delimeters;

            if (!Regex.Match(stringOfDelimeters, GetConstantValue.RegexForDelimeter.ToString()).Success && !delimeters.Contains(stringOfDelimeters))
                delimeters.Add(stringOfDelimeters);

            delimeters.AddRange(GetListSubStringsByRegex(stringOfDelimeters, GetConstantValue.RegexForDelimeter));

            return delimeters;
        }

        private IEnumerable<string> GetListSubStringsByRegex(string input, Regex regex)
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
        var delimeters = delimetersServices.FillDelimeters(input);
        delimeters.Should().Contain("***");
    }

    [Fact]
    public void should_get_more_then_one_delimeters()
    {
        const string input = "//[***][!!!]\n1***2!!!3";

        var delimetersServices = new DelimetersServices();
        var delimeters = delimetersServices.FillDelimeters(input);
        delimeters.Should().Contain("***", "!!!");
    }

    [Fact]
    public void should_not_get_const_delimeters()
    {
        const string input = "1,2,3";

        var delimetersServices = new DelimetersServices();
        var delimeters = delimetersServices.FillDelimeters(input);
        delimeters.Should().NotContain("");

    }

    [Fact]
    public void should_get_delimeter_without_scope()
    {
        const string input = "//+\n1+2+3";

        var delimetersServices = new DelimetersServices();
        var delimeters = delimetersServices.FillDelimeters(input);
        delimeters.Should().Contain("+");
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
