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
    public class DrawableComponent : UpdatableComponent,IDrawComponent
    {
        protected GraphicsDevice _device;
        protected ContentManager _contentManager;
        protected ICam _camera;
        public ICam IDrawComponent.Camera
        {
            set { this._camera = value; }
        }
        public DrawableComponent(IGameData game):base(game)
        {
            this._device=game.GraphicsDevice;
            _contentManager = game.ContentManager;
        }
        public override void Initialize()
        {
            this.LoadContent();
            base.Initialize();
        }
        public virtual void LoadContent()
        { }
        public virtual void Draw(GameTime gameTime)
        { }
    }
}
