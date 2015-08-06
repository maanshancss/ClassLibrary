using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement
{
    /// <summary>
    /// Circle Ȧ�ṹ�����̰߳�ȫ��
    /// </summary>
    /// <typeparam name="T">Ȧ��ÿ���ڵ�洢�Ķ��������</typeparam>
    public class Circle<T>
    {
        private IList<T> list = new List<T>();
        private int currentPosition = 0;

        #region Ctor
        public Circle() { }

        public Circle(IList<T> _list)
        {
            if (_list != null)
            {
                this.list = _list;
            }
        }        
        #endregion

        #region Header
        public T Header
        {
            get
            {
                if (list.Count == 0)
                {
                    return default(T);
                }

                return this.list[0];
            }
        } 
        #endregion

        #region Tail
        public T Tail
        {
            get
            {
                if (list.Count == 0)
                {
                    return default(T);
                }

                return this.list[this.list.Count - 1];
            }
        }
        #endregion

        #region Count
        public int Count
        {
            get
            {
                return this.list.Count;
            }
        } 
        #endregion

        #region Current
        public T Current
        {
            get
            {
                if (this.list.Count == 0)
                {
                    return default(T);
                }

                return this.list[this.currentPosition];
            }
        } 
        #endregion

        #region MoveNext
        public void MoveNext()
        {
            if (this.list.Count == 0)
            {
                return;
            }

            this.currentPosition = (this.currentPosition + 1) % this.list.Count;
        } 
        #endregion

        #region MoveBack
        public void MoveBack()
        {
            if (this.list.Count == 0)
            {
                return;
            }

            this.currentPosition = (this.currentPosition + this.list.Count - 1) % this.list.Count;
        } 
        #endregion

        #region PeekNext
        public T PeekNext()
        {
            this.MoveNext();
            T next = this.Current;
            this.MoveBack();

            return next;
        } 
        #endregion

        #region PeekBack
        public T PeekBack()
        {
            this.MoveBack();
            T previous = this.Current;
            this.MoveNext();

            return previous;
        }
        #endregion

        #region SetCurrent
        public void SetCurrent(T val)
        {
            if (this.Current.Equals(val))
            {
                return;
            }

            for (int i = 0; i < this.list.Count; i++)
            {
                if (this.list[i].Equals(val))
                {
                    this.currentPosition = i;
                    return;
                }
            }
        } 
        #endregion

        #region Append
        public void Append(T obj)
        {
            this.list.Add(obj);
        } 
        #endregion

        #region Insert
        public void InsertAt(T obj, int postionIndex)
        {
            if (this.list.Count == 0)
            {
                this.list.Add(obj);
                return;
            }

            int index = postionIndex % this.list.Count;
            this.list.Insert(index, obj);

            if (index <= this.currentPosition)
            {
                ++ this.currentPosition;
            }
        } 
        #endregion

        #region RemoveTail
        public void RemoveTail()
        {
            if (this.list.Count == 0)
            {
                return;
            }

            this.RemoveAt(this.list.Count - 1);
        } 
        #endregion

        #region RemoveAt
        public void RemoveAt(int postionIndex)
        {
            if (this.list.Count == 0)
            {               
                return;
            }

            int index = postionIndex % this.list.Count;
            this.list.RemoveAt(index);

            if (index < this.currentPosition)
            {
                --this.currentPosition;
            }
        } 
        #endregion
    }
}
