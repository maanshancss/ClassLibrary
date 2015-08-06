using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Increasing
{
    /// <summary>
    /// IPhaseIncreaseAccesser 用于从各个源访问每一阶段的增量数据。
    /// </summary>   
    /// <typeparam name="TSourceToken">增量来源的标志</typeparam>
    /// <typeparam name="TKey">Round的ID的类型</typeparam>
    /// <typeparam name="TKey">每个增量Object的标志</typeparam>
    /// <typeparam name="TObject">增量Object的类型</typeparam>
    public interface IPhaseIncreaseAccesser<TSourceToken, TRoundID, TKey, TObject>
    {
        /// <summary>
        /// GetMaxKeyOfPreviousRound 获取上一轮各个源中的数据的最大标志。
        /// </summary>       
        IDictionary<TSourceToken, TKey> GetMaxKeyOfPreviousRound();

        /// <summary>
        /// NextIsLastPhaseOfRound 下一增量（基于now时刻）是否为当前Round的最后一个Phase。如果是，则out出每个源的最后Phase的最大标志。
        /// </summary>               
        bool NextIsLastPhaseOfRound(DateTime now ,out TRoundID currentRoundID, out IDictionary<TSourceToken, TKey> lastKeyOfRoundDic);

        /// <summary>
        /// GetMaxKey 获取指定源中的截止now时刻（可以等于）的最大标志。
        /// </summary> 
        TKey GetMaxKey(DateTime now, TSourceToken token);      

        /// <summary>
        /// Retrieve 获取某一阶段的增量。maxKeyOfPrePhase 《 本阶段增量 《=  maxKeyOfThisPhase
        /// </summary>   
        IList<TObject> Retrieve(TSourceToken token, TKey maxKeyOfPrePhase, TKey maxKeyOfThisPhase);
    }
}
