using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Loggers
{
    [EnumDescription("�쳣/�������ؼ���")]
    public enum ErrorLevel
    {
        [EnumDescription("������", 4)]
        Fatal,
        [EnumDescription("��", 3)]
        High,
        [EnumDescription("��ͨ", 2)]
        Standard,
        [EnumDescription("��", 1)]
        Low
    }    
}
