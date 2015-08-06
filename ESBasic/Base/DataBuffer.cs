using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic
{
    /// <summary>
    /// DataBuffer 数据缓冲区。
    /// </summary>
    [Serializable]
    public class DataBuffer
    {
        #region Ctor
        public DataBuffer() { }

        public DataBuffer(byte[] _data)
        {
            this.data = _data;
            this.length = this.data.Length;
            this.offset = 0;
        }

        public DataBuffer(byte[] _data, int len)
        {
            this.data = _data;
            this.length = len;
            this.offset = 0;
        }

        public DataBuffer(byte[] _data, int _offset, int len)
        {
            this.data = _data;
            this.offset = _offset;
            this.length = len;
        } 
        #endregion

        #region Data
        private byte[] data;
        /// <summary>
        /// Data 存储数据的内存。
        /// </summary>
        public byte[] Data
        {
            get { return data; }
            set { data = value; }
        } 
        #endregion

        #region Offset
        private int offset;
        /// <summary>
        /// Offset 有效数据起始处的偏移位置。
        /// </summary>
        public int Offset
        {
            get { return offset; }
            set { offset = value; }
        } 
        #endregion

        #region Length
        private int length;
        /// <summary>
        /// Length 有效数据的长度。
        /// </summary>
        public int Length
        {
            get { return length; }
            set { length = value; }
        } 
        #endregion

        #region GetPureData
        public byte[] GetPureData()
        {
            if (this.offset == 0 && this.data.Length == this.length)
            {
                return this.data;
            }

            byte[] buff = new byte[this.length];
            Buffer.BlockCopy(this.data, this.offset, buff, 0, this.length);
            return buff;
        } 
        #endregion

        #region MergeDataBuffers
        /// <summary>
        /// MergeDataBuffers 按顺序将所有DataBuffer中的数据复制到一个新的缓冲区中。
        /// </summary>     
        public static byte[] MergeDataBuffers(params DataBuffer[] ary)
        {
            if (ary == null || ary.Length == 0)
            {
                return null;
            }

            if (ary.Length == 1)
            {
                return ary[0].GetPureData();
            }

            int totalLen = 0;
            for (int i = 0; i < ary.Length; i++)
            {
                totalLen += ary[i].Length;
            }

            byte[] buff = new byte[totalLen];
            int offset = 0;
            for (int i = 0; i < ary.Length; i++)
            {
                Buffer.BlockCopy(ary[i].Data, ary[i].Offset, buff, offset, ary[i].Length);
                offset += ary[i].Length;
            }

            return buff;
        } 
        #endregion
    }
}
