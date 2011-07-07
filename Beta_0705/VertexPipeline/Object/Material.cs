#region File Description
//-----------------------------------------------------------------------------
// CpuSkinnedModel.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VertexPipeline
{

    public class Material : Effect
    {

        public Material(GraphicsDevice graphic,byte[] code)
            : base(graphic,code)
        { }
    }
    public class MaterialCollection : List<Material>
    {

    }

 

}
