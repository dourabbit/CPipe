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
    public delegate void AfterInitialized();
   
    public abstract class DrawableComponent : UpdatableComponent,IDrawableComponent,ITransObj
    {
        protected GraphicsDevice _device;
        protected MyContentManager _contentManager;
        protected ICamera _camera;
        protected BasicEffect _basicEffect;
        protected RasterizerState _rasterizerState;
        protected bool _isMuteTransform;
        public virtual bool  IsMuteTransform
        {
            get { return _isMuteTransform; }
        }
        public AfterInitialized AfterInitializedHandler;
       
        protected Color _color;
        public TransformNode TransformNode { get; set; }

        public DrawableComponent(IGame game):base(game)
        {
            _basicEffect = new BasicEffect(game.GraphicsDevice);
            _color = Color.AliceBlue;
            _basicEffect.LightingEnabled = true;
            _rasterizerState = RasterizerState.CullNone;

            this.TransformNode = new TransformNode();
            this._device=game.GraphicsDevice;
            _contentManager = game.MyContentManager;
        }
        public override void Initialize()
        {

            this._camera = (ICamera)_game.Services.GetService(typeof(ICamera));
            this.LoadContent();
            base.Initialize();

            if (this.AfterInitializedHandler != null)
                AfterInitializedHandler.Invoke();
        }
        protected virtual void LoadContent()
        { 
        
        }
        public override void Update(GameTime gameTime)
        {
            this._basicEffect.DiffuseColor = _color.ToVector3();
            //TransformNode.UpdateTransform();
            base.Update(gameTime);
        }
        public virtual void Draw(GameTime gameTime, ICamera cam)
        { 
        
        }
        public DrawableComponent GetCopy()
        {
            return 
                (DrawableComponent)this.MemberwiseClone();
        }
    }
}
