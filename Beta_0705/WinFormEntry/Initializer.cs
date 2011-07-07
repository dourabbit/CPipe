#region File Description
//-----------------------------------------------------------------------------
// Program.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using XNASysLib.XNAKernel;
using XNABuilder;
using Microsoft.Xna.Framework.Content;
#endregion

namespace WinFormsContentLoading
{
    public class Initializer
    {
       // SysCollector _collector;
        //ContentBuilder contentBuilder;
        //MyContentManager contentManager;
        //SceneEntry _entry;
        public static Initializer Singleton;

        public Initializer()
        {
            /////////
            //
           // contentBuilder = new ContentBuilder();

            //contentBuilder.buildProject
            Singleton = this;
            //_collector = new SysCollector();
            


            ////////
           // _collector.Initialize();
        }
        
    
    
    }

}
