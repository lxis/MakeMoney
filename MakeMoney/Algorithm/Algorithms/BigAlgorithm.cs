using System;
using Manager.data;

namespace Algorithm.Algorithms
{
    //选取比较交易量的股票持有
    public class BigAlgorithm : IAlgorithm
    {
        public Operations calcaulate(History history, DateTime date, Holds holds)
        {
            Operations operations = new Operations();
            foreach (Hold hold in holds.holds)
            {
                var day = history.getDay(hold.stockName, date);
                if (day == null)
                {
                    continue;
                }
                if (hold.buyTime.AddDays(20) > date)
                {
                    continue;
                }
                operations.operations.Add(sell(hold));
            }


            foreach (var stockName in history.stocks.Keys)
            {
                DayResult currentDay = history.getDay(stockName, date);
                if (currentDay == null || currentDay.volume == 0 || currentDay.volume * currentDay.adjClose < 100000000)
                {
                    continue;
                }
                DateTime currentDate = currentDay.date;
                operations.operations.Add(buy(stockName, currentDay.adjClose, holds.cash));

            }
            return operations;
        }

        private static Operation buy(string stockName, decimal currentPrice, decimal currentCash)
        {
            Operation operation = new Operation();
            operation.StockName = stockName;
            operation.amount = (int)(50000 / currentPrice);
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
