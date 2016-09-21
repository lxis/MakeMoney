using Analysis;
using Analysis.ShowFormat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            analysisTimes.Click += (s, o) => tradeResultGrid.DataContext = new Analyst().getTimesShowText();
            analysisBenefit.Click += (s, o) => tradeResultGrid.DataContext = new Analyst().getBenefitShowText();
            analysisFlow.Click += (s, o) =>
            {
                int year = getIntFromString(yearSelect.Text);
                int month = getIntFromString(monthSelect.Text);
                int day = getIntFromString(daySelect.Text);
                tradeResultGrid.DataContext = new Analyst().getFlowShowText(year, month, day);
            };
        }

        private int getIntFromString(String text)
        {
            if (String.IsNullOrEmpty(text)){
                return 0;
            } else
            {
                return Convert.ToInt32(text);
            }

        }
    }
}
