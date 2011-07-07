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
#endregion

namespace VertexPipeline
{
    public interface ICamera
    {
        Matrix WorldMatrix { get; set; }
        Matrix ViewMatrix { get; set; }
        Matrix ProjectionMatrix { get; set; }
        Viewport Viewport { get; set; }
        Quaternion RotationQuat { get; set; }

        void Translate(Vector3 dis, out Vector3 position);
        void Translate(Vector3 dis);
        void Rotate(Vector3 axis, float angle);
        
    }
}
