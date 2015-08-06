using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Arithmetic.Sorting
{
    public static class SortHelper<T>
    {
        public static void Swap(IList<T> list, int i, int j)
        {
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
