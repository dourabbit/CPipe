
#region File Description
//-----------------------------------------------------------------------------
// ModelViewerControl.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;


#endregion

namespace VertexPipeline
{
    public class MyContentManager : ContentManager
    {
        public MyContentManager(IServiceProvider serviceProvider, string root)
            : base(serviceProvider,root)
        { }

        public override T Load<T>(string assetName)
        {
            return ReadAsset<T>(assetName, IgnoreDisposableAsset);
        }

        void IgnoreDisposableAsset(IDisposable disposable)
        {
        }
    }

}
