using System;
using System.Collections.Generic ;

namespace ESBasic.ObjectManagement.Trees.Multiple
{
	/// <summary>
    /// IMultiTree ����������ӿڡ��ýӿڵ�ʵ�ֱ������̰߳�ȫ�ġ�
	/// zhuweisky 2005.07.28
	/// </summary>
    public interface IMultiTree<TVal> where TVal : IMTreeVal
	{
        /// <summary>
        /// Initialize ʹ���Ѿ����ڵ�ĳ��������������������һ��������
        /// </summary>      
        void Initialize(MNode<TVal> _rootNode);


        /// <summary>
        /// Initialize ͨ�������ڵ���ڲ�ֵ�����¹��������Ĳ㼶��ϵ��
        /// </summary>
        void Initialize(TVal rootVal, IList<TVal> members);       

        /// <summary>
        /// Root ���ض�����ĸ��ڵ㡣
        /// </summary>
        MNode<TVal> Root { get;}	
	
        /// <summary>
        /// Count ������ĵ�ǰ�ڵ�������
        /// </summary>
		int   Count {get ;}

        /// <summary>
        /// GetNodesOnDepthIndex ��ȡĳһ��ȵ����нڵ㡣Root���������Ϊ0
        /// </summary>        
        IList<MNode<TVal>> GetNodesOnDepthIndex(int depthIndex);

        /// <summary>
        /// GetNodesOnDepthIndex ��ȡ����idPath��ϵ�²������ΪdepthIndex�����нڵ㡣Root���������Ϊ0
        /// </summary>        
        IList<MNode<TVal>> GetNodesOnDepthIndex(string idPath, char separator ,int depthIndex);

        MNode<TVal> GetNodeByID(string valID);

        /// <summary>
        /// GetNodeByPath ���ݽڵ��ID��·��������������Ӧ�Ľڵ㣬��idPath--0.1.1.0.2.5����GetNodeByIDЧ��Ҫ�ߡ�
        /// </summary>        
        MNode<TVal> GetNodeByPath(string idPath, char separator);

        /// <summary>
        /// GetFamilyByPath ��ȡ·���ϵ����нڵ㣨�Ӹ���Ҷ��˳�򣬼������б�ĵ�һ��Ԫ��Ϊ���������·���ϵ�ĳ���ڵ�ֵ�����в����ڣ���ֱ�ӷ���null��
        /// </summary>        
        List<MNode<TVal>> GetFamilyByPath(string idPath, char separator);

        /// <summary>
        /// GetOffsprings ��ȡĳ���ڵ����������ڵ㡣
        /// </summary>       
        IList<MNode<TVal>> GetOffsprings(string valID);
        
        /// <summary>
        /// GetLeaves ��ȡĳ��·���µ�����Ҷ�ӽڵ㡣���Path�Ѿ���Ҷ�ӽڵ㣬�򷵻ذ����Լ����б�
        /// </summary>     
        IList<TVal> GetLeaves(string path, char separator);

        /// <summary>
        /// ActionOnEachNode ��root��ʼ��ÿ���ڵ����һ��action��
        /// </summary>       
        void ActionOnEachNode(CbGeneric<MNode<TVal>> action);
	}
}

