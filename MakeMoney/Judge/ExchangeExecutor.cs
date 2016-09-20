﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Algorithm;
using Manager.data;
using Analysis;

namespace Exchange
{
    // 交易所，负责撮合交易，需要尽量模拟真实情况。
    public class ExchangeExecutor
    {
        public static void Match(QuickDay quickDay, DateTime time, Operations operations, Holds holds)
        {
            foreach (Operation operation in operations.operations)
            {

                DayResult day = quickDay.GetDay(operation.StockName,time);
                if (day == null)
                {
                    continue;
                }
                decimal price = day.adjClose;
                if (operation.Type == OperationType.Buy)
                {
                    decimal cost = price * operation.amount;
                    if (holds.cash < cost)
                    {
                        continue;
                    }
                    holds.cash = holds.cash - cost;
                    bool isHeld = false;
                    foreach(Hold hold in holds.holds)
                    {
                        if(hold.stockName == operation.StockName)
                        {
                            isHeld = true;
                            hold.amount += operation.amount;
                            hold.buyTime = time;
                            hold.buyPrice = price;
                            ResultContainer.Instance.addTrade(new Trade() { name = hold.stockName, date = time, type = TradeType.Buy });
                            break;
                        }
                    }
                    if (!isHeld)
                    {
                        Hold hold = new Hold();
                        hold.stockName = operation.StockName;
                        hold.amount = operation.amount;
                        hold.buyTime = time;
                        hold.buyPrice = price;
                        ResultContainer.Instance.addTrade( new Trade() { name = hold.stockName, date = time, type = TradeType.Buy });
                        holds.holds.Add(hold);
                    }

                } else
                {
                    foreach (Hold hold in holds.holds)
                    {
                        if (hold.stockName == operation.StockName)
                        {
                            decimal soldAmount;
                            if (hold.amount > operation.amount)
                            {
                                soldAmount = operation.amount;
                                hold.amount = hold .amount- operation.amount;
                            } else
                            {
                                soldAmount = hold.amount;
                                hold.amount = 0;
                                holds.holds.Remove(hold);
                            }
                            decimal benefit = (price - hold.buyPrice) * soldAmount;
                            decimal percent = (price - hold.buyPrice) / hold.buyPrice;
                            ResultContainer.Instance.addTrade( new Trade() { name = hold.stockName, date = time, type = TradeType.Sell });
                            holds.cash += soldAmount * price;
                            break;
                        }
                    }
                }
            }
        }
        
    }
}
