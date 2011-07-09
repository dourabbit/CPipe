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
    public class SceneXNAObj : ScenePrimitive
    {
        protected Model _model;
        protected Vector3 _modelCenter;
        protected SpriteFont _font;
        protected SpriteBatch _sprite;
        public Dictionary<string, object> TagData
        {
            get
            {
                Dictionary<string, object> tag
                    = (Dictionary<string, object>)this._model.Tag;
                if (tag == null)
                {
                    throw new InvalidOperationException(
                        "Model.Tag is not set correctly. Make sure your model " +
                        "was built using the custom TrianglePickingProcessor.");
                }
                return tag;
            }
        }
        public SceneXNAObj(IGame game)
            : base(game)
           
        {
            
        }
        
        
   }
}

