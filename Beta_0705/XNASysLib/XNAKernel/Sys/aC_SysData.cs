﻿using System;
using VertexPipeline;

namespace XNASysLib.XNAKernel
{

    public abstract class aC_SysData :
        System.EventArgs, IDisposable, ISysData
    {

        object _sender;
        object _reciever;
        double _gameTime;

        //0 nothing
        //>0 Event
        //<0 Data
        protected int? _event;

        public double SysDataTime
        {
            get { return _gameTime; }
        }
        public object Sender
        {
            get { return _sender; }
        }
        public object Reciever
        {
            get { return _reciever; }
        }
        public int SysDataType
        {
            get
            {
                return _event ?? 0;
            }
        }
        public aC_SysData(object sender, object reciever, double gameTime)
        {
            this._reciever = reciever;
            this._sender = sender;
            this._gameTime = gameTime;
        }

        public void Dispose()
        {
            this._reciever = null;
            this._sender = null;
            this._event = null;
        }

        public void ISysInvoker()
        {
           SysCollector.Singleton.Listener.Invoke(this);
        }

        public int CompareTo(object obj)
        {
            aC_SysData data = (aC_SysData)obj;

            if (data.SysDataTime
               == this.SysDataTime)
                return 0;

            return data.SysDataTime
                > this.SysDataTime ?
                1 : -1;
        }
    }
}
