using System;

namespace VertexPipeline
{

    public enum OBJTYPE
    { 
        Building=0,
        Camera,
        Pipe,
        Valve,
        Chamber,
        Well,
        HoleEllipse,
        HoleRect
    }
    public enum SYSEVN
    { 
        Initial=0,
        Save,
        Import,
        Delete,
        Tool,
        Panel,
        Select
    }
}
