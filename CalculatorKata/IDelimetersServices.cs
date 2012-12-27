using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorKata
{
    public interface IDelimetersServices
    {
        string GetStringOfDelimeters(string input);
        List<string> FillDelimeters(string input);
    }
}
