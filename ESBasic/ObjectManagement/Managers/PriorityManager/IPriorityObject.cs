using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Managers
{
    /// <summary>
    /// IPriorityObject 具有优先级的对象的接口。
    /// </summary>
    public interface IPriorityObject
    {        
        int PriorityLevel { get; }
    }
}
