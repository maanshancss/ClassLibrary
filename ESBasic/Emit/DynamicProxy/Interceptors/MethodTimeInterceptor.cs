using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using ESBasic.ObjectManagement.Pool;
using ESBasic.Loggers;

namespace ESBasic.Emit.DynamicProxy
{
    /// <summary>
    /// MethodTimeInterceptor ��AroundInterceptor���ڼ��Ŀ�귽��ִ�е�ʱ�䡣
    /// zhuweisky 2008.06.13
    /// </summary>
    public sealed class MethodTimeInterceptor : IAopInterceptor, IPooledObjectCreator<TimeLogArounder>
    {
        private IObjectPool<TimeLogArounder> timeLogArounderPool = new ObjectPool<TimeLogArounder>();

        #region MethodTimeLogger
        private IMethodTimeLogger methodTimeLogger;
        public IMethodTimeLogger MethodTimeLogger
        {
            set { methodTimeLogger = value; }
        } 
        #endregion

        #region Ctor
        public MethodTimeInterceptor() { }
        public MethodTimeInterceptor(string logFilePath, int minSpanInMSecsToLog)
            : this(new MethodTimeFileLogger(new FileAgileLogger(logFilePath), minSpanInMSecsToLog))
        {
        }

        public MethodTimeInterceptor(IAgileLogger logger, int minSpanInMSecsToLog)
            : this(new MethodTimeFileLogger(logger, minSpanInMSecsToLog))
        {            
        }
        public MethodTimeInterceptor(IMethodTimeLogger logger)
        {
            this.methodTimeLogger = logger;
            this.timeLogArounderPool.MinObjectCount = 1;
            this.timeLogArounderPool.PooledObjectCreator = this;
            this.timeLogArounderPool.Initialize();
        } 
        #endregion      

        #region IPooledObjectCreator<TimeLogArounder> ��Ա

        public TimeLogArounder Create()
        {
            return new TimeLogArounder(this.timeLogArounderPool ,this.methodTimeLogger);
        }

        public void Reset(TimeLogArounder obj)
        {
            
        }

        #endregion

        #region IAopInterceptor ��Ա

        public void PreProcess(InterceptedMethod method)
        {
            
        }

        public void PostProcess(InterceptedMethod method, object returnVal)
        {
           
        }

        public IArounder NewArounder()
        {
            return this.timeLogArounderPool.Rent();
        }

        #endregion
    }

    #region TimeLogArounder
    public sealed class TimeLogArounder : IArounder
    {
        private Stopwatch stopwatch = new Stopwatch();
        private IMethodTimeLogger methodTimeLogger;
        private InterceptedMethod interceptedMethod;
        private IObjectPool<TimeLogArounder> objectPool;

        public TimeLogArounder(IObjectPool<TimeLogArounder> pool, IMethodTimeLogger logger)
        {
            this.objectPool = pool;
            this.methodTimeLogger = logger;
        }

        #region IArounder ��Ա

        public void BeginAround(InterceptedMethod method)
        {
            this.interceptedMethod = method;           

            this.stopwatch.Reset();
            this.stopwatch.Start();
        }

        public void EndAround(object returnVal)
        {
            this.stopwatch.Stop();
            string methodPath = string.Format("{0}.{1}", this.interceptedMethod.Target.GetType().ToString() , this.interceptedMethod.MethodName);
            this.methodTimeLogger.Log(methodPath, this.interceptedMethod.GenericTypes, this.interceptedMethod.ArgumentNames, this.interceptedMethod.ArgumentValues, this.stopwatch.ElapsedMilliseconds);

            this.stopwatch.Reset();
            this.objectPool.GiveBack(this);
        }

        public void OnException(InterceptedMethod method, Exception ee)
        {
            this.stopwatch.Stop();
            this.stopwatch.Reset();
            this.objectPool.GiveBack(this);
        }

        #endregion
    }
    #endregion
   
}
