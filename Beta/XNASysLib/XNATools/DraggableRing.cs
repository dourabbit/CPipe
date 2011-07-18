#region File Description
//-----------------------------------------------------------------------------
// Primitives3DGame.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNASysLib.XNAKernel;
using XNASysLib.XNATools;
using XNASysLib.Primitives3D;
using VertexPipeline;
#endregion

namespace XNASysLib.XNATools
{

    /// <summary>
    /// Geometric primitive class for drawing spheres.
    /// </summary>
    public class DraggableRing :SceneDraggablePrimitive //ToolPrimitive
    {
        protected aCTool _tool;
        public GeometricNode ShapeNode { get; set; }
        public float Width{get;set;}
        public float Radius{get;set;}
        /// <summary>
        /// Tool parts entries
        /// </summary>
        /// <param name="game"></param>
        /// <param name="diameter"></param>
        /// <param name="tessellation"></param>
        public DraggableRing(IGame game, float radius, float width,
            int tessellation, IDrawableComponent toolTarget) :
            base(game)
            //base(game, toolTarget)
        {
            Width = width;
            Radius = radius;

            ShapeNode = new GeometricNode(game.GraphicsDevice);
            ShapeNode.PreDrawHandler += delegate
            {


                _game.GraphicsDevice.RasterizerState =
                    RasterizerState.CullNone;
                if (_rasterizerState.FillMode == FillMode.WireFrame)
                {
                    _color = Color.Yellow;
                }
                else
                    _color = Color.Red;
            };
            ConstructFaces(game,Radius,Width, tessellation);   
        }


        /// <summary>
        /// Constructs a new sphere primitive,
        /// Test Code...; DraggableSphere
        /// </summary>
        public DraggableRing(IGame game, float radius, float width,
            int tessellation):base(game)
        {
            ShapeNode = new GeometricNode(game.GraphicsDevice);
            Width = width;
            Radius = radius;
            this.ConstructFaces(game, Radius, Width, tessellation);
            ShapeNode.PreDrawHandler += delegate
            {


                _game.GraphicsDevice.RasterizerState =
                    RasterizerState.CullClockwise;
                if (_rasterizerState.FillMode == FillMode.WireFrame)
                {
                    _color = Color.Yellow;
                }
                else
                    _color = Color.Red;
            };
            this._dragHandler=null;
            game.Components.Add(this);
        }



        void ConstructFaces(IGame game,float radius, float width, int tessellation)
        { 
            if (tessellation < 3)
                throw new ArgumentOutOfRangeException("tessellation");

            int segments = tessellation;

            // Create rings of vertices at progressively higher latitudes.
            for (int i = 0; i < segments; i++)
            {
                float theta = (i *(2* MathHelper.Pi) /
                                            segments);

                float dy = (float)Math.Sin(theta);
                float dx = (float)Math.Cos(theta);

                Vector3 normal=new Vector3(dx,dy,0);

                ShapeNode.AddVertex(radius*(new Vector3(dx,dy,-width/2f)), 
                                    normal, Vector2.Zero);
                ShapeNode.AddVertex(radius*(new Vector3(dx, dy, width / 2f)),
                                    normal, Vector2.Zero);

                //// Create a single ring of vertices at this latitude.
                //for (int j = 0; j < horizontalSegments; j++)
                //{
                //    float longitude = j * MathHelper.TwoPi / horizontalSegments;

                //    float dx = (float)Math.Cos(longitude) * dxz;
                //    float dz = (float)Math.Sin(longitude) * dxz;

                //    Vector3 normal = new Vector3(dx, dy, dz);
                //    ShapeNode.AddVertex(normal * radius, normal, Vector2.Zero);
             
                //}
            }

            int digit;
            int factor;
            // Create a fan connecting the bottom vertex to the bottom latitude ring.
            for (int i = 0; i < segments*2; i++)
            {
                factor = i % 2 == 0 ? -1 : 1;
                digit = i + 1;
                if (digit >= segments * 2)
                    digit -= segments * 2;
                ShapeNode.AddIndex(digit);
                ShapeNode.AddIndex(digit + factor);

                int lastDigit = digit - factor;
                if (lastDigit <= 0)
                    lastDigit += segments * 2;
                else if (lastDigit >= segments * 2)
                    lastDigit -= segments * 2;

                ShapeNode.AddIndex(lastDigit);
            }


            ShapeNode.InitializePrimitive(TransformNode.World);
        }
        
        public override void Initialize()
        {
            
           
           base.Initialize();
        }
        protected override void OnModifyBoundingSpheres()
        {
            if (_modelRadius == 0)
            {
                Vector3 modelCenter = Vector3.Zero;

                foreach (VertexPositionNormalTexture vecs in this.ShapeNode.Vertices)
                {

                    modelCenter += vecs.Position;
                }
                modelCenter /= this.ShapeNode.Vertices.Count;


                // Now we know the center point, we can compute the model radius
                // by examining the radius of each mesh bounding sphere.
                _modelRadius = 0;

                foreach (VertexPositionNormalTexture vecs in this.ShapeNode.Vertices)
                {

                    float radius = (modelCenter - vecs.Position).Length();

                    _modelRadius = Math.Max(_modelRadius, radius);
                }

                this._selCompData.shape = this.ShapeNode;
            
            }
            this._selCompData.BoundingSpheres =
                new BoundingSphere[1];

            //this._selCompData.BoundingSpheres[0].Center
            //    = Vector3.Transform(base.TransformNode.Pivot.Translation, base.TransformNode.World);

            this._selCompData.BoundingSpheres[0].Center
                = TransformNode.Translate + TransformNode.Pivot.Translation;

            this._selCompData.BoundingSpheres[0].Radius
                = this._modelRadius + 0.01f;

            this._selCompData.transform = this.TransformNode;
        }
        protected override void LoadContent()
        {

            Vector3 modelCenter = Vector3.Zero;

            foreach (VertexPositionNormalTexture vecs in this.ShapeNode.Vertices)
            {

                modelCenter += vecs.Position;
            }
            modelCenter /= this.ShapeNode.Vertices.Count;


            // Now we know the center point, we can compute the model radius
            // by examining the radius of each mesh bounding sphere.
            _modelRadius = 0;

            foreach (VertexPositionNormalTexture vecs in this.ShapeNode.Vertices)
            {

                float radius = (modelCenter - vecs.Position).Length();

                _modelRadius = Math.Max(_modelRadius, radius);
            }


            base.LoadContent();
        }
        protected override void OnDragStart()
        {
            if (this._tool.PreExe != null)
                this._tool.PreExe.Invoke();
            base.OnDragStart();
        }
        protected override void OnDragEnd()
        {

            if (this._tool.AfterExe != null)
                this._tool.AfterExe.Invoke();


            base.OnDragEnd();
        }
        protected override void OnDragExe()
        {
            if (this._tool.Exe != null)
                this._tool.Exe.Invoke();

            base.OnDragExe();
        }
        public override void Update(GameTime gameTime)
        {
            if (!_isInitialized)
                Initialize();
            //this.TransformNode.UpdateTransform();
            this.ShapeNode.UpdateShapeNod(_game.GraphicsDevice);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, ICamera cam)
        {

            

            ShapeNode.Draw(gameTime, cam, this._basicEffect,this.TransformNode.AbsoluteTransform);
            base.Draw(gameTime, cam);
        }



    }

}
