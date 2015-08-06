using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ESBasic.ObjectManagement;

namespace ESBasic.Threading.Engines
{
    public class WorkerEngine<T> : IEngineActor, IWorkerEngine<T>
    {
        private AgileCycleEngine[] agileCycleEngines;
        private CircleQueue<T> queueOfWork ;
       
        #region Property
        #region MaxWaitWorkCount        
        /// <summary>
        /// MaxWaitWorkCount 历史中最大的处于等待状态的任务数量。
        /// </summary>
        public int MaxWaitWorkCount
        {
            get
            {
                if (this.queueOfWork == null)
                {
                    return 0;
                }

                return this.queueOfWork.MaxCount;
            }
        }
        #endregion

        #region WorkProcesser
        protected IWorkProcesser<T> workProcesser;
        /// <summary>
        /// WorkProcesser 用于处理任务的处理器。
        /// </summary>
        public IWorkProcesser<T> WorkProcesser
        {
            set { workProcesser = value; }
        }
        #endregion

        #region WorkerThreadCount
        private int workerThreadCount = 1;
        /// <summary>
        /// WorkerThreadCount 工作者线程的数量。默认值为1。
        /// </summary>
        public int WorkerThreadCount
        {
            get { return workerThreadCount; }
            set
            {
                if (workerThreadCount < 1)
                {
                    throw new Exception("The number of worker must be > 0 !");
                }
                workerThreadCount = value;
            }
        }
        #endregion

        #region WorkCount
        /// <summary>
        /// WorkCount 当前任务队列中的任务数。
        /// </summary>
        public int WorkCount
        {
            get
            {
                return this.queueOfWork.Count;
            }
        }
        #endregion

        #region IdleSpanInMSecs
        private int idleSpanInMSecs = 10;
        /// <summary>
        /// IdleSpanInMSecs 当没有工作要处理时，工作者线程休息的时间间隔。默认为10ms
        /// </summary>
        public int IdleSpanInMSecs
        {
            get { return idleSpanInMSecs; }
            set { idleSpanInMSecs = value; }
        }
        #endregion 
        #endregion

        #region Initialize
        public void Initialize()
        {
            this.Initialize(10000);
        }
        public void Initialize(int capacity)
        {
            this.queueOfWork = new CircleQueue<T>(capacity);
            this.agileCycleEngines = new AgileCycleEngine[this.workerThreadCount];
            for (int i = 0; i < this.agileCycleEngines.Length; i++)
            {
                this.agileCycleEngines[i] = new AgileCycleEngine(this);
                this.agileCycleEngines[i].DetectSpanInSecs = 0;
            }
        } 
        #endregion

        #region Start
        public void Start()
        {
            foreach (AgileCycleEngine engine in this.agileCycleEngines)
            {
                engine.Start();
            }
        } 
        #endregion

        #region Stop
        public void Stop()
        {
            foreach (AgileCycleEngine engine in this.agileCycleEngines)
            {
                engine.Stop();
            }
        } 
        #endregion

        #region AddWork
        /// <summary>
        /// AddWork 添加任务。
        /// </summary>
        public void AddWork(T work)
        {
            this.queueOfWork.Enqueue(work);           
        } 
        #endregion       

        #region DoWork
        private void DoWork()
        {
            T work = default(T);

            if (this.queueOfWork.Dequeue(out work))            
            {
                this.workProcesser.Process(work);
            }
            else
            {
                Thread.Sleep(this.idleSpanInMSecs);
            }
        } 
        #endregion

        #region IEngineActor 成员
        public bool EngineAction()
        {
            this.DoWork();
            return true;
        }
        #endregion
    }
}
