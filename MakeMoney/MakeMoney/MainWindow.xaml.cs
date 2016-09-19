
using Analysis;
using Manager;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MakeMoney
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ResultContainer.Instance.addOutputHandler((s) => text.Text = s + Environment.NewLine + text.Text);
            startButton.Click += (s, o) =>
            {
                click();
            };
            //DataDownloader.run();
            analysisButton.Click += (s, o) =>
            {
                navigateToAnalysis();
            };
        }

        private void navigateToAnalysis()
        {
            new AnalysisWindow().Show();
                
        }

        private async void click()
        {
            await Manager.Platform.run();

            //showTrades();
        }

        private void showTrades()
        {
            List<Trade> trades = ResultContainer.Instance.getTrades();
            trades.ForEach((s)=> 
            {
                text.Text = s.name + Environment.NewLine + text.Text;
            });
            
        }
    }
}
