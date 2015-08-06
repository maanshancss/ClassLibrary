using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Emit.ForEntity
{
    /// <summary>
    /// IPropertyQuicker 用于避免反射而直接读取或设定entity对象的属性值。
    /// </summary>   
    public interface IPropertyQuicker<TEntity> : IPropertyQuicker
    {
        object GetPropertyValue(TEntity entity, string propertyName);

        /// <summary>
        ///  SetPropertyValue 如果TEntity是值类型，则此方法将无法改变entity对应的属性值。
        /// </summary>       
        void SetPropertyValue(TEntity entity, string propertyName, object propertyValue);       
    }

    public interface IPropertyQuicker
    {
        object GetValue(object entity, string propertyName);
        void SetValue(object entity, string propertyName, object propertyValue);

    }
}
