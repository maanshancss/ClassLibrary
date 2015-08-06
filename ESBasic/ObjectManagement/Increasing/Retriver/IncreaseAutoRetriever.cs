using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Threading.Engines;
using ESBasic.Collections;

namespace ESBasic.ObjectManagement.Increasing
{
    /// <summary>
    /// IncreaseAutoRetriever 增量数据自动获取器。每隔一段时间就从各个来源（TSourceToken）获取一次增量（TObject），并触发事件将增量发布出去。
    /// (1)一个Round由多个连续的Phase构成。当获取某Round的最后一个Phase增量时，触发的事件中的isLastPhaseOfRound参数为true。
    /// (2)假设增量标志是逐渐递增的。
    /// zhuweisky 2009.02.24
    /// </summary>
    /// <typeparam name="TSourceToken">增量来源的标志</typeparam>
    /// <typeparam name="TKey">每个增量Object的标志</typeparam>
    /// <typeparam name="TObject">增量Object的类型</typeparam>
    public class IncreaseAutoRetriever<TSourceToken, TRoundID, TKey, TObject> : BaseCycleEngine, IIncreaseAutoRetriever<TSourceToken, TRoundID, TKey, TObject>
    {
        private IDictionary<TSourceToken, TKey> maxKeyOfLastPhaseDictionary;
        private object locker = new object();       

        #region Event
        public event CbIncreasementRetrieved<TRoundID ,TObject> IncreasementRetrieved;
        public event CbException ExceptionOccurred;
        #endregion

        #region Ctor
        public IncreaseAutoRetriever()
        {
            this.IncreasementRetrieved += delegate { };
            this.ExceptionOccurred += delegate { };
        } 
        #endregion

        #region AutoRetrieveSpanInSecs
        public int AutoRetrieveSpanInSecs
        {
            get { return base.DetectSpanInSecs; }
            set { base.DetectSpanInSecs = value; }
        }
        #endregion

        #region PhaseIncreaseAccesser
        private IPhaseIncreaseAccesser<TSourceToken, TRoundID, TKey, TObject> phaseIncreaseAccesser;
        public IPhaseIncreaseAccesser<TSourceToken, TRoundID, TKey, TObject> PhaseIncreaseAccesser
        {
            set { phaseIncreaseAccesser = value; }
        } 
        #endregion

        #region Initialize
        public void Initialize()
        {          
            this.maxKeyOfLastPhaseDictionary = this.phaseIncreaseAccesser.GetMaxKeyOfPreviousRound();
            base.Start();
        } 
        #endregion

        #region ManualRefresh
        public void ManualRefresh()
        {
            this.DoDetect();
        } 
        #endregion

        #region DoDetect
        protected override bool DoDetect()
        {
            try
            {
                lock (this.locker)
                {
                    DateTime now = DateTime.Now; //防止在接下来的流程中出现时间不一致的现象。
                    List<TObject> container = new List<TObject>();
                    IDictionary<TSourceToken, TKey> lastKeyOfRoundDic = null;
                    IList<TSourceToken> sourceList = CollectionConverter.CopyAllToList<TSourceToken>(this.maxKeyOfLastPhaseDictionary.Keys);
                    TRoundID currentRoundID = default(TRoundID);
                    bool isLastPhaseOfRound = this.phaseIncreaseAccesser.NextIsLastPhaseOfRound(now, out currentRoundID, out lastKeyOfRoundDic);

                    if (!isLastPhaseOfRound)
                    {
                        foreach (TSourceToken token in sourceList)
                        {
                            TKey maxKeyOfThisPhase = this.phaseIncreaseAccesser.GetMaxKey(now, token);
                            IList<TObject> list = this.phaseIncreaseAccesser.Retrieve(token, this.maxKeyOfLastPhaseDictionary[token], maxKeyOfThisPhase);
                            this.maxKeyOfLastPhaseDictionary[token] = maxKeyOfThisPhase;

                            foreach (TObject val in list)
                            {
                                container.Add(val);
                            }
                        }
                    }
                    else
                    {
                        foreach (TSourceToken token in sourceList)
                        {
                            IList<TObject> list = this.phaseIncreaseAccesser.Retrieve(token, this.maxKeyOfLastPhaseDictionary[token], lastKeyOfRoundDic[token]);
                            this.maxKeyOfLastPhaseDictionary[token] = lastKeyOfRoundDic[token];
                            foreach (TObject val in list)
                            {
                                container.Add(val);
                            }
                        }
                    }

                    this.IncreasementRetrieved(container, currentRoundID, isLastPhaseOfRound);

                    return true;
                }
            }
            catch (Exception ee)
            {
                this.ExceptionOccurred(ee);
                return false;
            }
        } 
        #endregion
    }    
}
