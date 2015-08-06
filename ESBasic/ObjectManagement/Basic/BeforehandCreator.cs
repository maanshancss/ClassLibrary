using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Threading.Engines;

namespace ESBasic.ObjectManagement
{
    public interface ICreator<T>
    {
        T Create();
    }

    public class DefaultCreator : ICreator<int>
    {
        private int id = 0;

        #region Ctor
        public DefaultCreator() { }
        public DefaultCreator(int curMaxID)
        {
            this.id = curMaxID;
        }
        
        #endregion
        #region ICreator<int> 成员
        public int Create()
        {
            System.Threading.Interlocked.Increment(ref this.id);
            return this.id;
        }
        #endregion
    }


    /// <summary>
    /// BeforehandCreator 提前创建器。
    /// zhuweisky 2010.05.04
    /// </summary>
    /// <typeparam name="T">要创建的对象类型</typeparam>
    public abstract class BeforehandCreator<T> : IEngineActor, ICreator<T>, IDisposable
    {
        private AgileCycleEngine engine;
        private Queue<T> queue = new Queue<T>(); 
      
        protected abstract T DoCreate();

        #region BeforehandCount
        private int beforehandCount = 10;
        public int BeforehandCount
        {
            get { return beforehandCount; }
            set { beforehandCount = value; }
        }
        #endregion

        #region DetectSpanInSecs
        private int detectSpanInSecs = 1;
        public int DetectSpanInSecs
        {
            get { return detectSpanInSecs; }
            set { detectSpanInSecs = value; }
        } 
        #endregion

        #region Initialize
        public void Initialize()
        {
            this.engine = new AgileCycleEngine(this);
            this.engine.DetectSpanInSecs = this.detectSpanInSecs;
            this.engine.Start();
        }  
        #endregion       

        #region Create
        private object locker4Create = new object();
        public T Create()
        {
            lock (this.locker4Create)
            {
                while (this.queue.Count == 0)
                {
                    System.Threading.Thread.Sleep(10);
                }

                lock (this.queue)
                {
                    return this.queue.Dequeue();
                }
            }
        } 
        #endregion

        #region EngineAction
        public bool EngineAction()
        {
            while (this.queue.Count < this.beforehandCount)
            {
                try
                {
                    T t = this.DoCreate();
                    lock (this.queue)
                    {
                        this.queue.Enqueue(t);
                    }
                }
                catch (Exception ee)
                {
                    System.Threading.Thread.Sleep(10);
                }
            }

            return true;
        } 
        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            if (this.engine != null)
            {
                this.engine.Stop();
            }
        }

        #endregion
    }
}
