using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.data
{
    // 股票数据
    public class Stock
    {
        // 股票名
        public string name;
        // 股票每天的交易记录
        public List<DayResult> days = new List<DayResult>();
    }
}
