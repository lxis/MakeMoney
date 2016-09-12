using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange
{
    // 购买费率的配置类
    class ExchangeSetting
    {
        public decimal yinHua = 0.001M;//印花只卖出收

        public decimal yongJin = 0.0003M;//佣金双向

        public decimal minYongJin = 5;//佣金最低5元

        public decimal guoHu = 1;//过户每1000股1元
    }
}
