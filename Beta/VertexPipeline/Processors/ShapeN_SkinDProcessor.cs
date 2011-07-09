#region File Description
//-----------------------------------------------------------------------------
// CpuSkinnedModelProcessor.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using VertexPipeline.Animation;
using Microsoft.Xna.Framework.Graphics;
using VertexPipeline.Data;
namespace VertexPipeline
{
    [ContentProcessor(DisplayName = "ShapeN_SkinDProcessor")]
    class ShapeN_SkinDProcessor : ContentProcessor<NodeContent, ShapeN_SkinDContent_Writing>
    {
        private ContentProcessorContext context;
        private ShapeN_SkinDContent_Writing outputModel;


        public override ShapeN_SkinDContent_Writing Process
            (NodeContent input, ContentProcessorContext context)
        {


            
            this.context = context;
            outputModel = new ShapeN_SkinDContent_Writing();


            // cpu skinning can support any number of bones, 
            //so we'll just use int.MaxValue as our limit.
            //outputModel.SkinningData = SkinningHelpers.
                //GetSkinningData(input, context, int.MaxValue);

            //PipelineHelpers.GetTransData(input,context,10);
            int curIndex=-1;
            int parentIndex = -1;
            PipelineHelpers.ProcessNode(input, outputModel,ref curIndex,ref parentIndex);

            return outputModel;
        }
        /*
        void ProcessNode(NodeContent node, ShapeN_SkinDContent outputModel)
        {
            // Is this node in fact a mesh?
            MeshContent mesh = node as MeshContent;

            if (mesh != null)
            {
                // Reorder vertex and index data so triangles will render in
                // an order that makes efficient use of the GPU vertex cache.
                MeshHelper.OptimizeForCache(mesh);

                // Process all the geometry in the mesh.
                foreach (GeometryContent geometry in mesh.Geometry)
                {
                    ProcessGeometry(geometry, outputModel);
                }
            }

            // Recurse over any child nodes.
            foreach (NodeContent child in node.Children)
            {
                ProcessNode(child, outputModel);
            }
        }

        void ProcessGeometry(GeometryContent geometry, 
                            ShapeN_SkinDContent output)
        {
            // find and process the geometry's bone weights
            for (int i = 0; i < geometry.Vertices.Channels.Count; i++)
            {
                string channelName = geometry.Vertices.Channels[i].Name;
                string baseName = VertexChannelNames.DecodeBaseName(channelName);

                if (baseName == "Weights")
                {
                    ProcessWeightsChannel(geometry, i);
                }
            }

            // retrieve the four vertex channels we require for CPU skinning. we ignore any
            // other channels the model might have.
            string normalNm = VertexChannelNames.EncodeName(VertexElementUsage.Normal, 0);
            string texCoordNm = VertexChannelNames.EncodeName(VertexElementUsage.TextureCoordinate, 0);
            string blendWeightNm = VertexChannelNames.EncodeName(VertexElementUsage.BlendWeight, 0);
            string blendIndexNm = VertexChannelNames.EncodeName(VertexElementUsage.BlendIndices, 0);

            string positionNm = VertexChannelNames.EncodeName(VertexElementUsage.Position, 0);

            //var tmp = geometry.Vertices.Channels[positionNm] as VertexChannel<Vector3>;
            VertexChannel<Vector3> normals;
            VertexChannel<Vector2> texCoords;
            VertexChannel<Vector4> blendWeights;
            VertexChannel<Vector4> blendIndices;
            
            //if(geometry.Vertices.Channels.Contains(normalNm))
            normals = 
                geometry.Vertices.Channels[normalNm] as VertexChannel<Vector3>;

            if (geometry.Vertices.Channels.Contains(texCoordNm))
            texCoords = 
                geometry.Vertices.Channels[texCoordNm] as VertexChannel<Vector2>;

            if (geometry.Vertices.Channels.Contains(blendWeightNm))
            blendWeights =
                geometry.Vertices.Channels[blendWeightNm] as VertexChannel<Vector4>;

            if (geometry.Vertices.Channels.Contains(blendIndexNm))
            blendIndices = 
                geometry.Vertices.Channels[blendIndexNm] as VertexChannel<Vector4>;

            // create our array of vertices
            int triangleCount = geometry.Indices.Count / 3;
            VertexData[] verticesData = new VertexData[geometry.Vertices.VertexCount];
            Vector3[] vertice = new Vector3[verticesData.Length];
            for (int i = 0; i < verticesData.Length; i++)
            {
                verticesData[i] = new VertexData
                {
                    Position = geometry.Vertices.Positions[i],
                    Normal = normals[i],
                    //TextureCoordinate = texCoords[i],
                    //BlendWeights = blendWeights[i],
                    //BlendIndices = blendIndices[i]
                };
                vertice[i] = verticesData[i].Position;
            }

            BoundingSphere[] bSpheres=new BoundingSphere[1]
                    { 
                        BoundingSphere.CreateFromPoints(vertice)
                    };
            // Add the new piece of geometry to our output model.
            output.SetShapeNode(triangleCount, geometry.Indices, verticesData, bSpheres);
        }

        static void ProcessWeightsChannel(GeometryContent geometry, int vertexChannelIndex)
        {
            // create a map of Name->Index of the bones
            BoneContent skeleton = MeshHelper.FindSkeleton(geometry.Parent);
            Dictionary<string, int> boneIndices = new Dictionary<string, int>();
            IList<BoneContent> flattenedBones = MeshHelper.FlattenSkeleton(skeleton);
            for (int i = 0; i < flattenedBones.Count; i++)
            {
                boneIndices.Add(flattenedBones[i].Name, i);
            }

            // convert all of our bone weights into the correct indices and weight values
            VertexChannel<BoneWeightCollection> inputWeights = geometry.Vertices.Channels[vertexChannelIndex] as VertexChannel<BoneWeightCollection>;
            Vector4[] outputIndices = new Vector4[inputWeights.Count];
            Vector4[] outputWeights = new Vector4[inputWeights.Count];
            for (int i = 0; i < inputWeights.Count; i++)
            {
                ConvertWeights(inputWeights[i], boneIndices, outputIndices, outputWeights, i, geometry);
            }

            // create our new channel names
            int usageIndex = VertexChannelNames.DecodeUsageIndex(inputWeights.Name);
            string indicesName = VertexChannelNames.EncodeName(VertexElementUsage.BlendIndices, usageIndex);
            string weightsName = VertexChannelNames.EncodeName(VertexElementUsage.BlendWeight, usageIndex);

            // add in the index and weight channels
            geometry.Vertices.Channels.Insert(vertexChannelIndex + 1, indicesName, outputIndices);
            geometry.Vertices.Channels.Insert(vertexChannelIndex + 2, weightsName, outputWeights);

            // remove the original weights channel
            geometry.Vertices.Channels.RemoveAt(vertexChannelIndex);
        }

        static void ConvertWeights(BoneWeightCollection inputWeights, Dictionary<string, int> boneIndices, 
            Vector4[] outIndices, Vector4[] outWeights, int vertexIndex, GeometryContent geometry)
        {
            // we only handle 4 weights per bone
            const int maxWeights = 4;

            // create some temp arrays to hold our values
            int[] tempIndices = new int[maxWeights];
            float[] tempWeights = new float[maxWeights];

            // cull out any extra bones
            inputWeights.NormalizeWeights(maxWeights);

            // get our indices and weights
            for (int i = 0; i < inputWeights.Count; i++)
            {
                BoneWeight weight = inputWeights[i];

                tempIndices[i] = boneIndices[weight.BoneName];
                tempWeights[i] = weight.Weight;
            }

            // zero out any remaining spaces
            for (int i = inputWeights.Count; i < maxWeights; i++)
            {
                tempIndices[i] = 0;
                tempWeights[i] = 0;
            }

            // output the values
            outIndices[vertexIndex] = new Vector4(tempIndices[0], tempIndices[1], tempIndices[2], tempIndices[3]);
            outWeights[vertexIndex] = new Vector4(tempWeights[0], tempWeights[1], tempWeights[2], tempWeights[3]);
        }
*/

    }
}