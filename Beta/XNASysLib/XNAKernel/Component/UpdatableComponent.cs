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
using VertexPipeline;
#endregion

namespace XNASysLib.XNAKernel
{
    public class UpdatableComponent : IUpdatableComponent
    {
        protected IGame _game;
        protected bool _isInitialized = false;
        protected string _ID;
        public virtual string ID
        { 
            get { return _ID; }
            set { _ID = value; }
        }
        public IGame Game
        {
            get { return this._game; }
        }

        //Default constructor added only for serialization
        protected UpdatableComponent()
        { 
        
        }
        public UpdatableComponent(IGame game)
        {
            this._game = game;
           // _game.Components.Add(this);
        }
        ~UpdatableComponent()
        {
            Dispose(false);
        }
        public virtual void Initialize()
        {

            _isInitialized = true;  
        }
        public virtual void Update(GameTime gameTime)
        {
            if (!_isInitialized)
            {
                this.Initialize();
            }
        
        }
        public virtual void Dispose() 
        {
            this._game.Components.Remove(this);

            Dispose(true);
            GC.SuppressFinalize(this);
    
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            { 
                
            }
        }
    }
}
