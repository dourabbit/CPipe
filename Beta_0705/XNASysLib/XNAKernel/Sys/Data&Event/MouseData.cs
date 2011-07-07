using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VertexPipeline;

namespace XNASysLib.XNAKernel
{
    public struct MouseData : IDisposable, ISysData
    {
        public int SysDataType
        {
            get { return -1; }
        }
        public object Sender
        {
            get { return null; }
        }
        public double SysDataTime
        {
            get
            {
                if (_gameTime == null)
                    return 0;
                return (double)_gameTime;
            }
            set { _gameTime = value; }
        }

        double? _gameTime;
        /// <summary>
        /// Mouse World Position
        /// Which centre of screen is origin point
        /// </summary>
        public Point MousePos;
        public Rectangle SelectionRect;
        public Vector3 SelectionCentre;
        public Vector3[] SelectionWorldPos; 
        public bool IsLeftBtnDown;
        public bool IsLeftBtnUp;
        public bool IsMiddleDown;
        public bool IsMiddleUp;
        public bool IsRightBtnDown;
        public bool IsRightBtnUp;
        public bool IsMoving;
        public bool IsDragging
        {
            get {
                    return IsLeftBtnDown & !IsRightBtnUp & IsMoving;
            }
        }
        public bool IsClicking
        {
            get { return (IsLeftBtnDown ^ IsRightBtnDown^IsMiddleDown); }
        }
        public bool IsReleasing
        {
            get { return (IsLeftBtnUp ^ IsRightBtnUp^IsMiddleUp) & !IsMoving; }
        }

        public void Dispose()
        {
            this._gameTime = null;
            this.MousePos = Point.Zero;
            this.SelectionRect = Rectangle.Empty;
        }

        public void ISysInvoker()
        {
            SysCollector.Singleton.Listener.Invoke(this);
        }

        public int CompareTo(object obj)
        {
            MouseData data = (MouseData)obj;

            if (data.SysDataTime
               == this.SysDataTime)
                return 0;

            return data.SysDataTime
                > this.SysDataTime ?
                1 : -1;
        }

    }
}
