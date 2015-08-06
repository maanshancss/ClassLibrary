using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Pool
{
    /// <summary>
    /// DefaultPooledObjectCreator 直接使用被池化类型的默认构造函数创建对象。
    /// zhuweisky 2008.06.13
    /// </summary>   
    public class DefaultPooledObjectCreator<TObject> : IPooledObjectCreator<TObject> where TObject : class
    {
        #region IPooledObjectCreator<TObject> 成员

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
