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

    public class Valve : PipeBase
    {
        [MyShowProperty]
        public string Name
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
            }
        }

        [MyShowProperty]
        public override float TranslateX
        {
            get
            {
                return base.TranslateX;
            }
            set
            {
                base.TranslateX = value;
            }
        }
        [MyShowProperty]
        public override float TranslateY
        {
            get
            {
                return base.TranslateY;
            }
            set
            {
                base.TranslateY = value;
            }
        }
        [MyShowProperty]
        public override float TranslateZ
        {
            get
            {
                return base.TranslateZ;
            }
            set
            {
                base.TranslateZ = value;
            }
        }
        [MyShowProperty]
        public override float RotationX
        {
            get
            {
                return base.RotationX;
            }
            set
            {
                base.RotationX = value;
            }
        }
        [MyShowProperty]
        public override float RotationY
        {
            get
            {
                return base.RotationY;
            }
            set
            {
                base.RotationY = value;
            }
        }
        [MyShowProperty]
        public override float RotationZ
        {
            get
            {
                return base.RotationZ;
            }
            set
            {
                base.RotationZ = value;
            }
        }


        int _color;
        [MyShowProperty]
        public int Color
        {
            get { return _color; }
            set { _color = value; }
        }
        public Valve(IGame game)
            : base(game)
        {
        }

        protected override void OnModify()
        {
            base.OnModify();
        }
        public override void Initialize()
        {
            
            base.Initialize();

        }
       

        public override void Update(GameTime gameTime)
        {
            //TempCode 1012
            if (!_isInitialized)
                this.Initialize();
            
            
            
            base.Update(gameTime);

        }
        public override void Draw(GameTime gameTime,ICamera cam)
        {

            base.Draw(gameTime,cam);

        }
    }

}