using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Threading.Timers.RichTimer
{
    /// <summary>
    /// RichTimerType 定时器类型 
    /// </summary>
    [EnumDescription("定时器类型")]
    public enum RichTimerType
    {
        [EnumDescription("不定时" ,0)]
        None ,
        [EnumDescription("每小时一次" ,1)]
        PerHour,
        [EnumDescription("每天一次" ,2)]
        PerDay,
        [EnumDescription("每周一次" ,3)]
        PerWeek,
        [EnumDescription("每月一次" ,4)]
        PerMonth,       
        /// <summary>
        /// EverySpan 在TimerConfiguration将Hour、Minute、Second属性看作Span的设置
        /// </summary>
        [EnumDescription("每周期一次" ,5)]
        EverySpan,
        [EnumDescription("仅仅在目标时间执行一次",6)]
        JustOnce
    }
}
