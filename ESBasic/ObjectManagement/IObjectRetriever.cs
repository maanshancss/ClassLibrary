using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement
{
    /// <summary>
    /// IObjectRetriever 对象提取器。比如，智能缓存用于从其它地方获取缓存中不存在的object。    
    /// </summary>  
    public interface IObjectRetriever<Tkey ,TVal> 
    {
        /// <summary>
        /// Retrieve 根据ID获取目标对象。
        /// </summary>
        TVal Retrieve(Tkey id);

        /// <summary>
        /// RetrieveAll 获取所有的对象。
        /// </summary>      
        IDictionary<Tkey, TVal> RetrieveAll();
    }
}
