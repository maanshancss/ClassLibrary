using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace ESBasic.Emit.DynamicProxy
{
    /// <summary>
    /// InterceptedMethod ��װ���ػ�ķ����Ļ�����Ϣ��
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
        /// MethodName ���ػ��Ŀ�귽��
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
        /// Target ���ػ�ķ�����Ҫ���ĸ������ϵ��á�
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
        /// ���ñ��ػ�ķ����Ĳ�������
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
        /// ���ñ��ػ�ķ����Ĳ���ֵ
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
        /// ���Ŀ�귽��Ϊ���ͷ�����������Լ�¼���Ͳ��������͡�
        /// </summary>
        public Type[] GenericTypes
        {
            get { return genericTypes; }
            set { genericTypes = value; }
        } 
        #endregion
    }
}
