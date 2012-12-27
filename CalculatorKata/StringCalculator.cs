using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace CalculatorKata
{
    public class StringCalculator
    {
        private IDelimetersServices delimetersServices;

        public StringCalculator(IDelimetersServices delimetersServices)
        {
            this.delimetersServices = delimetersServices;
        }

        public int Sum(string stringNumbers)
        {
            if (String.IsNullOrEmpty(stringNumbers))
                return 0;

            var listDelimeters = delimetersServices.FillDelimeters(stringNumbers);

            var substringNumbers = listDelimeters.Any(prefix => stringNumbers.StartsWith(Configuration.startSubString)) ?
                GetSubStringInRange(stringNumbers, Configuration.endSubString, "") :
                stringNumbers;

            var numbers = SplitStringByDelimeters(substringNumbers, listDelimeters);
            CheckContainNegativeNambers(numbers);

            return CalculatinSumInRange(numbers, Configuration.startRange, Configuration.endRange);
        }

        private List<int> SplitStringByDelimeters(string input, List<string> listDelimeters)
        {
            return input.Split(listDelimeters.ToArray(), StringSplitOptions.None).ToList().ConvertAll(int.Parse);
        }

        private string GetSubStringInRange(string input, string start, string end)
        {
            var startIndex = input.IndexOf(start, StringComparison.Ordinal);
            var endIndex = String.IsNullOrEmpty(end) ? input.Length : input.IndexOf(end, StringComparison.Ordinal);
            return input.Substring(startIndex + 1, endIndex - startIndex - 1);
        }

        private int CalculatinSumInRange(IEnumerable<int> numbers, int startRange, int endRenge)
        {
            return numbers.Select(x => x).Where(z => z >= startRange && z <= endRenge).Sum();
        }

        private void CheckContainNegativeNambers(IEnumerable<int> numbers)
        {
            var listNegativs = numbers.Select(x => x).Where(x => x < 0).ToList();

            if (listNegativs.Any())
                throw new Exception(Configuration.exceptionMsg + " " + string.Join(", ", listNegativs));
        }
    }

    public class StringCalculatorTests
    {
        private StringCalculator stringCalculator;
        readonly IDelimetersServices delimetersServices = Substitute.For<IDelimetersServices>();

        public StringCalculatorTests()
        {
            stringCalculator = new StringCalculator(delimetersServices);
        }

        [Fact]
        public void should_return_0_for_empty_string()
        {
            var sum = stringCalculator.Sum("");
            sum.Should().Be(0);
        }

        [Fact]
        public void should_return_1_for_1_as_string()
        {
            delimetersServices.FillDelimeters(Arg.Any<string>())
                .Returns(new List<string> { "\n", ",", ";" });

            var sum = stringCalculator.Sum("1,2");
            sum.Should().Be(3);
        }

        [Fact]
        public void should_work_with_unknown_amount_of_numbers()
        {
            var random = new Random();

            delimetersServices.FillDelimeters(Arg.Any<string>())
                    .Returns(new List<string> { "\n", ",", ";" });

            var listInt = Enumerable.Range(0, random.Next(100))
                .Select(r => random.Next(10)).ToList();
            var expectSum = listInt.Sum();
            var str = string.Join(",", listInt.Select(x => x.ToString()).ToArray());

            var sum = stringCalculator.Sum(str);
            sum.Should().Be(expectSum);
        }

        [Fact]
        public void should_separete_use_user_symbols()
        {
            delimetersServices.FillDelimeters(Arg.Any<string>())
                 .Returns(new List<string> { "\n", ",", ";" });

            var sum = stringCalculator.Sum("1,2,3\n4");
            sum.Should().Be(10);
        }

        [Fact]
        public void should_return_error_if_koma_and_user_symbol_stand_together()
        {
            const string numberDChar = "//!\n1!,2!3";
            delimetersServices.FillDelimeters(Arg.Any<string>())
                 .Returns(new List<string> { "\n", ",", ";", "!" });
            //Assert.Throws<FormatException>(() => stringCalculator.Sum(numberDChar));
            Action act = () => stringCalculator.Sum(numberDChar);
            act.ShouldThrow<FormatException>();
        }

        [Fact]
        public void input_string_should_contain_separate_string_with_numbers()
        {
            const string numberDChar = "//!\n1!2!3";

            delimetersServices.FillDelimeters(Arg.Any<string>())
                .Returns(new List<string> { "\n", ",", ";", "!"});

            var sum = stringCalculator.Sum(numberDChar);
            sum.Should().Be(6);
        }

        [Fact]
        public void should_throw_exception_if_string_contain_negativ_numbers()
        {
            const string numbers = "1;-2;3;-9";

            delimetersServices.FillDelimeters(Arg.Any<string>())
                .Returns(new List<string> { "\n", ",", ";" });
            var exception = Assert.Throws<Exception>(() => stringCalculator.Sum(numbers));
            exception.Message.Should().Be("Negatives not allowed: -2, -9");
        }

        [Fact]
        public void numbers_more_then_1000_should_be_ignore()
        {
            delimetersServices.FillDelimeters(Arg.Any<string>())
                .Returns(new List<string> { "\n", ",", ";" });
            var sum = stringCalculator.Sum("1;2;1001");
            sum.Should().Be(3);
        }

        [Fact]
        public void should_work_with_delimiters_any_length()
        {
            const string str = "//[***]\n1***2***3";

            delimetersServices.FillDelimeters(Arg.Any<string>())
                .Returns(new List<string> { "\n", ",", ";", "***" });
            var sum = stringCalculator.Sum(str);
            sum.Should().Be(6);
        }

        [Fact]
        public void should_work_with_multiple_delimiter()
        {
            const string str = "//[*][%]\n1*2%3";
            delimetersServices.FillDelimeters(Arg.Any<string>())
                .Returns(new List<string> { "\n", ",", ";", "*","%" });
            var sum = stringCalculator.Sum(str);
            sum.Should().Be(6);
        }
    }
}
