using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement
{
    /// <summary>
    /// IObjectRetriever ������ȡ�������磬���ܻ������ڴ������ط���ȡ�����в����ڵ�object��    
    /// </summary>  
    public interface IObjectRetriever<Tkey ,TVal> 
    {
        /// <summary>
        /// Retrieve ����ID��ȡĿ�����
        /// </summary>
        TVal Retrieve(Tkey id);

        /// <summary>
        /// RetrieveAll ��ȡ���еĶ���
        /// </summary>      
        IDictionary<Tkey, TVal> RetrieveAll();
    }
}
