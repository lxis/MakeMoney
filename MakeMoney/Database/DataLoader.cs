using Platform.data;
using Newtonsoft.Json;
using Platform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Database
{
    // 从本地加载历史数据
    public class DataLoader
    {
        public static History loadHistory()
        {
            History history = new History();            
            //System.Environment.CurrentDirectory "D:\\Personal\\MakeMoney\\Github\\MakeMoney\\MakeMoney\\MakeMoney\\bin\\Debug\\" 
            DirectoryInfo directory = new DirectoryInfo(Const.PATH);
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
