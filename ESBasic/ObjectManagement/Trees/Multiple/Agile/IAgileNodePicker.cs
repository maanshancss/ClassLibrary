using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Trees.Multiple
{
    /// <summary>
    /// IAgileNodePicker 用于从其它地方（如数据库等）提取特定节点的信息
    /// </summary>   
    public interface IAgileNodePicker<TVal> :IObjectRetriever<string ,TVal> where TVal : IMTreeVal        
    {
        
        TVal PickupRoot();

        //TVal Retrieve(Tkey id);
        //IDictionary<Tkey, TVal> RetrieveAll();         
    }
}
