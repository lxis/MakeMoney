using Manager.data;
using Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;
using System.Net.Http;

namespace Manager
{
    // 从雅虎下载数据
    public class DataDownloader
    {
        public static async void run()
        {
            List<string> stockNames = StockListProvider.getStockNameList();
            foreach(var stockName in stockNames)
            {
                string result = await downloadOnce(stockName);
                try
                {
                    KeyValuePair<string, Dictionary<DateTime, DayResult>> stock = getStock(stockName,result);
                    if (stock.Value.Count == 0)
                    {
                        continue;
                    }
                    String stockJson = Newtonsoft.Json.JsonConvert.SerializeObject(stock);
                    var stream = File.CreateText(Const.PATH + stockName);
                    await stream.WriteAsync(stockJson);
                    stream.Dispose();
                }
                catch(Exception)
                {

                }
            }
        }

        private static KeyValuePair<string, Dictionary<DateTime, DayResult>> getStock(string stockName, string result)
        {
            string[] lines = result.Split('\n');
            KeyValuePair<string, Dictionary<DateTime, DayResult>> dataList = new KeyValuePair<string, Dictionary<DateTime, DayResult>>(stockName, new Dictionary<DateTime, DayResult>());
            for (int i = 0; i < lines.Count(); i++)
            {
                if (i == 0)
                {
                    continue;
                }
                string line = lines[i];
                if (String.IsNullOrEmpty(line))
                {
                    continue;
                }
                DayResult data = new DayResult();
                string[] items = line.Split(',');
                if (items.Length < 6)
                {
                    continue;
                }

                String[] dayTexts = items[0].Split('-');
                data.date = new DateTime(Convert.ToInt32(dayTexts[0]), Convert.ToInt32(dayTexts[1]), Convert.ToInt32(dayTexts[2]));

                try
                {
                    data.open = Convert.ToDecimal(items[1]);
                    data.high = Convert.ToDecimal(items[2]);
                    data.low = Convert.ToDecimal(items[3]);
                    data.close = Convert.ToDecimal(items[4]);
                    data.volume = Convert.ToDecimal(items[5]);
                    data.adjClose = Convert.ToDecimal(items[6]);
                }
                catch(Exception)
                {
                    continue;
                }
                dataList.Value.Add(data.date, data);
            }
            return dataList;
        }

        private static async Task<string> downloadOnce(string company)
        {
            var response = await new HttpClient().GetAsync("http://ichart.yahoo.com/table.csv?s="+company+"&a=00&b=00&c=1900&d=00&e=00&f=2020&g=d");
            return await response.Content.ReadAsStringAsync();
        }
    }
}
