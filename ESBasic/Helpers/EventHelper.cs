using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Loggers;

namespace ESBasic.Helpers
{
    /// <summary>
    /// EventHelper 只有当事件的声明是以CbGeneric以及其泛型类型为委托类型时，才可以使用EventHelper来安全触发事件。
    /// </summary>
    public static class EventHelper
    {
        #region SpringEventSafely
        public static void SpringEventSafelyAsyn(IAgileLogger agileLogger, string eventPath, Delegate theEvent)
        {
            if (theEvent == null)
            {
                return;
            }

            CbGeneric<IAgileLogger, string, Delegate> cb = new CbGeneric<IAgileLogger, string, Delegate>(EventHelper.SpringEventSafely);
            cb.BeginInvoke(agileLogger, eventPath, theEvent, null, null);
        }

        public static void SpringEventSafely(IAgileLogger agileLogger, string eventPath, Delegate theEvent)
        {
            if (theEvent == null)
            {
                return;
            }

            foreach (Delegate invocation in theEvent.GetInvocationList())
            {
                try
                {
                    CbGeneric cb = (CbGeneric)invocation;
                    cb();
                }
                catch (Exception ee)
                {
                    while (ee.InnerException != null)
                    {
                        ee = ee.InnerException;
                    }
                    agileLogger.Log(ee, string.Format("{0} On handle event [{1}].", ReflectionHelper.GetMethodFullName(invocation.Method), eventPath), ErrorLevel.Standard);
                }
            }
        } 
        #endregion

        #region SpringEventSafely<T1>
        public static void SpringEventSafelyAsyn<T1>(IAgileLogger agileLogger, string eventPath, Delegate theEvent, T1 t1)
        {
            if (theEvent == null)
            {
                return;
            }

            CbGeneric<IAgileLogger, string, Delegate, T1> cb = new CbGeneric<IAgileLogger, string, Delegate, T1>(EventHelper.SpringEventSafely<T1>);
            cb.BeginInvoke(agileLogger, eventPath, theEvent, t1, null, null);
        }

        public static void SpringEventSafely<T1>(IAgileLogger agileLogger, string eventPath, Delegate theEvent, T1 t1)
        {
            if (theEvent == null)
            {
                return;
            }

            foreach (Delegate invocation in theEvent.GetInvocationList())
            {
                try
                {
                    CbGeneric<T1> cb = (CbGeneric<T1>)invocation;
                    cb(t1);
                }
                catch (Exception ee)
                {
                    while (ee.InnerException != null)
                    {
                        ee = ee.InnerException;
                    }
                    agileLogger.Log(ee, string.Format("{0} On handle event [{1}].", ReflectionHelper.GetMethodFullName(invocation.Method), eventPath), ErrorLevel.Standard);
                }
            }
        } 
        #endregion

        #region SpringEventSafely<T1, T2>
        public static void SpringEventSafelyAsyn<T1, T2>(IAgileLogger agileLogger, string eventPath, Delegate theEvent, T1 t1, T2 t2)
        {
            if (theEvent == null)
            {
                return;
            }

            CbGeneric<IAgileLogger, string, Delegate, T1, T2> cb = new CbGeneric<IAgileLogger, string, Delegate, T1, T2>(EventHelper.SpringEventSafely<T1, T2>);
            cb.BeginInvoke(agileLogger, eventPath, theEvent, t1, t2, null, null);
        }

        public static void SpringEventSafely<T1, T2>(IAgileLogger agileLogger, string eventPath, Delegate theEvent, T1 t1, T2 t2)
        {
            if (theEvent == null)
            {
                return;
            }

            foreach (Delegate invocation in theEvent.GetInvocationList())
            {
                try
                {
                    CbGeneric<T1, T2> cb = (CbGeneric<T1, T2>)invocation;
                    cb(t1, t2);
                }
                catch (Exception ee)
                {
                    while (ee.InnerException != null)
                    {
                        ee = ee.InnerException;
                    }
                    agileLogger.Log(ee, string.Format("{0} On handle event [{1}].", ReflectionHelper.GetMethodFullName(invocation.Method), eventPath), ErrorLevel.Standard);
                }
            }
        } 
        #endregion

        #region SpringEventSafelyAsyn<T1, T2, T3>
        public static void SpringEventSafelyAsyn<T1, T2, T3>(IAgileLogger agileLogger, string eventPath, Delegate theEvent, T1 t1, T2 t2, T3 t3)
        {
            if (theEvent == null)
            {
                return;
            }

            CbGeneric<IAgileLogger, string, Delegate, T1, T2, T3> cb = new CbGeneric<IAgileLogger, string, Delegate, T1, T2, T3>(EventHelper.SpringEventSafely<T1, T2, T3>);
            cb.BeginInvoke(agileLogger, eventPath, theEvent, t1, t2, t3, null, null);
        }

        public static void SpringEventSafely<T1, T2, T3>(IAgileLogger agileLogger, string eventPath, Delegate theEvent, T1 t1, T2 t2, T3 t3)
        {
            if (theEvent == null)
            {
                return;
            }

            foreach (Delegate invocation in theEvent.GetInvocationList())
            {
                try
                {
                    CbGeneric<T1, T2, T3> cb = (CbGeneric<T1, T2, T3>)invocation;
                    cb(t1, t2, t3);
                }
                catch (Exception ee)
                {
                    while (ee.InnerException != null)
                    {
                        ee = ee.InnerException;
                    }
                    agileLogger.Log(ee, string.Format("{0} On handle event [{1}].", ReflectionHelper.GetMethodFullName(invocation.Method), eventPath), ErrorLevel.Standard);
                }
            }
        } 
        #endregion

        #region SpringEventSafely<T1, T2, T3 ,T4>
        public static void SpringEventSafelyAsyn<T1, T2, T3, T4>(IAgileLogger agileLogger, string eventPath, Delegate theEvent, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            if (theEvent == null)
            {
                return;
            }

            CbGeneric<IAgileLogger, string, Delegate, T1, T2, T3, T4> cb = new CbGeneric<IAgileLogger, string, Delegate, T1, T2, T3, T4>(EventHelper.SpringEventSafely<T1, T2, T3, T4>);
            cb.BeginInvoke(agileLogger, eventPath, theEvent, t1, t2, t3, t4, null, null);
        }

        public static void SpringEventSafely<T1, T2, T3, T4>(IAgileLogger agileLogger, string eventPath, Delegate theEvent, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            if (theEvent == null)
            {
                return;
            }

            foreach (Delegate invocation in theEvent.GetInvocationList())
            {
                try
                {
                    CbGeneric<T1, T2, T3, T4> cb = (CbGeneric<T1, T2, T3, T4>)invocation;
                    cb(t1, t2, t3, t4);
                }
                catch (Exception ee)
                {
                    while (ee.InnerException != null)
                    {
                        ee = ee.InnerException;
                    }
                    agileLogger.Log(ee, string.Format("{0} On handle event [{1}].", ReflectionHelper.GetMethodFullName(invocation.Method), eventPath), ErrorLevel.Standard);
                }
            }
        } 
        #endregion
    }

    
}
