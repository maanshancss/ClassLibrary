using System;
using System.Collections.Generic;
using ESBasic.Collections;
using System.Collections;

namespace ESBasic.Emit.ForEntity
{
    /// <summary>
    /// IObjectClassifier 能根据对象的属性值对其进行分类的分类器。
    /// (1)发射的动态对象分类器内部采用N层嵌套字典实现。
    /// (2)发射的动态对象分类器内部最终采用IObjectContainer来存储分类后的对象。
    /// (3)发射的动态对象分类器可以被序列化。
    /// </summary>
    /// <typeparam name="TObject">对象分类器能处理的对象类型</typeparam>
    public interface IObjectClassifier<TObject> 
    {
        /// <summary>
        /// Properties4Classify 依据哪些属性的值进行分类。其长度等于嵌套字典的层数。
        /// </summary>
        string[] Properties4Classify { get; }

        /// <summary>
        /// 初始化。请特别注意：需要在分类器创建成功或反序列化完成后调用。
        /// </summary>        
        void Initialize(IObjectContainerCreator<TObject> creator);

        /// <summary>
        /// Add 将对象依据其分类属性值放到正确的容器中。注意，对象的用于分类的属性值不得为null。      
        /// </summary>     
        void Add(TObject entity);

        /// <summary>
        /// GetContainers 根据一组分类的属性值进行匹配以获取对应的容器。
        /// 如果参数中某个属性值为null，表示可以匹配所有对象。如果所有的参数都不为null，则返回的列表中最多包含一个容器。
        /// </summary>        
        List<IObjectContainer<TObject>> GetContainers(params object[] propertyValues4Classify);

        /// <summary>
        /// GetDistinctValues 获取分类器内部所有对象的某个分类属性的所有区分值。
        /// </summary>       
        IList GetDistinctValues(string property4Classify);

        /// <summary>
        /// GetAllContainers 获取分类器内部所有的对象容器。
        /// </summary>       
        List<IObjectContainer<TObject>> GetAllContainers();
    }

    #region Example 动态生成的2层对象分类器类型
    [Serializable]
    public class EntityClassifier<TObject> : IObjectClassifier<TObject>
    {
        private Dictionary<string, Dictionary<int, IObjectContainer<TObject>>> dic = new Dictionary<string, Dictionary<int, IObjectContainer<TObject>>>();
        private string[] properties4Classify;

        private List<IObjectContainer<TObject>> allContainerList = new List<IObjectContainer<TObject>>();
        private SortedArray<string> idDistinctArray = new SortedArray<string>();
        private SortedArray<int> ageDistinctArray = new SortedArray<int>();

        [NonSerialized]
        private IObjectContainerCreator<TObject> creator;        
        [NonSerialized]
        private IPropertyQuicker<TObject> quicker;


        
        public EntityClassifier(string[] _propertys4Classify)
        {
            this.properties4Classify = _propertys4Classify;
        }

        public void Initialize(IObjectContainerCreator<TObject> creator)
        {
        }

        public void Add(TObject entity)//, IPropertyQuicker<TObject> quicker, IObjectContainerCreator<TObject> creator)
        {
            string proval0 = (string)quicker.GetPropertyValue(entity, this.properties4Classify[0]);
            this.idDistinctArray.Add(proval0);
            if (!this.dic.ContainsKey(proval0))
            {
                this.dic.Add(proval0, new Dictionary<int, IObjectContainer<TObject>>());
            }

            int proval1 = (int)quicker.GetPropertyValue(entity, this.properties4Classify[1]);
            this.ageDistinctArray.Add(proval1);
            if (!this.dic[proval0].ContainsKey(proval1))
            {
                IObjectContainer<TObject> container = creator.CreateNewContainer();
                this.allContainerList.Add(container) ;
                this.dic[proval0].Add(proval1,container);
            }

            this.dic[proval0][proval1].Add(entity);
        }

        public List<IObjectContainer<TObject>> GetContainers(object[] propertyValues4Classify)
        {
            IList[] distinctValList = new IList[2];
            distinctValList[0] = this.idDistinctArray.GetAll();
            distinctValList[1] = this.ageDistinctArray.GetAll();

            List<object[]> mappingList = DynamicObjectClassifierEmitter.AdjustMappingValues(propertyValues4Classify, distinctValList);

            List<IObjectContainer<TObject>> containerList = new List<IObjectContainer<TObject>>();

            for (int i = 0; i < mappingList.Count; i++)
            {
                IObjectContainer<TObject> container = this.DoGetContainer(mappingList[i]);
                if (container != null)
                {
                    containerList.Add(container);
                }
            }

            return containerList ;
        }

        private IObjectContainer<TObject> DoGetContainer(object[] propertyValues4Classify)
        {
            string proval0 = (string)propertyValues4Classify[0];
            if (!this.dic.ContainsKey(proval0))
            {
                return null;
            }

            int proval1 = (int)propertyValues4Classify[1];
            if (!this.dic[proval0].ContainsKey(proval1))
            {
                return null;
            }

            return this.dic[proval0][proval1];
        }

        #region INTierDictionary<TEntity> 成员

        public string[] Properties4Classify
        {
            get 
            {
                return this.properties4Classify;
            }
        }

        public List<IObjectContainer<TObject>> GetAllContainers()
        {
            return this.allContainerList;
        }

     
        public IList GetDistinctValues(string property4Classify)
        {
            if (property4Classify == this.properties4Classify[0])
            {
                return this.idDistinctArray.GetAll();
            }

            if (property4Classify == this.properties4Classify[1])
            {
                return this.ageDistinctArray.GetAll();
            }

            return null;
        }

        #endregion
    }
    #endregion
}
