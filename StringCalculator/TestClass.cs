using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;

namespace StringCalculator
{
    public class TestClass2
    {
        [Fact]
        public void FailedTest1()
        {
            Assert.True(false);
        }

        [Fact]
        public void FailedTest2()
        {
            throw new InvalidOperationException();
        }

      /*  [RunAfterTestFailed]
        public void TestFailed()
        {
            Debug.WriteLine("Run this whenever a test fails");
        }*/
    }
}
