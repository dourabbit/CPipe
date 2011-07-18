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
#endregion

namespace XNASysLib.Primitives3D
{
    public class SceneNodModel : ScenePrimitive
    {
        public ShapeNode ShapeNode{get;set;}
        protected Vector3 _modelCenter;
   
        //public Matrix ObjectSpace { get; internal set; }


        /*
        public override Vector3 Translate
        {
            get { return this._translation; }
            set
            {
                this._translation = value;


                if (_selCompData.dataModifitionHandler != null)
                    _selCompData.dataModifitionHandler.Invoke();
            }
        }
        public override Vector3 Pivot
        {
            get
            {
                _world = TransformHelper.RotInObjSpace
                         (_rotation, _pivot, _translation, _scale);

                return Vector3.Transform(_pivot, _world);
            }
            set
            {
                Vector3 offset =
                    value - Vector3.Transform(_pivot, _world);

                _pivot += offset;
            }
        }
        public override Vector3 Rotate
        {
            get { return this._rotation; }
        }
        */

        public SceneNodModel(IGame game, ShapeNode data, string assetNm)
            : base(game)
        {
            this.ShapeNode = data;
            _AssetNm = assetNm;
        }

        public override void Initialize()
        {
           // this.ObjectSpace = Matrix.Identity;

            _contentManager =
                (MyContentManager)_game.Services.
                GetService(typeof(MyContentManager));
            base.Initialize();
        }
        protected override void LoadContent()
        {
            if(ShapeNode==null)
                ShapeNode = _contentManager.Load<ShapeNode>(_AssetNm).GetModel();

            string a = Path.GetFileNameWithoutExtension(_AssetNm);
            _game.Components.GetNm(a, out _ID);
            this.ShapeNode.ID = _ID + "_Shape";
            base.LoadContent();
        }
        protected override void OnModifyBoundingSpheres()
        {

            this._selCompData.BoundingSpheres =
                new BoundingSphere[1];

            BoundingSphere Bsphere =
                ShapeNode.BoundingSpheres[0];
            /*
            _world =
            TransformHelper.RotInObjSpace
            (this._rotation, this._pivot,
             this._translation, this._scale, ref this._rotQuaternion);
            */
           // this.TransformNode.UpdateTransform();

            //float scale = _scale.X > _scale.Y ? _scale.X : _scale.Y;
            //scale = scale > _scale.Z ? scale : _scale.Z;
            float scale = TransformNode.Scale.X > TransformNode.Scale.Y ?
                TransformNode.Scale.X : TransformNode.Scale.Y;
            scale = scale > TransformNode.Scale.Z ?
                scale : TransformNode.Scale.Z;



            Bsphere.Radius *= scale;

            Bsphere.Center =
                Vector3.Transform(Bsphere.Center, this.TransformNode.World);

            this._selCompData.BoundingSpheres[0] = Bsphere;
            this._selCompData.transform = TransformNode;
            this._selCompData.shape = ShapeNode;

        }
        protected virtual void ProcessModel()
        { 
        
        }
        public override void Update(GameTime gameTime)
        {

           // this.TransformNode.UpdateTransform();
            if(ShapeNode!=null)
                this.ShapeNode.UpdateShapeNod(_game.GraphicsDevice);
            
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime,ICamera cam)
        {


            _game.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            _game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            this._basicEffect.World = this.TransformNode.AbsoluteTransform;
            this._basicEffect.View = cam.ViewMatrix;
            this._basicEffect.Projection = cam.ProjectionMatrix;
            _basicEffect.DiffuseColor = Color.Aqua.ToVector3();
            _basicEffect.AmbientLightColor = Color.Black.ToVector3();
            _basicEffect.SpecularColor = Color.Azure.ToVector3();
            _basicEffect.EnableDefaultLighting();
            _basicEffect.PreferPerPixelLighting = true;

            this.ShapeNode.Draw(gameTime, cam, _basicEffect,TransformNode.AbsoluteTransform);
            if (_rasterizerState.FillMode == FillMode.WireFrame)
            {
                _game.GraphicsDevice.BlendState = BlendState.Additive;
                _game.GraphicsDevice.RasterizerState = _rasterizerState;

                this.ShapeNode.Draw(gameTime, cam, _basicEffect, this.TransformNode.AbsoluteTransform);
            
            }
            _game.GraphicsDevice.BlendState = BlendState.Opaque;
            _game.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
        }
       
        
   }
}

