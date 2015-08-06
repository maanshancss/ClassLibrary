using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Arithmetic.Sorting
{
    /// <summary>
    /// InsertionSorter 插入排序
    /// CQ，2008.12.13
    /// </summary>
    public static class InsertionSorter<T> where T : System.IComparable
    {
        #region Sort
        public static void Sort(IList<T> list, bool isAsc)
        {
            T next;
            int j;
            for (int i = 1; i < list.Count; i++)
            {
                next = list[i];
                if (isAsc)
                {
                    for (j = i - 1; j >= 0 && list[j].CompareTo(next) > 0; j--)
                    {
                        list[j + 1] = list[j];
                    }
                }
                else
                {
                    for (j = i - 1; j >= 0 && list[j].CompareTo(next) < 0; j--)
                    {
                        list[j + 1] = list[j];
                    }
                }
                list[j + 1] = next;
            }
        } 
        #endregion
        
    }
}
