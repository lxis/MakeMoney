using Algorithm;
using Manager;
using Manager.data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;
using Analysis;
using Algorithm.Algorithms;

namespace Manager
{
    public class Platform
    {
        public static async Task run()
        {
            ResultContainer.Instance.addOutput("开始");
            //DateTime startTime = new DateTime(1990, 12, 19);
            //DateTime endTime = new DateTime(2016, 8, 1);
            DateTime startTime = new DateTime(2006, 1, 1);
            DateTime endTime = new DateTime(2016, 1, 1);
            History quickDay = new History();
            await quickDay.load();
            Holds holds = new Holds();
            holds.cash = 100000;
            DateTime lastDay = startTime.AddDays(-1);
            while (startTime < endTime)
            {
                quickDay.SetLimitDate(startTime);
                Operations operations = new ZhihuAlgorithm().calcaulate(quickDay, startTime, holds);
                
                Exchange.ExchangeExecutor.Match(quickDay, startTime, operations, holds);
                if (startTime.Year != lastDay.Year || startTime.Month != lastDay.Month)
                {
                    ResultContainer.Instance.addOutput(startTime.ToString());
                    decimal currentMoney = calculateResult(startTime, quickDay, holds);
                    DayResult marketResult = quickDay.market.Value.SingleOrDefault(c => c.Key == startTime).Value;
                    if (currentMoney != -1 && marketResult != null)
                    {
                        lastDay = startTime;
                        ResultContainer.Instance.addMonthResult(new MonthResult { year = lastDay.Year, month = lastDay.Month, money = currentMoney, market = marketResult.adjClose });
                    }
                }
                startTime = startTime.AddDays(1);

            }
            //减3是因为截止到7月最后一天，+1就到了8月一日，而7月底的最后两天是假期
            calculateResult(endTime, quickDay, holds);
            ResultContainer.Instance.addOutput("全部结束");
        }

        private static decimal calculateResult(DateTime endTime, History  quickDay, Holds holds)
        {
            decimal result = holds.cash;
            int nullCount = 0;
            foreach (var holdStock in holds.holds)
            {
                var day = quickDay.getDay(holdStock.stockName, endTime);
                if (day == null)
                {
                    nullCount++;
                    continue; ;
                }
                result += day.adjClose * holdStock.amount;
            }            
            ResultContainer.Instance.addOutput(result.ToString());
            if (nullCount * 2 > holds.holds.Count)
            {
                return -1;
            } else
            {
                return result;
            }
            
        }
    }
}
