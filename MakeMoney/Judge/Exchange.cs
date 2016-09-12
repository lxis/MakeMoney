using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Algorithm;
using Manager.data;

namespace Judge
{
    // 交易所，负责撮合交易，需要尽量模拟真实情况。
    public class Exchange
    {
        public static void Match(History history, DateTime time, Operations operations, Holds holds)
        {
            foreach (Operation operation in operations.operations)
            {

                DayResult day = history.quickDay.GetDay(operation.StockName,time);
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
                            //Console.WriteLine("Buy:" + hold.stockName + "," + operation.amount + "," + time);
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
                        //Console.WriteLine("Buy:" + hold.stockName + "," + hold.amount + "," + time);
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
                            //Console.WriteLine("Sell:" + hold.stockName + "," +  Convert.ToInt32(soldAmount) + "," + hold.buyTime + "," + time + "," + Convert.ToInt32(benefit) + "，"+ price + "，" + hold.buyPrice + "，"  + percent);
                            holds.cash += soldAmount * price;
                            break;
                        }
                    }
                }
            }
        }
        
    }
}
