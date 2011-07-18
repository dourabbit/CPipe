#region File Description
//-----------------------------------------------------------------------------
// CpuSkinningContentData.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using VertexPipeline.Animation;
using VertexPipeline.Data;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VertexPipeline.Animation
{
    /// <summary>
    /// The Date for writing into xnb file, including the transform information and shape information
    /// </summary>
    public class ShapeN_SkinDContent_Writing
    {
        public List<ShapeReadingData> ShapeNodes = new List<ShapeReadingData>();
        public TransDatas TransDatas= new TransDatas();

     

        public void SetShapeNode(
            string name,
            int parentIndex,
            int triangleCount,
            IndexCollection indexCollection,
            VertexData[] vertices,
            BoundingSphere[] bSpheres )
        {

            ShapeNodes.Add(new ShapeReadingData
            {
                Name=name,
                ParentIndex=parentIndex,
                TriangleCount = triangleCount,
                IndexCollection = indexCollection,
                Vertices = vertices,
                BoundingSpheres = bSpheres,
            });
        }
    }

}
