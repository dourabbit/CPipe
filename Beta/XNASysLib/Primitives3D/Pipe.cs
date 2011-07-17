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
        public override string Name
        {
            get
            {

                //return ((SceneNodHierachyModel)this.Root).NodeNm;
                return this.NodeNm;
            }
            set
            {

                //((SceneNodHierachyModel)this.Root).NodeNm = value;
                this.NodeNm = value;
            }
        }



        //[MyShowProperty]
        //public override float TranslateX
        //{
        //    get
        //    {
        //        object obj = this.Root;

        //        return ((SceneNodHierachyModel)this.Root).TranslateX;
        //    }
        //    set
        //    {

        //        ((SceneNodHierachyModel)Root).TranslateX = value;
        //    }
        //}
        //[MyShowProperty]
        //public override float TranslateY
        //{
        //    get
        //    {

        //        return ((SceneNodHierachyModel)this.Root).TranslateY;
        //    }
        //    set
        //    {
        //        ((SceneNodHierachyModel)Root).TranslateY = value;
        //    }
        //}
        //[MyShowProperty]
        //public override float TranslateZ
        //{
        //    get
        //    {

        //        return ((SceneNodHierachyModel)this.Root).TranslateZ;
        //    }
        //    set
        //    {

        //        ((SceneNodHierachyModel)this.Root).TranslateZ = value;
        //    }
        //}
        //[MyShowProperty]
        //public override float RotationX
        //{
        //    get
        //    {
        //        return ((SceneNodHierachyModel)this.Root).RotationX;
        //    }
        //    set
        //    {
        //        ((SceneNodHierachyModel)this.Root).RotationX = value;
        //    }
        //}
        //[MyShowProperty]
        //public override float RotationY
        //{
        //    get
        //    {
        //        return ((SceneNodHierachyModel)this.Root).RotationY;
        //    }
        //    set
        //    {
        //        ((SceneNodHierachyModel)this.Root).RotationY = value;
        //    }
        //}
        //[MyShowProperty]
        //public override float RotationZ
        //{
        //    get
        //    {
        //        return ((SceneNodHierachyModel)this.Root).RotationZ;
        //    }
        //    set
        //    {

        //        ((SceneNodHierachyModel)this.Root).RotationZ = value;
        //    }
        //}


        [MyShowProperty]
        public override float TranslateX
        {
            get
            {
                //object obj = this.Root;

                //return ((SceneNodHierachyModel)this.Root).TranslateX;
                return base.TranslateX;
            }
            set
            {

                //((SceneNodHierachyModel)Root).TranslateX = value;
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

        string _material;
        [MyShowProperty]
        public string Material
        {
            get { return _material; }
            set { _material = value; }
        }

        //Color _color = Microsoft.Xna.Framework.Color.Bisque;
        [MyShowProperty]
        public override string ObjColor
        {
            get
            {
                return base.ObjColor;
            }
            set
            {
                base.ObjColor = value;
            }
        }


        public Pipe(IGame game)
            : base(game)
        { 
        
        }
    
    
    }

}