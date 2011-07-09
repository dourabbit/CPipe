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
    public class Chamber : SceneNodHierachyModel
    {
        //int _color;
        [MyShowProperty]
        public string Color
        {
            get { return _color.ToString(); }
            set
            {
                //Color col = new Microsoft.Xna.Framework.Color(

                MyConsole.WriteLine(value);

                int r = Int32.Parse(value.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                int g = Int32.Parse(value.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                int b = Int32.Parse(value.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

                this._color = new Color(r, g, b);



            }
        }
        public Chamber(IGame game)
            : base(game)
        { 
        
        }
    
    
    }

}