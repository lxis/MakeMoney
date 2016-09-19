﻿using Algorithm;
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

namespace Manager
{
    public class Platform
    {
        public static async Task run()
        {
            ResultContainer.Instance.addOutput("开始");
            DateTime startTime = new DateTime(1990, 12, 19);
            DateTime endTime = new DateTime(2016, 8, 1);
            History history = await DataLoader.loadHistory();
            ResultContainer.Instance.addOutput("加载结束");
            history.quickDay = new QuickDay(history);
            ResultContainer.Instance.addOutput("转换结束");
            Holds holds = new Holds();
            holds.cash = 100000;
            DateTime lastDay = startTime.AddDays(-1);
            while (startTime < endTime)
            {
                history.quickDay.SetLimitDate(startTime);
                Operations operations = ZhihuAlgorithm.calcaulate(history.quickDay, startTime, holds);
                
                Exchange.ExchangeExecutor.Match(history, startTime, operations, holds);
                if (startTime.Year != lastDay.Year)
                {
                    ResultContainer.Instance.addOutput(startTime.ToString());
                    decimal currentMoney = calculateResult(startTime, history, holds);
                    DayResult marketResult = history.market.days.SingleOrDefault(c => c.Date == startTime);
                    if (currentMoney != -1 && marketResult != null)
                    {
                        lastDay = startTime;
                        ResultContainer.Instance.addYearResult(new YearResult { year = lastDay.Year, money = currentMoney, market = marketResult.adjClose });
                    }
                }
                startTime = startTime.AddDays(1);

            }
            //减3是因为截止到7月最后一天，+1就到了8月一日，而7月底的最后两天是假期
            calculateResult(endTime.AddDays(-3), history, holds);
            ResultContainer.Instance.addOutput("全部结束");
        }

        private static decimal calculateResult(DateTime endTime, History history, Holds holds)
        {
            decimal result = holds.cash;
            int nullCount = 0;
            foreach (var holdStock in holds.holds)
            {
                var day = history.quickDay.GetDay(holdStock.stockName, endTime);
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
