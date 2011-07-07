using System;
using System.Collections.Generic;
using VertexPipeline;

namespace XNASysLib.XNAKernel
{
    public class SysDataList<T>
          where T : ISysData
    {
        #region Fields
        private object[] _contents;
        private const int _offset = 100;
        const int MaxNumOfData = 200;
        #endregion

        #region Properties


        #endregion

        public SysDataList()
        {
            this._contents = new object[MaxNumOfData];
            
            for (int i = 0; i < MaxNumOfData; i++)
            {
                _contents[i] = new List<ISysData>();

            }
        }

        public bool IsSlotEmpty(int index)
        {
            return ((List<ISysData>)_contents[index]).Count == 0 ? true : false;
        }


        public int EmptySlot(Type dataType)
        {
            if (dataType.Name != "ObjData")
                new Exception();

            int dataOffset = -50;
            for (int index = 0; index < MaxNumOfData; index++)
            {
                _contents[index] = new List<ISysData>();
                if (IsSlotEmpty(index))
                    return index+dataOffset;
            }
            return 0;
        }

        /// <summary>
        /// Add the data value to it's data slot
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Add(ISysData value)
        {

            int index = _offset + value.SysDataType;
            
            try
            {
                List<ISysData> dataList =
                    (List<ISysData>)_contents[index];
                dataList.Add(value);
                if (dataList.Count > 3)
                    dataList.RemoveRange(2, dataList.Count - 2);
                dataList.Sort();
                return index;
            }
            catch (Exception e)
            {
                throw new ArgumentException("??");
            }

        }
        public object GetData(int dataType, int dataIndex)
        {
            int index = dataType + _offset;
            return ((List<ISysData>)_contents[index])[dataIndex];
        }
        public object GetData(Type type)
        { 
            foreach(object obj in _contents)
            {
                List<ISysData> objC = (List<ISysData>)obj;
                if (objC[0].GetType() == type)
                    return objC[0];
            }

            return 0;
        }

        /*

         public void Clear()
         {
             _count = 0;
         }

         public bool Contains(object value)
         {
             bool inList = false;
             for (int i = 0; i < Count; i++)
             {
                 if (_contents[i] == value)
                 {
                     inList = true;
                     break;
                 }
             }
             return inList;
         }

         public int IndexOf(object value)
         {
             int itemIndex = -1;
             for (int i = 0; i < Count; i++)
             {
                 if (_contents[i] == value)
                 {
                     itemIndex = i;
                     break;
                 }
             }
             return itemIndex;
         }

         public void Insert(int index, object value)
         {
             if ((_count + 1 <= _contents.Length) && (index < Count) && (index >= 0))
             {
                 _count++;

                 for (int i = Count - 1; i > index; i--)
                 {
                     _contents[i] = _contents[i - 1];
                 }
                 _contents[index] = value;
             }
         }

         public bool IsFixedSize
         {
             get
             {
                 return true;
             }
         }

         public bool IsReadOnly
         {
             get
             {
                 return false;
             }
         }

         public void Remove(object value)
         {
             RemoveAt(IndexOf(value));
         }

         public void RemoveAt(int index)
         {
             if ((index >= 0) && (index < Count))
             {
                 for (int i = index; i < Count - 1; i++)
                 {
                     _contents[i] = _contents[i + 1];
                 }
                 _count--;
             }
         }

         public object this[int index]
         {
             get
             {
                 return _contents[index];
             }
             set
             {
                 _contents[index] = value;
             }
         }

         // ICollection Members

         public void CopyTo(Array array, int index)
         {
             int j = index;
             for (int i = 0; i < Count; i++)
             {
                 array.SetValue(_contents[i], j);
                 j++;
             }
         }

         public int Count
         {
             get
             {
                 return _count;
             }
         }

         public bool IsSynchronized
         {
             get
             {
                 return false;
             }
         }

         // Return the current instance since the underlying store is not
         // publicly available.
         public object SyncRoot
         {
             get
             {
                 return this;
             }
         }

         // IEnumerable Members

         public IEnumerator<ISysData> GetEnumerator()
         {
             // Refer to the IEnumerator documentation for an example of
             // implementing an enumerator.
             throw new Exception("The method or operation is not implemented.");
         }*/
    }
}
