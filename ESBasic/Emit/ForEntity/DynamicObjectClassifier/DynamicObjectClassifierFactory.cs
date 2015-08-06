using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using ESBasic.Helpers;
using ESBasic.Emit.Management;

namespace ESBasic.Emit.ForEntity
{
    public static class DynamicObjectClassifierFactory
    {
        /// <summary>
        /// 如果动态的对象分类器对象被【序列化】存储，则在反序列化加载之前，必须先调用CreateObjectClassifier()在内存中创建正确的实体分类器类型，否则，反序列化将失败。
        /// DynamicAssemblyManager对象用于确保反序列化实体分类器实例时，能够被正确解析。
        /// </summary>
        private static DynamicAssemblyManager dynamicAssemblyManager = new DynamicAssemblyManager();

        private static DynamicObjectClassifierEmitter DynamicObjectClassifierEmitter = new DynamicObjectClassifierEmitter();
        private static IDictionary<string, Type> NTierDictionaryTypeDic = new Dictionary<string, Type>(); //EntityType_GenericTypeContactString ["Student_String_Int_String"] -- nested Dic Type

        #region CreateObjectClassifier
        /// <summary>
        /// CreateObjectClassifier 创建一个对象分类器实例。
        /// </summary>
        /// <typeparam name="TObject">需要分类器实例处理的对象类型</typeparam>              
        /// <param name="properties4Classify">基于哪些列（属性）对实体对象进行分类，其长度就是嵌套的Dictionary的层数</param>
        /// <returns>实现了IObjectClassifier接口的实体分类器实例。必须调用其Initialize方法以完成初始化。</returns>
        public static IObjectClassifier<TObject> CreateObjectClassifier<TObject>(params string[] properties4Classify)
        {
            lock (DynamicObjectClassifierFactory.DynamicObjectClassifierEmitter)
            {
                Type entityType = typeof(TObject);

                Type[] nestedKeyTypes = new Type[properties4Classify.Length];
                string typeContactStr = entityType.ToString();
                for (int i = 0; i < properties4Classify.Length; i++)
                {
                    PropertyInfo propertyInfo = entityType.GetProperty(properties4Classify[i]);
                    if (propertyInfo == null)
                    {
                        throw new Exception(string.Format("Property named {0} not found in Type {1} !", properties4Classify[i], entityType));
                    }

                    nestedKeyTypes[i] = propertyInfo.PropertyType;

                    typeContactStr += "_" + TypeHelper.GetClassSimpleName(nestedKeyTypes[i]);
                }

                string typeName = string.Format("{0}_Classifier", typeContactStr);

                if (!DynamicObjectClassifierFactory.NTierDictionaryTypeDic.ContainsKey(typeContactStr))
                {
                    //在已加载的程序集中搜索，如果曾有保存为程序集的，则不用再Emit一次了
                    Type newType = null;
                    Assembly[] assAry = AppDomain.CurrentDomain.GetAssemblies();
                    foreach (Assembly ass in assAry)
                    {
                        foreach (Type t in ass.GetTypes())
                        {
                            if (t.Assembly.FullName.Split(',')[0] == DynamicObjectClassifierEmitter.AssemblyName && t.FullName == typeName)
                            {
                                newType = t;
                            }
                        }
                    }
                    if (newType == null)
                    {
                        newType = DynamicObjectClassifierFactory.DynamicObjectClassifierEmitter.EmitDynamicNTierDictionaryType<TObject>(nestedKeyTypes);
                        //DynamicObjectClassifierFactory.DynamicObjectClassifierEmitter.Save();
                    }

                    DynamicObjectClassifierFactory.NTierDictionaryTypeDic.Add(typeContactStr, newType);
                }

                Type nestedDicType = DynamicObjectClassifierFactory.NTierDictionaryTypeDic[typeContactStr];

                return (IObjectClassifier<TObject>)Activator.CreateInstance(nestedDicType, new object[] { (string[])properties4Classify });
            }
        } 
        #endregion      
    }
}
