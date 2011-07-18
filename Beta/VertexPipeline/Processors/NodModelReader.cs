#region File Description
//-----------------------------------------------------------------------------
// CpuSkinnedModelReader.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using VertexPipeline.Animation;

namespace VertexPipeline.Data
{

    /// <summary>
    /// A list, store the ShapeNode and sharing the same TransDatas
    /// </summary>
    public class NodesGrp : List<ShapeNode>
    {
        public TransDatas TransData { get; internal set; }

        public NodesGrp(TransDatas data)
        {
            this.TransData = data;
        }
    }
    /// <summary>
    /// A custom reader to read our CpuSkinnedModel from an XNB.
    /// </summary>
    public class NodModelReader : ContentTypeReader<NodesGrp>
    {
        protected override NodesGrp Read(ContentReader input, NodesGrp existingInstance)
        {
            // read in the model parts
            List<ShapeReadingData> Datas = input.ReadObject<List<ShapeReadingData>>();
            List<ShapeNode> shapesGrp = new List<ShapeNode>();
            // read in the skinning data
            TransDatas transDatas = input.ReadObject<TransDatas>();

            foreach (ShapeReadingData data in Datas)
                shapesGrp.Add(new ShapeNode(data));
            
            NodesGrp result = new NodesGrp(transDatas);
            foreach (ShapeNode node in shapesGrp)
                result.Add(node);
            return result;
        }
    }

    /// <summary>
    /// A custom reader to read ShapeNode information
    /// </summary>
    public class ShapeNodeReader : ContentTypeReader<ShapeReadingData>
    {
        protected override ShapeReadingData Read(ContentReader input, 
                                        ShapeReadingData existingInstance)
        {
            // read in all of our data
            string name = input.ReadString();
            int parentIndex = input.ReadInt32();
            int triangleCount = input.ReadInt32();
            VertexData[] cpuVertices = input.ReadObject<VertexData[]>();
            IndexBuffer indexBuffer = input.ReadObject<IndexBuffer>();
            BoundingSphere[] boundingSphere=input.ReadObject<BoundingSphere[]>();

 
            ShapeReadingData data = new ShapeReadingData
            {
                Name=name,
                ParentIndex=parentIndex,
                TriangleCount = triangleCount,
                BoundingSpheres = boundingSphere,
                Vertices = cpuVertices,
                IndexBuffer=indexBuffer
            };

            // read in the BasicEffect as a shared resource
            //input.ReadSharedResource<BasicEffect>(fx => modelPart.Effect = fx);

            return data;
        }
    }
}
