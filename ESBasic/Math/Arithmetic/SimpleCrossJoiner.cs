using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Arithmetic
{
    public class SimpleCrossJoiner<T>
    {
        #region Result
        private List<CrossJoinPath<T>> result;
        public List<CrossJoinPath<T>> Result
        {
            get { return result; }
        }
        #endregion

        #region CrossJoin
        public void CrossJoin(ICollection<T> collection)
        {
            if (collection == null || collection.Count == 0)
            {
                return;
            }

            if (this.result == null)
            {
                this.result = new List<CrossJoinPath<T>>();
                foreach (T t in collection)
                {
                    this.result.Add(new CrossJoinPath<T>(t));
                }
                return;
            }

            List<CrossJoinPath<T>> newResult = new List<CrossJoinPath<T>>();
            foreach (CrossJoinPath<T> path in result)
            {
                foreach (T t in collection)
                {
                    newResult.Add(path.CrossJoin(t));
                }
            }

            this.result = newResult;
        }
        #endregion
    }
   
    public class CrossJoinPath<T>
    {
        #region Path
        private List<T> path = new List<T>();
        public List<T> Path
        {
            get { return path; }
        }
        #endregion

        #region Ctor
        public CrossJoinPath(T t)
        {
            this.path.Add(t);
        }

        public CrossJoinPath(List<T> _path)
        {
            this.path = _path;
        } 
        #endregion

        #region CrossJoin
        public CrossJoinPath<T> CrossJoin(T t)
        {
            List<T> newPath = new List<T>();
            foreach (T old in path)
            {
                newPath.Add(old);
            }

            newPath.Add(t);

            return new CrossJoinPath<T>(newPath);
        } 
        #endregion
    }   
}
