using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Trees.Multiple
{
    /// <summary>
    /// IAgileMultiTree ���ڻ��棨��棩��״����֯�ṹ���ýӿڵ�ʵ�ֱ������̰߳�ȫ�ġ�
    /// ����Ҫ�������ǣ�����ͨ����ȡ��������ӵ���֯�ڵ�������ط����ص��������С�
    /// ǰ���ǣ���֯�ṹ�е�ÿһ�ڵ㶼���Լ���SequenceCode�����������֯�и�λ�á�
    /// </summary>    
    public interface IAgileMultiTree<TVal> : IMultiTree<TVal> where TVal : IMTreeVal        
    {
        char SequenceCodeSplitter { get;set; }
        IAgileNodePicker<TVal> AgileNodePicker { set;}
      
        /// <summary>
        /// Initialize ���غͳ�ʼ������AgileTree���÷�������ȡ�������IMultiTree.Initialize������
        /// </summary>
        void Initialize() ;

        /// <summary>
        /// EnsureNodeExist ����ȷ��Ŀ��ڵ㼰�������ϼ��ڵ㶼�������У���������ڣ���ֱ�ӷ���Ŀ��ڵ㡣
        /// ����ͨ��IAgileMultiTreeHelper����������Ҫ�������ϼ��ڵ��Ŀ��ڵ㣬Ȼ�󷵻�Ŀ��ڵ㡣
        /// �����ʹͨ��IAgileMultiTreeHelperҲ�޷���ȡĳ���ϼ��ڵ��Ŀ��ڵ㣬�򷵻�null��
        /// </summary>       
        MNode<TVal> EnsureNodeExist(string nodeSequenceCode);
    }
}
