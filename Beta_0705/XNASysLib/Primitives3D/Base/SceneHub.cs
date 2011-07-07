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
    public class SceneHub : DrawableComponent
    {
        protected SpriteFont _font;
        protected static SpriteBatch _sprite;
        public string Output { get; set; }


        /// <summary>
        /// Setting vector3f, converting the 3D vector to 2D position
        /// </summary>
        public Vector3 OutputPos3f
        {
            set
            {
                ICamera  cam=(ICamera)_game.Services.GetService(typeof(ICamera));
                Vector3 pos= this._game.ActiveViewport.Project(value,
                    cam.ProjectionMatrix,cam.ViewMatrix,Matrix.Identity);

                OutputPos = new Vector2(pos.X,pos.Y);
            }
        }
        public Vector2 OutputPos 
        { get; set; }


        public SceneHub(IGame game, SpriteFont font, SpriteBatch sprite)
            : base(game)
           
        {
            _sprite = sprite;
            _font = font;
        }

        public void Draw(GameTime gameTime, string output,Vector3 pos)
        {
            _sprite.Begin();
           
            this.Output = output;
            
            Vector2 offset= this._font.MeasureString(this.Output);
            this.OutputPos3f = pos;
            
            Vector2 outPos= new Vector2(
            OutputPos.X - offset.X / 2,
            OutputPos.Y - offset.Y);

            _sprite.DrawString(_font, Output, outPos, Color.Blue);


            _sprite.End();
        }

        public override void Draw(GameTime gameTime, ICamera cam)
        {
            _sprite.Begin();
            _sprite.DrawString(_font,Output,OutputPos,Color.Blue);    


            _sprite.End();
            
            base.Draw(gameTime, cam);
        }

   }
}

