using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Integration
{
    /// <summary>
    /// ISegment 片段，一个片段有有序的多个元素TVal构成。
    /// </summary>   
    /// <typeparam name="TSegmentID">片段标志的类型</typeparam>
    /// <typeparam name="TVal">构成片段的元素的类型</typeparam>
    public interface ISegment<TSegmentID, TVal>
    {
        /// <summary>
        /// ID 每个片段的唯一标志。
        /// </summary>
        TSegmentID ID { get; }

        /// <summary>
        /// GetContent 获取片段中的所有元素，从小到大排列。
        /// </summary>        
        IList<TVal> GetContent();
    }
}
