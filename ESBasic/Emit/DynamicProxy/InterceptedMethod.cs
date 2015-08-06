using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace ESBasic.Emit.DynamicProxy
{
    /// <summary>
    /// InterceptedMethod 封装被截获的方法的基本信息。
    /// zhuweisky 2008.05.20
    /// </summary>
    public sealed class InterceptedMethod
    {
        #region Ctor
        public InterceptedMethod() { }
        public InterceptedMethod(object _target, string _method,Type[] _genericTypes,string[] paraNames, object[] paraValues)
        {
            this.target = _target;
            this.methodName = _method;
            this.genericTypes = _genericTypes;
            this.argumentNames = paraNames;
            this.argumentValues = paraValues;          
        } 
        #endregion
        
        #region MethodName
        private string methodName;
        /// <summary>
        /// MethodName 被截获的目标方法
        /// </summary>
        public string MethodName
        {
            get { return methodName; }
            set { methodName = value; }
        }
        #endregion

        #region Target
        private object target;
        /// <summary>
        /// Target 被截获的方法需要在哪个对象上调用。
        /// </summary>
        public object Target
        {
            get { return target; }
            set { target = value; }
        } 
        #endregion

        #region ArgumentNames
        private string[] argumentNames;
        /// <summary>
        /// 调用被截获的方法的参数名称
        /// </summary>
        public string[] ArgumentNames
        {
            get { return argumentNames; }
            set { argumentNames = value; }
        } 
        #endregion

        #region ArgumentValues
        private object[] argumentValues;
        /// <summary>
        /// 调用被截获的方法的参数值
        /// </summary>
        public object[] ArgumentValues
        {
            get { return argumentValues; }
            set { argumentValues = value; }
        } 
        #endregion     

        #region GenericTypes
        private Type[] genericTypes = null;
        /// <summary>
        /// 如果目标方法为泛型方法，则该属性记录泛型参数的类型。
        /// </summary>
        public Type[] GenericTypes
        {
            get { return genericTypes; }
            set { genericTypes = value; }
        } 
        #endregion
    }
}
