using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Trees.Multiple
{
    /// <summary>
    /// IAgileNodePicker ���ڴ������ط��������ݿ�ȣ���ȡ�ض��ڵ����Ϣ
    /// </summary>   
    public interface IAgileNodePicker<TVal> :IObjectRetriever<string ,TVal> where TVal : IMTreeVal        
    {
        
        TVal PickupRoot();

        //TVal Retrieve(Tkey id);
        //IDictionary<Tkey, TVal> RetrieveAll();         
    }
}
