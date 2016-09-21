using System;
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
        public static void Match(History quickDay, DateTime time, Operations operations, Holds holds)
        {
            foreach (Operation operation in operations.operations)
            {
                DayResult day = quickDay.getDay(operation.StockName, time);
                if (day == null)
                {
                    continue;
                }
                decimal price = day.adjClose;
                if (operation.Type == OperationType.Buy)
                {
                    buy(time, holds, operation, price);
                }
                else
                {
                    sell(time, holds, operation, price);
                }
            }
        }

        private static void sell(DateTime time, Holds holds, Operation operation, decimal price)
        {
            Hold hold = holds.holds.Single(c => c.stockName == operation.StockName);
            decimal soldAmount;
            if (hold.amount > operation.amount)
            {
                soldAmount = operation.amount;
                hold.amount = hold.amount - operation.amount;
            }
            else
            {
                soldAmount = hold.amount;
                hold.amount = 0;
                holds.holds.Remove(hold);
            }

            decimal sellCash = soldAmount * price;
            decimal fee = calculateSellFee(soldAmount, sellCash);
            ResultContainer.Instance.addTrade(new Trade()
            {
                name = hold.stockName,
                date = time,
                type = TradeType.Sell,
                amount = soldAmount,
                price = price,
                cash = sellCash,
                fee = fee
            });
            holds.cash += sellCash - fee;
        }

        private static decimal calculateSellFee(decimal soldAmount, decimal sellCash)
        {
            decimal yinhuashui = sellCash * (decimal)0.001;// 印花税
            decimal yongjin = sellCash * (decimal)0.0003;// 佣金
            if (yongjin < 5)
            {
                yongjin = 5;
            }
            decimal guohufei = Math.Floor(soldAmount / 1000);// 过户费
            decimal fee = yinhuashui + yongjin + guohufei;
            return fee;
        }

        private static void buy(DateTime time, Holds holds, Operation operation, decimal price)
        {
            decimal stockCost = price * operation.amount;
            decimal fee = calculateBuyFee(operation, stockCost);
            decimal totalCost = stockCost + fee;
            if (holds.cash < totalCost)
            {
                return;
            }
            holds.cash = holds.cash - totalCost;
            Hold currentHold = holds.holds.SingleOrDefault(c => c.stockName == operation.StockName);
            if (currentHold == null)
            {
                currentHold = new Hold();
                currentHold.stockName = operation.StockName;
                holds.holds.Add(currentHold);
            }
            currentHold.amount += operation.amount;
            currentHold.buyTime = time;
            currentHold.buyPrice = price;
            ResultContainer.Instance.addTrade(new Trade()
            {
                name = currentHold.stockName,
                date = time,
                type = TradeType.Buy,
                amount = operation.amount,
                price = price,
                fee = fee,
                cash = stockCost
            });
        }

        private static decimal calculateBuyFee(Operation operation, decimal cost)
        {
            decimal yongjin = cost * (decimal)0.0003;// 佣金
            if (yongjin < 5)
            {
                yongjin = 5;
            }
            decimal guohufei = Math.Floor((decimal)operation.amount / 1000);// 过户费
            decimal fee = yongjin + guohufei;
            return fee;
        }
    }
}
