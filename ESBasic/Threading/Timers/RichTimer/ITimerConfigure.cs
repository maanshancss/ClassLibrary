using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Threading.Timers.RichTimer
{
    /// <summary>
    /// ITimerConfigure UIʵ�ִ˽ӿ����ṩ��TimerConfiguration�����á�
    /// zhuweisky 2006.06
    /// </summary>
    public interface ITimerConfigure
    {
        TimerConfiguration TimerConfiguration { get;set; }
    }
}
