using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Engine
{
    public class AgileEngine : IAgileEngine, IEngineTacheUtil
    {
        private IList<IEngineTache> engineTacheList;          
        private IEngineTache currentTache ;
        private bool hasNecceryEnder = false;

        #region events       
        public event CbProgress PartProgressPublished;

        public event CbSimpleStr MessagePublished;
        public event CbSimpleStr IgnoredMessagePublished;      
        public event CbSimpleStr EngineDisruptted;
        public event CbSimple EngineCompleted;
        public event CbSimpleStr TitleChanged;
        public event CbSimpleObj EngineStatusChanged;
        #endregion

        #region InitializeEventHandler
        private void InitializeEventHandler()
        {           
            this.PartProgressPublished += delegate { };         
            this.MessagePublished += delegate { };
            this.IgnoredMessagePublished += delegate { };           
            this.EngineCompleted += delegate { };
            this.EngineDisruptted += delegate { };
            this.TitleChanged += delegate { };
            this.EngineStatusChanged += delegate { };
        }
        #endregion

        #region IEngine 成员

        #region Initialize
        public void Initialize(IList<IEngineTache> tacheList, bool has_NecceryEnder)
        {
            this.hasNecceryEnder = has_NecceryEnder;
            this.engineTacheList = tacheList;
            this.InitializeEventHandler();

            foreach (IEngineTache tache in this.engineTacheList)
            {
                //传递IEngineTacheUtil引用
                tache.Initialize(this);
                
                //传递事件
                tache.IgnoredMessagePublished += new CbSimpleStr(tache_IgnoredMessagePublished);
                tache.ProgressPublished += new CbProgress(tache_ProgressPublished);
                tache.MessagePublished += new CbSimpleStr(tache_MessagePublished);
                tache.EngineStatusChanged += new CbSimpleObj(tache_EngineStatusChanged);
            }
        }

        void tache_EngineStatusChanged(object obj)
        {
            this.EngineStatusChanged(obj);
        }       

        void tache_MessagePublished(string str)
        {
            this.MessagePublished(str);
        }

        void tache_ProgressPublished(int val, int total)
        {          
            this.PartProgressPublished(val, total);
        }

        void tache_IgnoredMessagePublished(string str)
        {
            this.IgnoredMessagePublished(str);
        } 
        #endregion

        #region GetEngineTache
        public IEngineTache GetEngineTache(int tacheID)
        {
            foreach (IEngineTache tache in this.engineTacheList)
            {
                if (tache.EngineTacheID == tacheID)
                {
                    return tache;
                }
            }

            return null;
        } 
        #endregion

        #region Excute
        public void Excute()
        {
            if (this.running)
            {
                return;
            }

            this.running = true;
            CbSimple cb = new CbSimple(this.Worker);
            cb.BeginInvoke(new AsyncCallback(WorkerCallBack), null);
        }

        private void WorkerCallBack(IAsyncResult res)
        {
            this.running = false ;
        }
        #endregion

        #region Worker
        private void Worker()
        {
            string failureCause = "";
            for (int i = 0; i < this.engineTacheList.Count; i++)
            {               
                this.currentTache = this.engineTacheList[i];

                this.TitleChanged(this.currentTache.Title);

                bool excuteSucceed = false;
                try
                {
                    excuteSucceed = this.currentTache.Excute(out failureCause);
                }
                catch(Exception ee)
                {
                    failureCause += string.Format("EngineTache({0}) Exception Message : {1}",this.currentTache.EngineTacheID , ee.Message); 
                }

                if (!excuteSucceed)
                {
                    if (this.hasNecceryEnder && (i != (this.engineTacheList.Count - 1)))
                    {
                        string cause = null;
                        this.currentTache = this.engineTacheList[this.engineTacheList.Count-1];
                        this.currentTache.Excute(out cause);
                    }
                    this.EngineDisruptted(failureCause);
                    return;
                }
            }           

            this.EngineCompleted();
        } 
        #endregion

        #region Pause
        public void Pause()
        {
            if (this.currentTache == null)
            {
                return;
            }

            this.currentTache.Pause();
        }        
        #endregion

        #region Continue
        public void Continue()
        {
            if (this.currentTache == null)
            {
                return;
            }

            this.currentTache.Continue();
        } 
        #endregion

        #region Stop
        public void Stop()
        {
            if (this.currentTache == null)
            {
                return;
            }

            this.currentTache.Stop();
        }   
        #endregion     

        #region Running
        private bool running = false;
        public bool Running
        {
            get
            {
                return this.running;
            }
        } 
        #endregion

        #endregion

        #region IEngineTacheUtil 成员
        private Hashtable htUtil = Hashtable.Synchronized(new Hashtable());

        public void RegisterObject(string name, object obj)
        {
            this.htUtil.Add(name, obj);
        }

        public object GetObject(string name)
        {
            return this.htUtil[name];
        }

        public void Remove(string name)
        {
            this.htUtil.Remove(name);
        }

        public void Clear()
        {
            this.htUtil.Clear();
        }

        #endregion
    }
}
