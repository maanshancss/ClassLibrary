using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Collections
{  
    /// <summary>
    /// DispersiveKeyScope ���ڱ�ʾ���������Χ��һ����ɢ������ֵ
    /// </summary>
    public class DispersiveKeyScope
    {
        #region DispersiveKeyList   
        private SortedArray<int> dispersiveKeySortedArray = new SortedArray<int>();
        public ICollection<int> DispersiveKeyList
        {
            set
            {
                this.dispersiveKeySortedArray = new SortedArray<int>(value);
            }
        }
        #endregion

        #region DispersiveKeyScopeList Ԫ��ΪKeyScope
        private IList<KeyScope> dispersiveKeyScopeList = new List<KeyScope>();
        public IList<KeyScope> DispersiveKeyScopeList
        {
            set
            {
                if (value == null)
                {
                    this.dispersiveKeyScopeList = new List<KeyScope>();
                }
                else
                {
                    this.dispersiveKeyScopeList = value;
                }
            }
        }
        #endregion

        #region Contains
        public bool Contains(int val)
        {
            bool found = this.dispersiveKeySortedArray.Contains(val);

            if (!found)
            {
                found = CollectionHelper.ContainsSpecification<KeyScope>(this.dispersiveKeyScopeList, delegate(KeyScope scope) { return scope.Contains(val); });               
            }

            return found;
        }
        #endregion

        #region Add
        public void Add(int val)
        {
            this.dispersiveKeySortedArray.Add(val);
        } 
        #endregion
    }   
}
