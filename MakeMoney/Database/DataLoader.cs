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
        public static async Task<History> loadHistory()
        {
            History history = new History();            
            //System.Environment.CurrentDirectory "D:\\Personal\\MakeMoney\\Github\\MakeMoney\\MakeMoney\\MakeMoney\\bin\\Debug\\" 
            DirectoryInfo directory = new DirectoryInfo(Const.PATH);
            var files = directory.GetFiles();
            for (int i = 0;i< files.Count();i++)
            {
                //if (i > 10)
                //{
                //    break;
                //}
                var fileReader = files[i].OpenText();
                string text = await fileReader.ReadToEndAsync();
                Stock stock = JsonConvert.DeserializeObject<Stock>(text);
                if (stock.name == "000001.ss")
                {
                    history.market = stock;
                }
                else
                {
                    history.stocks.Add(stock);
                }
            }
            return history;
        }
    }
}
