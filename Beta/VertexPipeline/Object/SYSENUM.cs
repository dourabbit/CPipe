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
        New=0,
        Save,
        Import,
        Delete,
        Tool,
        Select
    }
}
