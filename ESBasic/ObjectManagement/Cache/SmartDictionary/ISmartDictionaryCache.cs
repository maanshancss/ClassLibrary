using System;
using System.Collections.Generic;

namespace ESBasic.ObjectManagement.Cache
{
    /// <summary>
    /// ISmartDictionaryCache 智能缓存。首先在当前缓存中查找目标对象，如果不存在，则采用ObjectRetriever提取，并缓存。
    /// 该接口的实现必须是线程安全的。
    /// </summary>   
    public interface ISmartDictionaryCache<Tkey ,TVal>
    {
        IObjectRetriever<Tkey, TVal> ObjectRetriever { set; }

        /// <summary>
        /// 是否启用缓存。如果为false，则Get方法将直接通过调用IObjectRetriever.Retrieve返回。
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// 当前缓存中的对象个数。
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 初始化，调用IObjectRetriever.RetrieveAll加载。
        /// </summary>
        void Initialize();        

        /// <summary>
        /// Get 如果缓存中不存在id对应的object，则采用ObjectRetriever提取一次，如果仍然提取不到则返回null。
        /// </summary>       
        TVal Get(Tkey id);

        /// <summary>
        /// 清空缓存。
        /// </summary>
        void Clear();

        /// <summary>
        /// HaveContained 当前容器是否已经存在目标对象。
        /// </summary>       
        bool HaveContained(Tkey id);                 

        IList<TVal> GetAllValListCopy();

        IList<Tkey> GetAllKeyListCopy();
    }
}
