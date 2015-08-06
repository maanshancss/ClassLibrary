using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Integration
{
    /// <summary>
    /// Combiner 对象合并器。
    /// </summary>
    public static class Combiner
    {
        #region CombineOnSameID
        /// <summary>
        /// CombineOnSameID 将多个list中的对象依据ID合并起来。
        /// </summary>
        /// <typeparam name="TID">被合并对象的标志类型</typeparam>
        /// <typeparam name="TObj">被合并对象的类型</typeparam>        
        public static IDictionary<TID, TObj> CombineOnSameID<TID, TObj>(IEnumerable<IList<TObj>> listAry) where TObj : ICombined<TID, TObj>
        {
            IDictionary<TID, TObj> result = new Dictionary<TID, TObj>();
            if (listAry == null)
            {
                return result;
            }

            foreach (IList<TObj> list in listAry)
            {
                if (list != null)
                {
                    foreach (TObj obj in list)
                    {
                        if (!result.ContainsKey(obj.ID))
                        {
                            result.Add(obj.ID, obj);
                        }
                        else
                        {
                            result[obj.ID].Combine(obj);
                        }
                    }
                }
            }

            return result;
        }
        #endregion

        #region CombineIntoContainer
        /// <summary>
        /// IDictionary 将多个collection中的对象依据ID合并到container中去。
        /// </summary>
        /// <typeparam name="TID">被合并对象的标志类型</typeparam>
        /// <typeparam name="TObj">被合并对象的类型</typeparam> 
        public static void CombineIntoContainer<TID, TObj>(ref IDictionary<TID, TObj> container, params IEnumerable<TObj>[] collectionAry) where TObj : ICombined<TID, TObj>
        {
            if (collectionAry == null)
            {
                return;
            }

            foreach (IList<TObj> list in collectionAry)
            {
                if (list != null)
                {
                    foreach (TObj obj in list)
                    {
                        if (!container.ContainsKey(obj.ID))
                        {
                            container.Add(obj.ID, obj);
                        }
                        else
                        {
                            container[obj.ID].Combine(obj);
                        }
                    }
                }
            }           
        } 
        #endregion
    }
}
