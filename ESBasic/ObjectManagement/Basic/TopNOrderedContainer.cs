using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Threading.Synchronize;

namespace ESBasic.ObjectManagement
{
    /// <summary>
    /// TopNOrderedContainer 用于始终保持排行榜前N名的Object。该实现是线程安全的。
    /// zhuweisky 2009.05.23
    /// </summary>    
    /// <typeparam name="TObj">被排名的对象类型</typeparam>
    public class TopNOrderedContainer<TObj> where TObj : IOrdered<TObj>
    {
        private TObj[] orderedArray = null;
        private int validObjCount = 0;
        private SmartRWLocker smartRWLocker = new SmartRWLocker();

        #region TopNumber
        private int topNumber = 10;
        public int TopNumber
        {
            get { return topNumber; }
            set { topNumber = value; }
        } 
        #endregion

        #region Ctor
        public TopNOrderedContainer() { }
        public TopNOrderedContainer(int _topNumber)
        {
            this.topNumber = _topNumber;
        }
        #endregion

        #region Initialize
        public void Initialize()
        {
            if (this.topNumber < 1)
            {
                throw new Exception("The value of TopNumber must greater than 0 ");
            }

            this.orderedArray = new TObj[this.topNumber];
        } 
        #endregion

        #region Add List
        public void Add(IEnumerable<TObj> list)
        {
            if (list == null)
            {
                return;
            }

            using (this.smartRWLocker.Lock(AccessMode.Write))
            {
                foreach (TObj obj in list)
                {
                    this.DoAdd(obj);
                }
            }
        } 
        #endregion

        #region Add
        public void Add(TObj obj)
        {
            using (this.smartRWLocker.Lock(AccessMode.Write))
            {
                this.DoAdd(obj);
            }
        } 
        #endregion        

        #region GetTopN
        public TObj[] GetTopN()
        {
            using (this.smartRWLocker.Lock(AccessMode.Read))
            {
                return (TObj[])this.orderedArray.Clone();
            }
        } 
        #endregion

        #region Private
        #region DoAdd
        private void DoAdd(TObj obj)
        {
            if (obj == null)
            {
                return;
            }

            if (this.validObjCount < this.topNumber)
            {
                this.orderedArray[this.validObjCount] = obj;
                this.Adjust(this.validObjCount);

                ++this.validObjCount;
                return;
            }

            if (this.orderedArray[this.topNumber - 1].IsTopThan(obj))
            {
                return;
            }

            this.orderedArray[this.topNumber - 1] = obj;
            this.Adjust(this.topNumber - 1);
        }
        #endregion

        #region Adjust    

        /// <summary>
        /// Adjust 新加入的对象初始时被放置于最后一个有效位置即posIndex，需要将其调整到正确的位置。
        /// </summary>        
        private void Adjust(int posIndex)
        {
            TObj target = this.orderedArray[posIndex];
            int targetPosIndex = -1;

            #region 寻找合适的目标位置
            if (target.IsTopThan(this.orderedArray[0]))
            {
                targetPosIndex = 0;
            }
            else
            {
                int left = 0;
                int right = posIndex;

                while (right - left > 1)
                {
                    int middle = (left + right) / 2;

                    if (target.IsTopThan(this.orderedArray[middle]))
                    {
                        right = middle;
                    }
                    else
                    {
                        left = middle;
                    }
                }

                targetPosIndex = left;
                if (right != left)
                {
                    if (!target.IsTopThan(this.orderedArray[left]))
                    {
                        targetPosIndex = right;
                    }
                }
            } 
            #endregion

            for (int i = posIndex; i > targetPosIndex; i--)
            {
                this.orderedArray[i] = this.orderedArray[i-1] ;
            }

            this.orderedArray[targetPosIndex] = target;
        }

        #endregion
        #endregion
    }

    /// <summary>
    /// IOrdered 参与排行榜排序的对象必须实现的接口。
    /// </summary>
    /// <typeparam name="TOrderedObj">参与排行榜排序的对象的类型</typeparam>
    public interface IOrdered<TOrderedObj>
    {
        bool IsTopThan(TOrderedObj other);
    }
}
