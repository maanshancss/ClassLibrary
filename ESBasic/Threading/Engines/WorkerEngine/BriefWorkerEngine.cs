using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Threading.Engines
{
    /// <summary>
    /// BriefWorkerEngine 精简的工作者引擎。它使用多线程处理一批任务，这批任务处理结束后，工作者线程会被自动释放，而该引擎实例也就可以被结束了。
    /// 与WorkerEngine的区别在于，WorkerEngine是在系统运行期间一直处于工作状态的。
    /// </summary>    
    public sealed class BriefWorkerEngine<T> : IEngineActor ,IDisposable
    {
        private AgileCycleEngine[] agileCycleEngines;
        private IWorkProcesser<T> workProcesser;
        private IList<T> workList;
        private int currentWorkIndex = -1;
        private object locker = new object();

        #region Ctor
        public BriefWorkerEngine(IWorkProcesser<T> processer, int workerCount, IEnumerable<T> works)
        {
            if (processer == null || workerCount <= 0 || works == null)
            {
                throw new Exception("Parameter not valid !");
            }

            this.workProcesser = processer;
            this.workList = ESBasic.Collections.CollectionConverter.CopyAllToList<T>(works);

            this.agileCycleEngines = new AgileCycleEngine[workerCount];
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

        #region IsFinished
        public bool IsFinished()
        {
            foreach (AgileCycleEngine engine in this.agileCycleEngines)
            {
                if (engine.IsRunning)
                {
                    return false;
                }
            }

            return true;
        } 
        #endregion

        #region GetNextWork
        private bool GetNextWork(out T work)
        {
            work = default(T);
            lock (this.locker)
            {
                if (this.currentWorkIndex >= this.workList.Count - 1)
                {
                    return false;
                }

                ++this.currentWorkIndex;
                work = this.workList[this.currentWorkIndex];

                return true;
            }
        } 
        #endregion

        #region IEngineActor 成员

        public bool EngineAction()
        {
            T work = default(T);
            bool haveWork = this.GetNextWork(out work);
            if (! haveWork)
            {
                return false;
            }

            this.workProcesser.Process(work);
            return true;
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            foreach (AgileCycleEngine engine in this.agileCycleEngines)
            {
                engine.Stop();
            }
        }

        #endregion
    }
}
