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
    public class HoleRect : SceneNodHierachyModel
    {
        int _length;
        [MyShowProperty]
        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }

        int _width;
        [MyShowProperty]
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        int _height;
        [MyShowProperty]
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        int _color;
        [MyShowProperty]
        public int Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public HoleRect(IGame game)
            : base(game)
        { 
        
        }
    
    
    }

}