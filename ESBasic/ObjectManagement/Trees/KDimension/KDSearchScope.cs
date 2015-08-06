using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Trees.KDimension
{
    /// <summary>
    /// KDSearchScope 用于表示某维的查询范围或Like查询
    /// </summary>
    public class KDSearchScope
    {
        #region Ctor
        public KDSearchScope() { }
        public KDSearchScope(string column ,IComparable min, bool _minClosed, IComparable max, bool _maxClosed)
        {
            this.columnName = column;
            this.minValue = min;
            this.maxValue = max;
            this.minClosed = _minClosed;
            this.maxClosed = _maxClosed;
        }
        public KDSearchScope(string column, KDSearchType searchType, string _matchString)
        {
            this.columnName = column;
            this.kDSearchType = searchType;
            this.matchString = _matchString;
        }
        #endregion       

        #region ColumnName
        private string columnName = "";
        public string ColumnName
        {
            get { return columnName; }
            set { columnName = value; }
        } 
        #endregion

        #region KDSearchType
        private KDSearchType kDSearchType = KDSearchType.Default;
        public KDSearchType KDSearchType
        {
            get { return kDSearchType; }
            set { kDSearchType = value; }
        } 
        #endregion

        #region MinValue
        private IComparable minValue;
        public IComparable MinValue
        {
            get { return minValue; }
            set { minValue = value; }
        }
        #endregion

        #region MinClosed
        private bool minClosed = false;

        public bool MinClosed
        {
            get { return minClosed; }
            set { minClosed = value; }
        }
        #endregion

        #region MaxValue
        private IComparable maxValue;

        public IComparable MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }
        #endregion

        #region MaxClosed
        private bool maxClosed = false;

        public bool MaxClosed
        {
            get { return maxClosed; }
            set { maxClosed = value; }
        }
        #endregion

        #region MatchString
        private string matchString;
        /// <summary>
        /// MatchString 用于Like或NotLike的匹配字串。当KDSearchType为Default时，将忽略该字段。
        /// </summary>
        public string MatchString
        {
            get { return matchString; }
            set { matchString = value; }
        }
        #endregion
    }

    public enum KDSearchType
    {
        /// <summary>
        /// Default 按范围搜索
        /// </summary>
        Default = 0, 
        Like,
        NotLike
    }
}
