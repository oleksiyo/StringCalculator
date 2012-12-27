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
        public string GetDelimeters(string input)
        {
            var r = new Regex(Conf.RegexBetweenTwoStrings);
            var matches = r.Matches(input);
            return (from Match match in matches select match.Groups[1].Value).FirstOrDefault();
        }

        public List<string> FillDelimeters(string input)
        {
            var delimeter = GetDelimeters(input);
            var delimeters = new List<string> { "\n", ",", ";" };
            if (string.IsNullOrEmpty(delimeter))
                return delimeters;

            if (!Regex.Match(delimeter, Conf.RegexForDelimeters).Success && !delimeters.Contains(delimeter))
                delimeters.Add(delimeter);

            var r = new Regex(Conf.RegexForDelimeters);
            var matches = r.Matches(delimeter);
            delimeters.AddRange(from Match match in matches select match.Groups[1].Value);

            return delimeters;
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
        var delimeters = delimetersServices.GetDelimeters(input);
        delimeters.Should().Be("[***]");
    }

    [Fact]
    public void should_get_more_then_one_delimeters()
    {
        const string input = "//[***][!!!]\n1***2!!!3";

        var delimetersServices = new DelimetersServices();
        var delimeters = delimetersServices.GetDelimeters(input);
        delimeters.Should().Be("[***][!!!]");
    }

    [Fact]
    public void should_not_get_const_delimeters()
    {
        const string input = "1,2,3";

        var delimetersServices = new DelimetersServices();
        var delimeters = delimetersServices.GetDelimeters(input);
        delimeters.Should().Be(null);


    }

    [Fact]
    public void should_get_delimeter_without_scope()
    {
        const string input = "//;\n1;-2;3";

        var delimetersServices = new DelimetersServices();
        var delimeters = delimetersServices.GetDelimeters(input);
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
