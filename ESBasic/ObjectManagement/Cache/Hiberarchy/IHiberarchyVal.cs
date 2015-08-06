using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.ObjectManagement.Trees.Multiple;

namespace ESBasic.ObjectManagement.Cache
{
    /// <summary>
    /// IHiberarchyVal 可以存放在HiberarchyCache的对象必须实现该接口。
    /// </summary>
    public interface IHiberarchyVal : IMTreeVal
    {
        string SequenceCode { get; }
    }
}
