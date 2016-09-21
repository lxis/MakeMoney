using Manager.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.Algorithms
{
    // 对算法进行抽象，算法的职责就是根据历史和当前的用户情况决定操作。
    public interface IAlgorithm
    {
        Operations calcaulate(History history, DateTime date, Holds holds);
    }
}
