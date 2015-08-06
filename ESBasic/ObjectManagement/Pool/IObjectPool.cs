using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Pool
{
    /// <summary>
    /// IObjectPool 对象池。
    /// zhuweisky 2008.06.13
    /// </summary>
    public interface IObjectPool<TObject> where TObject : class
    {
        /// <summary>
        /// MinObjectCount 对象池中最少同时存在的对象数。
        /// </summary>
        int MinObjectCount { get;set; }

        /// <summary>
        /// MaxObjectCount 对象池中最多同时存在的对象数。
        /// </summary>
        int MaxObjectCount { get;set; }

        /// <summary>
        /// DetectSpanInMSecs 当池中没有空闲的对象且数量已达到MaxObjectCount时，如果这时发生Rent调用，则检测空闲对象的时间间隔。
        /// 默认值为10ms。 
        /// </summary>
        int DetectSpanInMSecs { get;set; }

        /// <summary>
        /// PooledObjectCreator 用于创建池中对象的创建器。默认为DefaultPooledObjectCreator
        /// </summary>
        IPooledObjectCreator<TObject> PooledObjectCreator { set; }

        void Initialize();

        TObject Rent();
        void GiveBack(TObject obj);
    }
}
