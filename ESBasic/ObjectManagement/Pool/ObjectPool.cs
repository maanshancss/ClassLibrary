using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Pool
{
    public class ObjectPool<TObject> : IObjectPool<TObject> where TObject : class
    {
        private IList<TObject> idleList = new List<TObject>();
        private IDictionary<TObject, TObject> busyDictionary = new Dictionary<TObject, TObject>();
        private object locker = new object();

        #region property
        #region MinObjectCount
        private int minObjectCount = 0;
        public int MinObjectCount
        {
            get { return minObjectCount; }
            set { minObjectCount = value; }
        }
        #endregion

        #region MaxObjectCount
        private int maxObjectCount = int.MaxValue;
        public int MaxObjectCount
        {
            get { return maxObjectCount; }
            set { maxObjectCount = value; }
        }
        #endregion

        #region DetectSpanInMSecs
        private int detectSpanInMSecs = 10;
        public int DetectSpanInMSecs
        {
            get { return detectSpanInMSecs; }
            set { detectSpanInMSecs = value; }
        } 
        #endregion

        #region PooledObjectCreator
        private IPooledObjectCreator<TObject> pooledObjectCreator = new DefaultPooledObjectCreator<TObject>();
        public IPooledObjectCreator<TObject> PooledObjectCreator
        {
            set { pooledObjectCreator = value ?? new DefaultPooledObjectCreator<TObject>(); }
        }
        #endregion 
        #endregion

        #region IObjectPool<TObject> ≥…‘±
        #region Initialize
        public void Initialize()
        {
            if (this.minObjectCount < 0)
            {
                throw new Exception("The MinObjectCount must be greater than 0 !");
            }

            if (this.minObjectCount > this.maxObjectCount)
            {
                throw new Exception("The MinObjectCount can't be greater than MaxObjectCount !");
            }

            if (this.detectSpanInMSecs < 0)
            {
                throw new Exception("The DetectSpanInMSecs must be greater than 0 !");
            }

            for (int i = 0; i < this.minObjectCount; i++)
            {
                TObject obj = this.pooledObjectCreator.Create();
                this.idleList.Add(obj);
            }
        } 
        #endregion

        #region Rent
        public TObject Rent()
        {
            lock (this.locker)
            {
                if ((this.idleList.Count == 0) && (this.busyDictionary.Count < this.maxObjectCount))
                {
                    TObject obj = this.pooledObjectCreator.Create();
                    this.busyDictionary.Add(obj, obj);
                    return obj;
                }

                while (this.idleList.Count == 0)
                {
                    System.Threading.Thread.Sleep(this.detectSpanInMSecs);
                }

                TObject objToRent = this.idleList[0];
                this.idleList.RemoveAt(0);
                this.busyDictionary.Add(objToRent, objToRent); 
                return objToRent;
            }
        } 
        #endregion

        #region GiveBack
        public void GiveBack(TObject obj)
        {
            lock (this.locker)
            {
                if (!this.busyDictionary.ContainsKey(obj))
                {
                    return;
                }

                this.pooledObjectCreator.Reset(obj);
                this.busyDictionary.Remove(obj);
                this.idleList.Add(obj);
            }
        } 
        #endregion

        #endregion
    }
}
