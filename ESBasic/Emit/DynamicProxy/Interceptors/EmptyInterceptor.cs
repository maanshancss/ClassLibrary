using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Emit.DynamicProxy
{
    /// <summary>
    /// 不执行任何动作的Interceptor。存在的主要目的，是可以将目标对象适配到其没有实现的接口（前提，接口中的方法在目标对象中都有对应的匹配）。
    /// </summary>
    public sealed class EmptyInterceptor : IAopInterceptor, IArounder
    {
        public void PreProcess(InterceptedMethod method)
        {
           
        }

        public void PostProcess(InterceptedMethod method, object returnVal)
        {
            
        }

        public IArounder NewArounder()
        {
            return this;
        }

        public void BeginAround(InterceptedMethod method)
        {
            
        }

        public void EndAround(object returnVal)
        {
            
        }

        public void OnException(InterceptedMethod method, Exception ee)
        {
            
        }
    }
}
