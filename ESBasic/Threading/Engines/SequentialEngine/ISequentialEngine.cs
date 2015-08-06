using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Threading.Engines
{
    /// <summary>
    /// ISequentialEngine 用于将各个IEngineTache组装起来形成一个引擎，并顺序执行。同时传递各个环的事件信息
    /// </summary>
    public interface ISequentialEngine 
    {
        /// <summary>
        /// Initialize 初始化引擎。将调用每个引擎环的Initialize方法。
        /// </summary>
        void Initialize(IList<IEngineTache> tacheList, bool has_NecceryEnder);        

        /// <summary>
        /// Excute 启动引擎
        /// </summary>
        void Excute();

        /// <summary>
        /// Pause 暂停引擎
        /// </summary>
        void Pause();

        /// <summary>
        /// Continue 继续运行引擎
        /// </summary>
        void Continue();

        /// <summary>
        /// Stop 停止引擎
        /// </summary>
        void Stop();

        IEngineTache GetEngineTache(int tacheID);

        /// <summary>
        /// Running 引擎是否正在运行
        /// </summary>
        bool Running { get;}
       
        /// <summary>
        /// PartProgressPublished 单个IEngineTache的执行进度变化
        /// </summary>
        event CbProgress  PartProgressPublished;         

        /// <summary>
        /// TitleChanged 当紧接的下游引擎环被调用时，将发布该引擎环的Title
        /// </summary>
        event CbSimpleStr TitleChanged;

        /// <summary>
        /// EngineDisruptted 引擎任务被中断，事件参数说明中断原因
        /// </summary>
        event CbSimpleStr EngineDisruptted; 

        event CbSimple    EngineCompleted;
        event CbSimpleObj EngineStatusChanged;
        event CbSimpleStr MessagePublished;
        event CbSimpleStr IgnoredMessagePublished;
    }
}
