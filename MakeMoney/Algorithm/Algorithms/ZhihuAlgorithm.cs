﻿using Algorithm.Algorithms;
using Manager.data;
using System;
using System.Collections.Generic;

namespace Algorithm
{
    public class ZhihuAlgorithm : IAlgorithm
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
                DayResult currentDay = history.getDay(stockName, date.AddDays(0));
                if (currentDay == null || currentDay.volume == 0)
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
