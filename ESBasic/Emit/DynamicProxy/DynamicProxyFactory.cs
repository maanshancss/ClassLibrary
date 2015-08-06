using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using ESBasic.Loggers;

namespace ESBasic.Emit.DynamicProxy
{
    /// <summary>
    /// DynamicProxyFactory ���ڴ����������Ͷ�̬����Ĺ������̰߳�ȫ��
    /// ��������ж�̬�������Ͷ��̳���MarshalByRefObject��
    /// zhuweisky 2008.03.01
    /// </summary>
    public static class DynamicProxyFactory
    {       
        private static EfficientAopProxyEmitter EfficientAopProxyEmitter = new EfficientAopProxyEmitter(false);
      
        public static TInterface CreateEfficientAopProxy<TInterface>(object origin, IAopInterceptor aopInterceptor)
        {
            lock (DynamicProxyFactory.EfficientAopProxyEmitter)
            {
                Type orignType = origin.GetType();
                Type dynamicType = DynamicProxyFactory.EfficientAopProxyEmitter.EmitProxyType<TInterface>(orignType);
                //DynamicProxyFactory.SimpleAopProxyEmitter.Save();
                ConstructorInfo ctor = dynamicType.GetConstructor(new Type[] { orignType, typeof(IAopInterceptor) });
                return (TInterface)ctor.Invoke(new object[] { origin, aopInterceptor});
            }
        }

        public static TInterface CreateEfficientAopProxy<TInterface>(TInterface origin, IAopInterceptor aopInterceptor)
        {
            lock (DynamicProxyFactory.EfficientAopProxyEmitter)
            {
                Type orignType = typeof(TInterface);
                Type dynamicType = DynamicProxyFactory.EfficientAopProxyEmitter.EmitProxyType<TInterface>(orignType);
                //DynamicProxyFactory.EfficientAopProxyEmitter.Save();
                ConstructorInfo ctor = dynamicType.GetConstructor(new Type[] { orignType, typeof(IAopInterceptor) });
                return (TInterface)ctor.Invoke(new object[] { origin, aopInterceptor });
            }
        }

        public static object CreateEfficientAopProxy(Type proxyIntfaceType, object origin, IAopInterceptor aopInterceptor)
        {
            lock (DynamicProxyFactory.EfficientAopProxyEmitter)
            {
                Type orignType = origin.GetType();
                Type dynamicType = DynamicProxyFactory.EfficientAopProxyEmitter.EmitProxyType(proxyIntfaceType ,orignType);
                //DynamicProxyFactory.SimpleAopProxyEmitter.Save();
                ConstructorInfo ctor = dynamicType.GetConstructor(new Type[] { orignType, typeof(IAopInterceptor) });
                return ctor.Invoke(new object[] { origin, aopInterceptor });
            }
        }             
    }
}
