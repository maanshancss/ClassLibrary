using System;
using System.Collections.Generic;
using System.Text;
using System.Threading ;
using ESBasic.Threading.Engines;
using ESBasic.Collections;

namespace ESBasic.Threading.Timers
{
    /// <summary>
    /// CallbackTimer 回调任务定时器。
    /// 注意：回调任务会异步在ThreadPool的WorkerThread上执行。即使目标任务抛出异常也不会影响CallbackTimer的继续运行。
    /// </summary>   
    public class CallbackTimer<T> : BaseCycleEngine, ICallbackTimer<T>
    {
        private IDictionary<int, CallbackTask<T>> dicTask = new Dictionary<int, CallbackTask<T>>();
        private object locker = new object();
        private int idCount = 0;

        #region TaskCount
        public int TaskCount
        {
            get
            {
                return this.dicTask.Count;
            }
        } 
        #endregion

        public CallbackTimer()
        {
            this.DetectSpanInSecs = 1 ;
        }

        public override int DetectSpanInSecs
        {
            get
            {
                return base.DetectSpanInSecs;
            }
            set
            {
                if (value < 1)
                {
                    throw new Exception("DetectSpanInSecs must greater than 0");
                }

                base.DetectSpanInSecs = value;
            }
        }

        #region DoDetect override
        protected override bool DoDetect()
        {
            IList<CallbackTask<T>> taskList = null;
            //要先拷贝taskList ，因为回调方法执行时可能会调用AddCallback/RemoveCallback，修改dicTask集合。
            lock (this.locker)
            {
                taskList = CollectionConverter.CopyAllToList<CallbackTask<T>>(this.dicTask.Values);
            }

            foreach (CallbackTask<T> task in taskList)
            {
                bool onTime = task.SecondsPassed(this.DetectSpanInSecs);
                if (onTime)
                {
                    this.RemoveCallback(task.ID);
                    task.Callback.BeginInvoke(task.CallbackPara ,null ,null);                    
                }
            }

            return true;
        }       
        #endregion

        #region AddCallback
        public int AddCallback(int spanInSecs, CbGeneric<T> _callback, T _callbackPara)
        {
            lock (this.locker)
            {
                ++this.idCount;
                this.dicTask.Add(this.idCount, new CallbackTask<T>(this.idCount, spanInSecs, _callback, _callbackPara));
                return this.idCount;
            }
        } 
        #endregion

        #region RemoveCallback
        public void RemoveCallback(int taskID)
        {
            lock (this.locker)
            {
                if (this.dicTask.ContainsKey(taskID))
                {
                    this.dicTask.Remove(taskID);
                }
            }
        } 
        #endregion

        #region RemoveCallbackAndAddNew
        public int RemoveCallbackAndAddNew(int taskIDToRemoved, int spanInSecs, CbGeneric<T> _newCallback, T _newCallbackPara)
        {
            this.RemoveCallback(taskIDToRemoved);
            return this.AddCallback(spanInSecs, _newCallback, _newCallbackPara);
        }        
        #endregion        

        #region GetLeftSeconds
        public int GetLeftSeconds(int taskID)
        {
            lock (this.locker)
            {
                if (!this.dicTask.ContainsKey(taskID))
                {
                    return 0;
                }

                return this.dicTask[taskID].LeftSeconds;
            }
        } 
        #endregion

        #region Clear
        public void Clear()
        {
            lock (this.locker)
            {
                this.dicTask.Clear();
            }
        } 
        #endregion

    }   
}
