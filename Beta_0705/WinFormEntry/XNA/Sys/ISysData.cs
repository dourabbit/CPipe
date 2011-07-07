using System;

namespace SysLib
{
    public interface ISysData : IComparable
    {
        Int16 ISysDataType
        {
            get;
        }
        double ISysDataTime
        {
            get;
        }
        void ISysInvoker();
    }
}
