using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runners.Tests
{
    public class Calculator
    {        
        public void AddValue(int value)
        {
            Total += value;
        }
        public void SubtractValue(int i)
        {
            Total -= i;
        }

        public void Clear()
        {
            Total = 0;
        }

        public int Total { get; set; }

    }
}
