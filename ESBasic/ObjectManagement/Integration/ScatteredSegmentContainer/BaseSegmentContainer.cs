using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Integration
{
    public abstract class BaseSegmentContainer<TSegmentID, TVal> : ISegmentContainer<TSegmentID, TVal>
    {
        #region abstract
        protected abstract ISegment<TSegmentID, TVal> GetSmallestSegment();
        protected abstract ISegment<TSegmentID, TVal> GetBiggestSegment();

        ///// <summary>
        ///// GetNextSegment 按照fromSmallToBig指定的顺序返回下一个Segment。如果下一个Segment为null，则返回下下个Segment，依此类推。
        ///// 如果返回null，则表示不再有后续的Segment了。
        ///// </summary>      
        protected abstract ISegment<TSegmentID, TVal> GetNextSegment(TSegmentID curSegmentID, bool fromSmallToBig); 
        #endregion

        #region PickFromSmallToBig
        private bool pickFromSmallToBig = true;
        public bool PickFromSmallToBig
        {
            get { return pickFromSmallToBig; }
            set { pickFromSmallToBig = value; }
        } 
        #endregion

        #region Pick
        /// <summary>
        /// Pick 从整合后的整体中提取有序块。
        /// </summary>
        /// <param name="startIndex">目标块在整合后的整体中的起始位置</param>
        /// <param name="pickCount">提取元素的个数</param>
        /// <returns>有序的列表（从小到大或从大到小，与PickFromSmallToBig一致）</returns> 
        public List<TVal> Pick(int startIndex, int pickCount)
        {
            if (startIndex < 0)
            {
                throw new Exception("startIndex must greater than 0 !");
            }

            if (pickCount <= 0)
            {
                return new List<TVal>();
            }

            return this.DoPick(startIndex, pickCount);
        }

        #region DoPick
        private List<TVal> DoPick(int startIndex, int pickCount)
        {
            List<TVal> resultList = new List<TVal>();

            int accumulateIndex = 0;
            bool startPointFound = false;
            int havePickedCount = 0;
            ISegment<TSegmentID, TVal> curSegment = null;
            if (this.pickFromSmallToBig)
            {
                curSegment = this.GetSmallestSegment();
            }
            else
            {
                curSegment = this.GetBiggestSegment();
            }

            while (curSegment != null)
            {
                bool startPointInCurSegment = false;
                IList<TVal> curContent = curSegment.GetContent();
                int contentCount = curContent.Count;

                #region Process
                if ((curContent != null) && (contentCount > 0))
                {
                    if (!startPointFound)
                    {
                        if (accumulateIndex + contentCount >= startIndex)
                        {
                            startPointFound = true;
                            startPointInCurSegment = true;
                        }
                        else
                        {
                            accumulateIndex += contentCount;
                        }
                    }

                    if (startPointFound)
                    {
                        if (this.pickFromSmallToBig)
                        {
                            int offset = startPointInCurSegment ? startIndex - accumulateIndex : 0;
                            for (int i = offset; i < contentCount; i++)
                            {
                                resultList.Add(curContent[i]);
                                ++havePickedCount;
                                if (havePickedCount >= pickCount)
                                {
                                    return resultList;
                                }
                            }
                        }
                        else
                        {
                            int offset = startPointInCurSegment ? (contentCount - 1) - (startIndex - accumulateIndex) : contentCount - 1;

                            for (int i = offset; i >= 0; i--)
                            {
                                resultList.Add(curContent[i]);
                                ++havePickedCount;
                                if (havePickedCount >= pickCount)
                                {
                                    return resultList;
                                }
                            }
                        }
                    }
                }
                #endregion

                curSegment = this.GetNextSegment(curSegment.ID, this.pickFromSmallToBig);
            }

            return resultList;
        }
        #endregion       
        #endregion        

        #region GetAllSegments
        public virtual List<ISegment<TSegmentID, TVal>> GetAllSegments()
        {
            List<ISegment<TSegmentID, TVal>> list = new List<ISegment<TSegmentID, TVal>>();
            ISegment<TSegmentID, TVal> smallest = (ISegment<TSegmentID, TVal>)this.GetSmallestSegment();
            if (smallest == null)
            {
                return list;
            }

            list.Add(smallest);          

            ISegment<TSegmentID, TVal> current = smallest;
            while (true)
            {
                ISegment<TSegmentID, TVal> next = (ISegment<TSegmentID, TVal>)this.GetNextSegment(current.ID, true);
                if (next == null)
                {
                    break;
                }

                list.Add(next);
                current = next;
            }

            return list;
        }
        #endregion        
    }
}
