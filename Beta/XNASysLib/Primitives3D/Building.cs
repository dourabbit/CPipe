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
    public class Building : SceneNodHierachyModel
    {
        
        public override bool KeepSel
        {
            get
            {
                return base.KeepSel;
            }
            set
            {


                foreach (INode node in FlattenNods)
                {
                    SceneNodHierachyModel nodModel = node as SceneNodHierachyModel;
                    if (nodModel != null&&nodModel!=this)
                        nodModel.KeepSel = value;

                }

                base.KeepSel = value;
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
               base.TranslateX= value;
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
        public Building(IGame game)
            : base(game)
        { 
        
        }
    
    
    }

}