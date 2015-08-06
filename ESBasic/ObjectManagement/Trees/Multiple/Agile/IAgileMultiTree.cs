using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Trees.Multiple
{
    /// <summary>
    /// IAgileMultiTree 用于缓存（多叉）树状的组织结构。该接口的实现必须是线程安全的。
    /// 最主要的作用是，可以通过提取器将新添加的组织节点从其它地方加载到缓存树中。
    /// 前提是，组织结构中的每一节点都有自己的SequenceCode来表达其在组织中个位置。
    /// </summary>    
    public interface IAgileMultiTree<TVal> : IMultiTree<TVal> where TVal : IMTreeVal        
    {
        char SequenceCodeSplitter { get;set; }
        IAgileNodePicker<TVal> AgileNodePicker { set;}
      
        /// <summary>
        /// Initialize 加载和初始化整个AgileTree。该方法用于取代基类的IMultiTree.Initialize方法。
        /// </summary>
        void Initialize() ;

        /// <summary>
        /// EnsureNodeExist 用于确保目标节点及其所有上级节点都存在树中，如果都存在，则直接返回目标节点。
        /// 否则，通过IAgileMultiTreeHelper来加载所需要的所有上级节点和目标节点，然后返回目标节点。
        /// 如果即使通过IAgileMultiTreeHelper也无法提取某个上级节点或目标节点，则返回null。
        /// </summary>       
        MNode<TVal> EnsureNodeExist(string nodeSequenceCode);
    }
}
