using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VertexPipeline;

namespace XNASysLib.XNAKernel
{
    /// <summary>
    /// ToolInfoData Structure
    /// </summary>
    public struct ToolInfoData : IDisposable, ISysData
    {
        public int SysDataType
        {
            get { return -50; }
        }
        int _sysDataType;
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
        
        public IUpdatableComponent OBJ
        {
            get { return _obj; }
        }
        IUpdatableComponent _obj;

        public void Dispose()
        {
            this._gameTime = null;
        }
        public ToolInfoData(IUpdatableComponent Obj, int sysDataType)
        {
            this._gameTime = null;
            _obj = Obj;
            _sysDataType = sysDataType;
            //TempCode
            this.ISysInvoker();
        }

        public void ISysInvoker()
        {
            SysCollector.Singleton.Listener.Invoke(this);
        }
        

        //TempCode
        public int CompareTo(object obj)
        {

            ObjData data = (ObjData)obj;

            if (data.SysDataTime
               == this.SysDataTime)
                return 0;

            return data.SysDataTime
                > this.SysDataTime ?
                1 : -1;
        }

    }
}
