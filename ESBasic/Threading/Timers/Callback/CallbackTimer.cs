using System;
using System.Collections.Generic;
using System.Text;
using System.Threading ;
using ESBasic.Threading.Engines;
using ESBasic.Collections;

namespace ESBasic.Threading.Timers
{
    /// <summary>
    /// CallbackTimer �ص�����ʱ����
    /// ע�⣺�ص�������첽��ThreadPool��WorkerThread��ִ�С���ʹĿ�������׳��쳣Ҳ����Ӱ��CallbackTimer�ļ������С�
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
            //Ҫ�ȿ���taskList ����Ϊ�ص�����ִ��ʱ���ܻ����AddCallback/RemoveCallback���޸�dicTask���ϡ�
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
