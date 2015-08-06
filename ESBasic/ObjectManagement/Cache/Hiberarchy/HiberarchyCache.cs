using System.Collections.Generic;
using System.Text;
using ESBasic.ObjectManagement.Trees.Multiple;
using ESBasic.Collections;
using System;

namespace ESBasic.ObjectManagement.Cache
{
    public class HiberarchyCache<TVal> : IHiberarchyCache<TVal> where TVal : IHiberarchyVal        
    {        
        private ISmartDictionaryCache<string ,TVal> smartDictionaryCache = new SmartDictionaryCache<string ,TVal>();
        private IAgileMultiTree<TVal> agileMultiTree = new AgileMultiTree<TVal>();

        #region Property
        #region ObjectRetriever
        private IObjectRetriever<string ,TVal> objectRetriever;
        public IObjectRetriever<string, TVal> ObjectRetriever
        {
            set { objectRetriever = value; }
        }
         #endregion

        #region RootID
        private string rootID;
        public string RootID
        {
            set { rootID = value; }
        }
        #endregion

        #region SequenceCodeSplitter        
        private char sequenceCodeSplitter = ',';
        public char SequenceCodeSplitter
        {
            get { return sequenceCodeSplitter; }
            set { sequenceCodeSplitter = value; }
        }
        #endregion 
        #endregion

        #region Initialize
        public void Initialize()
        {
            this.smartDictionaryCache.ObjectRetriever = this.objectRetriever;
            this.smartDictionaryCache.Initialize();

            this.agileMultiTree.AgileNodePicker = new HiberarchyAgileNodePicker<TVal>(this.smartDictionaryCache, this.rootID);
            this.agileMultiTree.SequenceCodeSplitter = this.sequenceCodeSplitter;
            this.agileMultiTree.Initialize();
        } 
        #endregion

        #region Get
        public TVal Get(string id)
        {
            if (id == null)
            {
                return default(TVal);
            }

            if (this.smartDictionaryCache.HaveContained(id))
            {
                return this.smartDictionaryCache.Get(id);
            }

            TVal target = this.smartDictionaryCache.Get(id);
            if (target != null)
            {
                this.agileMultiTree.EnsureNodeExist(target.SequenceCode);
            }

            return target;
        } 
        #endregion

        #region HaveContained
        public bool HaveContained(string id)
        {
            return this.smartDictionaryCache.HaveContained(id);
        } 
        #endregion

        #region GetAllValListCopy
        public IList<TVal> GetAllValListCopy()
        {
            return this.smartDictionaryCache.GetAllValListCopy();
        } 
        #endregion

        #region GetAllKeyListCopy
        public IList<string> GetAllKeyListCopy()
        {
            return this.smartDictionaryCache.GetAllKeyListCopy();
        } 
        #endregion

        #region Count
        public int Count
        {
            get
            {
                return this.smartDictionaryCache.Count;
            }
        } 
        #endregion

        #region CreateHiberarchyTree
        public MultiTree<TVal> CreateHiberarchyTree()
        {
            MultiTree<TVal> tree = new MultiTree<TVal>();
            tree.Initialize(this.agileMultiTree.Root);
            return tree;
        } 
        #endregion

        #region GetNodesOnDepthIndex
        public IList<TVal> GetNodesOnDepthIndex(int depthIndex)
        {
            IList<MNode<TVal>> list = this.agileMultiTree.GetNodesOnDepthIndex(depthIndex);
            return ESBasic.Collections.CollectionConverter.ConvertAll<MNode<TVal>, TVal>(list, delegate(MNode<TVal> node) { return node.TheValue; });
        } 
        #endregion

        #region GetNodesOnDepthIndex
        public IList<TVal> GetNodesOnDepthIndex(string parentID, int depthIndex)
        {
            TVal target = this.Get(parentID);
            if (target == null)
            {
                return null;
            }

            IList<MNode<TVal>> list = this.agileMultiTree.GetNodesOnDepthIndex(target.SequenceCode, this.sequenceCodeSplitter, depthIndex);
            return CollectionConverter.ConvertAll<MNode<TVal>, TVal>(list, delegate(MNode<TVal> node) { return node.TheValue; });
        } 
        #endregion

        #region GetChildrenOf
        public IList<TVal> GetChildrenOf(string parentID)
        {
            TVal target = this.Get(parentID);
            if (target == null)
            {
                return null;
            }

            MNode<TVal> node = this.agileMultiTree.GetNodeByPath(target.SequenceCode, this.sequenceCodeSplitter);
            return CollectionConverter.ConvertAll<MNode<TVal>, TVal>(node.Children, delegate(MNode<TVal> child) { return child.TheValue; });
        } 
        #endregion

        #region GetChildrenCount
        public int GetChildrenCount(string parentID)
        {
            TVal target = this.Get(parentID);
            if (target == null)
            {
                return 0;
            }

            MNode<TVal> node = this.agileMultiTree.GetNodeByPath(target.SequenceCode, this.sequenceCodeSplitter);
            return node.Children.Count;
        } 
        #endregion
    }    
}
