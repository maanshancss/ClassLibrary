using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Managers
{
    /// <summary>
    /// IGroupingObject 能够被分组管理的对象必须实现的接口。
    /// </summary>   
    public interface IGroupingObject<TGroupKey,TObjectKey>
    {
        TObjectKey ID { get; }
        TGroupKey GroupID { get; }
    }
}
