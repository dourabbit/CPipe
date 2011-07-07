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
using XNABuilder;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using XNASysLib.XNAKernel;
using System.Collections.ObjectModel;
using VertexPipeline;
using VertexPipeline.Data;
#endregion

namespace XNASysLib.Primitives3D
{

    public class NodePipeLoader:IDisposable
    {

        string _AssetNm;
        IGame _game;
        List<TransformNode> nodes = new List<TransformNode>();
        void ReGroupNodes
            (TransformNode curNod, NodesGrp data,
            int index,ref TransformNode root)
        {
            string nm = data.TransData.NameGrp[index];
            int parentIndex = data.TransData.ParentIndex[index];
            Matrix relativeMat= data.TransData.RelativeMatrixGrp[index];

            curNod.Children = new NodeChildren<INode>();
            curNod.NodeNm = nm;
            if (parentIndex == -1)
            {
                root = curNod;
            }
            if (parentIndex >=0 && parentIndex < nodes.Count)
            {
                curNod.Parent = nodes[parentIndex];
                nodes[parentIndex].Children.Add(curNod);
            }
            curNod.World = relativeMat;
        }


        public void ProcessSceneNod(SceneNodHierachyModel curSceneNod,
            TransformNode transNod,NodesGrp shapeGrp, ref SceneNodHierachyModel root,
            ref int curIndex)
        {

            curIndex++;
            curSceneNod.NodeNm = transNod.NodeNm;
            curSceneNod.Children = new NodeChildren<INode>();
            curSceneNod.Root = root;
            curSceneNod.TransformNode = transNod;
            curSceneNod.TransformNode.AbsoluteTransform = Matrix.Identity;
            foreach (ShapeNode shape in shapeGrp)
                if (shape.ParentIndex == curIndex)
                    curSceneNod.ShapeNode = shape;

            // Recurse over any child nodes.
            foreach (TransformNode childTransNod in transNod.Children)
            {
                SceneNodHierachyModel childSceneH = new PipeBase(_game);
                curSceneNod.Children.Add(childSceneH);
                childSceneH.Parent = curSceneNod;
                ProcessSceneNod(childSceneH, childTransNod,shapeGrp,ref root, ref curIndex);
            }

        }

        public NodePipeLoader(IGame game, string assetNm)
        {
            _game = game;

            _AssetNm = assetNm;
            ContentBuilder builder=
                (ContentBuilder)game.Services.
                GetService(typeof(ContentBuilder));

            MyContentManager contentManager=
                (MyContentManager)game.Services.
                GetService(typeof(MyContentManager));

            NodesGrp shapeGrp = (NodesGrp)LoadNode
                    (builder, contentManager, String.Empty,
                    "ShapeN_SkinDProcessor");

            TransformNode transNodRoot = new TransformNode(); 
            for (int i = 0; i < shapeGrp.TransData.NameGrp.Count; i++)
            {
                TransformNode nod = new TransformNode();
                nodes.Add(nod);
                ReGroupNodes(nod,shapeGrp,i,ref transNodRoot);
                nod.Root = transNodRoot;
            }

            /*
            TransformNode rootNod = new TransformNode();
            rootNod.NodeNm = shapeGrp.TransData.NameGrp[0];
            rootNod.Parent = null;
            rootNod.Children = new NodeChildren<INode>();
            */

            SceneNodHierachyModel sceneRoot = new SceneNodHierachyModel(game);
            
            int index=-1;
            ProcessSceneNod(sceneRoot,transNodRoot,shapeGrp,ref sceneRoot,ref index);
            game.Components.Add(sceneRoot);   
        }

        public void Dispose()
        { 
        
        }

        object LoadNode(ContentBuilder builder, MyContentManager contentManager,
                        string importNm, string processorNm)
        {

            object result = null;

            //string error = builder.Build();
            //if (string.IsNullOrEmpty(error))
            //{
                result = contentManager.Load<NodesGrp>(_AssetNm);
            //}
            return result;
        }
      }
    }

