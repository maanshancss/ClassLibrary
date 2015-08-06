using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Threading.Synchronize;

namespace ESBasic.ObjectManagement.Trees.Multiple
{
    /// <summary>
    /// AgileMultiTree IAgileMultiTree�ӿڵĲο�ʵ�֣���֤���̰߳�ȫ�ġ�
    /// </summary>   
    [Serializable]
    public class AgileMultiTree<TVal> : MultiTree<TVal>, IAgileMultiTree<TVal> where TVal : IMTreeVal        
    {        
        #region SequenceCodeSplitter
        [NonSerialized]
        private char sequenceCodeSplitter = ',';
        public char SequenceCodeSplitter
        {
            get { return sequenceCodeSplitter; }
            set { sequenceCodeSplitter = value; }
        } 
        #endregion

        #region AgileNodePicker
        [NonSerialized]
        protected IAgileNodePicker<TVal> agileNodePicker;
        public IAgileNodePicker<TVal> AgileNodePicker
        {
            set { agileNodePicker = value; }
        }
        #endregion             

        #region Initialize
        public virtual void Initialize()
        {
            TVal root = this.agileNodePicker.PickupRoot();            
            IList<TVal> all = ESBasic.Collections.CollectionConverter.CopyAllToList<TVal>(this.agileNodePicker.RetrieveAll().Values);

            base.Initialize(root, all);
        } 
        #endregion

        #region EnsureNodeExist
        public MNode<TVal> EnsureNodeExist(string nodeSequenceCode)
        {
            using(base.SmartRWLocker.Lock(AccessMode.Write))
            {
                MNode<TVal> target = base.GetNodeByPath(nodeSequenceCode, this.sequenceCodeSplitter);
                if (target != null)
                {
                    return target;
                }

                string[] uppers = nodeSequenceCode.Split(this.sequenceCodeSplitter);
                if ((uppers[0] != base.Root.TheValue.CurrentID) || (uppers.Length < 2))
                {
                    return null;
                }

                return this.AppendOffSprings(nodeSequenceCode, uppers);
            }
        }

        #region SynAppendOffSprings
        /// <summary>
        /// SynAppendOffSprings ׷�ӽڵ�������ͬ������
        /// </summary>       
        private MNode<TVal> AppendOffSprings(string nodeSequenceCode, string[] uppers)
        {
            //˫��⣬�����һ���߳��Ѿ�׷�ӳɹ�����ֱ�ӷ��ء�
            MNode<TVal> target = base.GetNodeByPath(nodeSequenceCode, this.sequenceCodeSplitter);
            if (target != null)
            {
                return target;
            }

            string tempSeqcode = uppers[0];
            MNode<TVal> upperNode = base.Root;
            for (int i = 1; i < uppers.Length; i++)
            {
                tempSeqcode += this.sequenceCodeSplitter + uppers[i];
                MNode<TVal> temp = base.GetNodeByPath(tempSeqcode, this.sequenceCodeSplitter);
                if (temp == null)
                {
                    return this.DOAppendOffSprings(upperNode, uppers, i);
                }
                else
                {
                    upperNode = temp;
                }

            }

            return upperNode;
        } 
        #endregion

        #region DOAppendOffSprings
        private MNode<TVal> DOAppendOffSprings(MNode<TVal> current, string[] offSpringIDs, int startIndex)
        {
            MNode<TVal> temp = current;
            for (int i = startIndex; i < offSpringIDs.Length; i++)
            {
                TVal child = this.agileNodePicker.Retrieve(offSpringIDs[i]);
                if (child == null)
                {
                    return null;
                }
                else
                {
                    temp = temp.AddChild(child);
                }
            }

            return temp;
        }
        #endregion
        #endregion        
    }
}
