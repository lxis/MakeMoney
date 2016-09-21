using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    class StockListProvider
    {
        public static List<string> getStockNameList()
        {
            List<string> stockNames = new List<string>();
            stockNames.Add("000001.ss");//沪指
            stockNames.AddRange(getHuStockNameList());
            stockNames.AddRange(getShenStockNameList());
            return stockNames;
        }

        private static List<string> getShenStockNameList()
        {

            List<String> stockNames = new List<string>();
            for (int i = 2000; i < 10000; i++)
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
            for (int i = 0; i < 1000; i++)
            {
                String name = i.ToString();
                while (name.Length < 3)
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
    }
}
