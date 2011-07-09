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
    /// <summary>
    /// Obsolete
    /// </summary>
    public class ScenePreloadedObj : SceneXNAObj
    {
        protected Matrix[] _ObjSpaceTransforms;
       

        public ScenePreloadedObj(IGame game, string assetNm)
            : base(game)
        {
            _AssetNm = assetNm;
            this._keepSel = false;
        }
        protected override void OnModify()
        {

            this._selCompData.BoundingSpheres =
                new BoundingSphere[1];

            BoundingSphere Bsphere =
                (BoundingSphere)TagData["BoundingSphere"];

            //this.TransformNode.UpdateTransform();
            /*
            _world=
            TransformHelper.RotInObjSpace
            (this._rotation,this._pivot,
             this._translation,this._scale, ref this._rotQuaternion);
            */
            float scale = TransformNode.Scale.X > TransformNode.Scale.Y ? 
                TransformNode.Scale.X : TransformNode.Scale.Y;
            scale = scale > TransformNode.Scale.Z ? 
                scale : TransformNode.Scale.Z;


            Bsphere.Radius *= scale;

            Bsphere.Center =
                Vector3.Transform(Bsphere.Center, TransformNode.World);

            this._selCompData.BoundingSpheres[0] = Bsphere;
            
        }
        public override void Initialize()
        {
            _contentManager = (MyContentManager)_game.Services.
                GetService(typeof(MyContentManager));


            string a = Path.GetFileNameWithoutExtension(_AssetNm);
            
            _game.Components.GetNm(a, out _ID);
            

            base.Initialize();
        }
        protected override void LoadContent()
        {
            string assetNm = Path.GetFileNameWithoutExtension(_AssetNm);
            this._model = _contentManager.Load<Model>(assetNm);
            // Look up our custom collision data from the Tag property of the model.
            Dictionary<string, object> tagData = (Dictionary<string, object>)_model.Tag;

            if (tagData == null)
            {
                throw new InvalidOperationException(
                        "Model.Tag is not set correctly. Make sure your model " +
                        "was built using the custom processor.");
            }
            ProcessModel();
        }

        protected virtual void ProcessModel()
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

            //this._translation = this._modelCenter;
            
            /*_boneTransforms[0].Decompose(out _scale,out _rotQuaternion,out _translation);
            for (int i = 0; i < _boneTransforms.Length; i++)
            {
                _boneTransforms[i] = Matrix.Identity;
            }*/

            this._selCompData.dataModifitionHandler.Invoke();
        }
        
        

        public override void Update(GameTime gameTime)
        {
            //TempCode 1012
            if (!_isInitialized)
                this.Initialize();

            if (!this._keepSel)
                base.Update(gameTime);


            //else
               // SelectFunction.Select(this);

        }
        public override void Draw(GameTime gameTime,ICamera cam)
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
            
            foreach (ModelMesh mesh in _model.Meshes)
            {    
                foreach (BasicEffect effect in mesh.Effects)
                       {
                           // _world=
                          // TransformHelper.RotInObjSpace
                            //   (_rotation, this._pivot,_translation, _scale,
                             //  ref _rotQuaternion);

                            effect.World = //_world; 
                                _ObjSpaceTransforms[mesh.ParentBone.Index]*this.TransformNode.World;
                            effect.View = view;
                            effect.Projection = projection;
        
                            effect.EnableDefaultLighting();
                            effect.PreferPerPixelLighting = true;
                            effect.SpecularPower = 16;
                       }


                       mesh.Draw();
                    #region Old ModelMeshPart Draw Custom effect

                    /* foreach (ModelMeshPart mp in mesh.MeshParts)
                    {

                        if (mp.Effect is BasicEffect)
                        {
                            BasicEffect effect = (BasicEffect)mp.Effect;
                            effect.World = _boneTransforms[mesh.ParentBone.Index] * world;
                            effect.View = view;
                            effect.Projection = projection;
                            effect.GraphicsDevice.DepthStencilState 
                                = DepthStencilState.Default;
                            effect.EnableDefaultLighting();
                            effect.PreferPerPixelLighting = true;
                            effect.SpecularPower = 16;

                        }
                        else
                        {
                            Effect myEff = mp.Effect;
                            if (myEff.CurrentTechnique.Name == "trans")
                            {
                                myEff.Parameters["WorldViewProjection"].SetValue(world * view * projection);

                            }
                            else
                            {

                                myEff.Parameters["WorldITXf"].SetValue(Matrix.Transpose(Matrix.Invert(_boneTransforms[mesh.ParentBone.Index] * world)));
                                myEff.Parameters["WvpXf"].SetValue(Matrix.Multiply(_boneTransforms[mesh.ParentBone.Index] * world, Matrix.Multiply(view, projection)));
                                myEff.Parameters["WorldXf"].SetValue(_boneTransforms[mesh.ParentBone.Index] * world);
                                myEff.Parameters["ViewIXf"].SetValue(Matrix.Invert(view));
                                myEff.Parameters["gLamp0DirPos"].SetValue(new Vector4(18.3f, 73f, 332f, 1f));

                            }
                        }*/
                    
                    #endregion
               }


               

                if (_rasterizerState.FillMode == FillMode.WireFrame)
                {
                    _game.GraphicsDevice.BlendState = BlendState.Additive;
                    _game.GraphicsDevice.RasterizerState = _rasterizerState;
                   
                       foreach (ModelMesh mesh in _model.Meshes)
                        {
                            foreach (BasicEffect effect in mesh.Effects)
                            {
                              //  _world=
                              //  TransformHelper.RotInObjSpace
                             // (_rotation, _pivot, _translation, _scale,
                              //ref _rotQuaternion);


                                effect.World = //_world;
                                   _ObjSpaceTransforms[mesh.ParentBone.Index] * this.TransformNode.World;
                                effect.View = view;
                                effect.Projection = projection;

                                effect.EnableDefaultLighting();
                                effect.PreferPerPixelLighting = true;
                                effect.SpecularPower = 16;
                            }


                            mesh.Draw();
                        }
                }

                _game.GraphicsDevice.BlendState = BlendState.Opaque;
                _game.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
               // base.Draw(gameTime);
       }



        }
    }

