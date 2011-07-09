﻿#region File Description
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
#endregion

namespace XNASysLib.Primitives3D
{

    public class PipeBase : SceneNodHierachyModel
    {

        protected Vector3 _SideCenter=Vector3.Zero;
       // private Vector3 _SideBCenter;
        protected List<int> _SideBoundaryIndexer;
        //private List<int> _SideBIndexer;
        //private float _front = 1f;
        /*private float _rear = -1f;*/
       // private Vector3 _SideAObjSpace;
        //private Vector3 _SideBObjSpace;

        protected const float ZThreshold=3;



        


        /// <summary>
        /// SideBoundary in ObjSpace
        /// </summary>
        public Vector3 SideBoundary
        {
            get 
            {
                if (_SideCenter==Vector3.Zero)
                    GetBoundary(out _SideCenter);


                MyConsole.WriteLine("Get:" + _SideCenter.Z.ToString());
         
                return _SideCenter;
            }

            set
            {
                foreach (int i in _SideBoundaryIndexer)
                {
                    float x=
                    this.ShapeNode.Vertices[i].Position.X;
                    float y =
                    this.ShapeNode.Vertices[i].Position.Y;
                    //this.ShapeNode.GetPositions();
                    this.ShapeNode.SetPosition(
                        new Vector3(x,y,value.Z), i);

                    _SideCenter = Vector3.Zero;
                    MyConsole.WriteLine("Set:"+value.Z.ToString());
                }
                if (this.TransformNode.DataModifiedHandler!=null)
                this.TransformNode.DataModifiedHandler.Invoke();
            
            }
            
        }
     
        public PipeBase(IGame game)
            : base(game)
        {
            _SideBoundaryIndexer = new List<int>();
            
             _SideCenter=Vector3.Zero;
        }

        protected override void OnModify()
        {
            if (ShapeNode != null)
            {

                this._selCompData.BoundingSpheres =
                    new BoundingSphere[1];
                BoundingSphere bSphere =
                      ShapeNode.BoundingSpheres[0];
              
              
                ShapeNode.GetBounding(out bSphere.Center, out bSphere.Radius);
                bSphere.Center = Vector3.Transform(bSphere.Center,this.TransformNode.AbsoluteTransform);
                this._selCompData.BoundingSpheres[0] = bSphere;

                //if (_selCompData.BoundingSpheres[0].Radius == 0)//ShapeNode RenderData hasn't been initialized
                //{
                //    //Bsphere = null;
                //}
                //else
                //{

                //    float scale = TransformNode.Scale.X > TransformNode.Scale.Y ?
                //        TransformNode.Scale.X : TransformNode.Scale.Y;
                //    scale = scale > TransformNode.Scale.Z ?
                //        scale : TransformNode.Scale.Z;

                //    //Bsphere.Radius *= scale;

                //}


                
                this._selCompData.shape = ShapeNode;
                this._selCompData.transform = TransformNode;
            }
            foreach (SceneNodHierachyModel curNod in this.Children)
            {
                //curNod.OnModify();
            }

        }
        public override void Initialize()
        {
            
            base.Initialize();

        }
        protected override void LoadContent()
        {
            base.LoadContent();
            GetBoundary(out _SideCenter);
            /*
            this.MouseOfPipe[0] = new BoundingSphere(_SideCenter, 1f);
            this.MouseOfPipe[1] = new BoundingSphere(_SideBCenter, 1f);*/
        }


        void GetBoundary(out Vector3 SideA)
        {
            Vector3[] pos;

            this.ShapeNode.GetPositions(out pos);
            SideA = Vector3.Zero;
            if(_SideBoundaryIndexer.Count==0)
            {
                for (int i = 0; i < pos.Length; i++)
                {

                    if (pos[i].Z >= ZThreshold)
                        _SideBoundaryIndexer.Add(i);
                }
            }

            if (_SideBoundaryIndexer.Count == 0)
            {
              this.Parent.Children.Remove(this);
              SceneNodHierachyModel tmp = new SceneNodHierachyModel(_game);
              tmp.TransformNode = this.TransformNode;
              tmp.ShapeNode = this.ShapeNode;
              tmp.Children = this.Children;
              if(this.Parent!=null)
                tmp.Parent = this.Parent;
              tmp.Root = this.Root;
              if (this.Parent != null && this.Parent.Children != null)
                this.Parent.Children.Add(tmp);
              return;


            }
            foreach (int i in _SideBoundaryIndexer)
                SideA += pos[i];
            SideA /= _SideBoundaryIndexer.Count;
        }

        public override void Update(GameTime gameTime)
        {
            //TempCode 1012
            if (!_isInitialized)
                this.Initialize();
            
            
            
            base.Update(gameTime);

        }
        public override void Draw(GameTime gameTime,ICamera cam)
        {

            base.Draw(gameTime,cam);

        }
    }

}