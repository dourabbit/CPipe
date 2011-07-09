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
#endregion

namespace XNASysLib.Primitives3D
{

    public class InstenceObj : SceneCompileObj
    {
        protected int _copiesOfModel=1;
        const float _offset = 2f;
        /// <summary>
        /// Matrix for subPipe
        /// </summary>
        protected Matrix[] _modelsTrans;
        
        void getTrans(float offset, Matrix parentMatrix, Quaternion rotQuaternion,
            ref Matrix[] transMatrixs, ref int i)
        {
            if (i == 0)
            {
                transMatrixs[0] = parentMatrix;
            }
            else
            {

                Vector3 offsetTrans = i* new Vector3(0, 0, offset);

                offsetTrans = Vector3.Transform(offsetTrans,rotQuaternion);
                Matrix world= Matrix.Identity;
                world *= Matrix.CreateTranslation(offsetTrans);



                transMatrixs[i] = parentMatrix * world;
            }
            i++;
            
            if (i > transMatrixs.Length - 1)
                return;
            
            getTrans(offset, parentMatrix,rotQuaternion,ref transMatrixs, ref i);

        }
        #region showProperties
       
        [MyShowProperty]
        public float Longth
        {
            get 
            {
                return _copiesOfModel;
            }

            set
            {
                _copiesOfModel = (int)Math.Round
                    ((decimal)value);
                _modelsTrans = new
                    Matrix[_copiesOfModel];


               // Matrix tmp = Matrix.Identity;
                /* int i = 0;
                 _modelsTrans[0] = _world;
                 getTrans(_offset, ref _world,
                     ref _modelsTrans, ref i);*/

                this._selCompData.
                    dataModifitionHandler.Invoke();
            }
        }

        [MyShowProperty]
        public float Radius
        {
            get 
            { 
                return this._scale.X; 
            }
            set
            {
                this._scale.X = value;
                this._scale.Y = value;
                this._selCompData.
                    dataModifitionHandler.Invoke();
            }
        }


    

        #endregion
        public InstenceObj(IGame game, string assetNm)
            : base(game, assetNm)
        {

        }
        protected override void OnModify()
        {
            this._selCompData.BoundingSpheres =
              new BoundingSphere[_copiesOfModel];

            BoundingSphere Bsphere =
                (BoundingSphere)TagData["BoundingSphere"];

            _world =
            TransformHelper.RotInObjSpace
            (this._rotation, this._pivot,
             this._translation, this._scale, ref this._rotQuaternion);

            int j = 0;



            getTrans(_offset, _world,_rotQuaternion,
                ref _modelsTrans, ref j);


            float scale = _scale.X > _scale.Y ? _scale.X : _scale.Y;
            scale = scale > _scale.Z ? scale : _scale.Z;


            Bsphere.Radius *= scale;

            Bsphere.Center =
                Vector3.Transform(Bsphere.Center, _world);

            this._selCompData.BoundingSpheres[0] = Bsphere;
            Vector3 pos;
            Vector3 scal;
            Quaternion rot;
            for(int i=0;i<_selCompData.BoundingSpheres.Length;i++)
            {

                _modelsTrans[i].Decompose(out scal, out rot, out pos);
                _selCompData.BoundingSpheres[i].Center = pos;
              //      = _selCompData.BoundingSpheres[0].Center
              //          + new Vector3(0,0,i*_offset);
              


                _selCompData.BoundingSpheres[i].Radius
                    = this._selCompData.BoundingSpheres[0].Radius;

            }

        }
        public override void Initialize()
        {

            base.Initialize();

        }
        protected override void LoadContent()
        {
           base.LoadContent();
        }

        protected override void ProcessModel()
        {
            _ObjSpaceTransforms = new Matrix[this._model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(_ObjSpaceTransforms);


            foreach (ModelMesh mesh in _model.Meshes)
            {
                BoundingSphere meshBounds = mesh.BoundingSphere;
                Matrix trans = _ObjSpaceTransforms[mesh.ParentBone.Index];
                Vector3 meshCenter = Vector3.Transform(meshBounds.Center, trans);
                _modelCenter += meshCenter;
            }
            _modelCenter /= _model.Meshes.Count;


            // Now we know the center point, we can compute the model radius
            // by examining the radius of each mesh bounding sphere.
            _modelRadius = 0;

            foreach (ModelMesh mesh in _model.Meshes)
            {
                BoundingSphere meshBounds = mesh.BoundingSphere;
                Matrix transform = _ObjSpaceTransforms[mesh.ParentBone.Index];
                Vector3 meshCenter = Vector3.Transform(meshBounds.Center, transform);

                float transformScale = transform.Forward.Length();

                float meshRadius = (meshCenter - _modelCenter).Length() +
                                   (meshBounds.Radius * transformScale);

                _modelRadius = Math.Max(_modelRadius, meshRadius);
            }

            this.Longth = 1;
        }
        
        

        public override void Update(GameTime gameTime)
        {
            //TempCode 1012
            if (!_isInitialized)
                this.Initialize();

            base.Update(gameTime);

        }
        public override void Draw(GameTime gameTime)
        {


            if (_model == null || !_selCompData.IsInCam || !_selCompData.IsVisible)

                return;



            // Clear to the default control background color.
            Color backColor = new Color(Color.Black.R, Color.Black.G, Color.Black.B);

            // GraphicsDevice.Clear(backColor);
            _game.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            _game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            Matrix view = _camera.ViewMatrix;

            Matrix projection = _camera.ProjectionMatrix;

            // Draw the model.
            foreach (Matrix worldMatrix in _modelsTrans)
            {
                foreach (ModelMesh mesh in _model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                       // _world =
                      // TransformHelper.RotInObjSpace
                        //   (_rotation, this._pivot, _translation, _scale,
                         //  ref _rotQuaternion);

                        effect.World = //_world; 
                            _ObjSpaceTransforms[mesh.ParentBone.Index] 
                            * worldMatrix;
                        effect.View = view;
                        effect.Projection = projection;

                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;
                        effect.SpecularPower = 16;
                    }


                    mesh.Draw();

                }

            }

            if (_rasterizerState.FillMode == FillMode.WireFrame)
            {
                _game.GraphicsDevice.BlendState = BlendState.Additive;
                _game.GraphicsDevice.RasterizerState = _rasterizerState;


                foreach (Matrix worldMatrix in _modelsTrans)
                {
                    foreach (ModelMesh mesh in _model.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                           // _world =
                          //  TransformHelper.RotInObjSpace
                         // (_rotation, _pivot, _translation, _scale,
                         // ref _rotQuaternion);


                            effect.World = //_world;
                               _ObjSpaceTransforms[mesh.ParentBone.Index]
                               *  worldMatrix;
                            effect.View = view;
                            effect.Projection = projection;

                            effect.EnableDefaultLighting();
                            effect.PreferPerPixelLighting = true;
                            effect.SpecularPower = 16;
                        }


                        mesh.Draw();
                    }
                }

            }

            _game.GraphicsDevice.BlendState = BlendState.Opaque;
            _game.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
        }



        }
    }

