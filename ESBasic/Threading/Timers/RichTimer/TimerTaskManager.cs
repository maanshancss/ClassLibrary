using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using ESBasic.Loggers;

namespace ESBasic.Threading.Timers.RichTimer
{
    public class TimerTaskManager : ITimerTaskManager
    {        
        private IDictionary<string, TimerTask> dicTimerContent = new Dictionary<string, TimerTask>();//timerName -- task
        public event CbTimerTask TimerTaskExpired;

        #region Property
        #region Logger
        private ILogger logger = new EmptyLogger() ;
        public ILogger Logger
        {
            set 
            { 
                this.logger = value ?? new EmptyLogger(); 
            }
        } 
        #endregion

        #region TimerSpanSecs
        private int timerSpanSecs = 1;//sec
        public int TimerSpanSecs
        {
            get { return timerSpanSecs; }
            set { timerSpanSecs = value; }
        }
        #endregion       
        #endregion

        #region Ctor
        public TimerTaskManager()
        {
            this.TimerTaskExpired += delegate { };
        }
        #endregion

        #region Timer
        private Timer timer;

        public void Initialize()
        {
            this.timer = new Timer(new TimerCallback(this.Worker), null, this.timerSpanSecs * 1000, this.timerSpanSecs * 1000);
        }

        public void Dispose()
        {
            if (this.timer != null)
            {
                this.timer.Dispose();
            }
        }
        #endregion
       
        #region Worker
        protected void Worker(object state)
        {
            DateTime now = DateTime.Now;
            IList<TimerTask> expiredList = new List<TimerTask>();
            lock (this.dicTimerContent)
            {
                foreach (TimerTask task in this.dicTimerContent.Values)
                {
                    if (task.IsExpired(now))
                    {
                        expiredList.Add(task);
                    }
                    else
                    {
                        try
                        {
                            //内部为异步调用
                            task.HaveATry(this.timerSpanSecs ,now);
                        }
                        catch(Exception ee)
                        {
                            this.logger.LogWithTime("TimerTaskManager.Worker -- " + ee.Message);
                        }
                    }
                }

                foreach (TimerTask task in expiredList)
                {
                    this.dicTimerContent.Remove(task.Name);
                }
            }

            foreach (TimerTask task in expiredList)
            {
                try
                {
                    this.TimerTaskExpired(task);
                }
                catch { }
            }
        }
       
        #endregion

        #region IRichTimerManager 成员
        #region RemoveTimerTask
        public void RemoveTimerTask(string timerName)
        {
            lock (this.dicTimerContent)
            {
                this.dicTimerContent.Remove(timerName);
            }
        }
        #endregion

        #region AddTimerTask
        public void AddTimerTask(TimerTask task)
        {
            lock (this.dicTimerContent)
            {
                if (this.dicTimerContent.ContainsKey(task.Name))
                {
                    this.dicTimerContent.Remove(task.Name);
                }
                this.dicTimerContent.Add(task.Name, task);
            }
        }
        #endregion

        #region GetTimerTask
        public TimerTask GetTimerTask(string timerName)
        {
            lock (this.dicTimerContent)
            {
                if (this.dicTimerContent.ContainsKey(timerName))
                {
                    return this.dicTimerContent[timerName];
                }

                return null;
            }
        }
        #endregion

        #region TimerTaskList
        public IList<TimerTask> TimerTaskList
        {
            get
            {
                IList<TimerTask> list = new List<TimerTask>();
                lock (this.dicTimerContent)
                {
                    foreach (TimerTask task in this.dicTimerContent.Values)
                    {
                        list.Add(task);
                    }
                }

                return list;
            }
        } 
        #endregion
        #endregion
     
    }

    internal delegate void CbTimerList(IList<TimerTask> expiredList);
}
