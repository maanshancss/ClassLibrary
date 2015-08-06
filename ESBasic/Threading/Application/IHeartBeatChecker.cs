using System;
using ESBasic.Threading.Engines;

namespace ESBasic.Threading.Application
{
    /// <summary>
    /// IHeartBeatChecker 心跳监测器
    /// </summary>
    public interface IHeartBeatChecker 
    {
        /// <summary>
        /// SurviveSpanInSecs 在没有心跳到来时，可以存活的最长时间。SurviveSpanInSecs小于等于0，表示存活时间为无限长，而不需要进行心跳检查
        /// </summary>
        int SurviveSpanInSecs { get; set; }

        /// <summary>
        /// DetectSpanInSecs 隔多长时间进行一次状态检查。
        /// </summary>
        int DetectSpanInSecs { get;set; }

        /// <summary>
        /// Initialize 初始化并启动心跳监测器。
        /// </summary>
        void Initialize();

        /// <summary>
        /// RegisterOrActivate 注册一个新的客户端或激活它（收到心跳消息）。
        /// </summary>       
        void RegisterOrActivate(string id);

        /// <summary>
        /// Unregister 服务端主动取消对目标客户端的监测。
        /// </summary>        
        void Unregister(string id);

        /// <summary>
        /// Clear 清空所有的监测。
        /// </summary>
        void Clear();

        /// <summary>
        /// SomeOneTimeOuted  当在规定的时间内没有任何消息过来，那么将会触发该事件。
        /// 注意：该事件的处理函数严禁抛出任何异常。
        /// </summary>
        event CbSimpleStr SomeOneTimeOuted;
    }

    public class EmptyHeartBeatChecker : IHeartBeatChecker
    {
        #region IHeartBeatChecker 成员

        public int SurviveSpanInSecs
        {
            get
            {
                return 0;
            }
            set
            {
               
            }
        }

        public int DetectSpanInSecs
        {
            get
            {
                return 0;
            }
            set
            {
               
            }
        }

        public void Initialize()
        {
            
        }

        public void RegisterOrActivate(string id)
        {
           
        }

        public void Unregister(string id)
        {
            
        }

        public void Clear()
        {
            
        }

        public event CbSimpleStr SomeOneTimeOuted;

        #endregion
    }
 
}
