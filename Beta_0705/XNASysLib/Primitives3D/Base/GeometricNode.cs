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
using VertexPipeline;
#endregion

namespace XNASysLib.Primitives3D
{
    public class GeometricNode : ShapeNode, 
        IDisposable
    {
        #region Fields


        #endregion
       



        public GeometricNode(GraphicsDevice graphic):
            base(graphic)
            //base(numOfVertices, numOfIndices, graphic)
        {
            //_basicEffect = new BasicEffect(_game.GraphicsDevice);
            //_game.Components.Add(this);
        
        }
        
        public void AddVertex(Vector3 position, Vector3 normal,Vector2 uv)
        {
            this.Vertices.Add(new VertexPositionNormalTexture(position, normal,uv));
        }


        /// <summary>
        /// Adds a new index to the primitive model. This should only be called
        /// during the initialization process, before InitializePrimitive.
        /// </summary>
        public void AddIndex(int index)
        {
            if (index > ushort.MaxValue)
                throw new ArgumentOutOfRangeException("index");

            this._indices.Add((ushort)index);
        }


        /// <summary>
        /// Queries the index of the current vertex. This starts at
        /// zero, and increments every time AddVertex is called.
        /// </summary>
        public int CurrentVertex
        {
            get { return this.Vertices.Count; }
        }


        /// <summary>
        /// Once all the geometry has been specified by calling AddVertex and AddIndex,
        /// this method copies the vertex and index data into GPU format buffers, ready
        /// for efficient rendering.
        public void InitializePrimitive(Matrix trans)
        {/*
            // Create a vertex declaration, describing the format of our vertex data.

            // Create a vertex buffer, and copy our vertex data into it.
            Shape.VertexBuffer = new VertexBuffer(graphicsDevice,
                                            typeof(VertexPositionNormal),
                                            this.Shape.Vertices.Count, BufferUsage.None);

            Shape.VertexBuffer.SetData(Shape.Vertice.ToArray());

            // Create an index buffer, and copy our index data into it.
            _indexBuffer = new IndexBuffer(graphicsDevice, typeof(ushort),
                                          _indices.Count, BufferUsage.None);

            _indexBuffer.SetData(_indices.ToArray());

            // Create a BasicEffect, which will be used to render the primitive.
            _basicEffect = new BasicEffect(graphicsDevice);

            _basicEffect.EnableDefaultLighting();*/
            
        }


        /// <summary>
        /// Finalizer.
        /// </summary>
        ~GeometricNode()
        {
            Dispose(false);
        }


        /// <summary>
        /// Frees resources used by this object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Frees resources used by this object.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
              

                //this._game.Components.Remove(this);
            }
        }


        #region Draw

        /*public virtual void Draw(GameTime gameTime)
        {
            this.Draw(this._world,_camera.ViewMatrix,
                _camera.ProjectionMatrix,_color);
           

            Shape.Draw(this._camera,this.TransNod,gameTime,_basicEffect);
        } */

        /* /// <summary>
         /// Draws the primitive model, using the specified effect. Unlike the other
         /// Draw overload where you just specify the world/view/projection matrices
         /// and color, this method does not set any renderstates, so you must make
         /// sure all states are set to sensible values before you call it.
         /// </summary>
         void Draw(Effect effect)
         {
             GraphicsDevice graphicsDevice = effect.GraphicsDevice;

             // Set our vertex declaration, vertex buffer, and index buffer.
             graphicsDevice.SetVertexBuffer(Shape.VertexBuffer);

             graphicsDevice.Indices = _indexBuffer;


             foreach (EffectPass effectPass in effect.CurrentTechnique.Passes)
             {
                 effectPass.Apply();

                 int primitiveCount = _indices.Count / 3;

                 graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0,
                                                      _vertices.Count, 0, primitiveCount);

             }
         }


         /// <summary>
         /// Draws the primitive model, using a BasicEffect shader with default
         /// lighting. Unlike the other Draw overload where you specify a custom
         /// effect, this method sets important renderstates to sensible values
         /// for 3D model rendering, so you do not need to set these states before
         /// you call it.
         /// </summary>
         void Draw(Matrix world, Matrix view, Matrix projection, Color color)
         {
             // Set BasicEffect parameters.
             _basicEffect.World = world;
             _basicEffect.View = view;
             _basicEffect.Projection = projection;
             _basicEffect.DiffuseColor = color.ToVector3();
             _basicEffect.Alpha = color.A / 255.0f;

             GraphicsDevice device = _basicEffect.GraphicsDevice;
             //device.DepthStencilState = DepthStencilState.None;

             if (color.A < 255)
             {
                 // Set renderstates for alpha blended rendering.
                 device.BlendState = BlendState.AlphaBlend;
             }
             else
             {
                 // Set renderstates for opaque rendering.
                 device.BlendState = BlendState.Opaque;
             }

             // Draw the model, using BasicEffect.
             Draw(_basicEffect);
         }

 */
        #endregion
    }

}
