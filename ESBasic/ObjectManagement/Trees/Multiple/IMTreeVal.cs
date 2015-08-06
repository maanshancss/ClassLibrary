using System;
using System.Collections.Generic;

namespace ESBasic.ObjectManagement.Trees.Multiple
{
	/// <summary>
	/// IMTreeVal 如果一个对象需要包含于多叉树的节点中，则该对象必须实现IMTreeVal接口。
	/// zhuweisky 2005.07.28
	/// </summary>	
	public interface IMTreeVal
	{
        string CurrentID { get;}
        string FatherID { get;}

        /// <summary>
        /// DepthIndex 当前节点在多叉树中的深度索引。Root的深度索引为0。
        /// 若不想使用深度索引，请返回-1或负数。
        /// </summary>
        int DepthIndex { get; }
	}
}
