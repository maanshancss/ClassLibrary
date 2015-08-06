using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Loggers;

namespace ESBasic.Helpers
{
    /// <summary>
    /// 只有当事件的声明是以CbGeneric以及其泛型类型为委托类型时，才可以使用EventSafeTrigger来安全触发事件。
    /// </summary>
    public class EventSafeTrigger
    {
        #region AgileLogger
        private IAgileLogger agileLogger = new EmptyAgileLogger();
        public IAgileLogger AgileLogger
        {
            set { agileLogger = value; }
        }
        #endregion

        #region PublisherFullName
        private string publisherFullName = "";
        public string PublisherFullName
        {
            get { return publisherFullName; }
            set { publisherFullName = value; }
        }
        #endregion

        #region Ctor
        public EventSafeTrigger() { }
        public EventSafeTrigger(IAgileLogger logger, string publisherTypeFullName)
        {
            this.agileLogger = logger;
            this.publisherFullName = publisherTypeFullName;
        }
        #endregion

        #region Action
        public void ActionAsyn(string eventName, Delegate theEvent)
        {
            string eventPath = string.Format("{0}.{1}", this.publisherFullName, eventName);
            EventHelper.SpringEventSafelyAsyn(this.agileLogger, eventPath, theEvent);
        }

        public void Action(string eventName, Delegate theEvent)
        {
            string eventPath = string.Format("{0}.{1}", this.publisherFullName, eventName);
            EventHelper.SpringEventSafely(this.agileLogger, eventPath, theEvent);
        }
        #endregion

        #region Action<T1>
        public void ActionAsyn<T1>(string eventName, Delegate theEvent, T1 t1)
        {
            string eventPath = string.Format("{0}.{1}", this.publisherFullName, eventName);
            EventHelper.SpringEventSafelyAsyn<T1>(this.agileLogger, eventPath, theEvent, t1);
        }

        public void Action<T1>(string eventName, Delegate theEvent, T1 t1)
        {
            string eventPath = string.Format("{0}.{1}", this.publisherFullName, eventName);
            EventHelper.SpringEventSafely<T1>(this.agileLogger, eventPath, theEvent, t1);
        }
        #endregion

        #region Action<T1, T2>
        public void ActionAsyn<T1, T2>(string eventName, Delegate theEvent, T1 t1, T2 t2)
        {
            string eventPath = string.Format("{0}.{1}", this.publisherFullName, eventName);
            EventHelper.SpringEventSafelyAsyn<T1, T2>(this.agileLogger, eventPath, theEvent, t1, t2);
        }

        public void Action<T1, T2>(string eventName, Delegate theEvent, T1 t1, T2 t2)
        {
            string eventPath = string.Format("{0}.{1}", this.publisherFullName, eventName);
            EventHelper.SpringEventSafely<T1, T2>(this.agileLogger, eventPath, theEvent, t1, t2);
        }
        #endregion

        #region Action<T1, T2, T3>
        public void ActionAsyn<T1, T2, T3>(string eventName, Delegate theEvent, T1 t1, T2 t2, T3 t3)
        {
            string eventPath = string.Format("{0}.{1}", this.publisherFullName, eventName);
            EventHelper.SpringEventSafelyAsyn<T1, T2, T3>(this.agileLogger, eventPath, theEvent, t1, t2, t3);
        }

        public void Action<T1, T2, T3>(string eventName, Delegate theEvent, T1 t1, T2 t2, T3 t3)
        {
            string eventPath = string.Format("{0}.{1}", this.publisherFullName, eventName);
            EventHelper.SpringEventSafely<T1, T2, T3>(this.agileLogger, eventPath, theEvent, t1, t2, t3);
        }
        #endregion

        #region Action<T1, T2, T3 ,T4>
        public void ActionAsyn<T1, T2, T3, T4>(string eventName, Delegate theEvent, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            string eventPath = string.Format("{0}.{1}", this.publisherFullName, eventName);
            EventHelper.SpringEventSafelyAsyn<T1, T2, T3, T4>(this.agileLogger, eventPath, theEvent, t1, t2, t3, t4);
        }

        public void Action<T1, T2, T3, T4>(string eventName, Delegate theEvent, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            string eventPath = string.Format("{0}.{1}", this.publisherFullName, eventName);
            EventHelper.SpringEventSafely<T1, T2, T3, T4>(this.agileLogger, eventPath, theEvent, t1, t2, t3, t4);
        }
        #endregion
    }
}
