using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VertexPipeline;

namespace XNASysLib.XNAKernel
{
    public struct CameraData : IDisposable, ISysData
    {
        public int SysDataType
        {
            get { return -10; }
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

        ICamera _cam;
        public ICamera Camera
        {
            get { return _cam; }
        }
        object _sender;
        public object Sender
        {
            get { return _sender; }
        }
        public void Dispose()
        {
            this._gameTime = null;
        }
        public CameraData(object sender,ICamera cam)
        {
            this._cam = cam;
            this._gameTime = null;
            this._sender = sender;
            
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

            CameraData data = (CameraData)obj;

            if (data.SysDataTime
               == this.SysDataTime)
                return 0;

            return data.SysDataTime
                > this.SysDataTime ?
                1 : -1;
        }

    }
}
