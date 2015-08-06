using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using ESBasic.Loggers;

namespace ESBasic.Threading.Timers.RichTimer
{
    /// <summary>
    /// ITimerTaskManager �๦�ܶ�ʱ������������������еĶ�ʱ�����ִ��
    /// ��Ctor�������߳�
    /// zhuweisky 2006.06
    /// </summary>
    public interface ITimerTaskManager :IDisposable
    {
        /// <summary>
        /// Initialize ��ʼ���������ڲ���ʱ��
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