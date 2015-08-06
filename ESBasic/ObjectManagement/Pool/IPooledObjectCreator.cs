using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Pool
{
    /// <summary>
    /// IPooledObjectCreator 池化对象创建者。用于创建被池缓存的对象。并能清除对象的状态。
    /// zhuweisky 2008.06.13
    /// </summary>
    public interface IPooledObjectCreator<TObject> where TObject : class
    {
        TObject Create();
        void Reset(TObject obj);
    }
}
