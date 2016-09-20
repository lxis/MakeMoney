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
    public class QuickDay
    {
        // 时间限制，避免算法取到未来的股价来Cheat
        private DateTime limitDate;

        // 股票历史的HashMap结构存储
        public Dictionary<String, Dictionary<DateTime, DayResult>> stocks = new Dictionary<String, Dictionary<DateTime, DayResult>>();

        public KeyValuePair<String, Dictionary<DateTime, DayResult>> market;

        public QuickDay(History history)
        {
            foreach (var stock in history.stocks)
            {
                Dictionary<DateTime, DayResult> stockDic = new Dictionary<DateTime, DayResult>();
                stocks.Add(stock.name, stockDic); 
                foreach (var day in stock.days)
                {
                    stockDic.Add(day.Date, day);
                }
            }

            market = new KeyValuePair<string, Dictionary<DateTime, DayResult>>("000001.ss", new Dictionary<DateTime, DayResult>());
            foreach (var day in history.market.days)
            {
                market.Value.Add(day.Date, day);
            }
        }

        public QuickDay()
        {

        }

        // 获取某只股票某一天的交易结果
        public DayResult GetDay(String stock, DateTime date)
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

        public async Task write()
        {
            for(int i = 0;i<stocks.Keys.Count;i++)
            {
                var item = stocks.ElementAt(i);
                await writeSingle(item);
            }
            await writeSingle(market);
        }

        private async Task writeSingle(KeyValuePair<string, Dictionary<DateTime, DayResult>> item)
        {
            string stockJson = Newtonsoft.Json.JsonConvert.SerializeObject(item);
            var stream = File.CreateText(Const.QuickPATH + item.Key);
            await stream.WriteAsync(stockJson);
            stream.Dispose();
        }

        public async Task load()
        {
            DirectoryInfo directory = new DirectoryInfo(Const.QuickPATH);
            var files = directory.GetFiles();
            for (int i = 0; i < files.Count(); i++)
            {
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
            }
        }
    }
}
