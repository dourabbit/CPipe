
#region File Description
//-----------------------------------------------------------------------------
// BloomComponent.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion
#define _DEBUG
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
        [MyShowProperty]
        public override string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                base.Name = value;
            }
        }

        [MyShowProperty]
        public override float TranslateX
        {
            get
            {
                return base.TranslateX;
            }
            set
            {
                base.TranslateX = value;
            }
        }
        [MyShowProperty]
        public override float TranslateY
        {
            get
            {
                return base.TranslateY;
            }
            set
            {
                base.TranslateY = value;
            }
        }
        [MyShowProperty]
        public override float TranslateZ
        {
            get
            {
                return base.TranslateZ;
            }
            set
            {
                base.TranslateZ = value;
            }
        }
        [MyShowProperty]
        public override float RotationX
        {
            get
            {
                return base.RotationX;
            }
            set
            {
                base.RotationX = value;
            }
        }
        [MyShowProperty]
        public override float RotationY
        {
            get
            {
                return base.RotationY;
            }
            set
            {
                base.RotationY = value;
            }
        }
        [MyShowProperty]
        public override float RotationZ
        {
            get
            {
                return base.RotationZ;
            }
            set
            {
                base.RotationZ = value;
            }
        }

        [MyShowProperty]
        public virtual string ObjColor
        {
            get { return _color.ToString(); }
            set
            {
                //Color col = new Microsoft.Xna.Framework.Color(

                MyConsole.WriteLine(value);

                int r = Int32.Parse(value.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                int g = Int32.Parse(value.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                int b = Int32.Parse(value.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

                this._color = new Color(r, g, b);



            }
        }
        
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
            _hub = new SceneHub(_game, font);
            if (_ID == null)
                _ID = "none";
            _game.Components.GetNm(_ID, out _ID);
            base.Initialize();

            OnUpdate();
            if (this.ShapeNode != null)
                this.ShapeNode.ShapeNodeChangingHandler += this.OnShapeNodeChange;
            this.TransformNode.DataModifiedHandler += OnTransformDataChange;
            this.TransformNode.DataModifiedHandler.Invoke();
            _dataSlotIndex = DataReactor.GetDataSlot(typeof(ObjData));
        }
        public new object GetCopy()
        {
            return this.MemberwiseClone();
        }
        public virtual void OnShapeNodeChange()
        {
            MyConsole.WriteLine(this.NodeNm+this.TransformNode.AbsoluteTransform.ToString());
            
        }
        public virtual void OnTransformDataChange()
        {


            OnUpdate();
            OnModifyBoundingSpheres();
            this.ObjDataGen= new ObjData(this,_dataSlotIndex);
        }
        /// <summary>
        /// Update TransformNode hierachily
        /// </summary>
        public virtual void OnUpdate()
        {
            if (ShapeNode != null) //if the root node has shape info
            {
                if (ShapeNode.ShapeNodeChangingHandler != null)
                    ShapeNode.ShapeNodeChangingHandler.Invoke();

                this.ShapeNode.UpdateShapeNod(_game.GraphicsDevice);
            }

            if (this.Parent != null)
            {
                this.TransformNode.AbsoluteTransform =
                    this.TransformNode.World*
                    ((SceneNodHierachyModel)this.Parent).
                        TransformNode.AbsoluteTransform;
                MyConsole.WriteLine(this.ID+this.TransformNode.AbsoluteTransform.ToString());
              
            }
            else //No parent means it is the root node.
            {
                this.TransformNode.AbsoluteTransform = this.TransformNode.World;

            }

            foreach (SceneNodHierachyModel curNod in this.Children)
            {
                curNod.OnUpdate();
            }
        }
        protected override void OnModifyBoundingSpheres()
        {
            if (ShapeNode != null)
            {
                this._selCompData.BoundingSpheres =
                    new BoundingSphere[1];

                float scale = this.TransformNode.Scale.X;
                scale = (scale < this.TransformNode.Scale.Y) ? this.TransformNode.Scale.Y : scale;
                scale = (scale < this.TransformNode.Scale.Z) ? this.TransformNode.Scale.Z : scale;

                this._selCompData.BoundingSpheres[0] = ShapeNode.BoundingSpheres[0];

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

                //}

                this._selCompData.shape = ShapeNode;
                this._selCompData.transform = TransformNode;
                this._selCompData.BoundingSpheres[0].Center =
                    Vector3.Transform(this._selCompData.BoundingSpheres[0].Center,
                                      this.TransformNode.AbsoluteTransform);
                this._selCompData.BoundingSpheres[0].Radius *= scale;
               
                MyConsole.WriteLine(this._selCompData.transform.AbsoluteTransform.ToString());
            }
            foreach (SceneNodHierachyModel curNod in this.Children)
            {
                curNod.OnModifyBoundingSpheres();
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

