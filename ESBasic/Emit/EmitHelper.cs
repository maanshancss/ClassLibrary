using System;
using System.Reflection.Emit;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Emit
{
    /// <summary>
    /// EmitHelper zhuweisky 2007.0725
    /// </summary>
    public static class EmitHelper
    {
        private static MethodInfo GetTypeFromHandleMethodInfo;
        private static MethodInfo MakeByRefTypeMethodInfo;

        #region static Ctor
        static EmitHelper()
        {
            EmitHelper.GetTypeFromHandleMethodInfo = typeof(Type).GetMethod("GetTypeFromHandle", new Type[] { typeof(RuntimeTypeHandle) });
            EmitHelper.MakeByRefTypeMethodInfo = typeof(Type).GetMethod("MakeByRefType");
        } 
        #endregion

        #region GetParametersType
        /// <summary>
        /// GetParametersType ��ȡ�����Ĳ�������
        /// </summary>       
        public static Type[] GetParametersType(MethodInfo method)
        {
            ParameterInfo[] paras = method.GetParameters();
            Type[] paraTypes = new Type[paras.Length];
            for (int i = 0; i < paras.Length; i++)
            {
                paraTypes[i] = paras[i].ParameterType;
            }

            return paraTypes;
        } 
        #endregion

        #region LoadArg
        /// <summary>
        /// LoadArg ���ط����Ĳ���
        /// </summary>      
        public static void LoadArg(ILGenerator gen, int index)
        {
            switch (index)
            {
                case 0:
                    gen.Emit(OpCodes.Ldarg_0);
                    break;
                case 1:
                    gen.Emit(OpCodes.Ldarg_1);
                    break;
                case 2:
                    gen.Emit(OpCodes.Ldarg_2);
                    break;
                case 3:
                    gen.Emit(OpCodes.Ldarg_3);
                    break;
                default:
                    if (index < 128)
                    {
                        gen.Emit(OpCodes.Ldarg_S, index);
                    }
                    else
                    {
                        gen.Emit(OpCodes.Ldarg, index);
                    }
                    break;
            }
        } 
        #endregion

        #region ConvertTopArgType
        /// <summary>
        /// ConvertTopArgType ��������ת���Ĵ��룬����ջ�����Ĳ���ת��ΪĿ�����͡�
        /// </summary>        
        public static void ConvertTopArgType(ILGenerator ilGenerator, Type source, Type dest)
        {           
            if (!dest.IsAssignableFrom(source))
            {
                if (dest.IsClass)
                {
                    if (source.IsValueType)
                    {
                        ilGenerator.Emit(OpCodes.Box, dest);
                    }
                    else
                    {
                        ilGenerator.Emit(OpCodes.Castclass, dest);
                    }
                }
                else
                {
                    if (source.IsValueType)
                    {
                        ilGenerator.Emit(OpCodes.Castclass, dest);
                    }
                    else
                    {
                        ilGenerator.Emit(OpCodes.Unbox_Any, dest);
                    }
                }
            }
        } 
        #endregion

        #region GenericParameterNames
        /// <summary>
        /// GetGenericParameterNames ��ȡĿ�귽���ķ��Ͳ��������ơ�
        /// </summary>        
        public static string[] GetGenericParameterNames(MethodInfo method)
        {
            Type[] genericParaTypes = method.GetGenericArguments();
            string[] genericParaNames = new string[genericParaTypes.Length];
            for (int j = 0; j < genericParaNames.Length; j++)
            {
                genericParaNames[j] = genericParaTypes[j].Name;
            }

            return genericParaNames;
        } 
        #endregion

        #region DefineDerivedMethod
        /// <summary>
        /// DefineDerivedMethodSignature ���嶯̬���з�����ǩ����֧�ַ��ͷ�����
        /// </summary>      
        public static MethodBuilder DefineDerivedMethodSignature(TypeBuilder typeBuilder, MethodInfo baseMethod)
        {
            Type[] argTypes = EmitHelper.GetParametersType(baseMethod);
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(baseMethod.Name, baseMethod.Attributes & ~MethodAttributes.Abstract, baseMethod.ReturnType, argTypes);

            #region GenericMethod
            if (baseMethod.IsGenericMethod)
            {
                Type[] genericParaTypes = baseMethod.GetGenericArguments();
                string[] genericParaNames = EmitHelper.GetGenericParameterNames(baseMethod);
                GenericTypeParameterBuilder[] genericTypeParameterBuilders = methodBuilder.DefineGenericParameters(genericParaNames);
                for (int i = 0; i < genericTypeParameterBuilders.Length; i++)
                {
                    genericTypeParameterBuilders[i].SetInterfaceConstraints(genericParaTypes[i].GetGenericParameterConstraints());
                }
            }
            #endregion

            return methodBuilder;
        }
        #endregion

        #region Ldind
        /// <summary>
        /// Ldind ��Ӽ��أ����ӵ�ַ����[type����]�Ķ��󣩡���֧��decimal����
        /// </summary>       
        public static void Ldind(ILGenerator ilGenerator, Type type)
        {
            if (!type.IsValueType)
            {
                ilGenerator.Emit(OpCodes.Ldind_Ref);
                return;
            }

            if (type.IsEnum)
            {
                Type underType = Enum.GetUnderlyingType(type);
                EmitHelper.Ldind(ilGenerator, underType);
                return;
            }

            if (type == typeof(Int64))
            {
                ilGenerator.Emit(OpCodes.Ldind_I8);
                return;
            }

            if (type == typeof(Int32))
            {
                ilGenerator.Emit(OpCodes.Ldind_I4);
                return;
            }

            if (type == typeof(Int16))
            {
                ilGenerator.Emit(OpCodes.Ldind_I2);
                return;
            }

            if (type == typeof(Byte))
            {
                ilGenerator.Emit(OpCodes.Ldind_U1);
                return;
            }

            if (type == typeof(SByte))
            {
                ilGenerator.Emit(OpCodes.Ldind_I1);
                return;
            }

            if (type == typeof(Boolean))
            {
                ilGenerator.Emit(OpCodes.Ldind_I1);
                return;
            }

            if (type == typeof(UInt64))
            {
                ilGenerator.Emit(OpCodes.Ldind_I8);
                return;
            }

            if (type == typeof(UInt32))
            {
                ilGenerator.Emit(OpCodes.Ldind_U4);
                return;
            }

            if (type == typeof(UInt16))
            {
                ilGenerator.Emit(OpCodes.Ldind_U2);
                return;
            }

            if (type == typeof(Single))
            {
                ilGenerator.Emit(OpCodes.Ldind_R4);
                return;
            }

            if (type == typeof(Double))
            {
                ilGenerator.Emit(OpCodes.Ldind_R8);
                return;
            }

            if (type == typeof(System.IntPtr))
            {
                ilGenerator.Emit(OpCodes.Ldind_I4);
                return;
            }

            if (type == typeof(System.UIntPtr))
            {
                ilGenerator.Emit(OpCodes.Ldind_I4);
                return;
            }

            throw new Exception(string.Format("The target type:{0} is not supported by EmitHelper.Ldind()" ,type));
        } 
        #endregion

        #region Stind
        /// <summary>
        /// Stind ��Ӵ洢�����洢[type����]�Ķ����ַ��������Ӵ洢����֧��decimal����
        /// </summary>      
        public static void Stind(ILGenerator ilGenerator, Type type)
        {
            if (!type.IsValueType)
            {
                ilGenerator.Emit(OpCodes.Stind_Ref);
                return;
            }

            if (type.IsEnum)
            {
                Type underType = Enum.GetUnderlyingType(type);
                EmitHelper.Stind(ilGenerator, underType);
                return;
            }

            if (type == typeof(Int64))
            {
                ilGenerator.Emit(OpCodes.Stind_I8);
                return;
            }

            if (type == typeof(Int32))
            {
                ilGenerator.Emit(OpCodes.Stind_I4);
                return;
            }

            if (type == typeof(Int16))
            {
                ilGenerator.Emit(OpCodes.Stind_I2);
                return;
            }

            if (type == typeof(Byte))
            {
                ilGenerator.Emit(OpCodes.Stind_I1);
                return;
            }

            if (type == typeof(SByte))
            {
                ilGenerator.Emit(OpCodes.Stind_I1);
                return;
            }

            if (type == typeof(Boolean))
            {
                ilGenerator.Emit(OpCodes.Stind_I1);
                return;
            }

            if (type == typeof(UInt64))
            {
                ilGenerator.Emit(OpCodes.Stind_I8);
                return;
            }

            if (type == typeof(UInt32))
            {
                ilGenerator.Emit(OpCodes.Stind_I4);
                return;
            }

            if (type == typeof(UInt16))
            {
                ilGenerator.Emit(OpCodes.Stind_I2);
                return;
            }

            if (type == typeof(Single))
            {
                ilGenerator.Emit(OpCodes.Stind_R4);
                return;
            }

            if (type == typeof(Double))
            {
                ilGenerator.Emit(OpCodes.Stind_R8);
                return;
            }

            if (type == typeof(System.IntPtr))
            {
                ilGenerator.Emit(OpCodes.Stind_I4);
                return;
            }

            if (type == typeof(System.UIntPtr))
            {
                ilGenerator.Emit(OpCodes.Stind_I4);
                return;
            }

            throw new Exception(string.Format("The target type:{0} is not supported by EmitHelper.Stind_ForValueType()", type));
        } 
        #endregion

        #region LoadType
        /// <summary>
        /// LoadType ����һ��Type���󵽶�ջ��
        /// </summary>       
        public static void LoadType(ILGenerator gen, Type targetType )
        {         
            if (targetType.IsByRef) //�� ref/out ����
            {
                gen.Emit(OpCodes.Ldtoken, targetType.GetElementType());
                gen.Emit(OpCodes.Call, EmitHelper.GetTypeFromHandleMethodInfo);
                gen.Emit(OpCodes.Callvirt, EmitHelper.MakeByRefTypeMethodInfo);
            }
            else
            {
                gen.Emit(OpCodes.Ldtoken, targetType);
                gen.Emit(OpCodes.Call, EmitHelper.GetTypeFromHandleMethodInfo);
            }
        }      
        #endregion
    } 
    
}
