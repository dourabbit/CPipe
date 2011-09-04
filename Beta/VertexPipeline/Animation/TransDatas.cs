#region File Description
//-----------------------------------------------------------------------------
// SkinningData.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace VertexPipeline.Animation
{
    /// <summary>
    /// A serializable type, containning serval Lists, 
    /// like NameGrp; RelativeMatrixGrp; ParentIndex;
    /// to record the transform information from the FBX format.
    /// </summary>
    public class TransDatas
    {



        [ContentSerializer]
        public List<string> NameGrp { get; set; }


        [ContentSerializer]
        public List<Matrix> RelativeMatrixGrp { get; set; }

        /*
        [ContentSerializer]
        public List<int> TransIndex { get; set; }
        */
        [ContentSerializer]
        public List<int> ParentIndex { get; set; }
        //public Dictionary<string, Matrix> AnimationClips { get; private set; }

        public TransDatas()
        {
            NameGrp = new List<string>();
            RelativeMatrixGrp = new List<Matrix>();
            //TransIndex = new List<int>();
            ParentIndex = new List<int>();
        }
        /*

        public TransData(List<Matrix> relativeMat,List<int> transIndex, List<int> parentIndex,List<string> nameGrp)
        {
            RelativeMatrixGrp = relativeMat;
            //TransIndex = transIndex;
            ParentIndex = parentIndex;
            NameGrp = nameGrp;
        }*/
    }
}
