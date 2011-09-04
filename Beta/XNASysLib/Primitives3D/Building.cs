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
using VertexPipeline;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

#endregion

namespace XNASysLib.Primitives3D
{
    [Serializable]
    public class Building : SceneNodHierachyModel,ISerilization
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
      
        public Building(IGame game)
            : base(game)
        { 
        
        }
    
    
    }

}