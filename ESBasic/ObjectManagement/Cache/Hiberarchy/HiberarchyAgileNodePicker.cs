using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.ObjectManagement.Trees.Multiple;

namespace ESBasic.ObjectManagement.Cache
{
    /// <summary>
    /// HiberarchyAgileNodePicker 为HiberarchyCache所用，通过ISmartDictionaryCache实现IAgileNodePicker接口。
    /// </summary>   
    public class HiberarchyAgileNodePicker<TVal> : IAgileNodePicker<TVal> where TVal : IMTreeVal
    {
        private ISmartDictionaryCache<string ,TVal> smartDictionaryCache;
        private string rootID;

        #region Ctor
        public HiberarchyAgileNodePicker(ISmartDictionaryCache<string ,TVal> _cache, string _rootID)
        {
            this.smartDictionaryCache = _cache;
            this.rootID = _rootID;
        } 
        #endregion

        #region IAgileNodePicker<TVal> 成员

        public TVal Retrieve(string id)
        {
            return this.smartDictionaryCache.Get(id);
        }

        public TVal PickupRoot()
        {
            return this.smartDictionaryCache.Get(this.rootID);
        }

        public IDictionary<string, TVal> RetrieveAll()
        {
            IDictionary<string, TVal> dic = new Dictionary<string, TVal>();
            foreach (string id in this.smartDictionaryCache.GetAllKeyListCopy())
            {
                dic.Add(id, this.smartDictionaryCache.Get(id));
            }
            return dic;
        }

        #endregion
    }
}
