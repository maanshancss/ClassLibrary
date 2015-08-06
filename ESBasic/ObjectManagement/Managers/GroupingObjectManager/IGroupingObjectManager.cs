using System;
using System.Collections.Generic;

namespace ESBasic.ObjectManagement.Managers
{
    /// <summary>
    /// IGroupingObjectManager 高效地管理需要分组的对象。该接口的实现保证是线程安全的。
    /// </summary>   
    public interface IGroupingObjectManager<TGroupKey, TObjectKey, TObject> where TObject : IGroupingObject<TGroupKey ,TObjectKey>
    {
        /// <summary>
        /// Add 如果已经存在同ID的对象，则用新对象替换旧对象。
        /// </summary>        
        void Add(TObject obj);

        void Remove(TObjectKey objectID);

        /// <summary>
        /// Clear 清除所有对象与分组。
        /// </summary>
        void Clear();

        TObject Get(TObjectKey objectID);

        int TotalObjectCount { get;}

        /// <summary>
        /// GetCountOfGroup 获取某个分组中的对象的个数。
        /// </summary>        
        int GetCountOfGroup(TGroupKey groupID);

        /// <summary>
        /// GetAllObjectsCopy 获取管理器中的所有对象列表。
        /// </summary>        
        IList<TObject> GetAllObjectsCopy();

        /// <summary>
        /// GetGroupsCopy 获取所有的分组标志列表。
        /// </summary>       
        IList<TGroupKey> GetGroupsCopy();

        /// <summary>
        /// GetObjectsCopy 获取某个分组中的所有对象的列表。
        /// </summary>        
        IList<TObject> GetObjectsCopy(TGroupKey groupID);        
    }
}
