using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Arithmetic.Sorting
{
    /// <summary>
    /// QuickSorter 快速排序
    /// CQ，2008.12.13
    /// </summary>
    public static class QuickSorter<T> where T : System.IComparable
    {
        public static void Sort(IList<T> list, bool isAsc)
        {
            QuickSorter<T>.Sort(list, 0, list.Count - 1, isAsc);
        }

        #region Sort
        private static void Sort(IList<T> list, int left, int right, bool isAsc)
        {
            int i, j;
            T pivot;
            if (left < right)
            {
                i = left;
                j = right + 1;
                if (isAsc)
                {
                    while (i < j)
                    {
                        i++;
                        pivot = list[left];
                        while (i < right && list[i].CompareTo(pivot) < 0)
                        {
                            i++;
                        }
                        j--;
                        while (j >= left && list[j].CompareTo(pivot) > 0)
                        {
                            j--;
                        }
                        if (i < j)
                        {
                            SortHelper<T>.Swap(list, i, j);
                        }
                    }
                }
                else
                {
                    while (i < j)
                    {
                        i++;
                        pivot = list[left];
                        while (i < right && list[i].CompareTo(pivot) > 0)
                        {
                            i++;
                        }
                        j--;
                        while (j >= left && list[j].CompareTo(pivot) < 0)
                        {
                            j--;
                        }
                        if (i < j)
                        {
                            SortHelper<T>.Swap(list, i, j);
                        }
                    }

                }
                SortHelper<T>.Swap(list, left, j);
                QuickSorter<T>.Sort(list, left, j - 1, isAsc);
                QuickSorter<T>.Sort(list, j + 1, right, isAsc);
            }
        } 
        #endregion
    }
}
