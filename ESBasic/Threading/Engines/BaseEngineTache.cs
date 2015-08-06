using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Engine
{
    /// <summary>
    /// BaseEngineTache 引擎环的抽象基类，具体的引擎环从BaseEngineTache继承并覆写Abstract方法即可
    /// </summary>
    public abstract class BaseEngineTache :IEngineTache
    {
        protected volatile bool goStop = false;
        protected volatile bool paused = false;
        protected bool isActive = false;
        protected IEngineTacheUtil engineTacheUtil;

        #region event
        public abstract event CbProgress ProgressPublished;
        public abstract event CbSimpleStr MessagePublished;
        public abstract event CbSimpleStr IgnoredMessagePublished;
        public abstract event CbSimpleObj EngineStatusChanged;
        #endregion

        #region InitializeEventHandler
        private void InitializeEventHandler()
        {
            this.ProgressPublished += delegate { };
            this.MessagePublished += delegate { };
            this.IgnoredMessagePublished += delegate { };
            this.EngineStatusChanged += delegate { };
        }
        #endregion

        #region Initialize
        public virtual void Initialize(IEngineTacheUtil util)
        {
            this.engineTacheUtil = util;
            this.InitializeEventHandler();
        } 
        #endregion        

        #region Stop ,Pause ,Continue
        public void Stop()
        {
            this.goStop = true;
        }

        public void Pause()
        {
            this.paused = true;
        }

        public void Continue()
        {
            this.paused = false;
        }
        #endregion

        #region IsActive
       
        public bool IsActive
        {
            get { return this.isActive; }
        }
        #endregion

        #region Abstract     
        public abstract bool   Excute(out string failureCause);
        public abstract string Title { get;}
        public abstract int    EngineTacheID { get;}

        #endregion 
    }
}
