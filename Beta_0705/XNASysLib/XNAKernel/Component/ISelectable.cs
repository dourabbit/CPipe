#region File Description
//-----------------------------------------------------------------------------
// BloomComponent.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using VertexPipeline;
#endregion

namespace XNASysLib.XNAKernel
{
    public delegate void OnSelected(ISelectable sel, bool isSelected);
    //public delegate void OnDrag(Point scrPStart, Point scrPEnd);
    public delegate void OnRoll();
    public delegate void OnDrag();

    public struct SelectableCompData
    {
        //public string ID;
        public BoundingSphere[] BoundingSpheres;
        public ShapeNode shape;
        public TransformNode transform;
        public bool IsVisible;
        public bool IsInCam;
        public OnDataModified dataModifitionHandler;
        public OnSelected SelectionHandler;
    }
    public interface ISelectable : IComparable, ITransObj
    {
        string ID
        { get; }
        SelectableCompData Data
        { get; }
        OnRoll RollOverHandler
        { get; }
        OnRoll RollOutHandler
        { get; }
        float? DistOfObj
        { get; }
        /// <summary>
        /// Keep Obj selected
        /// </summary>
        bool KeepSel
        { set; get; }
        /// <summary>
        /// Lock the Obj, prevent it being selected
        /// </summary>
        bool Lock
        { set; get; }

        int CompareTo(object input);
    }
    public interface IDraggable : ISelectable
    {
        OnDrag DragHandler
        { get; }
    }
}
