using Database.data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class DataDownloader
    {
        public static async void run()
        {
            History history = new History();
            List<String> stockNames = getStockNameList();
            foreach(var stockName in stockNames)
            {
                string result = await downloadOnce(stockName);
                try
                {
                    Stock stock = getStock(result);
                    stock.name = stockName;
                    if (stock.days.Count == 0)
                    {
                        continue;
                    }
                    String stockJson = Newtonsoft.Json.JsonConvert.SerializeObject(stock);
                    var stream = File.CreateText(@"E:\Personal\Money\MakeMoney\Data\" + stock.name);
                    await stream.WriteAsync(stockJson);
                    stream.Dispose();
                    //history.days.Add(stock);
                }
                catch(Exception ex)
                {

                }
            }
        }

        private static Stock getStock(string result)
        {
            string[] lines = result.Split('\n');
            Stock dataList = new Stock();
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
                Day data = new Day();
                string[] items = line.Split(',');
                if (items.Length < 6)
                {
                    continue;
                }
                data.date = items[0];

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
                dataList.days.Add(data);
            }
            return dataList;
        }

        private static List<String> getStockNameList()
        {
            List<String> stockNames = new List<string>();
            stockNames.AddRange(getHuStockNameList());
            stockNames.AddRange(getShenStockNameList());
            return stockNames;
        }

        private static List<string> getShenStockNameList()
        {

            List<String> stockNames = new List<string>();
            for (int i = 0; i < 10000; i++)
            {
                String name = i.ToString();
                if (name.Length < 4)
                {
                    name = "0" + name;
                }
                name = "00" + name + ".SZ";
                stockNames.Add(name);
            }
            return stockNames;
        }

        private static List<string> getHuStockNameList()
        {
            List<String> stockNames = new List<string>();
            for (int i = 0;i<1000;i++)
            {
                String name = i.ToString();
                while (name.Length<3)
                {
                    name = "0" + name;
                }
                name = name + ".SS";
                stockNames.Add("600" + name);
                stockNames.Add("601" + name);
                stockNames.Add("603" + name);
            }
            return stockNames;
        }

        private static async Task<String> downloadOnce(string company)
        {
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
            var response = await client.GetAsync("http://ichart.yahoo.com/table.csv?s="+company+"&a=00&b=00&c=1900&d=31&e=7&f=2016&g=d");
            return await response.Content.ReadAsStringAsync();
        }
    }
}
