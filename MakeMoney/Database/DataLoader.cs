using Manager.data;
using Newtonsoft.Json;
using Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Analysis;

namespace Database
{
    // 从本地加载历史数据
    public class DataLoader
    {
        public static async Task<QuickDay> loadHistory()
        {
            QuickDay quickDay = new QuickDay();            
            DirectoryInfo directory = new DirectoryInfo(Const.PATH);
            var files = directory.GetFiles();
            for (int i = 0;i< files.Count();i++)
            {
                var fileReader = files[i].OpenText();                
                string text = await fileReader.ReadToEndAsync();
                KeyValuePair<string, Dictionary<DateTime, DayResult>> stock = JsonConvert.DeserializeObject<KeyValuePair<string, Dictionary<DateTime, DayResult>>>(text);
                if (stock.Key == "000001.ss")
                {
                    quickDay.market = stock;
                }
                else
                {
                    quickDay.stocks.Add(stock.Key, stock.Value);
                }
                if (i % 100 == 0)
                {
                    ResultContainer.Instance.addOutput("已加载完" + i + "个");
                }
            }
            return quickDay;
        }
    }
}
