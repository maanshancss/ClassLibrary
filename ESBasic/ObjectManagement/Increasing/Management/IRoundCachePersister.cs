using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Increasing.Management
{
    /// <summary>
    /// IRoundCachePersister 用于持久化或加载RoundCache。
    /// </summary>   
    public interface IRoundCachePersister<TRoundID, TRoundCache> where TRoundCache : IRoundCache<TRoundID>
    {
        /// <summary>
        /// Persist 注意，该方法不得抛出异常。
        /// </summary>       
        void Persist(TRoundCache roundCache);

        void Delete(TRoundID roundID);

        IDictionary<TRoundID, TRoundCache> LoadCaches(int maxHistoryCountInMemory);
    }
}
