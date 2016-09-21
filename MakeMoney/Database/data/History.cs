using Analysis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.data
{
    // 股票历史的HashMap结构
    public class History
    {
        // 时间限制，避免算法取到未来的股价来Cheat
        private DateTime limitDate;

        // 股票历史的HashMap结构存储
        public Dictionary<String, Dictionary<DateTime, DayResult>> stocks = new Dictionary<String, Dictionary<DateTime, DayResult>>();

        public KeyValuePair<String, Dictionary<DateTime, DayResult>> market;        
        

        // 获取某只股票某一天的交易结果
        public DayResult getDay(String stock, DateTime date)
        {
            if (limitDate < date)
            {
                return null;
            }
            if (!stocks.ContainsKey(stock))
            {
                return null;
            }
            var stockMap = stocks[stock];
            if (!stockMap.ContainsKey(date))
            {
                return null;
            }
            return stockMap[date];
        }

        // 设置显示日期
        public void SetLimitDate(DateTime startTime)
        {
            limitDate = startTime;
        }        

        public async Task load()
        {
            DirectoryInfo directory = new DirectoryInfo(Const.PATH);
            var files = directory.GetFiles();
            for (int i = 0; i < files.Count(); i++)
            {
                if (i>100)
                {
                    break;
                }
                var file = files[i];
                var fileReader = file.OpenText();
                string text = await fileReader.ReadToEndAsync();
                KeyValuePair<String, Dictionary<DateTime, DayResult>> stock = JsonConvert.DeserializeObject<KeyValuePair<String, Dictionary<DateTime, DayResult>>>(text);
                if (stock.Key == "000001.ss")
                {
                    market = stock;
                }
                else
                {
                    stocks.Add(stock.Key, stock.Value);
                }
                if (i % 100 == 0)
                {
                    ResultContainer.Instance.addOutput("已加载完" + i + "个");
                }
                fileReader.Close();                
            }
        }
    }
}
