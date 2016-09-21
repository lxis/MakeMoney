using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    // 用户当前的一个持股状态
    public class Hold
    {
        public String stockName;
        public decimal amount;
        // 不必须
        public DateTime buyTime;
        public decimal buyPrice;
    }
}
