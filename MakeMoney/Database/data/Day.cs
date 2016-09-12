using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.data
{
    public class Day
    {
        public string date;
        public decimal open;
        public decimal high;
        public decimal low;
        public decimal close;
        public decimal volume;
        public decimal adjClose;

        public DateTime Date
        {
            get
            {
                String[] dayTexts = date.Split('-');
                return new DateTime(Convert.ToInt32(dayTexts[0]), Convert.ToInt32(dayTexts[1]), Convert.ToInt32(dayTexts[2]));
            }
        }
    }
}
