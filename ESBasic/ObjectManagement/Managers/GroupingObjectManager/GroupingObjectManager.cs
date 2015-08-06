using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Managers
{
    public class GroupingObjectManager<TGroupKey, TObjectKey, TObject> : IGroupingObjectManager<TGroupKey, TObjectKey, TObject> where TObject : IGroupingObject<TGroupKey, TObjectKey>
    {
        private IDictionary<TGroupKey, IDictionary<TObjectKey, TObject>> groupDictionary = new Dictionary<TGroupKey, IDictionary<TObjectKey, TObject>>();//例子：userID - <objectID - obj>
        private IDictionary<TObjectKey, TObject> objectDictionary = new Dictionary<TObjectKey, TObject>(); //例子：objectID - obj
        private object locker = new object();

        #region Add
        public void Add(TObject obj)
        {
            lock (this.locker)
            {
                if (!this.groupDictionary.ContainsKey(obj.GroupID))
                {
                    this.groupDictionary.Add(obj.GroupID, new Dictionary<TObjectKey, TObject>());
                }

                if (this.groupDictionary[obj.GroupID].ContainsKey(obj.ID))
                {
                    this.groupDictionary[obj.GroupID].Remove(obj.ID);
                }

                this.groupDictionary[obj.GroupID].Add(obj.ID, obj);

                if (this.objectDictionary.ContainsKey(obj.ID))
                {
                    this.objectDictionary.Remove(obj.ID);
                }

                this.objectDictionary.Add(obj.ID, obj);
            }
        }
        #endregion
     
        #region Remove
        public void Remove(TObjectKey objectID)
        {
            lock (this.locker)
            {
                if (!this.objectDictionary.ContainsKey(objectID))
                {
                    return;
                }

                TObject obj = this.objectDictionary[objectID];
                this.objectDictionary.Remove(objectID);

                if (this.groupDictionary[obj.GroupID].ContainsKey(objectID))
                {
                    this.groupDictionary[obj.GroupID].Remove(objectID);
                    if (this.groupDictionary[obj.GroupID].Count == 0) //当该分组不再包含任何对象时
                    {
                        this.groupDictionary.Remove(obj.GroupID);
                    }
                }

                
            }
        }
        #endregion

        #region GetObjectsCopy
        public IList<TObject> GetObjectsCopy(TGroupKey groupID)
        {
            lock (this.locker)
            {
                if (!this.groupDictionary.ContainsKey(groupID))
                {
                    return new List<TObject>();
                }

                return ESBasic.Collections.CollectionConverter.CopyAllToList<TObject>(this.groupDictionary[groupID].Values);
            }
        }
        #endregion

        #region GetAllObjectsCopy
        public IList<TObject> GetAllObjectsCopy()
        {
            lock (this.locker)
            {
                return ESBasic.Collections.CollectionConverter.CopyAllToList<TObject>(this.objectDictionary.Values);
            }
        }
        #endregion

        #region GetGroupsCopy
        public IList<TGroupKey> GetGroupsCopy()
        {
            lock (this.locker)
            {
                return ESBasic.Collections.CollectionConverter.CopyAllToList<TGroupKey>(this.groupDictionary.Keys);
            }
        }
        #endregion

        #region Get
        public TObject Get(TObjectKey objectID)
        {
            lock (this.locker)
            {
                if (this.objectDictionary.ContainsKey(objectID))
                {
                    return this.objectDictionary[objectID];
                }
                return default(TObject);
            }
        }
        #endregion

        #region TotalObjectCount
        public int TotalObjectCount
        {
            get
            {
                return this.objectDictionary.Count;
            }
        } 
        #endregion

        #region GetCountOfGroup
        public int GetCountOfGroup(TGroupKey groupID)
        {
            lock (this.locker)
            {
                if (!this.groupDictionary.ContainsKey(groupID))
                {
                    return 0;
                }

                return this.groupDictionary[groupID].Count;
            }
        } 
        #endregion

        #region Clear
        public void Clear()
        {
            lock (this.locker)
            {
                this.groupDictionary.Clear();
                this.objectDictionary.Clear();
            }
        }
        #endregion
    }

    
}
