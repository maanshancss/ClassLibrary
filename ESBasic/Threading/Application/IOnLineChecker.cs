using System;
using System.Collections.Generic ;
using System.Threading ;
using ESBasic;
using ESBasic.Threading.Engines;

namespace ESBasic.Threading.Application
{
	/// <summary>
    /// IOnLineChecker 用于定时检查状态，如果在指定的时间间隔内没有激活id，则视为id超时。
    /// 注意：如果DetectSpanInSecs<=0表示不进行定时检查
	/// 作者：朱伟 sky.zhuwei@163.com 
	/// </summary>
    public interface IOnLineChecker : ICycleEngine
	{	
		void RegisterOrActivate(string id) ;
        void Unregister(string id); //被外界调用UnregisterUser时，不触发CheckSomeOneDisConnected事件

		event CbSimpleStr  SomeTimeOuted ; //仅是当定时检查出用户掉线时才触发
    }  
}
