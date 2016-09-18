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

            analysis.Click += (s, o) => { showResult(); };

        }

        private void showResult()
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
                    tradeResult.Text += lastTrade.date.Year + "年，交易" + ++currentYearTimes + Environment.NewLine;
                    currentYearTimes = 0;

                }
                else
                {
                    currentYearTimes++;
                }
            }
        }
    }
}
