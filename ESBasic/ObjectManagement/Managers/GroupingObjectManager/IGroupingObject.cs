using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Managers
{
    /// <summary>
    /// IGroupingObject �ܹ����������Ķ������ʵ�ֵĽӿڡ�
    /// </summary>   
    public interface IGroupingObject<TGroupKey,TObjectKey>
    {
        TObjectKey ID { get; }
        TGroupKey GroupID { get; }
    }
}
