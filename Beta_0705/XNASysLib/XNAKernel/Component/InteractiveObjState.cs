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
#endregion

namespace XNASysLib.XNAKernel
{

    [Flags]
    public enum InteractiveObjState
    {
        OnRollOver = 0x01,
        AwayFrom = 0x02,
        OnDrag = 0x04,
        OnDragStart = 0x08,
        OnDragEnd = 0x10,
        OnClick = 0x20
    }
}
