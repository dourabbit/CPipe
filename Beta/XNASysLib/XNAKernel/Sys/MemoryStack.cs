#region File Description
//-----------------------------------------------------------------------------
// Program.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using VertexPipeline;


#endregion

namespace XNASysLib.XNAKernel
{


    public class MemoryException : Exception
    {
        public MemoryException(string print):base(print)
        {
        }
    }

    public delegate void MemoryStackChanging(int curIndex, object stack);
    public class MemoryStack<T>: List<T> 

    {
        public event MemoryStackChanging MemChanging;
        int _curIndex=-1;
       // int _maxIndex;
        public int CurIndex
        {
            get
            {
                return _curIndex;
            }
            set
            {
                if (MemChanging != null)
                    MemChanging.Invoke(_curIndex,this);
                _curIndex = value;
            }
        }
        public T CurObj
        {
            get { return this[_curIndex]; }
        }
        public T GetNextSibling
        {

            get
            {
                if (_curIndex < this.Count - 1)
                    return this[_curIndex + 1];
                else
                    return default(T);
            }
        }
        //public void PushLast() 
        //{
        //    if (!TimeMechine.History.Exists(
        //                delegate(HistoryEntry matcher)
        //                {
        //                    return matcher.ToolNm == "Undo" ? true : false;
        //                }))
        //    {
        //        //TimeMechine.History.CurIndex++;



        //        //shape node is null when selecting group
        //        if (TimeMechine.LastEntry.Target.ShapeNode != null)
        //            TimeMechine.History.Push(
        //                  new HistoryEntry
        //                      (TimeMechine.Time, "Undo", TimeMechine.LastEntry.Target,
        //                       new SnapShots("Undo", TimeMechine.LastEntry.Target.TransformNode.GetCopy(),
        //                                    TimeMechine.LastEntry.Target.ShapeNode.GetCopy(),null)));

        //        else
        //            TimeMechine.History.Push(
        //              new HistoryEntry
        //                  (TimeMechine.Time, "Undo", TimeMechine.LastEntry.Target,
        //                   new SnapShots("Undo", TimeMechine.LastEntry.Target.TransformNode.GetCopy(),
        //                                null, null)));
        //    }
        
        //}
        public T Undo()
        {
            if (_curIndex <= 0)
            {

                if (MemChanging != null)
                    MemChanging.Invoke(_curIndex, this);

                // throw new MemoryException("It is the end of stack");
                return default(T);
            }
            else
            {
                int index = --_curIndex;
                if (MemChanging != null)
                    MemChanging.Invoke(index, this);
                return this[index];
            }
        }

        public T Redo()
        {
            if (_curIndex >= this.Count - 1)
            {
                if (MemChanging != null)
                    MemChanging.Invoke(_curIndex, this);
                return default(T);
            }
            else
            {
                int index = ++_curIndex;
                if (MemChanging != null)
                    MemChanging.Invoke(index, this);
                return this[index];
            }
        }

        public void Push(T historyEntry)
        {
            //TimeMechine.History.CurIndex++;
            _curIndex++;
            //Override the data, then clean the list
            if (_curIndex < this.Count-1)
            {

                //this[_curIndex] = historyEntry;
                //if (this.Count -1 > _curIndex)
                //    this.RemoveRange(_curIndex+1,this.Count-_curIndex-1);
                //return;
                this.RemoveRange(_curIndex, this.Count-_curIndex);
            }


            //Add the Data
           // _maxIndex++;

            this.Add(historyEntry);

            if (MemChanging != null)
                MemChanging.Invoke(_curIndex, this);
        }
        
    }













    //// Summary:
    ////     Represents a variable size last-in-first-out (LIFO) collection of instances
    ////     of the same arbitrary type.
    ////
    //// Type parameters:
    ////   T:
    ////     Specifies the type of elements in the stack.
    //[Serializable]
    //[DebuggerDisplay("Count = {Count}")]
    ////[DebuggerTypeProxy(typeof(System_StackDebugView<>))]
    //[ComVisible(false)]
    //public class MemoryStack<T> : IEnumerable<T>, ICollection, IEnumerable
    //{
    //    int _capacity;
    //    int _count;
    //    object[] _ObjArray;

    //    public MemoryStack(int capacity)
    //    {
    //        this._capacity=capacity;
    //        this._ObjArray=new object[capacity];
    //    }
        
    //    public int Count { get; }

    //    public void Clear();
        
    //    public Stack<T>.Enumerator GetEnumerator();
    //    //
    //    // Summary:
    //    //     Returns the object at the top of the System.Collections.Generic.Stack<T>
    //    //     without removing it.
    //    //
    //    // Returns:
    //    //     The object at the top of the System.Collections.Generic.Stack<T>.
    //    //
    //    // Exceptions:
    //    //   System.InvalidOperationException:
    //    //     The System.Collections.Generic.Stack<T> is empty.
    //    public T Peek()
    //    {}
    //    //
    //    // Summary:
    //    //     Removes and returns the object at the top of the System.Collections.Generic.Stack<T>.
    //    //
    //    // Returns:
    //    //     The object removed from the top of the System.Collections.Generic.Stack<T>.
    //    //
    //    // Exceptions:
    //    //   System.InvalidOperationException:
    //    //     The System.Collections.Generic.Stack<T> is empty.
    //    public T Pop()
    //    {}
    //    //
    //    // Summary:
    //    //     Inserts an object at the top of the System.Collections.Generic.Stack<T>.
    //    //
    //    // Parameters:
    //    //   item:
    //    //     The object to push onto the System.Collections.Generic.Stack<T>. The value
    //    //     can be null for reference types.
    //    public void Push(T item);
    //    //
    //    // Summary:
    //    //     Copies the System.Collections.Generic.Stack<T> to a new array.
    //    //
    //    // Returns:
    //    //     A new array containing copies of the elements of the System.Collections.Generic.Stack<T>.
        

    //    // Summary:
    //    //     Enumerates the elements of a System.Collections.Generic.Stack<T>.
    //    [Serializable]
    //    public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
    //    {

    //        // Summary:
    //        //     Gets the element at the current position of the enumerator.
    //        //
    //        // Returns:
    //        //     The element in the System.Collections.Generic.Stack<T> at the current position
    //        //     of the enumerator.
    //        //
    //        // Exceptions:
    //        //   System.InvalidOperationException:
    //        //     The enumerator is positioned before the first element of the collection or
    //        //     after the last element.
    //        public T Current { get; }

    //        // Summary:
    //        //     Releases all resources used by the System.Collections.Generic.Stack<T>.Enumerator.
    //        public void Dispose();
    //        //
    //        // Summary:
    //        //     Advances the enumerator to the next element of the System.Collections.Generic.Stack<T>.
    //        //
    //        // Returns:
    //        //     true if the enumerator was successfully advanced to the next element; false
    //        //     if the enumerator has passed the end of the collection.
    //        //
    //        // Exceptions:
    //        //   System.InvalidOperationException:
    //        //     The collection was modified after the enumerator was created.
    //        public bool MoveNext();
    //    }
    //}
}
