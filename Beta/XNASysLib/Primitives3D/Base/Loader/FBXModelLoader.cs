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
using XNASysLib.Primitives3D.Base.Loader;
#endregion

namespace XNASysLib.Primitives3D
{

    public class FBXModelLoader:IDisposable
    {

        //string _AssetNm;
        //IGame _game;

        public static SceneNodHierachyModel Import(IGame game, string assetNm)
        {
            //_game = game;
            //_AssetNm = assetNm;
            List<TransformNode> nodes = new List<TransformNode>();
            ContentBuilder builder=
                (ContentBuilder)game.Services.
                GetService(typeof(ContentBuilder));

            MyContentManager contentManager=
                (MyContentManager)game.Services.
                GetService(typeof(MyContentManager));

            NodesGrp shapeGrp = (NodesGrp)LoadNode
                    (builder, contentManager, assetNm, "ShapeN_SkinDProcessor");

            TransformNode transNodRoot = new TransformNode(); 
            for (int i = 0; i < shapeGrp.TransData.NameGrp.Count; i++)
            {
                TransformNode nod = new TransformNode();
                nodes.Add(nod);
                LoadHelper.ReGroupNodes(nod, shapeGrp, i, ref transNodRoot, nodes);
                nod.Root = transNodRoot;
            }

          
            SceneNodHierachyModel sceneRoot = new SceneNodHierachyModel(game);
            
            int index=-1;
            LoadHelper.ProcessSceneNod(game, sceneRoot, transNodRoot, shapeGrp, ref sceneRoot, ref index);
            game.Components.Add(sceneRoot);

            return sceneRoot;
        }

        public void Dispose()
        { 
            
        }

        static object LoadNode(ContentBuilder builder, 
                        MyContentManager contentManager,
                        string importNm, string processorNm)
        {
            string assetNm = Path.GetFileNameWithoutExtension(importNm);
            builder.Add(importNm, assetNm, importNm, processorNm);
            object result = null;

            string error = builder.Build();
            if (string.IsNullOrEmpty(error))
            {
                result = contentManager.Load<NodesGrp>(assetNm);
            }
            return result;
        }
      }
    }

