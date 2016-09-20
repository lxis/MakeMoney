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
            ObservableCollection<TimeShow> times = new ObservableCollection<TimeShow>();
            List<Trade> trades = ResultContainer.Instance.getTrades();

            times.Add(new TimeShow { year = "总共", totalTimes = trades.Count.ToString(),  dailyTimes = ""});

            int currentYearTimes = 0;
            for (int i = 1; i < trades.Count; i++)
            {
                Trade currentTrade = trades[i];
                Trade lastTrade = trades[i - 1];

                if (currentTrade.date.Year != lastTrade.date.Year)
                {
                    times.Add(new TimeShow { year = lastTrade.date.Year.ToString() + "年", totalTimes = "交易" +(++currentYearTimes).ToString() + "次", dailyTimes = "每个交易日" + ((double)currentYearTimes / (double)250) + "次" });
                    currentYearTimes = 0;
                }
                else
                {
                    currentYearTimes++;
                }
            }
            return times;
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
    }
}
