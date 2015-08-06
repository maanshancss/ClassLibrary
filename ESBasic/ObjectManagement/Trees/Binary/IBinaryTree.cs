using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Trees.Binary
{
    public interface IBinaryTree<TVal> where TVal : IComparable
    {
        Node<TVal> Root { get; }
        int Depth { get; }
        int Count { get; }		
    }
}
