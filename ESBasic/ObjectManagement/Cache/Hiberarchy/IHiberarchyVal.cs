using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.ObjectManagement.Trees.Multiple;

namespace ESBasic.ObjectManagement.Cache
{
    /// <summary>
    /// IHiberarchyVal ���Դ����HiberarchyCache�Ķ������ʵ�ָýӿڡ�
    /// </summary>
    public interface IHiberarchyVal : IMTreeVal
    {
        string SequenceCode { get; }
    }
}
