#region File Description
//-----------------------------------------------------------------------------
// CpuSkinnedModelWriter.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using VertexPipeline.Animation;

namespace VertexPipeline
{
    /// <summary>
    /// Writes out a CpuSkinnedModelContent object to an XNB file to be read in as
    /// a CpuSkinnedModel.
    /// </summary>
    [ContentTypeWriter]
    class ShapeN_SkinDModelWriter : ContentTypeWriter<ShapeN_SkinDContent_Writing>
    {

        protected override void Write(ContentWriter output, ShapeN_SkinDContent_Writing value)
        {
            
            output.WriteObject(value.ShapeNodes);
            output.WriteObject(value.TransDatas);
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return "VertexPipeline.Data.NodesGrp, VertexPipeline";
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "VertexPipeline.Data.NodModelReader, VertexPipeline";
        }
    }

    /// <summary>
    /// Writes out the type of ShapeReadingData 
    /// </summary>
    [ContentTypeWriter]
    class ShapeNodeWriter : ContentTypeWriter<ShapeReadingData>
    {
        protected override void Write(ContentWriter output, ShapeReadingData value)
        {
            output.Write(value.Name);
            output.Write(value.ParentIndex);
            //output.Write(value.TriangleCount);
            output.WriteObject(value.Vertices);
            output.WriteObject(value.IndexCollection);
            output.WriteObject(value.BoundingSpheres);
            //output.WriteSharedResource(value.Material);
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return "VertexPipeline.ShapeReadingData, VertexPipeline";
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "VertexPipeline.Data.ShapeNodeReader, VertexPipeline";
        }
    }
}
