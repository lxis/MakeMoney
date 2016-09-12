using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.data
{
    // 股票历史的HashMap结构
    public class QuickDay
    {
        // 时间限制，避免算法取到未来的股价来Cheat
        private DateTime limitDate;

        // 股票历史的HashMap结构存储
        public Dictionary<String, Dictionary<DateTime, DayResult>> stocks = new Dictionary<String, Dictionary<DateTime, DayResult>>();
        

        public QuickDay(History history)
        {
            foreach (var stock in history.stocks)
            {
                Dictionary<DateTime, DayResult> stockDic = new Dictionary<DateTime, DayResult>();
                stocks.Add(stock.name, stockDic); 
                foreach (var day in stock.days)
                {
                    stockDic.Add(day.Date, day);
                }
            }
        }

        // 获取某只股票某一天的交易结果
        public DayResult GetDay(String stock, DateTime date)
        {
            if (limitDate < date)
            {
                return null;
            }
            if (!stocks.ContainsKey(stock))
            {
                return null;
            }
            var stockMap = stocks[stock];
            if (!stockMap.ContainsKey(date))
            {
                return null;
            }
            return stockMap[date];
        }

        // 设置显示日期
        public void SetLimitDate(DateTime startTime)
        {
            limitDate = startTime;
        }
    }
}
