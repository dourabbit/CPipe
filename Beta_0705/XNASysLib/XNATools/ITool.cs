using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using XNASysLib.XNAKernel;
using System.Collections.Generic;
using XNASysLib.Primitives3D;
using VertexPipeline;

namespace XNASysLib.XNATools
{
    public interface ITool
    {
        string ToolNm
        { get; }
        ISelectable ToolTarget
        { get; }
    }

    public delegate void
            SpotOnSelect(DrawableComponent target);
    public delegate void
            ToolHandler();

    public interface IManipulator : ITool
    {
        List<IHotSpot> HotSpots
        { get; }
        SpotOnSelect SpotSelectionHandler
        { get; }



    }

    public interface IHotSpot : IDrawableComponent
    {
        /*
        GeometricPrimitive model
        { get; }
        */
        OnSelected SelectionHandler
        { get; set; }
        //   string ID
        // { get; }
    }
    public class Ts
    {
        void a(IGame game)
        {
            Plane a = new Plane();
            Ray b = new Ray();
            b.Intersects(a);
        }
    }

}