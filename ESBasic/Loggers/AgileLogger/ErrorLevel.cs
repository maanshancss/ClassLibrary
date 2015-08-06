using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Loggers
{
    [EnumDescription("异常/错误严重级别")]
    public enum ErrorLevel
    {
        [EnumDescription("致命的", 4)]
        Fatal,
        [EnumDescription("高", 3)]
        High,
        [EnumDescription("普通", 2)]
        Standard,
        [EnumDescription("低", 1)]
        Low
    }    
}
