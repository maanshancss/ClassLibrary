using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Emit.ForEntity
{   
    public static class PropertyQuickerFactory 
    {
        private static PropertyQuickerEmitter PropertyQuickerEmitter = new PropertyQuickerEmitter(false);
        private static Dictionary<Type, IPropertyQuicker> PropertyQuickerDic = new Dictionary<Type, IPropertyQuicker>();

        public static IPropertyQuicker<TEntity> CreatePropertyQuicker<TEntity>() 
        {
            return (IPropertyQuicker<TEntity>)PropertyQuickerFactory.CreatePropertyQuicker(typeof(TEntity));
        }

        public static IPropertyQuicker CreatePropertyQuicker(Type entityType)
        {
            lock (PropertyQuickerFactory.PropertyQuickerEmitter)
            {               
                if (!PropertyQuickerFactory.PropertyQuickerDic.ContainsKey(entityType))
                {
                    Type propertyQuickerType = PropertyQuickerFactory.PropertyQuickerEmitter.CreatePropertyQuickerType(entityType);
                    //PropertyQuickerFactory.PropertyQuickerEmitter.Save();
                    IPropertyQuicker quicker = (IPropertyQuicker)Activator.CreateInstance(propertyQuickerType);
                    PropertyQuickerFactory.PropertyQuickerDic.Add(entityType, quicker);
                }

                return PropertyQuickerFactory.PropertyQuickerDic[entityType];
            }
        }    
    }

}
