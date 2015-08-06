using System;
using System.Collections.Generic;

namespace ESBasic.Threading.Application
{
    /// <summary>
    /// ICircleTaskSwitcher 循环任务切换器。
    /// 将一天24小时分为多个时段，在不同的时段，会有不同的任务。当到达任务切换点时，切换器会触发切换事件。
    /// zhuweisky 2008.12.29
    /// </summary>
    public interface ICircleTaskSwitcher<TTask>
    {
        /// <summary>
        /// TaskDictionary key为任务的起始点(hour)，value为对应的任务。
        /// </summary>
        IDictionary<ShortTime, TTask> TaskDictionary { get; set; }

        TTask CurrentTask { get; }

        void Initialize();
       
        /// <summary>
        /// TaskSwitched 当任务发生切换时，触发此事件，事件参数为刚得到控制权的任务。
        /// </summary>
        event CbGeneric<TTask> TaskSwitched;
    }
}
