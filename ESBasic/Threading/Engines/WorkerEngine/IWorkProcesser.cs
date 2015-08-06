using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Threading.Engines
{
    /// <summary>
    /// IWorkProcesser 任务处理器。
    /// </summary>    
    public interface IWorkProcesser<T>
    {
        void Process(T work);
    }
}
