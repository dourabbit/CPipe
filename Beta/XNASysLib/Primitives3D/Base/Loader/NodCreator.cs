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
using System.Reflection;
using XNASysLib.Primitives3D.Base.Loader;
#endregion

namespace XNASysLib.Primitives3D
{
    /// <summary>
    /// Create preloaded model by passing model type
    /// </summary>
    public class NodCreator:IDisposable
    {

        //string _AssetNm;
        //IGame _game;
       // List<TransformNode> nodes = new List<TransformNode>();
        //Type Type;

        static NodCreator _singleton = new NodCreator();


        void ProcessSceneNod(SceneNodHierachyModel curSceneNod,
            TransformNode transNod,NodesGrp shapeGrp, ref SceneNodHierachyModel root,
            ref int curIndex, IGame _game, Type type)
        {

            curIndex++;
            curSceneNod.NodeNm = transNod.NodeNm;
            curSceneNod.ID = transNod.NodeNm;
            curSceneNod.Children = new NodeChildren<INode>();
            curSceneNod.Root = root;
            curSceneNod.TransformNode = transNod;
            curSceneNod.TransformNode.AbsoluteTransform = Matrix.Identity;


            ////If the shape's parent is cur node, then we have shape node.
            //foreach (ShapeNode shape in shapeGrp)
            //    if (shape.ParentIndex == curIndex)
            //        curSceneNod.ShapeNode = shape;

            //If only one obj, no child at all. New a dummy group
            if (curSceneNod == root && transNod.Children.Count == 0)
            {
               
                //Get the Constructor to new that type of class
                ConstructorInfo constructor = type.GetConstructor(new Type[] { _game.GetType() });
                SceneNodHierachyModel childSceneH = (SceneNodHierachyModel)constructor.Invoke(new object[] { _game });
                
                curSceneNod.Children.Add(childSceneH);
                childSceneH.Root = curSceneNod;
                childSceneH.Children = new NodeChildren<INode>();
                childSceneH.TransformNode = transNod;
                childSceneH.TransformNode.AbsoluteTransform = Matrix.Identity;
                childSceneH.ShapeNode = curSceneNod.ShapeNode;
                childSceneH.Parent = curSceneNod;
                childSceneH.ID = transNod.NodeNm;
                curSceneNod.ShapeNode = null;
                curSceneNod.ID = "root";
                curSceneNod.TransformNode = new TransformNode();
            }



            foreach (ShapeNode shape in shapeGrp)
            {
                if (shape.ParentIndex != curIndex)
                    continue;//skip when it is not current cild

                if (curSceneNod.ShapeNode == null)
                {
                    if (curSceneNod.ID == "root")
                    {
                        SceneNodHierachyModel child =
                         curSceneNod.Children[0] as SceneNodHierachyModel;

                        if (child.ShapeNode == null)
                            child.ShapeNode = shape;
                        else
                            ShapeNode.CombineShape(shape, child.ShapeNode);

                    }
                    else
                    {

                        curSceneNod.ShapeNode = shape;
                    }
                }
                else
                {

                    //SceneNodHierachyModel sibling = new SceneNodHierachyModel(_game);
                    //sibling.NodeNm = transNod.NodeNm;
                    //sibling.Children = new NodeChildren<INode>();
                    //sibling.Root = root;
                    //TransformNode newTransNod = new TransformNode();
                    //sibling.TransformNode = newTransNod;
                    //sibling.TransformNode.AbsoluteTransform = Matrix.Identity;
                    //sibling.ShapeNode = shape;
                    //curSceneNod.Children.Add(sibling);
                    ShapeNode.CombineShape(shape, curSceneNod.ShapeNode);
                }
            }
            // Recurse over any child nodes.
            foreach (TransformNode childTransNod in transNod.Children)
            {
                ConstructorInfo constructor = type.GetConstructor(new Type[] { _game.GetType() });
                SceneNodHierachyModel childSceneH = (SceneNodHierachyModel)constructor.Invoke(new object[] { _game });
                //new Pipe(_game);
                curSceneNod.Children.Add(childSceneH);
                childSceneH.Parent = curSceneNod;
                ProcessSceneNod(childSceneH, childTransNod, shapeGrp, ref root, ref curIndex, _game, type);
            }
        }
        /// <summary>
        /// for duplicating origianl obj
        /// </summary>
        /// <param name="game"></param>
        /// <param name="original"></param>
        public static SceneNodHierachyModel CopyNode(IGame _game, SceneNodHierachyModel original)
        {
            //_game = game;
            List<SceneNodHierachyModel> newModels = new List<SceneNodHierachyModel>();
            for (int i = 0; i < original.FlattenNods.Count; i++)
            {
                SceneNodHierachyModel model = (SceneNodHierachyModel)original.FlattenNods[i];

                ConstructorInfo cons = model.GetType().GetConstructor(new Type[] { _game.GetType() });
                SceneNodHierachyModel newModel = (SceneNodHierachyModel)cons.Invoke(new object[] { _game });
                newModel.ID = model.ID;
                newModel.Type = ((SceneNodHierachyModel)original.FlattenNods[i]).Type;
                
                newModel.TransformNode = model.TransformNode.GetCopy();
                if (model.ShapeNode != null)
                    newModel.ShapeNode = model.ShapeNode.GetCopy();
                else
                    newModel.ShapeNode = null;
                newModels.Add(newModel);
                newModel.Root = newModels[0];
                
                newModel.Children = new NodeChildren<INode>();

                if (original.FlattenNods[i].Parent != null)
                {
                    int parentIndex = original.FlattenNods.IndexOf(original.Parent);
                    newModel.Parent = newModels[parentIndex];
                    newModels[parentIndex].Children.Add(newModel);
                }
            }
            _game.Components.Add(newModels[0]);

            return newModels[0];
        }

        public static SceneNodHierachyModel CreateNode(IGame game, string assetNm, Type T)
        {
            //_game = game;
           // Type = T;
           // _AssetNm = assetNm;
            ContentBuilder builder=
                (ContentBuilder)game.Services.
                GetService(typeof(ContentBuilder));

            MyContentManager contentManager=
                (MyContentManager)game.Services.
                GetService(typeof(MyContentManager));

            NodesGrp shapeGrp = (NodesGrp)LoadNode
                    (builder, contentManager, assetNm, "ShapeN_SkinDProcessor");

            TransformNode transNodRoot = new TransformNode();
            List<TransformNode> nodes = new List<TransformNode>();
            for (int i = 0; i < shapeGrp.TransData.NameGrp.Count; i++)
            {
                TransformNode nod = new TransformNode();
                nodes.Add(nod);
                LoadHelper.ReGroupNodes(nod,shapeGrp,i,ref transNodRoot,nodes);
                nod.Root = transNodRoot;
            }


            SceneNodHierachyModel sceneRoot = new SceneNodHierachyModel(game);
            sceneRoot.Type = T;

            //ConstructorInfo constructor = Type.GetConstructor(new Type[] { _game.GetType() });
            //SceneNodHierachyModel sceneRoot = (SceneNodHierachyModel)constructor.Invoke(new object[] { _game });
            int index=-1;
            _singleton.ProcessSceneNod(sceneRoot, transNodRoot, shapeGrp, ref sceneRoot, ref index, game,T);
            game.Components.Add(sceneRoot);
            return sceneRoot;
        }

        public void Dispose()
        { 
        
        }

        static object LoadNode(ContentBuilder builder, MyContentManager contentManager,
                        string fileNm, string processorNm)
        {
            string assetNm = Path.GetFileNameWithoutExtension(fileNm);
            //builder.Add(this._AssetNm, assetNm, importNm, processorNm);
            object result = null;

            //string error = builder.Build();
            //if (string.IsNullOrEmpty(error))
            //{
            result = contentManager.Load<NodesGrp>(assetNm);
            //}
            return result;
        }
      }
    }

