using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Threading.Timers.RichTimer
{
    /// <summary>
    /// ITimerConfigure UI实现此接口以提供对TimerConfiguration的设置。
    /// zhuweisky 2006.06
    /// </summary>
    public interface ITimerConfigure
    {
        TimerConfiguration TimerConfiguration { get;set; }
    }
}
