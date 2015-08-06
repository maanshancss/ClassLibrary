using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace ESBasic.Arithmetic
{
    /// <summary>
    /// CrossJoinContainer 交叉连接(笛卡尔积)容器。   
    /// </summary>
    public class CrossJoinContainer<T> where T: ICrossJoinable<T>
    {
        #region ElementList
        private List<T> elementList = new List<T>();
        public List<T> ElementList
        {
            get { return elementList; }
        }
        #endregion

        #region Join
        /// <summary>
        /// Join 将内部elementList中的每个元素与参数list中的每个元素作交叉连接，再将交叉连接的结果替换内部的elementList。
        /// </summary>       
        public void Join(List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                return;
            }

            if (this.elementList == null || this.elementList.Count == 0)
            {
                this.elementList = list;
                return;
            }

            List<T> oldList = this.elementList;
            List<T> newList = new List<T>();
            foreach (T ele in oldList)
            {
                foreach (T target in list)
                {
                    newList.Add(ele.CrossJoin(target));
                }
            }

            this.elementList = newList;
        }
        #endregion
    }

    /// <summary>
    /// ICrossJoinable 可进行交叉连接(笛卡尔积)的对象必须实现的接口。
    /// </summary>
    public interface ICrossJoinable<T> where T: ICrossJoinable<T>
    {
        T CrossJoin(T other);
    }

    
}
