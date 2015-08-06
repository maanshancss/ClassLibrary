using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Increasing
{
    /// <summary>
    /// BasePhaseIncreaseAccesser 当一个Round是由N天构成时，可以借助BasePhaseIncreaseAccesser简化IPhaseIncreaseAccesser的实现。
    /// </summary>    
    public abstract class BasePhaseIncreaseAccesser<TSourceToken, TKey, TObject> : IPhaseIncreaseAccesser<TSourceToken, int, TKey, TObject>
    {
        #region Property
        #region CurrentRoundStartTime
        private DateTime currentRoundStartTime = DateTime.Now;
        public DateTime CurrentRoundStartTime
        {
            get { return currentRoundStartTime; }
        }
        #endregion

        #region DayOriginTime
        private ShortTime dayOriginTime = new ShortTime(0, 0, 0);
        public ShortTime DayOriginTime
        {
            get { return dayOriginTime; }
            set { dayOriginTime = value; }
        }
        #endregion

        #region ManyDaysInOneRound
        private int manyDaysInOneRound = 1;
        public int ManyDaysInOneRound
        {
            get { return manyDaysInOneRound; }
            set
            {
                if (value < 1)
                {
                    throw new Exception("The value of Property [ManyDaysInOneRound] must be greater than 1.");
                }
                manyDaysInOneRound = value;
            }
        }
        #endregion

        #region TodayIsFirstDay
        private bool todayIsFirstDay = false;
        /// <summary>
        /// TodayIsFirstDay 是将启动时刻作为当前Round的第一天还是最后一天？
        /// </summary>
        public bool TodayIsFirstDay
        {
            get { return todayIsFirstDay; }
            set { todayIsFirstDay = value; }
        }
        #endregion 

        #region SourceTokenList
        private IList<TSourceToken> sourceTokenList = new List<TSourceToken>();
        /// <summary>
        /// SourceTokenList 数据源的标志列表。至少需要有一个数据源标志。
        /// </summary>
        public IList<TSourceToken> SourceTokenList
        {           
            set { sourceTokenList = value; }
        } 
        #endregion

        #endregion

        #region Initialize
        /// <summary>
        /// Initialize 如果派生类override该方法，则在实现时必须先调用base.Initialize()方法。
        /// </summary>
        public virtual void Initialize()
        {
            if (this.sourceTokenList == null || this.sourceTokenList.Count == 0)
            {
                throw new Exception("There is no any SourceToken object in SourceTokenList !");
            }

            DateTime startTime = DateTime.Now;
            DateTime now = DateTime.Now;
            if (this.dayOriginTime.CompareTo(new ShortTime(now)) <= 0)
            {
                startTime = this.dayOriginTime.GetDateTime(now.Year, now.Month, now.Day);
            }
            else
            {
                DateTime yesterday = now.AddDays(-1);
                startTime = this.dayOriginTime.GetDateTime(yesterday.Year, yesterday.Month, yesterday.Day);
            }

            this.currentRoundStartTime = this.todayIsFirstDay ? startTime : startTime.AddDays(1 - this.manyDaysInOneRound);
        } 
        #endregion

        protected abstract TKey GetMaxKey(DateTime endTime, TSourceToken token, bool containsEndTime);        
        public abstract IList<TObject> Retrieve(TSourceToken token, TKey maxKeyOfPrePhase, TKey maxKeyOfThisPhase);

        #region IPhaseIncreaseAccesser<TSourceToken,int,TKey,TObject> 成员

        public TKey GetMaxKey(DateTime now, TSourceToken token)
        {
            return this.GetMaxKey(now, token, true);
        }

        /// <summary>
        /// GetMaxKeyBefore 获取endTime之前的最大的Key。注意TimeColumn要小于endTime，不能等于。
        /// </summary>      
        protected IDictionary<TSourceToken, TKey> GetMaxKeyBefore(DateTime endTime)
        {
            Dictionary<TSourceToken, TKey> dic = new Dictionary<TSourceToken, TKey>();
            foreach (TSourceToken token in this.sourceTokenList)
            {
                TKey maxKey = this.GetMaxKey(endTime, token, false);
                dic.Add(token, maxKey);
            }

            return dic;
        }

        public IDictionary<TSourceToken, TKey> GetMaxKeyOfPreviousRound()
        {
            return this.GetMaxKeyBefore(this.currentRoundStartTime);
        }

        public bool NextIsLastPhaseOfRound(DateTime now,out int currentRoundID, out IDictionary<TSourceToken, TKey> lastKeyOfRoundDic)
        {
            currentRoundID = new Date(this.currentRoundStartTime).ToDateInteger();
            lastKeyOfRoundDic = null;           

            TimeSpan span = now - this.currentRoundStartTime;
            bool isLastPhaseOfRound = span.TotalDays >= this.manyDaysInOneRound;

            if (isLastPhaseOfRound)
            {
                DateTime roundEndTime = this.dayOriginTime.GetDateTime(now.Year, now.Month, now.Day);
                lastKeyOfRoundDic = this.GetMaxKeyBefore(roundEndTime);

                this.currentRoundStartTime = roundEndTime; 
            }

            return isLastPhaseOfRound;
        }             

        #endregion
    }
}
