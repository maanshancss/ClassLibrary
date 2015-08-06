using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Managers
{
    /// <summary>
    /// IObjectManager ���ڹ������Ψһ��־�Ķ��󣬸ýӿ�ʵ�ֱ��뱣֤�̰߳�ȫ��
    /// zhuweisky 2008.05.31
    /// </summary>   
    public interface IObjectManager<TPKey, TObject>
    {
        event CbGeneric<TObject> ObjectRegistered;
        event CbGeneric<TObject> ObjectUnregistered;

        int Count { get; }

        /// <summary>
        /// Add ����Ѿ�����ͬID�Ķ��������¶����滻�ɶ���
        /// </summary>     
        void Add(TPKey key, TObject obj);

        void Remove(TPKey id);
        void RemoveByValue(TObject val);
        void RemoveByPredication(Predicate<TObject> predicate);
        void Clear();
        bool Contains(TPKey id);

        /// <summary>
        /// Get ��������ڣ��򷵻�default��TObject����
        /// </summary>        
        TObject Get(TPKey id);

        /// <summary>
        /// ���ص��б��ܱ��޸ġ���ʹ�û��桿
        /// </summary>        
        List<TObject> GetAllReadonly();

     
        List<TObject> GetAll();
        List<TPKey> GetKeyList();
        List<TPKey> GetKeyListByObj(TObject obj);
        Dictionary<TPKey, TObject> ToDictionary();
    }   
}
