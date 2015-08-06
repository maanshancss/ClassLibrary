using System;
using ESBasic.Threading.Engines;

namespace ESBasic.Threading.Timers
{
    /// <summary>
    /// ICallbackTimer 回调定时器。
    /// 注意：回调任务会异步在ThreadPool的WorkerThread上执行。即使目标任务抛出异常也不会影响INotifyTimer的继续运行。
    /// </summary>    
    public interface ICallbackTimer<T> : ICycleEngine
    {
        int TaskCount { get; }

        /// <summary>
        /// AddCallback 添加一个回调任务。目标任务会在spanInSecs后运行。仅仅运行一次。
        /// </summary>
        /// <param name="spanInSecs">多少秒后执行任务</param>
        /// <param name="_callback">目标方法的委托</param>
        /// <param name="_callbackPara">调用目标方法的参数</param>
        /// <returns>新的任务编号</returns>     
        int AddCallback(int spanInSecs, CbGeneric<T> _callback, T _callbackPara);

        /// <summary>
        /// RemoveCallback 删除目标回调任务。
        /// </summary>        
        void RemoveCallback(int taskID);

        /// <summary>
        /// RemoveCallbackAndAddNew 删除目标回调任务，并添加一个新的回调任务。
        /// </summary>
        int RemoveCallbackAndAddNew(int taskIDToRemoved, int spanInSecs, CbGeneric<T> _newCallback, T _newCallbackPara);
        
        /// <summary>
        /// GetLeftSeconds 离目标任务被回调执行还有多长时间（s）。返回0，表示任务不存在或者任务已经被执行。
        /// </summary>       
        int GetLeftSeconds(int taskID);

        /// <summary>
        /// Clear 清除所有回调任务。
        /// </summary>
        void Clear();
    }  
}
