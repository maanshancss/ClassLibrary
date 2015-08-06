using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Integration
{
    /// <summary>
    /// ICombined 可以被合并的对象必须实现的接口。
    /// zhuweisky 2009.05.23
    /// </summary>
    /// <typeparam name="TID">被合并对象的标志类型</typeparam>
    /// <typeparam name="TCombinedObj">被合并对象的类型</typeparam>   
    public interface ICombined<TID, TCombinedObj>
    {
        TID ID { get; }
        void Combine(TCombinedObj obj);
    }
}
