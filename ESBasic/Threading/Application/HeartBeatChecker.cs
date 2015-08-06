using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Threading.Engines;

namespace ESBasic.Threading.Application
{
    /// <summary>
    /// HeartBeatChecker 心跳监测器。
    /// IHeartBeatChecker 参考实现。
    /// </summary>
    public class HeartBeatChecker : BaseCycleEngine, IHeartBeatChecker
    {
        private IDictionary<string, DateTime> dicIDTime = new Dictionary<string, DateTime>();
        public event CbSimpleStr SomeOneTimeOuted;
        private object locker = new object();

        #region SurviveSpanInSecs
        private int surviveSpanInSecs = 10;
        public int SurviveSpanInSecs
        {
            get { return surviveSpanInSecs; }
            set { surviveSpanInSecs = value; }
        }        
        #endregion

        #region Ctor
        public HeartBeatChecker()
        {
            this.SomeOneTimeOuted += delegate { };
            base.DetectSpanInSecs = 5;
        }

        public HeartBeatChecker(int detectSpanInSecs, int _surviveSpanInSecs)
        {
            this.SomeOneTimeOuted += delegate { };
            base.DetectSpanInSecs = detectSpanInSecs;
            this.surviveSpanInSecs = _surviveSpanInSecs;
        }
        #endregion

        #region Initialize
        public void Initialize()
        {
            if (this.surviveSpanInSecs <= 0)
            {
                return;
            }

            base.Start();
        }        
        #endregion

        #region RegisterOrActivate
        public void RegisterOrActivate(string id)
        {
            lock (this.locker)
            {
                if (this.dicIDTime.ContainsKey(id))
                {
                    this.dicIDTime.Remove(id);
                }

                this.dicIDTime.Add(id, DateTime.Now);
            }
        } 
        #endregion

        #region Unregister
        public void Unregister(string id)
        {
            lock (this.locker)
            {
                if (this.dicIDTime.ContainsKey(id))
                {
                    this.dicIDTime.Remove(id);
                }
            }
        } 
        #endregion

        #region DoDetect
        protected override bool DoDetect()
        {
            IList<string> deadList = new List<string>();
            DateTime now = DateTime.Now;
            lock (this.locker)
            {
                foreach (string id in this.dicIDTime.Keys)
                {
                    TimeSpan span = now - this.dicIDTime[id];
                    if (span.TotalSeconds >= this.surviveSpanInSecs)
                    {
                        deadList.Add(id);
                    }
                }

                foreach (string deadID in deadList)
                {
                    this.dicIDTime.Remove(deadID);
                }
            }

            foreach (string deadID in deadList)
            {
                this.SomeOneTimeOuted(deadID);
            }

            return true;
        } 
        #endregion        

        #region Clear
        public void Clear()
        {
            lock (this.locker)
            {
                this.dicIDTime.Clear();
            }
        } 
        #endregion
    }
}
