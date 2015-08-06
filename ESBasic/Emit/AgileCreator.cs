using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Text;
using ESBasic.Helpers;

namespace ESBasic.Emit
{
    /// <summary>
    /// AgileCreator 在需要大量的动态创建实例的时候，使用AgileCreator取代基于反射的对象创建以提高创建效率。
    /// 调用AgileCreator.New()方法创建对象。
    /// zhuweisky 2007.01.26
    /// </summary>
    public abstract class AgileCreator
    {
        private static AssemblyBuilder DynamicAssembly = null;
        private static ModuleBuilder ModuleBuilder = null;
        private static Dictionary<CreatorKey, AgileCreator> DicCreator = new Dictionary<CreatorKey, AgileCreator>();

        #region GetDynamicModule
        private static ModuleBuilder GetDynamicModule()
        {
            if (DynamicAssembly == null)
            {
                DynamicAssembly = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("DynamicAssembly"), AssemblyBuilderAccess.Run);
                ModuleBuilder = DynamicAssembly.DefineDynamicModule("MainModule");
            }

            return ModuleBuilder;
        }
        #endregion

        #region CreateMethod
        private static void CreateMethod(TypeBuilder tb, Type originalType, Object[] param)
        {
            MethodInfo mi = typeof(AgileCreator).GetMethod("CreateObject");

            MethodBuilder mb = tb.DefineMethod("CreateObject", mi.Attributes & ~MethodAttributes.Abstract, mi.CallingConvention, mi.ReturnType, new Type[] { typeof(Object[]) });

            ConstructorInfo[] cis = originalType.GetConstructors();
            ConstructorInfo theCi = null;
            ParameterInfo[] cpis = null;
            foreach (ConstructorInfo ci in cis)
            {
                cpis = ci.GetParameters();
                if (cpis.Length != param.Length)
                    continue;

                theCi = ci;
                for (int i = 0; i < cpis.Length; i++)
                {                    
                    #region param[i] == null
                    if (param[i] == null)
                    {
                        if (cpis[i].ParameterType.IsValueType)
                        {
                            theCi = null;
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    } 
                    #endregion

                    Type curActualParamType = param[i].GetType();
                    Type curFormalParamType = cpis[i].ParameterType;

                    #region IsNumbericType
                    if (TypeHelper.IsNumbericType(curActualParamType))
                    {
                        if (TypeHelper.IsNumbericType(curFormalParamType))
                        {
                            continue;
                        }
                    } 
                    #endregion

                    if (!(curActualParamType == curFormalParamType || curActualParamType.IsSubclassOf(curFormalParamType) || curFormalParamType.IsAssignableFrom(curActualParamType)))
                    {
                        theCi = null;
                        break;
                    }
                }
                if (theCi != null)
                    break;
            }


            if (theCi == null)
                throw new ArgumentException("The ctor parameter number or type is not correct!");

            ILGenerator ilg = mb.GetILGenerator();
            for (int i = 0; i < param.Length; i++)
            {
                ilg.Emit(OpCodes.Ldarg_1);
                ilg.Emit(OpCodes.Ldc_I4, i);
                ilg.Emit(OpCodes.Ldelem_Ref);
                if (cpis[i].ParameterType.IsValueType)
                    ilg.Emit(OpCodes.Unbox_Any, cpis[i].ParameterType);
            }
            ilg.Emit(OpCodes.Newobj, theCi);
            ilg.Emit(OpCodes.Ret);

            tb.DefineMethodOverride(mb, mi);    // 定义方法重载
        }
        #endregion

        #region GetCreator
        /// <summary>
        /// GetCreator 动态产生一个派生自AgileCreator的类，并生成其实例
        /// </summary>       
        private static AgileCreator GetCreator(Type type, Object[] param)
        {
            Type[] paramTypeAry = null;
            if (param != null)
            {
                paramTypeAry = new Type[param.Length];
                for (int i = 0; i < param.Length; i++)
                {
                    if (param[i] != null)
                    {
                        paramTypeAry[i] = param[i].GetType();
                    }
                    else
                    {
                        paramTypeAry[i] = typeof(object);
                    }
                }
            }

            CreatorKey key = new CreatorKey(type, paramTypeAry);

            if (!DicCreator.ContainsKey(key))
            {
                ModuleBuilder module = GetDynamicModule();
                TypeBuilder tb = module.DefineType("__dynamicCreator." + type.FullName + "_" + key.GetHashCode(), TypeAttributes.Public | TypeAttributes.Class, typeof(AgileCreator));
                CreateMethod(tb, type, param);
                DicCreator.Add(key, (AgileCreator)Activator.CreateInstance(tb.CreateType()));
            }
            return DicCreator[key];
        }
        #endregion

        /// <summary>
        /// CreateObject AgileCreator的派生类（通过GetCreator动态创建）将实现该方法，方法的实现中直接采用高效的new操作符来创建对象。
        /// </summary>    
        public abstract Object CreateObject(Object[] param);

        public static Object New(Type type, params Object[] param)
        {
            AgileCreator creator = AgileCreator.GetCreator(type, param);
            return creator.CreateObject(param);
        }
    }

    #region CreatorKey
    internal class CreatorKey
    {
        #region Ctor
        public CreatorKey()
        {
        }

        public CreatorKey(Type type, Type[] paraTypeAry)
        {
            this.targetType = type;
            this.paramTypeArray = paraTypeAry;
        }
        #endregion

        #region TargetType
        private Type targetType;
        public Type TargetType
        {
            get { return targetType; }
            set { targetType = value; }
        }
        #endregion

        #region ParamTypeArray
        private Type[] paramTypeArray;
        public Type[] ParamTypeArray
        {
            get { return paramTypeArray; }
            set { paramTypeArray = value; }
        }
        #endregion

        #region Equals ,GetHashCode
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof(CreatorKey))
            {
                return false;
            }

            #region Old
            CreatorKey key = (CreatorKey)obj;
            if (key.targetType != this.targetType)
            {
                return false;
            }

            if ((key.paramTypeArray == null) && (this.paramTypeArray != null))
            {
                return false;
            }

            if ((key.paramTypeArray != null) && (this.paramTypeArray == null))
            {
                return false;
            }

            if (key.paramTypeArray.Length != this.paramTypeArray.Length)
            {
                return false;
            }

            for (int i = 0; i < this.paramTypeArray.Length; i++)
            {
                if (this.paramTypeArray[i] != key.paramTypeArray[i])
                {
                    return false;
                }
            }

            return true;
            #endregion

            #region new
            //return (this.GetHashCode() == obj.GetHashCode()) ; 
            #endregion
        }

        public override int GetHashCode()
        {
            string hashCodeStr = this.targetType.ToString();
            if (this.paramTypeArray != null)
            {
                for (int i = 0; i < this.paramTypeArray.Length; i++)
                {
                    hashCodeStr += "_" + this.paramTypeArray[i].ToString();
                }
            }

            return hashCodeStr.GetHashCode();
        }
        #endregion
    }
    #endregion
}
