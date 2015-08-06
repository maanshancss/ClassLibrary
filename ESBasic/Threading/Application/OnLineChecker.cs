using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ESBasic.Threading.Engines;

namespace ESBasic.Threading.Application
{
    public sealed class OnLineChecker : BaseCycleEngine, IOnLineChecker
    {
        private IDictionary<string, bool> dicState = new Dictionary<string, bool>();

        public event CbSimpleStr SomeTimeOuted;   

        public OnLineChecker()
        {
            this.SomeTimeOuted += delegate { };
        }

        #region Start
        public override void Start()
        {
            if (base.DetectSpanInSecs <= 0)
            {
                return;
            }

            base.Start();          
        }
        #endregion

        #region Stop
        public override void Stop()
        {    
            base.Stop();

            lock (this.dicState)
            {
                this.dicState.Clear();
            }
        }
        #endregion

        #region RegisterOrActivate
        public void RegisterOrActivate(string id)
        {
            lock (this.dicState)
            {
                if (this.dicState.ContainsKey(id))
                {
                    this.dicState[id] = true;
                }
                else
                {
                    this.dicState.Add(id, true);
                }
            }
        }
        #endregion

        #region Unregister
        public void Unregister(string id)
        {
            lock (this.dicState)
            {
                if (this.dicState.ContainsKey(id))
                {
                    this.dicState.Remove(id);
                }
            }
        }
        #endregion      

        #region DoDetect
        protected override bool DoDetect()
        {
            IList<string> list = new List<string>();
            lock (this.dicState)
            {
                foreach (string id in this.dicState.Keys)
                {
                    if (false == (bool)this.dicState[id])
                    {
                        list.Add(id);
                    }
                }

                foreach (string id in list)
                {
                    this.dicState.Remove(id);
                }

                //一定要拷贝，因为在对Dictionary进行foreach操作时，是不能修改Dictionary中的key和value的
                string[] arrayKeys = new string[this.dicState.Keys.Count];
                this.dicState.Keys.CopyTo(arrayKeys, 0);
                foreach (string key in arrayKeys)
                {
                    this.dicState[key] = false; //修改Dictionary中的Value
                }
            }

            foreach (string id in list)
            {
                this.SomeTimeOuted(id);
            }

            return true;
        }
        #endregion
    }
}
