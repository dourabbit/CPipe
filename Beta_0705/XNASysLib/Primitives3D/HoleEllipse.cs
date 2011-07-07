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
    public class HoleEllipse : SceneNodHierachyModel
    {

        int _a;
        [MyShowProperty]
        public int A
        {
            get { return _a; }
            set { _a = value; }
        }
        int _b;
        [MyShowProperty]
        public int B
        {
            get { return _b; }
            set { _b = value; }
        }
        int _c;
        [MyShowProperty]
        public int C
        {
            get { return _c; }
            set { _c = value; }
        }
        int _color;
        [MyShowProperty]
        public int Color
        {
            get { return _color; }
            set { _color = value; }
        }
        public HoleEllipse(IGame game)
            : base(game)
        { 
        
        }
    
    
    }

}