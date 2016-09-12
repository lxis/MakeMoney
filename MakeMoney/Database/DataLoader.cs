using Database.data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class DataLoader
    {
        public static History loadHistory()
        {
            History history = new History();
            DirectoryInfo directory = new DirectoryInfo(@"E:\Personal\Money\MakeMoney\Data\");
            var files = directory.GetFiles();
            for (int i = 0;i< files.Count();i++)
            {

                var fileReader = files[i].OpenText();
                string text = fileReader.ReadToEnd();
                Stock stock = JsonConvert.DeserializeObject<Stock>(text);
                history.stocks.Add(stock);
            }
            return history;
        }
    }
}
