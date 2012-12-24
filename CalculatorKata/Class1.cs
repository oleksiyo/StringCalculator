using System;
using System.Linq;
using Xunit;

namespace CalculatorKata
{
    public class StringCalculator
    {
        public int Sum(string s, string symbol)
        {
            if (String.IsNullOrEmpty(s))
                return 0;
            var strings = s.Split(new[] { ",", symbol }, StringSplitOptions.None);
            return strings.Select(int.Parse).Sum();
        }
    }

    public class StringCalculatorTests
    {
        [Fact]
        public void should_return_0_for_empty_string()
        {
            var stringCalculator = new StringCalculator();
            var sum = stringCalculator.Sum("", null);
            Assert.Equal(0, sum);
        }

        [Fact]
        public void should_return_1_for_1_as_string()
        {
            var stringCalculator = new StringCalculator();
            var sum = stringCalculator.Sum("1,2", null);
            Assert.Equal(3, sum);
        }

        [Fact]
        public void should_work_with_unknown_amount_of_numbers()
        {
            var random = new Random();
            var stringCalculator = new StringCalculator();

            var listInt = Enumerable.Range(0, random.Next(100))
                .Select(r => random.Next(10)).ToList();
            var expectSum = listInt.Sum();
            var str = string.Join(",", listInt.Select(x => x.ToString()).ToArray());

            var sum = stringCalculator.Sum(str, null);
            Assert.Equal(expectSum, sum);
        }

        [Fact]
        public void should_separete_use_user_symbols()
        {
            var stringCalculator = new StringCalculator();
            var sum = stringCalculator.Sum("1,2,3!4", "!");
            Assert.Equal(10,sum);

            var sum2 = stringCalculator.Sum("1,2,3\n4", "\n");
            Assert.Equal(10, sum);

        }

        [Fact]
        public void should_return_error_if_koma_and_user_symbol_stand_together()
        {
            var stringCalculator = new StringCalculator();
            var sum = stringCalculator.Sum("1,2,3,!4", "!");
            Assert.Equal(10,sum);
            ////И

        }
    }
}
