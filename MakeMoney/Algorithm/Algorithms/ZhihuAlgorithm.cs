using Manager.data;
using System;
using System.Collections.Generic;

namespace Algorithm
{
    public class ZhihuAlgorithm
    {
        public static Operations calcaulate(QuickDay history, DateTime date, Holds holds)
        {
            Operations operations = new Operations();
            foreach (Hold hold in holds.holds)
            {
                var day = history.GetDay(hold.stockName, date);
                if (day == null)
                {
                    continue;
                }
                if (hold.buyTime.AddDays(10) > date)
                {
                    continue;
                }
                operations.operations.Add(sell(hold));
            }


            foreach (var stockName in history.stocks.Keys)
            {
                DayResult currentDay = history.GetDay(stockName, date.AddDays(0));
                if (currentDay == null || currentDay.volume == 0)
                {
                    continue;
                }
                DateTime currentDate = currentDay.Date;
                    operations.operations.Add(buy(stockName, currentDay.adjClose));
            }
            return operations;
        }

        private static Operation buy(string stockName, decimal currentPrice)
        {
            Operation operation = new Operation();
            operation.StockName = stockName;
            operation.amount = 1000 / currentPrice;
            operation.Type = OperationType.Buy;
            return operation;
        }

        private static Operation sell(Hold hold)
        {
            Operation operation = new Operation();
            operation.StockName = hold.stockName;
            operation.amount = hold.amount;
            operation.Type = OperationType.Sell;
            return operation;
        }
    }
}
