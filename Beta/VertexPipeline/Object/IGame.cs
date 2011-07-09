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
using System.Collections.Generic;
using System.ComponentModel.Design;
#endregion

namespace VertexPipeline
{
    public interface IGame
    {
        GraphicsDevice GraphicsDevice { get; }
        MyContentManager MyContentManager { get; }
        GameTime GameTime { get; }
        GameComponents<IUpdatableComponent> Components { get; }
        ServiceContainer Services { get; }
        Rectangle ActiveViewRect { get; set; }
        Viewport ActiveViewport { get; set; }
    }
    
}
