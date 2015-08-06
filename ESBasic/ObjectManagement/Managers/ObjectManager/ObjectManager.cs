using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Helpers;

namespace ESBasic.ObjectManagement.Managers
{
    public class ObjectManager<TPKey, TObject> : IObjectManager<TPKey, TObject> 
    {
        protected IDictionary<TPKey, TObject> objectDictionary = new Dictionary<TPKey, TObject>();
        private List<TObject> readonlyCopy = null ;
        protected object locker = new object();

        public event CbGeneric<TObject> ObjectRegistered;
        public event CbGeneric<TObject> ObjectUnregistered;
       
        public ObjectManager()
        {
            this.ObjectRegistered += delegate { };
            this.ObjectUnregistered += delegate { };
        }

        /// <summary>
        /// 返回的列表不能被修改。【使用缓存】
        /// </summary>  
        public List<TObject> GetAllReadonly()
        {
            lock (this.locker)
            {
                if (this.readonlyCopy == null)
                {
                    this.readonlyCopy = new List<TObject>(this.objectDictionary.Values);
                }

                return this.readonlyCopy;
            }
        }

        #region IObjectManager<TObject,TPKey> 成员

        public int Count
        {
            get
            {
                return this.objectDictionary.Count;
            }
        }

        public virtual void Add(TPKey key, TObject obj)
        {
            lock (this.locker)
            {
                if (this.objectDictionary.ContainsKey(key))
                {
                    this.objectDictionary.Remove(key);
                }

                this.objectDictionary.Add(key, obj);

                this.readonlyCopy = null;
            }

            this.ObjectRegistered(obj);
        }

        public virtual void Remove(TPKey id)
        {
            TObject target = default(TObject);
            lock (this.locker)
            {
                if (this.objectDictionary.ContainsKey(id))
                {
                    target = this.objectDictionary[id];
                    this.objectDictionary.Remove(id);
                    this.readonlyCopy = null;
                }
            }

            if (target != null)
            {
                this.ObjectUnregistered(target);
            }
        }

        public virtual void RemoveByValue(TObject val)
        {
            lock (this.locker)
            {
                List<TPKey> keyList = new List<TPKey>(this.objectDictionary.Keys);
                foreach (TPKey key in keyList)
                {
                    if (this.objectDictionary[key].Equals(val))
                    {
                        this.objectDictionary.Remove(key);
                    }
                }
                this.readonlyCopy = null;
            }
        }

        public void RemoveByPredication(Predicate<TObject> predicate)
        {
            lock (this.locker)
            {
                List<TPKey> keyList = new List<TPKey>(this.objectDictionary.Keys);
                foreach (TPKey key in keyList)
                {
                    if (predicate(this.objectDictionary[key]))
                    {
                        this.objectDictionary.Remove(key);
                    }
                }

                this.readonlyCopy = null;
            }
        }

        public virtual void Clear()
        {
            lock (this.locker)
            {
                this.objectDictionary.Clear();
                this.readonlyCopy = null;
            }
        }

        public TObject Get(TPKey id)
        {
            lock (this.locker)
            {
                if (this.objectDictionary.ContainsKey(id))
                {
                    return this.objectDictionary[id];
                }
            }

            return default(TObject);
        }

        public bool Contains(TPKey id)
        {
            lock (this.locker)
            {
                return this.objectDictionary.ContainsKey(id);               
            }
        }

        public List<TObject> GetAll()
        {
            lock (this.locker)
            {
                return ESBasic.Collections.CollectionConverter.CopyAllToList<TObject>(this.objectDictionary.Values);
            }
        }

        public List<TPKey> GetKeyList()
        {
            lock (this.locker)
            {
                return ESBasic.Collections.CollectionConverter.CopyAllToList<TPKey>(this.objectDictionary.Keys);
            }
        }

        public List<TPKey> GetKeyListByObj(TObject obj)
        {
            lock (this.locker)
            {
                List<TPKey> list = new List<TPKey>();
                foreach (TPKey key in this.GetKeyList())
                {
                    if (this.objectDictionary[key].Equals(obj))
                    {
                        list.Add(key);
                    }
                }

                return list;
            }
        }

        public Dictionary<TPKey, TObject> ToDictionary()
        {
            lock (this.locker)
            {
                return new Dictionary<TPKey, TObject>(this.objectDictionary);
            }
        }


        #endregion
    }
}
