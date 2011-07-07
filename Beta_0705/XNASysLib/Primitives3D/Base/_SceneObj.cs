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
#endregion

namespace XNASysLib.Primitives3D
{
    public class SceneObj : ScenePrimitive, ISceneObj, IMesh,IHierachy
    {
        protected IHierachy _parent;
        protected int _index;
        protected Model _model;


        protected static List<ISceneObj> ObjList;
        public Dictionary<string, object> TagData
        {
            get
            {
                if (_model == null)
                    return null;

                Dictionary<string, object> _tag
                    = (Dictionary<string, object>)this._model.Tag;
                if (_tag == null)
                {
                    throw new InvalidOperationException(
                        "Model.Tag is not set correctly. Make sure your model " +
                        "was built using the custom TrianglePickingProcessor.");
                }
                return _tag;
            }
        }
        public IHierachy Root
        {
            get
            {
                if (this._model != null)
                    return this;
                else
                    return this.Parent.Root;
            }
        }
        public int Index
        {
            get { return this._index; }
            set { _index = value; }
        }
        public IHierachy Parent
        {
            get { return this._parent; }
            set { _parent = value; }
        }
        public IHierachy HierachyNod
        {
            get { return this; }
            set 
            {
                this.Index = value.Index;
                this.Parent = value.Parent;
                this.Children = value.Children;
            }
        }
        public ITransformNode TransformNod
        { 
            get { return this;}
            set 
            {
                this.World = value.World;
                this.Translate = value.Translate;
                this.Rotate = value.Rotate;
                this.Pivot = value.Pivot;
                this.Scale = value.Scale;
                this.ID = value.ID;
            }
        }
        public IMesh ShapeNod
        {
            get { return this; }
            set 
            {
                this.Mesh = value.Mesh;
            }
        }

        ModelMesh _mesh;
        public ModelMesh Mesh
        {
            get { return _mesh; }
            set { _mesh = value; }
        }

        protected HierachyObjs<IHierachy> _children;
        public HierachyObjs<IHierachy> Children
        {
            get { return this._children; }
            set { this._children = value; }
        }

        public SceneObj(IGame game, string assetNm) :
            base(game)
        {
            this._AssetNm = assetNm;
            this._lockSel = false;
            if (ObjList == null)
                ObjList = new List<ISceneObj>();
            this._selCompData.BoundingSpheres = new BoundingSphere[1];
        }


        public override void Initialize()
        {
            
            this._contentBuilder = (ContentBuilder)_game.Services.
              GetService(typeof(ContentBuilder));

            _contentManager = (MyContentManager)_game.Services.
                GetService(typeof(MyContentManager));
            string tmp = Path.GetFileNameWithoutExtension(_AssetNm);
            _game.Components.GetNm(tmp, out _ID);
          
            
            base.Initialize();
        }

        

        void SetHierachy
            (ModelBoneCollection collection,
            SceneObj parent)
        {
            foreach (ModelBone bone in collection)
            {
                SceneObj obj = new SceneObj(_game, string.Empty);
                obj.Index = bone.Index;
                obj.ID = bone.Name;
                obj.World = bone.Transform;
                obj.Parent = parent;
                
                if (parent.Children == null)
                    parent.Children = 
                        new HierachyObjs<IHierachy>();
                parent.Children.Add(obj);
                SetMesh(obj);
                ObjList.Add(obj);

                //Set recursively
                SetHierachy(bone.Children, obj);
                
            }
        }

        void SetMesh(SceneObj obj)
        {
            foreach (ModelMesh mesh in _model.Meshes)
            {
                if (mesh.ParentBone.Index == obj.Index)
                    obj.Mesh = mesh;
                else
                    new NullReferenceException();
            }   
        }
        protected override void LoadContent()
        {

            if (this.TransformNod.ID != null
                && this.ShapeNod.Mesh != null
                && this.HierachyNod.Parent != null)
            {

                ProcessModelBounding();
                return;
            }

            string assetNm = Path.GetFileNameWithoutExtension(_AssetNm);

            this._contentBuilder.Add(this._AssetNm, assetNm,
                null, "VertexProcessor");

            string error = _contentBuilder.Build();
            if (string.IsNullOrEmpty(error))
            {
                _model = _contentManager.Load<Model>(assetNm);
                // Look up our custom collision data from the Tag property of the model.
                Dictionary<string, object> tagData = (Dictionary<string, object>)_model.Tag;

                if (tagData == null)
                {
                    throw new InvalidOperationException(
                        "Model.Tag is not set correctly. Make sure your model " +
                        "was built using the custom processor.");
                }
            }
            else
            {
                throw new InvalidOperationException
                ("Model Error" + error);
            }

            SetHierachy(_model.Bones, this);
            

            _selCompData.dataModifitionHandler.Invoke();
            //Skip base LoadContent
            //base.LoadContent();
        }
        protected virtual void ProcessModelBounding()
        {

            if (Mesh == null)
                return;
            


            BoundingSphere meshBound =(BoundingSphere)
                ((SceneObj)Root).TagData["BoundingSphere"];

            
            meshBound.Center = Vector3.
                Transform(meshBound.Center, World);

            this._selCompData.BoundingSpheres[0] = meshBound;
            

        }

        protected override void OnModify()
        {
            FlattenWorldFromParentToChild(ref this._world, this);

            base.OnModify();
            
        }
        protected override bool CheckOnRoll(ISelectable obj)
        {

            return base.CheckOnRoll(obj);
        }
        protected void FlattenWorldFromParentToChild
            (ref Matrix parentWorld, SceneObj sceneObj)
        {
            sceneObj.World=parentWorld*
            TransformHelper.RotInObjSpace
            (sceneObj.Rotate,sceneObj.Pivot,
             sceneObj.Translate, sceneObj.Scale);

            Matrix tmp = sceneObj.World;

            if (sceneObj.Children != null)
                for (int i = 0; i < sceneObj.Children.Count; i++)
                {
                    SceneObj child = (SceneObj)sceneObj.Children[i];
                    FlattenWorldFromParentToChild(ref tmp, child);
                }

        }
        public override void Update(GameTime gameTime)
        {
            //TempCode 1012
            if (!_isInitialized)
                this.Initialize();

            if (!this._lockSel&&this.Mesh!=null)
                base.Update(gameTime);

            if(this._children!=null)
            foreach (IHierachy child in this._children)
            {
                SceneObj obj = (SceneObj)child;
                obj.Update(gameTime);
            }

        }

        public override void Draw(GameTime gameTime)
        {

            if (_model == null || !_selCompData.IsInCam || !_selCompData.IsVisible)

                return;


            //PreRender Setting
            Color backColor = new Color(Color.Black.R, Color.Black.G, Color.Black.B);
            _game.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            _game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Matrix view = _camera.ViewMatrix;
            Matrix projection = _camera.ProjectionMatrix;


            foreach (SceneObj obj in ObjList)
            {
                foreach (BasicEffect effect in obj.Mesh.Effects)
                {
                    effect.World = _world;
                    effect.View = view;
                    effect.Projection = projection;

                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.SpecularPower = 16;
                }
                obj.Mesh.Draw();
            }

            if (_rasterizerState.FillMode == FillMode.WireFrame)
            {
                _game.GraphicsDevice.BlendState = BlendState.Additive;
                _game.GraphicsDevice.RasterizerState = _rasterizerState;

                foreach (SceneObj obj in ObjList)
                {
                    foreach (BasicEffect effect in obj.Mesh.Effects)
                    {
                        effect.World = _world;
                        effect.View = view;
                        effect.Projection = projection;

                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;
                        effect.SpecularPower = 16;
                    }
                    obj.Mesh.Draw();
                }



            }

            _game.GraphicsDevice.BlendState = BlendState.Opaque;
            _game.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            // base.Draw(gameTime);
        }




    }
}

