using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Threading.Timers.RichTimer
{
    /// <summary>
    /// RichTimerType ��ʱ������ 
    /// </summary>
    [EnumDescription("��ʱ������")]
    public enum RichTimerType
    {
        [EnumDescription("����ʱ" ,0)]
        None ,
        [EnumDescription("ÿСʱһ��" ,1)]
        PerHour,
        [EnumDescription("ÿ��һ��" ,2)]
        PerDay,
        [EnumDescription("ÿ��һ��" ,3)]
        PerWeek,
        [EnumDescription("ÿ��һ��" ,4)]
        PerMonth,       
        /// <summary>
        /// EverySpan ��TimerConfiguration��Hour��Minute��Second���Կ���Span������
        /// </summary>
        [EnumDescription("ÿ����һ��" ,5)]
        EverySpan,
        [EnumDescription("������Ŀ��ʱ��ִ��һ��",6)]
        JustOnce
    }
}
