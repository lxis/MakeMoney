using Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MakeMoney
{
    /// <summary>
    /// AnalysisWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AnalysisWindow : Window
    {
        public AnalysisWindow()
        {
            InitializeComponent();
            analysisTimes.Click += (s, o) => showTimes();
            analysisBenefit.Click += (s, o) => showBenefit();
            analysisRisk.Click += (s, o) => showRisk();
        }

        private void showTimes()
        {
            List<Trade> trades = ResultContainer.Instance.getTrades();

            tradeResult.Text = "总共交易次数:" + trades.Count + Environment.NewLine;

            int currentYearTimes = 0;
            for (int i = 1; i < trades.Count; i++)
            {
                Trade currentTrade = trades[i];
                Trade lastTrade = trades[i - 1];

                if (currentTrade.date.Year != lastTrade.date.Year)
                {
                    tradeResult.Text += lastTrade.date.Year + "年，交易" + ++currentYearTimes + "次，每个交易日" + (double)currentYearTimes / (double)250 + "次" + Environment.NewLine;
                    currentYearTimes = 0;
                }
                else
                {
                    currentYearTimes++;
                }
            }
        }

        private void showBenefit()
        {
            tradeResult.Text = "";
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
                tradeResult.Text += years[i].year + "年，钱"+currentMoney.ToString("f2")+"，盈利" + benefit.ToString("f2") + "，百分比" + benefitPercentText + "对冲收益"+ benefitHedgePercentText + Environment.NewLine;
            }

        }

        private void showRisk()
        {

        }
    }
}
