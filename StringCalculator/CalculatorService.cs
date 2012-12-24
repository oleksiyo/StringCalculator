using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringCalculator
{
    public class CalculatorService
    {
        public int CulculateSum(string str)
        {       
            List<int> list = new List<int>();
            var mas = str.Split(new char[] { ',' }, StringSplitOptions.None);

            for (int i = 0; i < mas.Length; i++)
            {
                int result = 0;
                int.TryParse(mas[i], out result);
                list.Add(result);
            }
            
            return list.Sum(x=>x);
        }

        public void SplitToInt(string str)
        {
            int result =0;
            var mas= str.Split(new char[] {','}, 0, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < mas.Length; i++)
            {
               int.TryParse(mas[i],out result);
            }
        }
    }
}
