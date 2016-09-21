using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analysis
{
    public class Trade
    {
        public TradeType type;
        public string name;
        public DateTime date;
        public decimal amount;
        public decimal price;
        public decimal cash;
        public decimal fee;
    }

    public enum TradeType
    {
        Buy,
        Sell
    }
}
