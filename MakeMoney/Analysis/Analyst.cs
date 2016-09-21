using Analysis.ShowFormat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analysis
{
    public class Analyst
    {
        public ObservableCollection<TimeShow> getTimesShowText()
        {
            List<Trade> trades = ResultContainer.Instance.getTrades();            
            Dictionary<int, TimeShow> timeShows = new Dictionary<int, TimeShow>();
            timeShows.Add(0, new TimeShow { 年 = "总共", 总次数 = trades.Count.ToString(), 每日次数 = "" });
            for (int i = 0; i < trades.Count; i++)
            {
                int currentYearTimes = 1;
                Trade currentTrade = trades[i];
                if (timeShows.ContainsKey(currentTrade.date.Year))
                {
                    continue;
                }
                for (int j = i+1; j < trades.Count; j++)
                {
                    if (trades[j].date.Year == currentTrade.date.Year)
                    {
                        currentYearTimes++;
                    }
                }
                if (currentYearTimes != 0) { }
                TimeShow timeShow = getTimeShow(currentYearTimes, currentTrade);
                timeShows.Add(currentTrade.date.Year, timeShow);
            }
            return new ObservableCollection<TimeShow>(timeShows.Values.ToList());
        }



        private static TimeShow getTimeShow(int currentYearTimes, Trade lastTrade)
        {
            return new TimeShow { 年 = lastTrade.date.Year.ToString() + "年", 总次数 = "交易" + currentYearTimes.ToString() + "次", 每日次数 = "每个交易日" + ((double)currentYearTimes / (double)250) + "次" };
        }

        public ObservableCollection<BenefitShow> getBenefitShowText()
        {
            ObservableCollection<BenefitShow> benefits = new ObservableCollection<BenefitShow>();
            List<YearResult> years = ResultContainer.Instance.getYearResults();
            for (int i = 1; i < years.Count; i++)
            {
                decimal currentMoney = years[i].money;
                decimal lastMoney = years[i - 1].money;
                decimal benefit = currentMoney - lastMoney;
                decimal benefitPercent = 0;
                if (lastMoney != 0)
                {
                    benefitPercent = benefit / lastMoney;
                }
                decimal currentMarket = years[i].market;
                decimal lastMarket = years[i - 1].market;
                decimal benefitMarket = currentMarket - lastMarket;
                decimal benefitMarketPercent = 0;
                if (lastMarket != 0)
                {
                    benefitMarketPercent = benefitMarket / lastMarket;
                }
                decimal benefitHedge = benefitPercent - benefitMarketPercent;
                string benefitPercentText = (benefitPercent * 100).ToString("f2") + "%";
                string benefitHedgePercentText = ((benefitPercent - benefitMarketPercent) * 100).ToString("f2") + "%";
                benefits.Add(new BenefitShow { 年 = years[i].year.ToString(), 钱 = currentMoney.ToString("f2"), 收益 = benefit.ToString("f2"), 收益百分比 = benefitPercentText, 对冲收益百分比 = benefitHedgePercentText });
            }
            return benefits;
        }        

        public ObservableCollection<FlowShow> getFlowShowText(int year, int month, int day)
        {
            ObservableCollection<FlowShow> flows = new ObservableCollection<FlowShow>();
            var trades = ResultContainer.Instance.getTrades();
            foreach (var trade in trades)
            {
                if (year !=0 && trade.date.Year != year)
                {
                    continue;
                }

                if (month != 0 && trade.date.Month != month)
                {
                    continue;
                }

                if (day != 0 && trade.date.Day != day)
                {
                    continue;
                }
                flows.Add(new FlowShow()
                {
                    日期 = trade.date.ToString(),
                    类型 = trade.type == TradeType.Buy ? "买" : "卖",
                    股票 = trade.name,
                    股价 = trade.price.ToString("f2"),
                    股数 = trade.amount.ToString("f2"),
                    费用 = trade.fee.ToString("f2"),
                    钱数 = trade.cash.ToString("f2")
                });
            }
            return flows;
        }
    }
}
