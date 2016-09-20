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
            analysisTimes.Click += (s, o) => showTimes();
            analysisBenefit.Click += (s, o) => showBenefit();
        }

        private void showTimes()
        {
            tradeResultGrid.DataContext = new Analyst().getTimesShowText();
            
        }

        private void showBenefit()
        {
            tradeResultGrid.DataContext = new Analyst().getBenefitShowText();
        }        
    }
}
