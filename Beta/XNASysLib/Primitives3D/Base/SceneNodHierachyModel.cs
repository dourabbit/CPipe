﻿#define _DEBUG
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
using VertexPipeline;
using VertexPipeline.Data;
using XNAPhysicsLib;
#endregion

namespace XNASysLib.Primitives3D
{
    /*
    public class HierachyModelCollection : List<SceneNodHierachyModel>
    { }*/

    public interface ISceneNod : INode
    {
        Type Type
        {
            get;
            set;
        }
    }
    public class SceneNodHierachyModel : ScenePrimitive, ISceneNod
    {
        public ShapeNode ShapeNode { get; set; }
        protected Vector3 _modelCenter;
        public Type Type { get; set; }
        public string NodeNm
        {
            get { return this._ID; }
            set { this._ID = value; }
        }
        public INode Parent { get; set; }
        public NodeChildren<INode> Children{ get; set; }
        public INode Root { get; set; }
        public int Index { get; set; }
        public int DataSlot { get { return _dataSlotIndex; } }
        int _dataSlotIndex;
        public ObjData ObjDataGen;

        protected SceneHub _hub;


#if _DEBUG
        DebugDraw _debugDraw;
#endif

        public List<INode> FlattenNods
        {
            get
            {
                List<INode> result = new List<INode>();
                int index=-1;
                SceneNodHierachyModel.FlattenNodTree(Root,ref result,ref index);
                return result;
            }
        }

        public override bool Lock
        {
            get
            {
                return _lock;
            }
            set
            {


                foreach (INode node in FlattenNods)
                {
                    SceneNodHierachyModel nodModel = node as SceneNodHierachyModel;
                    if (nodModel != null && nodModel != this)
                        nodModel._lock = value;

                }

                _lock = value;
            }
        }


        public SceneNodHierachyModel(IGame game)//, string assetNm)
            : base(game)
        {

        }

        public override void Initialize()
        {

#if _DEBUG
            this._debugDraw = new
                DebugDraw(_game.GraphicsDevice);
#endif
            MyContentManager contentManager =
               (MyContentManager)_game.Services.
               GetService(typeof(MyContentManager));

            SpriteFont font = contentManager.Load<SpriteFont>("_FontSetup");
            _hub = new SceneHub(_game, font,
                new Microsoft.Xna.Framework.Graphics.SpriteBatch(_game.GraphicsDevice));
            base.Initialize();
            //foreach (SceneNodHierachyModel curNod in this.Children)
            //{
            //    curNod.Initialize();
            //}
            OnUpdate();
            this.TransformNode.DataModifiedHandler += OnTransformDataChange;
            this.TransformNode.DataModifiedHandler.Invoke();
            _dataSlotIndex = DataReactor.GetDataSlot(typeof(ObjData));
        }
        public object GetCopy()
        {

            return this.MemberwiseClone();
        }
        public virtual void OnTransformDataChange()
        {

            OnUpdate();
            OnModify();
            this.ObjDataGen= new ObjData(this,_dataSlotIndex);
           
        }
        /// <summary>
        /// Update TransformNode hierachily
        /// </summary>
        public void OnUpdate()
        {

            if (this.Parent != null)
            {
                this.TransformNode.AbsoluteTransform =
                    this.TransformNode.World*
                    ((SceneNodHierachyModel)this.Parent).
                        TransformNode.AbsoluteTransform;
                MyConsole.WriteLine(this.ID+this.TransformNode.AbsoluteTransform.ToString());
                if (ShapeNode != null) //if the root node has shape info
                    this.ShapeNode.UpdateShapeNod( _game.GraphicsDevice);
            }
            else //No parent means it is the root node.
            {
                this.TransformNode.AbsoluteTransform = this.TransformNode.World;

                if (ShapeNode != null) //if the root node has shape info
                    this.ShapeNode.UpdateShapeNod(_game.GraphicsDevice);

            }

            foreach (SceneNodHierachyModel curNod in this.Children)
            {
                curNod.OnUpdate();
            }
        }
        protected override void OnModify()
        {
            if (ShapeNode != null)
            {
                this._selCompData.BoundingSpheres =
                    new BoundingSphere[1];
                this._selCompData.BoundingSpheres[0] = ShapeNode.BoundingSpheres[0];

                if (_selCompData.BoundingSpheres[0].Radius == 0)//ShapeNode RenderData hasn't been initialized
                {
                    //Bsphere = null;
                }
                else
                {

                    float scale = TransformNode.Scale.X > TransformNode.Scale.Y ?
                        TransformNode.Scale.X : TransformNode.Scale.Y;
                    scale = scale > TransformNode.Scale.Z ?
                        scale : TransformNode.Scale.Z;

                }

                this._selCompData.shape = ShapeNode;
                this._selCompData.transform = TransformNode;
                this._selCompData.BoundingSpheres[0].Center =
                    Vector3.Transform(this._selCompData.BoundingSpheres[0].Center,
                                      this.TransformNode.AbsoluteTransform);
               
                MyConsole.WriteLine(this._selCompData.transform.AbsoluteTransform.ToString());
            }
            foreach (SceneNodHierachyModel curNod in this.Children)
            {
                curNod.OnModify();
            }

        }
    
        public override void Update(GameTime gameTime)
        {
           


            foreach (SceneNodHierachyModel curNod in this.Children)
            {
                curNod.Update(gameTime);
            }
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime,ICamera cam)
        {


            if (!_isInitialized)
                Initialize();
#if _DEBUG
             _debugDraw.Begin(//this.TransformNode.AbsoluteTransform,cam.ViewMatrix,//this._world, cam.ViewMatrix),
                    Matrix.Identity,
                    cam.ViewMatrix,
                    cam.ProjectionMatrix);
         

            if (this.Data.BoundingSpheres!=null)
                foreach(BoundingSphere bSphere in this.Data.BoundingSpheres)
                    _debugDraw.DrawWireSphere(bSphere,Color.Black);
            
            _debugDraw.End();

#endif



            if (this.ShapeNode != null)
            {
                _game.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                _game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                this._basicEffect.World = this.TransformNode.AbsoluteTransform;
                this._basicEffect.View = cam.ViewMatrix;
                this._basicEffect.Projection = cam.ProjectionMatrix;
                _basicEffect.DiffuseColor = _color.ToVector3();//Color.Aqua.ToVector3();
                _basicEffect.AmbientLightColor = Color.Black.ToVector3();
                _basicEffect.SpecularColor = _color.ToVector3();//Color.Azure.ToVector3();
                _basicEffect.EnableDefaultLighting();
                _basicEffect.PreferPerPixelLighting = true;

                this.ShapeNode.Draw(gameTime, cam, _basicEffect,TransformNode.AbsoluteTransform);
                if (_rasterizerState.FillMode == FillMode.WireFrame)
                {
                    _game.GraphicsDevice.BlendState = BlendState.Additive;
                    _game.GraphicsDevice.RasterizerState = _rasterizerState;

                    this.ShapeNode.Draw(gameTime, cam, _basicEffect,TransformNode.AbsoluteTransform);

                }
                _game.GraphicsDevice.BlendState = BlendState.Opaque;
                _game.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            }
            foreach (SceneNodHierachyModel curNod in this.Children)
            {
                    curNod.Draw(gameTime, cam);
            }



            //Draw Hub info
            if(ShapeNode!=null)
                _hub.Draw(gameTime, this.ID, this._selCompData.BoundingSpheres[0].Center);

            //base.Draw(gameTime, cam);
        }
        

        public static void FlattenNodTree(INode node, ref List<INode> result,
                                                            ref int curIndex)
        {

            curIndex++;
            result.Add(node);
            node.Index = curIndex;


            // Recurse over any child nodes.
            if (node.Children != null && node.Children.Count != 0)
                foreach (INode child in node.Children)
                {
                    FlattenNodTree(child, ref result, ref curIndex);
                }

            
        }


   }
}

