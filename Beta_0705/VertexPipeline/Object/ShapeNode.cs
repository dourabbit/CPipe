#region File Description
//-----------------------------------------------------------------------------
// CpuSkinnedModel.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexPipeline.Animation;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using VertexPipeline.Data;
using System;

namespace VertexPipeline
{

    /// <summary>
    /// The Struture to store the ShapeNode Data 
    /// </summary>
    public struct ShapeReadingData
    {
        public string Name;
        public int ParentIndex;
        public int TriangleCount;
        public VertexData[] Vertices;
        public IndexCollection IndexCollection;
        public BoundingSphere[] BoundingSpheres;
        public IndexBuffer IndexBuffer;
    }
    public delegate void PreDraw();

    public class ShapeNode : IShapeNode
    {
        public string ID { get; set; }
        public  int ParentIndex{get;set;}
        //protected int _triangleCount;
        protected IndexBuffer _indexBuffer;
        protected GraphicsDevice _graphic;
        protected List<ushort> _indices;

       // public BoundingSphere BoundingSphere{get;internal set;}
        public List<ushort> Indices
        {
            get { return _indices; }
            set { _indices = value; }
        }
       // protected List<VertexPositionNormalTexture> RenderVertices { get; set; }
        public List<VertexPositionNormalTexture> Vertices 
        { 
            get { return _vertices; } 
        }
        private List<VertexPositionNormalTexture> _vertices; 
        public PreDraw PreDrawHandler;
        public DynamicVertexBuffer VertexBuffer { get; internal set; }

        public ShapeNode GetCopy()
        {
            return (ShapeNode)this.MemberwiseClone();
        }
        public IndexBuffer IndexBuffer
        { get { return _indexBuffer; } }
        //public IndexBuffer IndexBuffer 
        //{ 
        //    get { return _indexBuffer; }
        //    set { _indexBuffer = value; _triangleCount = _indexBuffer.IndexCount / 3; }
        //}
        public BoundingSphere[] BoundingSpheres { get; set; }
        public bool IsIntialized { get; set; }
        public ShapeNode(GraphicsDevice graphic)
            //(int numOfVertices,int numOfIndices,GraphicsDevice graphic)
        {
            _graphic = graphic;
            _indices = new List<ushort>();
            //this._vertexCount = numOfVertices;
            _vertices = new List<VertexPositionNormalTexture>();
            this.IsIntialized = false;
            //RenderVertices = new List<VertexPositionNormalTexture>();
        }
        /// <summary>
        /// Combine two shapes into shapeNode, the second arguement
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static void CombineShape(ShapeNode from, ShapeNode to)
        {
            int offset = to._vertices.Count;
            foreach (VertexPositionNormalTexture vertice in from.Vertices)
            {
                to.Vertices.Add(vertice);
            }

            foreach (ushort indice in from.Indices)
            {

                ushort newIndice = Convert.ToUInt16(offset + indice);
                to.Indices.Add(newIndice);
            }
            Vector3 p1=from.BoundingSpheres[0].Center; 
            Vector3 p2=to.BoundingSpheres[0].Center;
            float r1 = from.BoundingSpheres[0].Radius;
            float r2 = to.BoundingSpheres[0].Radius;

            Vector3 p= (p1 + p2) / 2.0f;
            float tmp1=Vector3.Distance(p,p1)+r1;
            float tmp2=Vector3.Distance(p,p2)+r2;

            to.BoundingSpheres[0].Center = p;
            to.BoundingSpheres[0].Radius= tmp1 > tmp2 ? tmp1 : tmp2;
        }


        public ShapeNode(ShapeReadingData data)//, TransData transData)
        {
            this.ID = data.Name;
            this.ParentIndex = data.ParentIndex;

            this._vertices = getVertices(data);
            //this._vertexCount = data.Vertices.Length;
            //this.IndexBuffer = data.IndexBuffer;
            _indices = new List<ushort>();
            ushort[] indices=new ushort[data.IndexBuffer.IndexCount];
            
            data.IndexBuffer.GetData<ushort>(indices);
            foreach (ushort index in indices)
                _indices.Add(index);
            //RenderVertices = new List<VertexPositionNormalTexture>();
            this.IsIntialized = false;
            BoundingSpheres = data.BoundingSpheres;
            /*VertexBuffer = new DynamicVertexBuffer(IndexBuffer.GraphicsDevice,
                typeof(VertexPositionNormalTexture), data.Vertices.Length, BufferUsage.WriteOnly);
            */
        }

        public void GetBounding(out Vector3 center, out float radius)
        {
            List<Vector3> points = new List<Vector3>();

            foreach (VertexPositionNormalTexture point in Vertices)
                points.Add(point.Position);

            if (this.BoundingSpheres == null)
                this.BoundingSpheres = new BoundingSphere[1];

            this.BoundingSpheres[0] = BoundingSphere.CreateFromPoints(points.ToArray());
          
            center = this.BoundingSpheres[0].Center;
            radius = this.BoundingSpheres[0].Radius;



        }

        private List<VertexPositionNormalTexture> getVertices(ShapeReadingData data)
        {
            List<VertexPositionNormalTexture> result = 
                new List<VertexPositionNormalTexture>();


            for (int i = 0; i < data.Vertices.Length; i++)
                result.Add(
                    new VertexPositionNormalTexture
                    {
                        Position = data.Vertices[i].Position,
                        Normal = data.Vertices[i].Normal,
                        TextureCoordinate = new Vector2(0, 0)
                    });


            return result;

        
        }

        public ShapeNode GetModel()
        {
            return (ShapeNode)MemberwiseClone();
        }

        public void GetPositions(out Vector3[] positions)
        {
            positions=new Vector3[this._vertices.Count];

            for (int i = 0; i < _vertices.Count; i++)
            {
                positions[i]=_vertices[i].Position;
            }

        }

        public void Freeze(TransformNode node)
        {
            for (int index = 0; index < Vertices.Count; index++)
            {
                Vector3 normal = Vertices[index].Normal;
                Vector2 uv = Vertices[index].TextureCoordinate;
                VertexPositionNormalTexture tmp = new VertexPositionNormalTexture
                {
                    Position = Vector3.Transform( Vertices[index].Position,node.World),
                    Normal = normal,
                    TextureCoordinate = uv
                };
                Vertices[index] = tmp;
                //RenderVertices[index] = tmp;
            }


            node.World = Matrix.Identity;
            node.Rotate = Vector3.Zero;
            node.Translate = Vector3.Zero;
            node.Scale = Vector3.One;
            node.RotQuaternion = Quaternion.Identity;
        
        }
        /// <summary>
        /// Setting the Position for Vertice, update the RenderVertices
        /// </summary>
        /// <param name="position"></param>
        /// <param name="index"></param>
        public void SetPosition(Vector3 position, int index)
        {

                Vector3 normal = Vertices[index].Normal;
                Vector2 uv = Vertices[index].TextureCoordinate;
                VertexPositionNormalTexture tmp = new VertexPositionNormalTexture
                {
                    Position = position,
                    Normal = normal,
                    TextureCoordinate = uv
                };
                Vertices[index] = tmp;
                //RenderVertices[index] = tmp;
                VertexBuffer.SetData(Vertices.ToArray(),
                    0, Vertices.Count, SetDataOptions.Discard);
        }


        /*
        void SetBones(Matrix[] bones)
        {
            // skin all of the vertices
            for (int i = 0; i < _vertexCount; i++)
            {
                AniVertexData[] vertices =
                    Vertice as AniVertexData[];
                if (vertices != null)
                {
                    CpuSkinningHelpers.SetVertex(
                        bones,
                        ref vertices[i].Position,
                        ref vertices[i].Normal,
                        ref vertices[i].BlendIndices,
                        ref vertices[i].BlendWeights,
                        out Vertices[i].Position,
                        out Vertices[i].Normal);
                }
                else
                {
                    Vertices[i].Position = Vertice[i].Position;
                    Vertices[i].Normal = Vertice[i].Normal;
                }
            }

            // put the vertices into our vertex buffer
            VertexBuffer.SetData(Vertices, 0, _vertexCount, 
                SetDataOptions.Discard);
        }*/
        //public void UpdateVertice(Matrix transform)
        //{
        //   for (int i = 0; i < Vertices.Count; i++)
        //    {
        //        Vertices.ToArray()[i].Position = 
        //            Vector3.Transform(Vertices.ToArray()[i].Position, transform);
        //        Vertices.ToArray()[i].Normal = 
        //            Vector3.Transform(Vertices.ToArray()[i].Normal, transform);
        //    }
        
        //}

        public void UpdateShapeNod(GraphicsDevice g)
        {
            if (this.IsIntialized == false)
                this.InitializeVertice(g);
            this.IsIntialized = true;
        }

        /// <summary>
        /// Initialization new RenderVertices
        /// </summary>
        /// <param name="g"></param>
        private void InitializeVertice(GraphicsDevice g)
        {

           if(_graphic==null)
            _graphic = g;
            
            //RenderVertices.RemoveAll(
            //   delegate(VertexPositionNormalTexture matcher)
            //   {
            //       return true;
            //   });

            //for (int i = 0; i < Vertices.Count; i++)
            //{
            //    VertexPositionNormalTexture tmp = new VertexPositionNormalTexture();

            //    tmp.Position = Vertices.ToArray()[i].Position;
            //    tmp.Normal = Vertices.ToArray()[i].Normal;
                
            //    RenderVertices.Add(tmp);
            //}

           //RenderVertices = Vertices;

            this._indexBuffer = new IndexBuffer(_graphic, typeof(ushort),_indices.Count, BufferUsage.None);
            VertexBuffer = new DynamicVertexBuffer(_indexBuffer.GraphicsDevice,
                typeof(VertexPositionNormalTexture), this.Vertices.Count, BufferUsage.WriteOnly);
            _indexBuffer.SetData(this._indices.ToArray());
            VertexBuffer.SetData(Vertices.ToArray(), 0, Vertices.Count, SetDataOptions.Discard);
        }



        public void Draw(GameTime gameTime,ICamera cam, BasicEffect basicEffect, Matrix world)
        {
            if(PreDrawHandler!=null)
            PreDrawHandler.Invoke();

            if (this._indices.Count == 0)
                return;

            GraphicsDevice graphic = basicEffect.GraphicsDevice;
            graphic.Indices = this._indexBuffer;
            graphic.SetVertexBuffer(VertexBuffer);

            basicEffect.World = world;//Matrix.Identity;
            basicEffect.Projection = cam.ProjectionMatrix;
            basicEffect.View = cam.ViewMatrix;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                    pass.Apply();

                    graphic.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, Vertices.Count, 0, this._indexBuffer.IndexCount/3);
            }

            graphic.Indices = null;
            graphic.SetVertexBuffer(null);
 
        }

    }

}
