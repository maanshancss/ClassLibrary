using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Pool
{
    /// <summary>
    /// IPooledObjectCreator �ػ����󴴽��ߡ����ڴ������ػ���Ķ��󡣲�����������״̬��
    /// zhuweisky 2008.06.13
    /// </summary>
    public interface IPooledObjectCreator<TObject> where TObject : class
    {
        TObject Create();
        void Reset(TObject obj);
    }
}
