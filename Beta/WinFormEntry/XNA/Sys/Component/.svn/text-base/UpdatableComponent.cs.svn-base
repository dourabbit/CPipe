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
    public class UpdatableComponent : IUpdatableComponent
    {
        protected IGameData _game;
        public UpdatableComponent(IGameData game)
        {
            this._game = game;
            _game.Components.Add(this);
        }

        public virtual void Initialize()
        { 
            
        }
        public virtual void Update(GameTime gameTime)
        { }
    }
}
