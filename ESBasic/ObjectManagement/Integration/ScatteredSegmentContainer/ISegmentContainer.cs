using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Integration
{
    /// <summary>
    /// ISegmentContainer 用于存放片段ISegment的容器。可以将分散的片段整合为一个有序的整体。
    /// </summary>
    /// <typeparam name="TSegmentID">片段标志的类型</typeparam>
    /// <typeparam name="TVal">构成片段的元素的类型</typeparam>
    public interface ISegmentContainer<TSegmentID, TVal>
    {
        /// <summary>
        /// PickFromSmallToBig 当从整合后的整体中提取有序块时采用的顺序。
        /// </summary>
        bool PickFromSmallToBig { get; set; }

        /// <summary>
        /// Pick 从整合后的整体中提取有序块。
        /// </summary>
        /// <param name="startIndex">目标块在整合后的整体中的起始位置</param>
        /// <param name="pickCount">提取元素的个数</param>
        /// <returns>有序的列表（从小到大或从大到小，与PickFromSmallToBig属性一致）</returns> 
        List<TVal> Pick(int startIndex, int pickCount);

        /// <summary>
        /// GetAllSegments 获取容器内的所有片段。
        /// </summary>        
        List<ISegment<TSegmentID, TVal>> GetAllSegments();
    }
}
