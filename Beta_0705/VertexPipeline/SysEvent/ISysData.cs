using System;

namespace VertexPipeline
{
    public interface ISysData : IComparable
    {
        int SysDataType
        {
            get;
        }
        double SysDataTime
        {
            get;
        }
        object Sender
        {
            get;
        }
        void ISysInvoker();
    }
    public interface ISysEnv : ISysData
    {
        SYSEVN Event
        {
            get;
        }
        object[] Params
        {
            get;
        }

        OBJTYPE ObjType
        {
            get;
        }
    }
}
