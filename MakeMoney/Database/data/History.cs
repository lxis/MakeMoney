using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.data
{
    // 全部股票的历史交易记录
    public class History
    {
        // 股票历史交易记录的List数据结构
        public List<Stock> stocks = new List<Stock>();

        // 股票历史交易记录的HashMap数据结构
        public QuickDay quickDay;
    }
}
