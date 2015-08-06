using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Threading.Synchronize;

namespace ESBasic.ObjectManagement.Trees.Multiple
{   
    /// <summary>
    /// IMultiTree 的参考实现。该实现是线程安全的。
    /// </summary>
    [Serializable]
    public class MultiTree<TVal> : IMultiTree<TVal> where TVal : IMTreeVal        
    {
        private MNode<TVal> root;

        #region SmartRWLocker 
        [NonSerialized]
        private SmartRWLocker _smartRWLocker = null;
        /// <summary>
        /// SmartRWLocker 为支持反序列化后_smartRWLocker不为null而设计。
        /// </summary>
        protected SmartRWLocker SmartRWLocker
        {
            get
            {
                if (this._smartRWLocker == null)
                {
                    this._smartRWLocker = new SmartRWLocker();
                }

                return this._smartRWLocker;
            }
        }       
        #endregion  
        
        #region Initialize
        /// <summary>
        /// Initialize 使用已经存在的某个树（或子树）来构造一个新树。
        /// </summary>      
        public virtual void Initialize(MNode<TVal> _rootNode)
        {
            this.root = _rootNode;
        }

        public virtual void Initialize(TVal rootVal, IList<TVal> members)
        {
            if ((rootVal == null) || (members == null))
            {
                throw new Exception("Root must not be null!");
            }

            this.root = new MNode<TVal>(rootVal ,null);
            if (this.root.TheValue.DepthIndex != 0)
            {
                this.FillChildren(this.root, members);
            }
            else
            {
                #region InitializeFromLayer
                IDictionary<int, IList<TVal>> layerDic = new Dictionary<int, IList<TVal>>();
                foreach (TVal val in members)
                {
                    if (val.DepthIndex < 0)
                    {
                        throw new Exception("Not all DepthIndex is Valid !");
                    }

                    if (!layerDic.ContainsKey(val.DepthIndex))
                    {
                        layerDic.Add(val.DepthIndex, new List<TVal>());
                    }

                    layerDic[val.DepthIndex].Add(val);
                }

                IList<TVal>[] memberLayerAry = new IList<TVal>[layerDic.Count];               

                //如果members中不包含Root
                if (!layerDic.ContainsKey(0))
                {
                    layerDic.Add(0, new List<TVal>());
                    layerDic[0].Add(this.root.TheValue);
                }                
                
                for (int i = 0; i < layerDic.Count; i++)
                {
                    if (!layerDic.ContainsKey(i))
                    {
                        throw new Exception("DepthIndex are not continuous !");
                    }
                    memberLayerAry[i] = layerDic[i];
                }

                this.InitializeFromLayer(memberLayerAry); 
                #endregion
            }            
        }

        #region FillChildren
        private void FillChildren(MNode<TVal> father, IList<TVal> members)
        {
            foreach (TVal mem in members)
            {
                if ((mem.FatherID == father.TheValue.CurrentID) && (mem.CurrentID != (father.TheValue.CurrentID)))
                {                   
                    father.AddChild(mem);
                }
            }

            foreach (MNode<TVal> mem in father.Children)
            {
                this.FillChildren(mem, members);
            }
        }
        #endregion 

        #region InitializeFromLayer
        private void InitializeFromLayer(IList<TVal>[] memberLayerAry)
        {
            if ((memberLayerAry == null) || (memberLayerAry[0].Count != 1))
            {
                throw new Exception("Root must not be null and root must be unique!");
            }

            this.root = new MNode<TVal>(memberLayerAry[0][0] ,null);

            IDictionary<string, MNode<TVal>> parentDic = new Dictionary<string, MNode<TVal>>();
            parentDic.Add(this.root.TheValue.CurrentID, this.root);

            for (int i = 1; i < memberLayerAry.Length; i++)
            {
                IDictionary<string, MNode<TVal>> tempDic = new Dictionary<string, MNode<TVal>>();
                IList<TVal> memberAtThisLayer = memberLayerAry[i];
                foreach (TVal val in memberAtThisLayer)
                {
                    if (!parentDic.ContainsKey(val.FatherID))
                    {
                        throw new Exception(string.Format("The parent of [{0}] named [{1}] not found !", val.CurrentID ,val.FatherID));
                    }

                    MNode<TVal> child = parentDic[val.FatherID].AddChild(val);
                    tempDic.Add(child.TheValue.CurrentID, child);
                }

                parentDic = tempDic;
            }
        } 
        #endregion
        #endregion

        #region GetNodesOnDepthIndex
        public IList<MNode<TVal>> GetNodesOnDepthIndex(int depthIndex)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                IList<MNode<TVal>> list = new List<MNode<TVal>>();

                if (depthIndex == 0)
                {
                    list.Add(this.root);
                    return list;
                }

                this.DoGetNodesOnDepthIndex(ref list, this.root, 0, depthIndex);

                return list;
            }
        }

        #region DoGetNodesOnDepthIndex
        private void DoGetNodesOnDepthIndex(ref IList<MNode<TVal>> list, MNode<TVal> curNode, int curDepthIndex, int targetDepthIndex)
        {
            if (curDepthIndex == (targetDepthIndex - 1))
            {
                foreach (MNode<TVal> child in curNode.Children)
                {
                    list.Add(child);
                }

                return;
            }

            foreach (MNode<TVal> child in curNode.Children)
            {
                this.DoGetNodesOnDepthIndex(ref list, child, curDepthIndex + 1, targetDepthIndex);
            }
        }
        #endregion 
        #endregion

        #region GetNodesOnDepthIndex
        public IList<MNode<TVal>> GetNodesOnDepthIndex(string idPath, char separator, int depthIndex)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                string[] pathAry = idPath.Split(separator);
                int parentDepthIndex = pathAry.Length - 1;

                if (parentDepthIndex > depthIndex)
                {
                    return null;
                }

                MNode<TVal> target = this.GetNodeByPath(idPath, separator);
                if (target == null)
                {
                    return null;
                }

                MultiTree<TVal> childTree = new MultiTree<TVal>();
                childTree.Initialize(target);

                return childTree.GetNodesOnDepthIndex(depthIndex - parentDepthIndex);
            }
        } 
        #endregion

        #region GetNodeByPath
        public MNode<TVal> GetNodeByPath(string idPath, char separator)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                string[] pathAry = idPath.Split(separator);
                if (pathAry[0] != this.root.TheValue.CurrentID)
                {
                    return null;
                }

                MNode<TVal> temp = this.root;
                for (int i = 1; i < pathAry.Length; i++)
                {
                    bool found = false;
                    foreach (MNode<TVal> child in temp.Children)
                    {
                        if (child.TheValue.CurrentID == pathAry[i])
                        {
                            temp = child;
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        return null;
                    }
                }

                return temp;
            }
        } 
        #endregion

        #region GetFamilyByPath
        public List<MNode<TVal>> GetFamilyByPath(string idPath, char separator)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                string[] pathAry = idPath.Split(separator);
                if (pathAry[0] != this.root.TheValue.CurrentID)
                {
                    return null;
                }

                List<MNode<TVal>> nodeList = new List<MNode<TVal>>();
                nodeList.Add(this.root);
                MNode<TVal> temp = this.root;
                for (int i = 1; i < pathAry.Length; i++)
                {
                    bool found = false;
                    foreach (MNode<TVal> child in temp.Children)
                    {
                        if (child.TheValue.CurrentID == pathAry[i])
                        {
                            nodeList.Add(child);
                            temp = child;
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        return null;
                    }
                }

                return nodeList;
            }
        } 
        #endregion

        #region GetNodeByID
        public MNode<TVal> GetNodeByID(string valID)
        {
            if (valID == this.root.TheValue.CurrentID)
            {
                return this.root;
            }

            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                return this.DoGetNode(this.root, valID);
            }
        }

        private MNode<TVal> DoGetNode(MNode<TVal> curNode, string ValID)
        {
            foreach (MNode<TVal> node in curNode.Children)
            {
                if (node.TheValue.CurrentID == (ValID))
                {
                    return node;
                }               
            }

            foreach (MNode<TVal> node in curNode.Children)
            {
                MNode<TVal> target = this.DoGetNode(node, ValID);
                if (target != null)
                {
                    return target;
                }
            }

            return null;
        } 
        #endregion

        #region Root
        public MNode<TVal> Root
        {
            get
            {
                return this.root;
            }
        } 
        #endregion

        #region Count
        public int Count
        {
            get
            {
                using (this.SmartRWLocker.Lock(AccessMode.Read))
                {
                    if (this.root == null)
                    {
                        return 0;
                    }

                    int count = 1;
                    this.CountAllNodes(this.root, ref count);
                    return count;
                }
            }
        }

        #region CountAllNodes
        private void CountAllNodes(MNode<TVal> childTreeRoot, ref int count)
        {
            if (childTreeRoot == null)
            {
                return;
            }

            ++count;

            foreach (MNode<TVal> node in childTreeRoot.Children)
            {
                this.CountAllNodes(node, ref count);
            }
        }
        #endregion
        #endregion        

        #region GetOffsprings
        public IList<MNode<TVal>> GetOffsprings(string valID)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                MNode<TVal> curNode = this.GetNodeByID(valID);
                if (curNode == null)
                {
                    return null;
                }

                IList<MNode<TVal>> list = new List<MNode<TVal>>();

                this.DoGetOffsprings(curNode, ref list);

                return list;
            }
        }

        private void DoGetOffsprings(MNode<TVal> curNode, ref IList<MNode<TVal>> list)
        {
            foreach (MNode<TVal> child in curNode.Children)
            {
                list.Add(child);

                this.DoGetOffsprings(child, ref list);
            }
        } 
        #endregion

        #region GetLeaves
        public IList<TVal> GetLeaves(string idPath, char separator)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                IList<TVal> leafList = new List<TVal>();
                MNode<TVal> node = this.GetNodeByPath(idPath, separator);
                if (node == null)
                {
                    return leafList;
                }

                this.DoGetLeaves(node, ref leafList);
                return leafList;
            }
        } 

        #region DoGetLeaves
        private void DoGetLeaves(MNode<TVal> node, ref IList<TVal> leafList)
        {
            if ((node.Children == null) || (node.Children.Count == 0))
            {
                leafList.Add(node.TheValue);
                return;
            }

            foreach (MNode<TVal> child in node.Children)
            {
                this.DoGetLeaves(child, ref leafList);
            }
        } 
        #endregion

        #endregion        

        #region ActionOnEachNode
        public void ActionOnEachNode(CbGeneric<MNode<TVal>> action)
        {
            if (this.root == null)
            {
                return;
            }

            this.DoActionOnEach(action, this.root);
        }

        private void DoActionOnEach(CbGeneric<MNode<TVal>> action, MNode<TVal> node)
        {
            action(node);
            foreach (MNode<TVal> child in node.Children)
            {
                this.DoActionOnEach(action, child);
            }
        } 
        #endregion
    }   
}
