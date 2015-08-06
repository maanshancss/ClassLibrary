using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using ESBasic.Loggers;

namespace ESBasic.Threading.Timers.RichTimer
{
    /// <summary>
    /// ITimerTaskManager 多功能定时任务管理器，管理所有的定时任务的执行
    /// 在Ctor中启动线程
    /// zhuweisky 2006.06
    /// </summary>
    public interface ITimerTaskManager :IDisposable
    {
        /// <summary>
        /// Initialize 初始化并启动内部定时器
        /// </summary>
        void Initialize();

        ILogger Logger { set; }
        int TimerSpanSecs { get;set; }

        void RemoveTimerTask(string timerName);
        void AddTimerTask(TimerTask task);
        TimerTask GetTimerTask(string timerName);

        IList<TimerTask> TimerTaskList { get; }

        event CbTimerTask TimerTaskExpired;
    }   

    public delegate void CbTimerTask(TimerTask task);  
}