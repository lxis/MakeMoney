using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.data
{
    // 股票一天交易的结果
    public class DayResult
    {
        // 日期，String型
        public DateTime date;
        // 开盘价
        public decimal open;
        // 最高价
        public decimal high;
        // 最低价
        public decimal low;
        // 收盘价
        public decimal close;
        // 成交量
        public decimal volume;
        // 复权后的收盘价
        public decimal adjClose;
    }
}
