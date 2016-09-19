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
        private List<YearResult> mYearResults = new List<YearResult>();
        private List<String> mBenefits = new List<String>();
        private List<String> mRisks = new List<String>();


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

        public void addBenefit(String benefit)
        {
            mBenefits.Add(benefit);
        }

        public void addRisk(String risk)
        {
            mRisks.Add(risk);
        }

        public List<String> getBenefits()
        {
            return mBenefits;
        }

        public List<String> getRisks()
        {
            return mRisks;
        }
        
        
        public void addYearResult(YearResult yearResult)
        {
            mYearResults.Add(yearResult);            
        }

        public List<YearResult> getYearResults()
        {
            return mYearResults;
        }

    }
}
