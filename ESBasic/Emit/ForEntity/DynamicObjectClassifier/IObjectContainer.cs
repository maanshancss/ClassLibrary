using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Emit.ForEntity
{
    /// <summary>
    /// IObjectContainer 对象容器，动态的对象分类器最终将使用IObjectContainer来存储对象。
    /// 该接口的实现类最好支持可序列化。如此，动态对象分类器就可以支持序列化了。
    /// </summary>  
    public interface IObjectContainer<TEntity>
    {
        void Add(TEntity entity);
    }

    /// <summary>
    /// IObjectContainerCreator  动态的对象分类器借助IObjectContainerCreator来创建对象容器。
    /// </summary>    
    public interface IObjectContainerCreator<TEntity>
    {
        IObjectContainer<TEntity> CreateNewContainer();       
    }    
}
