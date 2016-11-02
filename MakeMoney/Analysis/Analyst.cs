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
                for (int j = i + 1; j < trades.Count; j++)
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
            return new TimeShow { 年 = lastTrade.date.Year.ToString() + "年", 总次数 = currentYearTimes.ToString(), 每日次数 = ((double)currentYearTimes / (double)250).ToString() };
        }

        public ObservableCollection<BenefitShow> getBenefitShowText(int mode, int year)
        {
            if (mode == 0)
            {
                ObservableCollection<BenefitShow> benefits = new ObservableCollection<BenefitShow>();
                List<MonthResult> months = ResultContainer.Instance.getMonthResults();
                addBenefitShowFromMonthResult(benefits, months);
                return benefits;
            }
            else
            {
                ObservableCollection<BenefitShow> benefits = new ObservableCollection<BenefitShow>();
                List<MonthResult> months = ResultContainer.Instance.getMonthResults();
                Dictionary<int, MonthResult> yearsDic = new Dictionary<int, MonthResult>();
                foreach (var month in months)
                {
                    if (yearsDic.ContainsKey(month.year))
                    {
                        yearsDic[month.year] = month;
                    }
                    else
                    {
                        yearsDic.Add(month.year, month);
                    }
                }

                List<MonthResult> years = yearsDic.Values.ToList();
                addBenefitShowFromMonthResult(benefits, years);
                return benefits;
            }
        }

        private static void addBenefitShowFromMonthResult(ObservableCollection<BenefitShow> benefits, List<MonthResult> months)
        {
            int successBenefitCount = 0;
            int successHedgeBenefitCount = 0;
            decimal baseMoney = months[0].money;
            decimal baseMarkey = months[0].market;
            for (int i = 1; i < months.Count; i++)
            {
                decimal currentMoney = months[i].money;
                decimal lastMoney = months[i - 1].money;
                decimal benefit = currentMoney - lastMoney;
                decimal benefitPercent = 0;
                if (lastMoney != 0)
                {
                    benefitPercent = benefit / lastMoney;
                }
                decimal currentMarket = months[i].market;
                decimal lastMarket = months[i - 1].market;
                decimal benefitMarket = currentMarket - lastMarket;
                decimal benefitMarketPercent = 0;
                if (lastMarket != 0)
                {
                    benefitMarketPercent = benefitMarket / lastMarket;
                }
                decimal benefitHedge = benefitPercent - benefitMarketPercent;
                string benefitPercentText = (benefitPercent * 100).ToString("f2") + "%";
                string benefitHedgePercentText = ((benefitPercent - benefitMarketPercent) * 100).ToString("f2") + "%";
                if (benefitPercent > 0)
                {
                    successBenefitCount++;
                }
                if (benefitPercent > benefitMarketPercent)
                {
                    successHedgeBenefitCount++;
                }
                decimal averageBenefitPercent = (decimal)Math.Pow((double)(currentMoney - baseMoney), (double)1 / (double)months.Count);
                string averageBenefitPercentText = (averageBenefitPercent * 100).ToString("f2") + "%";
                decimal averageMarketBenefitPercent = (decimal)Math.Pow((double)(currentMarket - baseMoney), (double)1 / (double)months.Count);
                string averageMarketBenefitPercentText = (averageMarketBenefitPercent * 100).ToString("f2") + "%";
                benefits.Add(new BenefitShow { 年 = months[i].year.ToString(), 月 = months[i].month.ToString(), 钱 = currentMoney.ToString("f2"), 收益 = benefit.ToString("f2"), 收益百分比 = benefitPercentText, 对冲收益百分比 = benefitHedgePercentText, 历史收益平均百分比 = averageBenefitPercentText, 历史对冲收益平均百分比 = averageMarketBenefitPercentText });
            }
            benefits.Insert(0, new BenefitShow()
            {
                收益百分比 = ((decimal)successBenefitCount / (decimal)months.Count * 100).ToString("f2") + "%",
                对冲收益百分比 = ((decimal)successHedgeBenefitCount / (decimal)months.Count * 100).ToString("f2") + "%"
            });
        }

        public ObservableCollection<FlowShow> getFlowShowText(int year, int month, int day)
        {
            ObservableCollection<FlowShow> flows = new ObservableCollection<FlowShow>();
            var trades = ResultContainer.Instance.getTrades();
            int successCount = 0;
            foreach (var trade in trades)
            {
                if (year != 0 && trade.date.Year != year)
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
                    钱数 = trade.cash.ToString("f2"),
                    收益 = trade.type == TradeType.Sell ? (trade.benefitPercent * 100).ToString("f2") + "%" : ""
                });
                if (trade.benefitPercent > 0)
                {
                    successCount++;
                }
            }
            flows.Insert(0, new FlowShow()
            {
                收益 = "正收益占比" + ((decimal)successCount / (decimal)trades.Count * 100).ToString("f2") + "%"
            });
            return flows;
        }
    }
}
