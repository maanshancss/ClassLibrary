using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Emit.DynamicProxy
{
    /// <summary>
    /// IAOPInterceptor �Է������нػ񲢼���Ԥ����ͺ���Around����
    /// zhuweisky 2008.05.20
    /// </summary>
    public interface IAopInterceptor
    {
        void PreProcess(InterceptedMethod method);

        void PostProcess(InterceptedMethod method ,object returnVal);

        /// <summary>
        /// NewArounder ��ע�ⷵ��ֵ�ز���Ϊnull��
        /// </summary>        
        IArounder NewArounder();
    }

    #region EmptyAOPInterceptor
    public sealed class EmptyAopInterceptor : IAopInterceptor
    {
        #region IMethodInterceptor ��Ա

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
