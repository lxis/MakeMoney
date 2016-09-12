using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.data
{
    public class QuickDay
    {
        private DateTime limitDate;

        public Dictionary<String, Dictionary<DateTime, Day>> stocks = new Dictionary<String, Dictionary<DateTime, Day>>();
        

        public QuickDay(History history)
        {
            foreach (var stock in history.stocks)
            {
                Dictionary<DateTime, Day> stockDic = new Dictionary<DateTime, Day>();
                stocks.Add(stock.name, stockDic); 
                foreach (var day in stock.days)
                {
                    stockDic.Add(day.Date, day);
                }
            }
        }

        public Day getDay(String stock, DateTime date)
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

        public void SetLimitDate(DateTime startTime)
        {
            limitDate = startTime;
        }
    }
}
