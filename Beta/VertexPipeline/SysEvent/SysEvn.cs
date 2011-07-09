using System;
using VertexPipeline;

namespace VertexPipeline
{

    public class SysEvn : ISysEnv
    {
        const int _dataType = 1;
        double _time;
        OBJTYPE _objT;
        SYSEVN _evnT;
        object[] _params;
        object _sender;
        public object[] Params
        {
            get { return _params; }
        }
        public OBJTYPE ObjType
        {
            get
            {
                return _objT;
            }
        }
        public SYSEVN Event
        {
            get { return _evnT; }
        }
        public int SysDataType
        {
            get
            {
                return _dataType;
            }
        }
        public double SysDataTime
        {
            get
            {
                return _time;
            }
        }
        public object Sender
        {
            get { return _sender; }
        }
        public SysEvn(double time,object sender, OBJTYPE type, 
            SYSEVN sysevn, params object[] inputs)
        {
            this._objT = type;
            this._time = time;
            this._evnT = sysevn;
            this._params = inputs;
            this._sender = sender;
            ISysInvoker();
        }
        public void ISysInvoker()
        {
            SysCollector.Singleton.Listener.Invoke(this);
        }
        public int CompareTo(object obj)
        {
            SysEvn data = (SysEvn)obj;

            if (data.SysDataTime
               == this.SysDataTime)
                return 0;

            return data.SysDataTime
                > this.SysDataTime ?
                1 : -1;
        }
    }
}
