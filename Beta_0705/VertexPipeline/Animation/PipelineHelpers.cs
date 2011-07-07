#region File Description
//-----------------------------------------------------------------------------
// SkinningHelper.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using VertexPipeline.Animation;
using VertexPipeline.Data;
using Microsoft.Xna.Framework.Graphics;

namespace VertexPipeline.Animation
{
    public static class PipelineHelpers
    {
        public static void ProcessNode(NodeContent node, 
            ShapeN_SkinDContent_Writing outputModel,
            ref int curIndex,
            ref int parentIndex)
        {

            curIndex++;
            outputModel.TransDatas.NameGrp.Add(node.Name);
            outputModel.TransDatas.ParentIndex.Add(parentIndex);
            outputModel.TransDatas.RelativeMatrixGrp.Add(node.Transform);

            parentIndex = curIndex;
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
                    ProcessGeometry(geometry, curIndex, "Shape:" + node.Name, outputModel);
                }
            }
            // Recurse over any child nodes.
            if (node.Children != null && node.Children.Count != 0)
                foreach (NodeContent child in node.Children)
                {
                    ProcessNode(child, outputModel, ref curIndex, ref parentIndex);
                }
            else
            {
                parentIndex = outputModel.TransDatas.ParentIndex[curIndex];
            }

            
        }

        static void ProcessGeometry(GeometryContent geometry, int parentIndex,
                            string shapeNm,
                            ShapeN_SkinDContent_Writing output)
        {
            // find and process the geometry's bone weights
            for (int i = 0; i < geometry.Vertices.Channels.Count; i++)
            {
                string channelName = geometry.Vertices.Channels[i].Name;
                string baseName = VertexChannelNames.DecodeBaseName(channelName);

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

            BoundingSphere[] bSpheres = new BoundingSphere[1]
                    { 
                        BoundingSphere.CreateFromPoints(vertice)
                    };
            // Add the new piece of geometry to our output model.
            
            output.SetShapeNode(
                shapeNm,
                parentIndex,
                triangleCount, geometry.Indices,
                verticesData, bSpheres);
        }


        static void GetTransMatrix(NodeContent node,
                                ContentProcessorContext context,
                                string parentBoneName)
        {
            MeshContent mesh = node as MeshContent;

            if (mesh != null)
            {
                // Validate the mesh.
                if (parentBoneName != null)
                {
                    context.Logger.LogWarning(null, null,
                        "Mesh {0} is a child of bone {1}. SkinnedModelProcessor " +
                        "does not correctly handle meshes that are children of bones.",
                        mesh.Name, parentBoneName);
                }


            }
            else if (node is BoneContent)
            {
                // If this is a bone, remember that we are now looking inside it.
                parentBoneName = node.Name;
            }

            // Recurse (iterating over a copy of the child collection,
            // because validating children may delete some of them).
            foreach (NodeContent child in new List<NodeContent>(node.Children))
                GetTransMatrix(child, context, parentBoneName);
        }

        public static TransDatas GetTransData(NodeContent input, 
            ContentProcessorContext context, int maxBones)
        {
            //ValidateMesh(input, context, null);

            // Find the skeleton.
            //BoneContent skeleton = MeshHelper.FindSkeleton(input);

            //if (skeleton == null)
                //throw new InvalidContentException("Input skeleton not found.");
                //return null;

            // We don't want to have to worry about different parts of the model being
            // in different local coordinate systems, so let's just bake everything.
            //FlattenTransforms(input, skeleton);

            // Read the bind pose and skeleton hierarchy data.
            //IList<BoneContent> bones = MeshHelper.FlattenSkeleton(skeleton);
            GetTransMatrix(input, context, null);
            /*if (bones.Count > maxBones)
            {
                throw new InvalidContentException
                    (string.Format
                        ("Skeleton has {0} bones, but the maximum supported is {1}.", 
                            bones.Count, maxBones));
            }

            List<Matrix> bindPose = new List<Matrix>();
            List<Matrix> inverseBindPose = new List<Matrix>();
            List<int> skeletonHierarchy = new List<int>();

            foreach (BoneContent bone in bones)
            {
                bindPose.Add(bone.Transform);
                inverseBindPose.Add(Matrix.Invert(bone.AbsoluteTransform));
                skeletonHierarchy.Add(bones.IndexOf(bone.Parent as BoneContent));
            }
            */
            // Convert animation data to our runtime format.
            //Dictionary<string, AnimationClip> animationClips;
            //animationClips = ProcessAnimations(skeleton.Animations, bones);

            return null;//new TransData(animationClips, bindPose, inverseBindPose, skeletonHierarchy);
        }

        /// <summary>
        /// Converts an intermediate format content pipeline AnimationContentDictionary
        /// object to our runtime AnimationClip format.
        /// </summary>
        static Dictionary<string, AnimationClip> ProcessAnimations(AnimationContentDictionary animations, IList<BoneContent> bones)
        {
            // Build up a table mapping bone names to indices.
            Dictionary<string, int> boneMap = new Dictionary<string, int>();

            for (int i = 0; i < bones.Count; i++)
            {
                string boneName = bones[i].Name;

                if (!string.IsNullOrEmpty(boneName))
                    boneMap.Add(boneName, i);
            }

            // Convert each animation in turn.
            Dictionary<string, AnimationClip> animationClips;
            animationClips = new Dictionary<string, AnimationClip>();

            foreach (KeyValuePair<string, AnimationContent> animation in animations)
            {
                AnimationClip processed = ProcessAnimation(animation.Value, boneMap);

                animationClips.Add(animation.Key, processed);
            }

            if (animationClips.Count == 0)
            {
                throw new InvalidContentException("Input file does not contain any animations.");
            }

            return animationClips;
        }

        /// <summary>
        /// Converts an intermediate format content pipeline AnimationContent
        /// object to our runtime AnimationClip format.
        /// </summary>
        static AnimationClip ProcessAnimation(AnimationContent animation, Dictionary<string, int> boneMap)
        {
            List<Keyframe> keyframes = new List<Keyframe>();

            // For each input animation channel.
            foreach (KeyValuePair<string, AnimationChannel> channel in
                animation.Channels)
            {
                // Look up what bone this channel is controlling.
                int boneIndex;

                if (!boneMap.TryGetValue(channel.Key, out boneIndex))
                {
                    throw new InvalidContentException(string.Format(
                        "Found animation for bone '{0}', " +
                        "which is not part of the skeleton.", channel.Key));
                }

                // Convert the keyframe data.
                foreach (AnimationKeyframe keyframe in channel.Value)
                {
                    keyframes.Add(new Keyframe(boneIndex, keyframe.Time,
                                               keyframe.Transform));
                }
            }

            // Sort the merged keyframes by time.
            keyframes.Sort(CompareKeyframeTimes);

            if (keyframes.Count == 0)
                throw new InvalidContentException("Animation has no keyframes.");

            if (animation.Duration <= TimeSpan.Zero)
                throw new InvalidContentException("Animation has a zero duration.");

            return new AnimationClip(animation.Duration, keyframes);
        }

        /// <summary>
        /// Comparison function for sorting keyframes into ascending time order.
        /// </summary>
        static int CompareKeyframeTimes(Keyframe a, Keyframe b)
        {
            return a.Time.CompareTo(b.Time);
        }

        /// <summary>
        /// Makes sure this mesh contains the kind of data we know how to animate.
        /// </summary>
        static void ValidateMesh(NodeContent node, 
                                ContentProcessorContext context, 
                                string parentBoneName)
        {
            MeshContent mesh = node as MeshContent;

            if (mesh != null)
            {
                // Validate the mesh.
                if (parentBoneName != null)
                {
                    context.Logger.LogWarning(null, null,
                        "Mesh {0} is a child of bone {1}. SkinnedModelProcessor " +
                        "does not correctly handle meshes that are children of bones.",
                        mesh.Name, parentBoneName);
                }

                if (!MeshHasSkinning(mesh))
                {
                    context.Logger.LogWarning(null, null,
                        "Mesh {0} has no skinning information, so it has been deleted.",
                        mesh.Name);
                    if(mesh.Parent!=null)
                        mesh.Parent.Children.Remove(mesh);
                    return;
                }
            }
            else if (node is BoneContent)
            {
                // If this is a bone, remember that we are now looking inside it.
                parentBoneName = node.Name;
            }

            // Recurse (iterating over a copy of the child collection,
            // because validating children may delete some of them).
            foreach (NodeContent child in new List<NodeContent>(node.Children))
                ValidateMesh(child, context, parentBoneName);
        }

        /// <summary>
        /// Checks whether a mesh contains skininng information.
        /// </summary>
        static bool MeshHasSkinning(MeshContent mesh)
        {
            foreach (GeometryContent geometry in mesh.Geometry)
            {
                if (!geometry.Vertices.Channels.Contains(VertexChannelNames.Weights()))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Bakes unwanted transforms into the model geometry,
        /// so everything ends up in the same coordinate system.
        /// </summary>
        static void FlattenTransforms(NodeContent node, BoneContent skeleton)
        {
            foreach (NodeContent child in node.Children)
            {
                // Don't process the skeleton, because that is special.
                if (child == skeleton)
                    continue;

                // Bake the local transform into the actual geometry.
                MeshHelper.TransformScene(child, child.Transform);

                // Having baked it, we can now set the local
                // coordinate system back to identity.
                child.Transform = Matrix.Identity;

                // Recurse.
                FlattenTransforms(child, skeleton);
            }
        }
    }
}
