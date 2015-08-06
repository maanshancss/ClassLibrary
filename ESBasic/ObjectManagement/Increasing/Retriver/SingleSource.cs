using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Increasing
{
    /// <summary>
    /// SingleSource 当增量源只有一个时，使用SingleSource作为泛型参数。
    /// </summary>
    public class SingleSource
    {
        private static SingleSource singleton = new SingleSource();
        public static SingleSource Singleton
        {
            get { return SingleSource.singleton; }           
        }

        private SingleSource()
        {
        }
    }
}
