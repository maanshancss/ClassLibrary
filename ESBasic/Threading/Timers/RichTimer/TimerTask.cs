using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Threading.Timers.RichTimer
{
    /// <summary>
    /// TimerTask 定时任务配置，在时间满足时将异步调用会调方法，目标方法应截获所有异常。
    /// zhuweisky 2006.06
    /// </summary>
    public class TimerTask
    {
        private CbRichTimer callBackHandler = null;
        private TimerConfiguration timerConfiguration;        

        #region Property
        #region Name
        private string name;
        public string Name
        {
            get
            {
                return this.name;
            }
        }
        #endregion

        #region Tag
        private object tag;
        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }
        #endregion            

        #region Enabled
        private bool enabled = true;
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        } 
        #endregion
        #endregion

        #region Ctor
        public TimerTask(string timerName, CbRichTimer handler ,TimerConfiguration config, object theTag)
        {
            this.callBackHandler = handler;
            this.timerConfiguration = config;
            this.name = timerName;
            this.tag = theTag;
        } 
        #endregion     

        #region IsExpired
        public bool IsExpired(DateTime now)
        {
            return this.timerConfiguration.IsExpired(now);
        }
        #endregion

        #region HaveATry
        public void HaveATry(int spanSecs, DateTime now)
        {
            if (!this.enabled)
            {
                return;
            }

            if (this.callBackHandler == null)
            {
                return;
            }

            if (this.timerConfiguration.IsOnTime(spanSecs ,now))
            {
                //this.callBackHandler(this.name, now, this.tag);
                this.callBackHandler.BeginInvoke(this.name, now, this.tag, null, null);
            }
        }        
        #endregion      
    } 

    public delegate void CbRichTimer(string timerName ,DateTime dt ,object tag) ; 
}
