using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Arithmetic.Sorting
{
    /// <summary>
    /// MergeSorter 归并排序
    /// CQ，2008.12.13
    /// </summary>    
    public static class MergeSorter<T> where T : System.IComparable
    {
        #region Sort
        public static void Sort(IList<T> list, bool isAsc)
        {
            int length = 1;
            IList<T> sortedList = new List<T>(list.Count);
            foreach (T item in list)
            {
                sortedList.Add(item);
            }
            while (length < list.Count)
            {
                MergeSorter<T>.MergePass(list, sortedList, length, isAsc);
                length *= 2;
                MergeSorter<T>.MergePass(sortedList, list, length, isAsc);
                length *= 2;
            }
        } 
        #endregion

        #region private
        private static void MergePass(IList<T> list, IList<T> sortedList, int length, bool isASC)
        {
            int i, j;
            for (i = 0; i <= list.Count - 2 * length; i += 2 * length)
            {
                MergeSorter<T>.Merge(list, i, i + length - 1, i + 2 * length - 1, sortedList, isASC);
            }
            if (i + length < list.Count)
            {
                MergeSorter<T>.Merge(list, i, i + length - 1, list.Count - 1, sortedList, isASC);
            }
            else
            {
                for (j = i; j < list.Count; j++)
                {
                    sortedList[j] = list[j];
                }
            }

        }
        /// <summary>
        /// 归并两个已经排序的子序列为一个有序的序列
        /// </summary>
        /// <param name="list">原始序列</param>
        /// <param name="startIndx">归并起始index</param>
        /// <param name="splitIndx">第一段结束的index</param>
        /// <param name="endIndx">第二段结束的index</param>
        /// <param name="sortedList">排序后的序列</param>
        /// <param name="isASC">是否升序</param>
        private static void Merge(IList<T> list, int startIndx, int splitIndx, int endIndx, IList<T> sortedList, bool isASC)
        {
            int right = splitIndx + 1;
            int left = startIndx;
            int sortIndx = startIndx;
            while (left <= splitIndx && right <= endIndx)
            {
                if (isASC)
                {
                    if (list[left].CompareTo(list[right]) <= 0)
                    {
                        sortedList[sortIndx++] = list[left++];
                    }
                    else
                    {
                        sortedList[sortIndx++] = list[right++];
                    }
                }
                else
                {
                    if (list[left].CompareTo(list[right]) >= 0)
                    {
                        sortedList[sortIndx++] = list[left++];
                    }
                    else
                    {
                        sortedList[sortIndx++] = list[right++];
                    }
                }
            }
            if (left > splitIndx)
            {
                for (int t = right; t <= endIndx; t++)
                {
                    sortedList[sortIndx + t - right] = list[t];
                }
            }
            else
            {
                for (int t = left; t <= splitIndx; t++)
                {
                    sortedList[sortIndx + t - left] = list[t];
                }
            }
        }
        
        #endregion
    }
}
