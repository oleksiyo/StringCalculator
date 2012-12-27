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

            var substring = listDelimeters.Any(prefix => stringNumbers.StartsWith(Conf.start)) ?
                GetSubString(stringNumbers, Conf.end, "") :
                stringNumbers;

            var numbers = SplitStringByDelimeters(substring, listDelimeters);
            CheckContainNegativNambers(numbers);

            return CalculateSum(numbers);
        }

        private List<int> SplitStringByDelimeters(string str, List<string> listDelimeters)
        {
            return str.Split(listDelimeters.ToArray(), StringSplitOptions.None).ToList().ConvertAll(int.Parse);
        }

        private string GetSubString(string input, string strStart, string strEnd)
        {
            var startIndex = input.IndexOf(strStart, StringComparison.Ordinal);
            var endIndex = String.IsNullOrEmpty(strEnd) ? input.Length : input.IndexOf(strEnd, StringComparison.Ordinal);
            return input.Substring(startIndex + 1, endIndex - startIndex - 1);
        }

        private int CalculateSum(IEnumerable<int> numbers)
        {
            return numbers.Select(x => x).Where(z => z >= 0 && z <= 1000).Sum();
        }

        private void CheckContainNegativNambers(IEnumerable<int> numbers)
        {
            var listNegativs = numbers.Select(x => x).Where(x => x < 0).ToList();

            if (listNegativs.Any())
                throw new Exception(Conf.exceptionMsg + " " + string.Join(", ", listNegativs));
        }
    }

    public class StringCalculatorTests
    {
        private StringCalculator stringCalculator;

        public StringCalculatorTests()
        {
            IDelimetersServices delimetersServices = new DelimetersServices();
            stringCalculator = new StringCalculator(delimetersServices);
        }

        #region Calculation rules

        [Fact]
        public void should_return_0_for_empty_string()
        {
            var sum = stringCalculator.Sum("");
            sum.Should().Be(0);
        }

        [Fact]
        public void should_return_1_for_1_as_string()
        {
            var sum = stringCalculator.Sum("1,2");
            sum.Should().Be(3);
        }

        [Fact]
        public void should_work_with_unknown_amount_of_numbers()
        {
            var random = new Random();

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
            var sum = stringCalculator.Sum("1,2,3\n4");
            sum.Should().Be(10);
        }

        [Fact]
        public void should_return_error_if_koma_and_user_symbol_stand_together()
        {
            const string numberDChar = "//!\n1!,2!3";

            //Assert.Throws<FormatException>(() => stringCalculator.Sum(numberDChar));
            Action act = () => stringCalculator.Sum(numberDChar);
            act.ShouldThrow<FormatException>();

        }

        [Fact]
        public void input_string_should_contain_separate_string_with_numbers()
        {
            const string numberDChar = "//!\n1!2!3";

            var sum = stringCalculator.Sum(numberDChar);
            sum.Should().Be(6);
        }

        [Fact]
        public void should_throw_exception_if_string_contain_negativ_numbers()
        {
            const string numbers = "1;-2;3;-9";

            var exception = Assert.Throws<Exception>(() => stringCalculator.Sum(numbers));
            exception.Message.Should().Be("Negatives not allowed: -2, -9");
        }

        [Fact]
        public void numbers_more_then_1000_should_be_ignore()
        {
            var sum = stringCalculator.Sum("1;2;1001");
            sum.Should().Be(3);
        }

        [Fact]
        public void should_work_with_delimiters_any_length()
        {
            const string str = "//[***]\n1***2***3";

            var sum = stringCalculator.Sum(str);
            sum.Should().Be(6);
        }

        [Fact]
        public void should_work_with_multiple_delimiter()
        {
            const string str = "//[*][%]\n1*2%3";
            var sum = stringCalculator.Sum(str);
            sum.Should().Be(6);
        }

        #endregion


        #region Test Private Methods

        [Fact]
        public void TEST()
        {
            const string str = "//[***]\n1***2***3";
            var delimetersServices = Substitute.For<IDelimetersServices>();
            delimetersServices.FillDelimeters(Arg.Any<string>())
                .Returns(new List<string> { "\n", ",", ";", "***" });

            var calculator = new StringCalculator(delimetersServices);
            var sum = calculator.Sum(str);
            sum.Should().Be(6);
        }

        #endregion

    }
}
