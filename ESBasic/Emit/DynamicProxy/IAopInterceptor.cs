using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Emit.DynamicProxy
{
    /// <summary>
    /// IAOPInterceptor 对方法进行截获并加入预处理和后处理、Around处理。
    /// zhuweisky 2008.05.20
    /// </summary>
    public interface IAopInterceptor
    {
        void PreProcess(InterceptedMethod method);

        void PostProcess(InterceptedMethod method ,object returnVal);

        /// <summary>
        /// NewArounder 请注意返回值必不能为null。
        /// </summary>        
        IArounder NewArounder();
    }

    #region EmptyAOPInterceptor
    public sealed class EmptyAopInterceptor : IAopInterceptor
    {
        #region IMethodInterceptor 成员

        public void PreProcess(InterceptedMethod method)
        {

        }

        public void PostProcess(InterceptedMethod method, object returnVal)
        {

        }
        

        public IArounder NewArounder()
        {
            return new EmptyArounder();
        }

        #endregion
    } 
    #endregion
}
