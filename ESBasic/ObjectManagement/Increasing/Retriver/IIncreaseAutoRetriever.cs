using System;
using System.Collections.Generic;

namespace ESBasic.ObjectManagement.Increasing
{
    /// <summary>
    /// IIncreaseAutoRetriever 增量数据自动获取器。每隔一段时间就从各个来源（TSourceToken）获取一次增量（TObject），并触发事件将增量发布出去。
    /// (1)一个Round由多个连续的Phase构成。当获取某Round的最后一个Phase增量时，触发的事件中的isLastPhaseOfRound参数为true。
    /// (2)假设增量标志是逐渐递增的。
    /// zhuweisky 2009.02.24
    /// </summary>
    /// <typeparam name="TSourceToken">增量来源的标志</typeparam>
    /// <typeparam name="TKey">每个增量Object的标志</typeparam>
    /// <typeparam name="TObject">增量Object的类型</typeparam>
    public interface IIncreaseAutoRetriever<TSourceToken,TRoundID, TKey, TObject>
    {
        /// <summary>
        /// AutoRetrieveSpanInSecs 设置多长时间为一增量阶段。
        /// </summary>
        int AutoRetrieveSpanInSecs { get; set; }
        
        IPhaseIncreaseAccesser<TSourceToken, TRoundID, TKey, TObject> PhaseIncreaseAccesser { set; }


        event CbIncreasementRetrieved<TRoundID ,TObject> IncreasementRetrieved;

        /// <summary>
        /// ExceptionOccurred 当提取增量数据出现异常或IncreasementRetrieved事件处理器抛出异常时，将触发此事件，并且引擎将停止运行。
        /// </summary>
        event CbException ExceptionOccurred;   
        
        void Initialize();

        /// <summary>
        /// ManualRefresh 手动刷新获取增量。
        /// </summary>
        void ManualRefresh();        
    }

    public delegate void CbIncreasementRetrieved<TRoundID ,TObject>(List<TObject> list, TRoundID currentRoundID, bool isLastPhaseOfRound);      
}
