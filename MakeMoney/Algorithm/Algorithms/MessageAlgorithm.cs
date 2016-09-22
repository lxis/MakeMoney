using Algorithm.Algorithms;
using Manager.data;
using System;
using System.Collections.Generic;

namespace Algorithm
{
    public class MessageAlgorithm : IAlgorithm
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
                decimal currentPrice = day.adjClose;
                if (currentPrice > hold.buyPrice * (decimal)1.1 || currentPrice < hold.buyPrice * (decimal)0.9)
                {
                    operations.operations.Add(sell(hold));
                }
            }
            foreach (var stockName in history.stocks.Keys)
            {
                List<DayResult> days = new List<DayResult>();
                DayResult currentDay = history.getDay(stockName, date.AddDays(0));
                if (currentDay == null || currentDay.volume == 0)
                {
                    continue;
                }
                for (int i = 1; i < 50; i++)
                {
                    DayResult day = history.getDay(stockName, date.AddDays(-i));
                    if (day != null && day.volume != 0)
                    {
                        days.Add(day);
                    }
                }
                if (days.Count < 20)
                {
                    continue;
                }
                bool isNotMatch = false;
                foreach (DayResult day in days)
                {
                    if (day.volume > currentDay.volume / 3)
                    {
                        isNotMatch = true;
                    }
                    if (day.adjClose > currentDay.adjClose)
                    {
                        isNotMatch = true;
                    }
                    if (day.adjClose < currentDay.adjClose * (decimal)0.7)
                    {
                        isNotMatch = true;
                    }
                }
                if (currentDay.adjClose * currentDay.volume < 10000000)
                {
                    isNotMatch = true;
                }
                if (isNotMatch)
                {
                    continue;
                }
                operations.operations.Add(buy(stockName, currentDay.adjClose));
            }
            return operations;
        }

        private static Operation buy(string stockName, decimal currentPrice)
        {
            Operation operation = new Operation();
            operation.StockName = stockName;
            int hand = (int)(1000 / currentPrice / 100);
            operation.amount = hand * 100;
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
