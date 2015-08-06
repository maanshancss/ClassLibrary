using System;

namespace ESBasic.Threading.Engines
{
    /// <summary>
    /// IWorkerEngine 工作者引擎。用于在后台使用多线程不间断、连续地处理任务。
    /// 一群工作者线程轮流从工作队列中取出工作进行处理，模仿完成端口的机制。
    /// </summary>    
    public interface IWorkerEngine<T>
    {
        /// <summary>
        /// IdleSpanInMSecs 当没有工作要处理时，工作者线程休息的时间间隔。默认为10ms
        /// </summary>
        int IdleSpanInMSecs { get;set; }

        /// <summary>
        /// WorkerThreadCount 工作者线程的数量。默认值为1。
        /// </summary>
        int WorkerThreadCount { get; set; }

        /// <summary>
        /// WorkProcesser 用于处理任务的处理器。
        /// </summary>
        IWorkProcesser<T> WorkProcesser { set; }
        
        /// <summary>
        /// WorkCount 当前任务队列中的任务数。
        /// </summary>
        int WorkCount { get; }

        /// <summary>
        /// MaxWaitWorkCount 历史中最大的处于等待状态的任务数量。
        /// </summary>
        int MaxWaitWorkCount { get; }

        void Initialize();
        void Start();
        void Stop();

        /// <summary>
        /// AddWork 添加任务。
        /// </summary>       
        void AddWork(T work); 
    }
}
