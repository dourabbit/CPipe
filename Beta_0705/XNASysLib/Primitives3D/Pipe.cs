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
    public class Pipe :PipeBase
    {
               [MyShowProperty]
        public virtual string Name
        {
            get
            {

                return ((SceneNodHierachyModel)this.Root).NodeNm;
            }
            set
            {

                ((SceneNodHierachyModel)this.Root).NodeNm = value;
            }
        }



        [MyShowProperty]
        public override float TranslateX
        {
            get
            {
                object obj = this.Root;

                return ((SceneNodHierachyModel)this.Root).TranslateX;
            }
            set
            {

                ((SceneNodHierachyModel)Root).TranslateX = value;
            }
        }
        [MyShowProperty]
        public override float TranslateY
        {
            get
            {

                return ((SceneNodHierachyModel)this.Root).TranslateY;
            }
            set
            {
                ((SceneNodHierachyModel)Root).TranslateY = value;
            }
        }
        [MyShowProperty]
        public override float TranslateZ
        {
            get
            {

                return ((SceneNodHierachyModel)this.Root).TranslateZ;
            }
            set
            {

                ((SceneNodHierachyModel)this.Root).TranslateZ = value;
            }
        }
        [MyShowProperty]
        public override float RotationX
        {
            get
            {
                return ((SceneNodHierachyModel)this.Root).RotationX;
            }
            set
            {
                ((SceneNodHierachyModel)this.Root).RotationX = value;
            }
        }
        [MyShowProperty]
        public override float RotationY
        {
            get
            {
                return ((SceneNodHierachyModel)this.Root).RotationY;
            }
            set
            {
                ((SceneNodHierachyModel)this.Root).RotationY = value;
            }
        }
        [MyShowProperty]
        public override float RotationZ
        {
            get
            {
                return ((SceneNodHierachyModel)this.Root).RotationZ;
            }
            set
            {

                ((SceneNodHierachyModel)this.Root).RotationZ = value;
            }
        }

        [MyShowProperty]
        public virtual float Radius
        {
            get
            {
                return this.TransformNode.Scale.X;
                //return this._scale.X;
            }
            set
            {
                if (this.Root is SceneNodHierachyModel)
                {
                    SceneNodHierachyModel root = (SceneNodHierachyModel)this.Root;
                    foreach (SceneNodHierachyModel node in root.FlattenNods)
                    {
                        PipeBase pipe = node as PipeBase;
                        if (pipe != null)
                            pipe.TransformNode.Scale =
                                new Vector3(value, value, this.TransformNode.Scale.Z);
                    }
                }
                this._selCompData.
                    dataModifitionHandler.Invoke();
            }
        }

        int _material;
        [MyShowProperty]
        public int Material
        {
            get { return _material; }
            set { _material = value; }
        }

        //Color _color = Microsoft.Xna.Framework.Color.Bisque;
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

                this._color = new Color(r,g,b);


            
            }
        }


        public Pipe(IGame game)
            : base(game)
        { 
        
        }
    
    
    }

}