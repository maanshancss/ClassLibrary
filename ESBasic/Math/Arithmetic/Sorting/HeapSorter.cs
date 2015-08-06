using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Arithmetic.Sorting
{
    /// <summary>
    /// HeapSorter 堆排序
    /// CQ，2008.12.13
    /// </summary>    
    public static class HeapSorter<T> where T : System.IComparable
    {
        #region Sort
        public static void Sort(IList<T> list, bool isAsc)
        {
            //如果是升序，将堆调整成一个最大堆，如果是降序，调整成最小堆
            for (int i = list.Count / 2 - 1; i >= 0; i--)
            {
                HeapSorter<T>.Adjust(list, i, list.Count - 1, isAsc);
            }
            //把跟结点跟最后一个结点的值交换，然后把除最后结点以外的结点调整成堆
            for (int i = list.Count - 1; i > 0; i--)
            {
                SortHelper<T>.Swap(list, 0, i);
                HeapSorter<T>.Adjust(list, 0, i - 1, isAsc);
            }
        }        
        #endregion

        #region Adjust
        private static void Adjust(IList<T> list, int nodeIndx, int maxAdjustIndx, bool isAsc)
        {
            T rootValue = list[nodeIndx];
            T temp = list[nodeIndx];
            int childNodeIndx = 2 * nodeIndx + 1;
            while (childNodeIndx <= maxAdjustIndx)
            {
                if (isAsc)
                {
                    if (childNodeIndx < maxAdjustIndx && list[childNodeIndx].CompareTo(list[childNodeIndx + 1]) < 0)
                    {
                        childNodeIndx++;
                    }
                    if (rootValue.CompareTo(list[childNodeIndx]) > 0)
                    {
                        break;
                    }
                    else
                    {
                        list[(childNodeIndx - 1) / 2] = list[childNodeIndx];
                        childNodeIndx = 2 * childNodeIndx + 1;
                    }
                }
                else
                {
                    if (childNodeIndx < maxAdjustIndx && list[childNodeIndx].CompareTo(list[childNodeIndx + 1]) > 0)
                    {
                        childNodeIndx++;
                    }
                    if (rootValue.CompareTo(list[childNodeIndx]) < 0)
                    {
                        break;
                    }
                    else
                    {
                        list[(childNodeIndx - 1) / 2] = list[childNodeIndx];
                        childNodeIndx = 2 * childNodeIndx + 1;
                    }
                }

            }
            list[(childNodeIndx - 1) / 2] = temp;
        } 
        #endregion
    }
}
