#region File Description
//-----------------------------------------------------------------------------
// CpuVertex.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using Microsoft.Xna.Framework;

namespace VertexPipeline.Data
{
    /// <summary>
    /// A struct that contains the vertex information we need on the CPU.
    /// This type is not used for a GPU vertex.
    /// </summary>
    public class AniVertexData:VertexData
    {
        public Vector2 TextureCoordinate;
        public Vector4 BlendWeights;
        public Vector4 BlendIndices;
    }
    public class VertexData
    {
        public Vector3 Position;
        public Vector3 Normal;


    }
}
