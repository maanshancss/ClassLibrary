using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Threading.Engines
{
    /// <summary>
    /// ICycleEngine 在后台线程中进行间隔循环的引擎
    /// zhuweisky 2006.12.21
    /// </summary>
    public interface ICycleEngine
    {
        /// <summary>
        /// Start 启动后台引擎线程
        /// </summary>
        void Start();

        /// <summary>
        /// Stop 停止后台引擎线程，只有当线程安全退出后，该方法才返回
        /// </summary>
        void Stop();

        /// <summary>
        /// IsRunning 引擎是否运行中
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// DetectSpanInSecs 引擎进行轮询的间隔，DetectSpanInSecs=0，表示无间隙运作引擎；DetectSpanInSecs小于0则表示不使用引擎。默认值为0。
        /// </summary>
        int DetectSpanInSecs { get;set; }

        /// <summary>
        /// EngineStopped 当引擎由运行变为停止状态时，将触发此事件。如果是异常停止，则事件参数为异常对象，否则，事件参数为null。
        /// </summary>
        event CbGeneric<Exception> EngineStopped;
    }    
}
