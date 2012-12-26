using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StringCalculator;
using Xunit;

namespace StringCalculatorTest
{
   public class CulculatorServiceTest
    {
       readonly CalculatorService serv;
       private CalculatorService calculatorService;

       public CulculatorServiceTest()
       {
           serv = new CalculatorService();
       }

       [Fact]
       public void If_String_Is_Empty_Return_0()
       {
           var sum = serv.CulculateSum("");
           Assert.Equal(0, sum);
       }

       [Fact]
       public void Must_Be_Calculate_Only_Int()
       {
           const string str = "1,2,qwe";
           var sum = serv.CulculateSum(str);
           Assert.Equal(3, sum);
       }
    }
}
