﻿#region File Description
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
#endregion

namespace SysLib
{
    public interface IDrawComponent:IUpdatableComponent
    {
        ICam Camera
        { set; }

        void LoadContent();
        void Draw(GameTime gameTime);

    }
}
