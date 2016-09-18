using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analysis
{
    public class ResultContainer
    {
        private static ResultContainer mInstance = new ResultContainer();

        public static ResultContainer Instance
        {
            get
            {
                return mInstance;
            }
        }

        private Action<String> mOutputHandler;

        private ResultContainer()
        {

        }

        private List<Trade> mTrade = new List<Trade>();


        public void addOutput(String output)
        {
            mOutputHandler.Invoke(DateTime.Now + ":" + output);
        }

        public void addOutputHandler(Action<String> outputHandler)
        {
            mOutputHandler = outputHandler;
        }

        public void addTrade(Trade trade)
        {
            mTrade.Add(trade);
        }

        public List<Trade> getTrades()
        {
            return mTrade;
        }

    }
}
