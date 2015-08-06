using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Pool
{
    /// <summary>
    /// DefaultPooledObjectCreator ֱ��ʹ�ñ��ػ����͵�Ĭ�Ϲ��캯����������
    /// zhuweisky 2008.06.13
    /// </summary>   
    public class DefaultPooledObjectCreator<TObject> : IPooledObjectCreator<TObject> where TObject : class
    {
        #region IPooledObjectCreator<TObject> ��Ա

        public TObject Create()
        {
            return Activator.CreateInstance<TObject>();
        }

        public void Reset(TObject obj)
        {
           
        }

        #endregion
    }
}
