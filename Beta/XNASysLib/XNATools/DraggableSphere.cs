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
    public class DraggableSphere :SceneDraggablePrimitive //ToolPrimitive
    {

        public GeometricNode ShapeNode { get; set; }
        /// <summary>
        /// Tool parts entries
        /// </summary>
        /// <param name="game"></param>
        /// <param name="diameter"></param>
        /// <param name="tessellation"></param>
        public DraggableSphere(IGame game, float diameter,
            int tessellation, IDrawableComponent toolTarget) :
            base(game)
            //base(game, toolTarget)
        {
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
            ConstructFaces(game,diameter, tessellation);   
        }


        /// <summary>
        /// Constructs a new sphere primitive,
        /// Test Code...; DraggableSphere
        /// </summary>
        public DraggableSphere(IGame game,
                               float diameter, 
            int tessellation):base(game)
        {
            ShapeNode = new GeometricNode(game.GraphicsDevice);
            this.ConstructFaces(game,diameter,tessellation);

            game.Components.Add(this);
        }



        void ConstructFaces(IGame game,float diameter, int tessellation)
        { 
            if (tessellation < 3)
                throw new ArgumentOutOfRangeException("tessellation");

            int verticalSegments = tessellation;
            int horizontalSegments = tessellation * 2;

            float radius = diameter / 2;

            // Start with a single vertex at the bottom of the sphere.
            ShapeNode.AddVertex(Vector3.Down * radius, Vector3.Down, Vector2.Zero);

            // Create rings of vertices at progressively higher latitudes.
            for (int i = 0; i < verticalSegments - 1; i++)
            {
                float latitude = ((i + 1) * MathHelper.Pi /
                                            verticalSegments) - MathHelper.PiOver2;

                float dy = (float)Math.Sin(latitude);
                float dxz = (float)Math.Cos(latitude);

                // Create a single ring of vertices at this latitude.
                for (int j = 0; j < horizontalSegments; j++)
                {
                    float longitude = j * MathHelper.TwoPi / horizontalSegments;

                    float dx = (float)Math.Cos(longitude) * dxz;
                    float dz = (float)Math.Sin(longitude) * dxz;

                    Vector3 normal = new Vector3(dx, dy, dz);
                    ShapeNode.AddVertex(normal * radius, normal, Vector2.Zero);
             
                }
            }

            // Finish with a single vertex at the top of the sphere.
            ShapeNode.AddVertex(Vector3.Up * radius, Vector3.Up, Vector2.Zero);

            // Create a fan connecting the bottom vertex to the bottom latitude ring.
            for (int i = 0; i < horizontalSegments; i++)
            {
                ShapeNode.AddIndex(0);
                ShapeNode.AddIndex(1 + (i + 1) % horizontalSegments);
                ShapeNode.AddIndex(1 + i);
            }

            // Fill the sphere body with triangles joining each pair of latitude rings.
            for (int i = 0; i < verticalSegments - 2; i++)
            {
                for (int j = 0; j < horizontalSegments; j++)
                {
                    int nextI = i + 1;
                    int nextJ = (j + 1) % horizontalSegments;
                    ShapeNode.AddIndex(1 + i * horizontalSegments + j);
                    ShapeNode.AddIndex(1 + i * horizontalSegments + nextJ);
                    ShapeNode.AddIndex(1 + nextI * horizontalSegments + j);

                    ShapeNode.AddIndex(1 + i * horizontalSegments + nextJ);
                    ShapeNode.AddIndex(1 + nextI * horizontalSegments + nextJ);
                    ShapeNode.AddIndex(1 + nextI * horizontalSegments + j);
                
                }
            }

            // Create a fan connecting the top vertex to the top latitude ring.
            for (int i = 0; i < horizontalSegments; i++)
            {
                ShapeNode.AddIndex(ShapeNode.CurrentVertex - 1);
                ShapeNode.AddIndex(ShapeNode.CurrentVertex - 2 - (i + 1) % horizontalSegments);
                ShapeNode.AddIndex(ShapeNode.CurrentVertex - 2 - i);
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
            base.OnModifyBoundingSpheres();
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

        public override void Update(GameTime gameTime)
        {
            if (!_isInitialized)
                Initialize();
           // this.TransformNode.UpdateTransform();
            this.ShapeNode.UpdateShapeNod(_game.GraphicsDevice);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, ICamera cam)
        {
            ShapeNode.Draw(gameTime, cam, this._basicEffect,TransformNode.AbsoluteTransform);
            base.Draw(gameTime, cam);
        }



    }

}
