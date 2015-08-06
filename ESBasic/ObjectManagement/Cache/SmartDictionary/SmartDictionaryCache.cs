using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Threading.Synchronize;

namespace ESBasic.ObjectManagement.Cache
{
    public class SmartDictionaryCache<Tkey, TVal> : ISmartDictionaryCache<Tkey ,TVal>
    {
        private IDictionary<Tkey, TVal> ditionary = new Dictionary<Tkey, TVal>();
        private object locker = new object();

        #region bool Enabled
        private bool enabled = true;
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        } 
        #endregion

        #region ObjectRetriever
        private IObjectRetriever<Tkey, TVal> objectRetriever;
        public IObjectRetriever<Tkey, TVal> ObjectRetriever
        {
            set { objectRetriever = value; }
        } 
        #endregion

        #region Initialize
        public void Initialize()
        {
            if (this.enabled)
            {
                this.ditionary = this.objectRetriever.RetrieveAll() ?? new Dictionary<Tkey, TVal>();
            }
        } 
        #endregion

        #region Get
        public TVal Get(Tkey id)
        {
            if (id == null)
            {
                return default(TVal);
            }

            if (!this.enabled)
            {
                return this.objectRetriever.Retrieve(id);
            }

            lock (this.locker)
            {
                if (this.ditionary.ContainsKey(id))
                {
                    return this.ditionary[id];
                }


                TVal val = this.objectRetriever.Retrieve(id);
                if (val != null)
                {
                    this.ditionary.Add(id, val);
                }

                return val;
            }
        } 
        #endregion    

        #region Clear
        public void Clear()
        {
            lock (this.locker)
            {
                this.ditionary.Clear();
            }
        } 
        #endregion

        #region GetAllValListCopy
        public IList<TVal> GetAllValListCopy()
        {
            lock (this.locker)
            {
                return new List<TVal>(this.ditionary.Values);
            }
        } 
        #endregion

        #region GetAllKeyListCopy
        public IList<Tkey> GetAllKeyListCopy()
        {
            lock (this.locker)
            {
                return ESBasic.Collections.CollectionConverter.CopyAllToList<Tkey>(this.ditionary.Keys);
            }
        } 
        #endregion

        #region HaveContained
        public bool HaveContained(Tkey id)
        {
            if (id == null)
            {
                return false;
            }

            return this.ditionary.ContainsKey(id);//不会与写线程冲突。
        } 
        #endregion

        #region Count
        public int Count
        {
            get
            {
                return this.ditionary.Count;
            }
        } 
        #endregion
    }
}
