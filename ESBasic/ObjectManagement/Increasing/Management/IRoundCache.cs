using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Increasing.Management
{
    /// <summary>
    /// IRoundCache 某一Round的完整的数据缓存。用于被序列化存储。
    /// </summary>
    public interface IRoundCache<TRoundID>
    {
        TRoundID RoundID { get; }
    }
}
