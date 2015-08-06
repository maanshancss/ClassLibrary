using System;
using System.Collections.Generic;
using System.Reflection ;
using System.IO;
using ESBasic.Helpers;

namespace ESBasic.Helpers
{
	/// <summary>
	/// ReflectionHelper ��ժҪ˵����
	/// </summary>
	public static class ReflectionHelper
	{
		#region GetType
        /// <summary>
        /// GetType  ͨ����ȫ�޶��������������ض�Ӧ�����͡�typeAndAssName��"ESBasic.Filters.SourceFilter,ESBasic"��
        /// ���Ϊϵͳ�����ͣ�����Բ����������ơ�
        /// </summary>       
		public static Type GetType(string typeAndAssName)
		{
			string[] names = typeAndAssName.Split(',');
            if (names.Length < 2)
            {
                return Type.GetType(typeAndAssName);
            }

			return ReflectionHelper.GetType(names[0].Trim(), names[1].Trim());
		}

		/// <summary>
        /// GetType ����assemblyName�����е���ΪtypeFullName�����͡�assemblyName���ô���չ�������Ŀ�������ڵ�ǰ�����У�assemblyName����null	
		/// </summary>		
		public static Type GetType(string typeFullName ,string assemblyName)
		{
			if(assemblyName == null)
			{
				return Type.GetType(typeFullName) ;
			}

			//������ǰ�����Ѽ��صĳ���
			Assembly[] asses = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly ass in asses)
			{
				string[] names = ass.FullName.Split(',') ;
				if(names[0].Trim() == assemblyName.Trim())
				{                    
					return ass.GetType(typeFullName) ;
				}
			}

			//����Ŀ�����
			Assembly tarAssem = Assembly.Load(assemblyName) ;
			if(tarAssem != null)
			{
				return tarAssem.GetType(typeFullName) ;
			}

			return null ;
		}
		#endregion

        #region GetTypeFullName
        public static string GetTypeFullName(Type t)
        {
            return t.FullName + "," + t.Assembly.FullName.Split(',')[0];
        } 
        #endregion

        #region LoadDerivedInstance
        /// <summary>
        /// LoadDerivedInstance �����������м̳���TBase������ʵ����
        /// </summary>
        /// <typeparam name="TBase">�������ͣ���ӿ����ͣ�</typeparam>
        /// <param name="asm">Ŀ�����</param>
        /// <returns>TBaseʵ���б�</returns>
        public static IList<TBase> LoadDerivedInstance<TBase>(Assembly asm)
        {
            IList<TBase> list = new List<TBase>();

            Type supType = typeof(TBase);
            foreach (Type t in asm.GetTypes())
            {
                if (supType.IsAssignableFrom(t) && (!t.IsAbstract) && (!t.IsInterface))
                {
                    TBase instance = (TBase)Activator.CreateInstance(t);
                    list.Add(instance);
                }
            }

            return list;
        }
        #endregion

        #region LoadDerivedType
        /// <summary>
        /// LoadDerivedType ����directorySearchedĿ¼�����г����е�����������baseType������
        /// </summary>
        /// <typeparam name="baseType">���ࣨ��ӿڣ�����</typeparam>
        /// <param name="directorySearched">������Ŀ¼</param>
        /// <param name="searchChildFolder">�Ƿ�������Ŀ¼�еĳ���</param>
        /// <param name="config">�߼����ã����Դ���null����Ĭ������</param>        
        /// <returns>���д�BaseType�����������б�</returns>
        public static IList<Type> LoadDerivedType(Type baseType ,string directorySearched, bool searchChildFolder, TypeLoadConfig config)
        {
            if (config == null)
            {
                config = new TypeLoadConfig();
            }

            IList<Type> derivedTypeList = new List<Type>();
            if (searchChildFolder)
            {
                ReflectionHelper.LoadDerivedTypeInAllFolder(baseType, derivedTypeList, directorySearched, config);
            }
            else
            {
                ReflectionHelper.LoadDerivedTypeInOneFolder(baseType, derivedTypeList, directorySearched, config);
            }

            return derivedTypeList;
        }

        #region TypeLoadConfig
        public class TypeLoadConfig
        {
            #region Ctor
            public TypeLoadConfig() { }
            public TypeLoadConfig(bool copyToMem, bool loadAbstract, string postfix)
            {
                this.copyToMemory = copyToMem;
                this.loadAbstractType = loadAbstract;
                this.targetFilePostfix = postfix;
            }
            #endregion

            #region CopyToMemory
            private bool copyToMemory = false;
            /// <summary>
            /// CopyToMem �Ƿ񽫳��򼯿������ڴ�����
            /// </summary>
            public bool CopyToMemory
            {
                get { return copyToMemory; }
                set { copyToMemory = value; }
            }
            #endregion

            #region LoadAbstractType
            private bool loadAbstractType = false;
            /// <summary>
            /// LoadAbstractType �Ƿ���س�������
            /// </summary>
            public bool LoadAbstractType
            {
                get { return loadAbstractType; }
                set { loadAbstractType = value; }
            }
            #endregion

            #region TargetFilePostfix
            private string targetFilePostfix = ".dll";
            /// <summary>
            /// TargetFilePostfix ������Ŀ����򼯵ĺ�׺��
            /// </summary>
            public string TargetFilePostfix
            {
                get { return targetFilePostfix; }
                set { targetFilePostfix = value; }
            }
            #endregion
        } 
        #endregion

        #region LoadDerivedTypeInAllFolder
        private static void LoadDerivedTypeInAllFolder(Type baseType, IList<Type> derivedTypeList, string folderPath, TypeLoadConfig config)
        {
            ReflectionHelper.LoadDerivedTypeInOneFolder(baseType, derivedTypeList, folderPath, config);
            string[] folders = Directory.GetDirectories(folderPath);
            if (folders != null)
            {
                foreach (string nextFolder in folders)
                {
                    ReflectionHelper.LoadDerivedTypeInAllFolder(baseType, derivedTypeList, nextFolder, config);
                }
            }
        } 
        #endregion

        #region LoadDerivedTypeInOneFolder
        private static void LoadDerivedTypeInOneFolder(Type baseType, IList<Type> derivedTypeList, string folderPath, TypeLoadConfig config)
        {
            string[] files = Directory.GetFiles(folderPath);
            foreach (string file in files)
            {
                if (config.TargetFilePostfix != null)
                {
                    if (!file.EndsWith(config.TargetFilePostfix))
                    {
                        continue;
                    }
                }

                Assembly asm = null;

                #region Asm
                try
                {
                    if (config.CopyToMemory)
                    {
                        byte[] addinStream = FileHelper.ReadFileReturnBytes(file);
                        asm = Assembly.Load(addinStream);
                    }
                    else
                    {
                        asm = Assembly.LoadFrom(file);
                    }
                }
                catch (Exception ee)
                {
                    ee = ee;
                }

                if (asm == null)
                {
                    continue;
                }
                #endregion

                Type[] types = asm.GetTypes();                

                foreach (Type t in types)
                {
                    if (t.IsSubclassOf(baseType) || baseType.IsAssignableFrom(t))
                    {
                        bool canLoad = config.LoadAbstractType ? true : (!t.IsAbstract);
                        if (canLoad)
                        {
                            derivedTypeList.Add(t);
                        }
                    }
                }
            }

        }        
        #endregion
        #endregion

        #region SetProperty
        /// <summary>
        /// SetProperty ���list�е�object����ָ����propertyName���ԣ������ø����Ե�ֵΪproValue
        /// </summary>		
        public static void SetProperty(IList<object> objs, string propertyName, object proValue)
        {
            object[] args = { proValue };
            foreach (object target in objs)
            {
                ReflectionHelper.SetProperty(target, propertyName, proValue);
            }
        }

        public static void SetProperty(object obj, string propertyName, object proValue)
        {
            ReflectionHelper.SetProperty(obj, propertyName, proValue ,true);
        }

        /// <summary>
        /// SetProperty ���object����ָ����propertyName���ԣ������ø����Ե�ֵΪproValue
        /// </summary>		
        public static void SetProperty(object obj, string propertyName, object proValue ,bool ignoreError)
        {
            Type t = obj.GetType();
            PropertyInfo pro = t.GetProperty(propertyName);
            if ((pro == null) || (!pro.CanWrite))
            {
                if (!ignoreError)
                {
                    string msg = string.Format("The setter of property named '{0}' not found in '{1}'.", propertyName, t);
                    throw new Exception(msg);
                }

                return;
            }

            #region ����ת������
            try
            {
                proValue = TypeHelper.ChangeType(pro.PropertyType, proValue);
            }
            catch { }
            #endregion

            object[] args = { proValue };
            t.InvokeMember(propertyName, BindingFlags.Public | BindingFlags.IgnoreCase |
                        BindingFlags.Instance | BindingFlags.SetProperty, null, obj, args);                       
        }
        #endregion

        #region GetProperty
        /// <summary>
        /// GetProperty ����ָ������������ȡĿ���������Ե�ֵ
        /// </summary>
        public static object GetProperty(object obj, string propertyName)
        {
            Type t = obj.GetType();

            return t.InvokeMember(propertyName, BindingFlags.Default | BindingFlags.GetProperty, null, obj, null);
        }
        #endregion

        #region GetFieldValue
        /// <summary>
        /// GetFieldValue ȡ��Ŀ������ָ��field��ֵ��field������private
        /// </summary>      
        public static object GetFieldValue(object obj, string fieldName)
        {
            Type t = obj.GetType();
            FieldInfo field = t.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
            if (field == null)
            {
                string msg = string.Format("The field named '{0}' not found in '{1}'." ,fieldName ,t);
                throw new Exception(msg);
            }

            return field.GetValue(obj);
        } 
        #endregion

        #region SetFieldValue
        /// <summary>
        /// SetFieldValue ����Ŀ������ָ��field��ֵ��field������private
        /// </summary>      
        public static void SetFieldValue(object obj, string fieldName ,object val)
        {
            Type t = obj.GetType();
            FieldInfo field = t.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetField | BindingFlags.Instance);
            if (field == null)
            {
                string msg = string.Format("The field named '{0}' not found in '{1}'.", fieldName, t);
                throw new Exception(msg);
            }

            field.SetValue(obj, val);
        }
        #endregion

        #region CopyProperty
        /// <summary>
        /// CopyProperty ��source�е����Ե�ֵ����target��ͬ��������
        /// ʹ��CopyProperty���Է����ʵ�ֿ������캯��
        /// </summary>        
        public static void CopyProperty(object source, object target)
        {
            ReflectionHelper.CopyProperty(source, target, null);
        }

        /// <summary>
        /// CopyProperty ��source�е����Ե�ֵ����target����ƥ������ԣ�ƥ���ϵͨ��propertyMapItemListȷ��
        /// </summary>        
        public static void CopyProperty(object source, object target, IList<MapItem> propertyMapItemList )
        {          
            Type sourceType = source.GetType();
            Type targetType = target.GetType();
            PropertyInfo[] sourcePros = sourceType.GetProperties();

            if (propertyMapItemList != null)
            {
                foreach (MapItem item in propertyMapItemList)
                {
                    object val = ReflectionHelper.GetProperty(source, item.Source);
                    ReflectionHelper.SetProperty(target, item.Target, val);
                }
            }
            else
            {
                foreach (PropertyInfo sourceProperty in sourcePros)
                {
                    if (sourceProperty.CanRead)
                    {
                        object val = ReflectionHelper.GetProperty(source, sourceProperty.Name);
                        ReflectionHelper.SetProperty(target, sourceProperty.Name, val);
                    }
                }
            }
        } 
        #endregion

        #region GetAllMethods��SearchMethod
        /// <summary>
        /// GetAllMethods ��ȡ�ӿڵ����з�����Ϣ�������̳е�
        /// </summary>       
        public static IList<MethodInfo> GetAllMethods(params Type[] interfaceTypes)
        {
            foreach (Type interfaceType in interfaceTypes)
            {
                if (!interfaceType.IsInterface)
                {
                    throw new Exception("Target Type must be interface!");
                }
            }

            IList<MethodInfo> list = new List<MethodInfo>();
            foreach (Type interfaceType in interfaceTypes)
            {
                ReflectionHelper.DistillMethods(interfaceType, ref list);
            }

            return list;
        }

        private static void DistillMethods(Type interfaceType, ref IList<MethodInfo> methodList)
        {
            foreach (MethodInfo meth in interfaceType.GetMethods())
            {
                bool isExist = false;
                foreach (MethodInfo temp in methodList)
                {
                    if ((temp.Name == meth.Name) && (temp.ReturnType == meth.ReturnType))
                    {
                        ParameterInfo[] para1 = temp.GetParameters();
                        ParameterInfo[] para2 = meth.GetParameters();
                        if (para1.Length == para2.Length)
                        {
                            bool same = true;
                            for (int i = 0; i < para1.Length; i++)
                            {
                                if (para1[i].ParameterType != para2[i].ParameterType)
                                {
                                    same = false;
                                }
                            }

                            if (same)
                            {
                                isExist = true;
                                break;
                            }
                        }                      
                    }
                }

                if (!isExist)
                {
                    methodList.Add(meth);
                }
            }

            foreach (Type superInterfaceType in interfaceType.GetInterfaces())
            {
                ReflectionHelper.DistillMethods(superInterfaceType, ref methodList);
            }
        }



        /// <summary>
        /// SearchGenericMethodInType ����ָ�����Ͷ���ķ��ͷ������������̳еġ�
        /// </summary>       
        public static MethodInfo SearchGenericMethodInType(Type originType, string methodName, Type[] argTypes)
        {
            foreach (MethodInfo method in originType.GetMethods())
            {
                if (method.ContainsGenericParameters && method.Name == methodName)
                {
                    bool succeed = true;
                    ParameterInfo[] paras = method.GetParameters();
                    if (paras.Length == argTypes.Length)
                    {
                        for (int i = 0; i < paras.Length; i++)
                        {
                            if (!paras[i].ParameterType.IsGenericParameter) //�������Ͳ���
                            {
                                if (paras[i].ParameterType.IsGenericType) //�������������Ƿ������ͣ���IList<T>
                                {
                                    if (paras[i].ParameterType.GetGenericTypeDefinition() != argTypes[i].GetGenericTypeDefinition())
                                    {
                                        succeed = false;
                                        break;
                                    }
                                }
                                else //��ͨ���͵Ĳ���
                                {
                                    if (paras[i].ParameterType != argTypes[i])
                                    {
                                        succeed = false;
                                        break;
                                    }
                                }
                            }
                        }
                        if (succeed)
                        {
                            return method;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// SearchMethod �������̳е����з�����Ҳ�������ͷ�����
        /// </summary>       
        public static MethodInfo SearchMethod(Type originType, string methodName, Type[] argTypes)
        {
            MethodInfo meth = originType.GetMethod(methodName, argTypes);
            if (meth != null)
            {
                return meth;
            }

            meth = ReflectionHelper.SearchGenericMethodInType(originType, methodName, argTypes);
            if (meth != null)
            {
                return meth;
            }

            //�������� 
            Type baseType = originType.BaseType ;
            if (baseType != null)
            {
                while (baseType != typeof(object))
                {
                    MethodInfo target = baseType.GetMethod(methodName, argTypes);
                    if (target != null)
                    {
                        return target;
                    }

                    target = ReflectionHelper.SearchGenericMethodInType(baseType, methodName, argTypes);
                    if (target != null)
                    {
                        return target;
                    }

                    baseType = baseType.BaseType;
                }
            }

            //�������ӿ�
            if (originType.GetInterfaces() != null)
            {
                IList<MethodInfo> list = ReflectionHelper.GetAllMethods(originType.GetInterfaces());
                foreach (MethodInfo theMethod in list)
                {
                    if (theMethod.Name != methodName)
                    {
                        continue;
                    }
                    ParameterInfo[] args = theMethod.GetParameters();
                    if (args.Length != argTypes.Length)
                    {
                        continue;
                    }

                    bool correctArgType = true;
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i].ParameterType != argTypes[i])
                        {
                            correctArgType = false;
                            break;
                        }                       
                    }

                    if (correctArgType)
                    {
                        return theMethod;
                    }
                }
            }

            return null;           
        }
    
        #endregion

        #region GetFullMethodName
        public static string GetMethodFullName(MethodInfo method)
        {
            return string.Format("{0}.{1}()", method.DeclaringType, method.Name);
        } 
        #endregion
	}    
}
