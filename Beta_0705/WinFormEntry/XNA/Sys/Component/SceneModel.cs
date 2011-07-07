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
#endregion

namespace SysLib
{
    public class SceneModel : DrawableComponent
    {

        protected Model _model;
        protected Matrix[] _boneTransforms;
        protected Vector3 _modelCenter;

        public delegate void ModelChangedEvent(string assetNm);
        public ModelChangedEvent ModelChangedHander;


        public SceneModel(IGameData game)
            : base(game)
        { 
            
        
        }
        public override void LoadContent()
        {

            base.LoadContent();
        }

        protected virtual void ProcessModel()
        {
            _boneTransforms = new Matrix[this._model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(_boneTransforms);
            _modelCenter = Vector3.Zero;

            foreach (ModelMesh mesh in _model.Meshes)
            {
                BoundingSphere meshBounds = mesh.BoundingSphere;
                Matrix trans = _boneTransforms[mesh.ParentBone.Index];
                Vector3 meshCenter = Vector3.Transform(meshBounds.Center,trans);
                _modelCenter += meshCenter;
            }
            _modelCenter /= _model.Meshes.Count;
            
        }
    }
}
