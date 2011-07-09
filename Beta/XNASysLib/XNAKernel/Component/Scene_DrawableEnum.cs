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
#endregion

namespace XNASysLib.XNAKernel
{
    /* public partial class Scene:IEnumerator
     {


         struct DrawComponent
         {
             public string ID;
             public BoundingSphere[] BoundingSpheres;
         }
         protected int _index = -1;
         protected List<DrawComponent> _drawComponents;
        
         public bool MoveNext()
         {
             _index++;
             return _index < this._drawComponents.Count;
         }
         public void Reset()
         {
             this._index = -1;
         }
         public object Current
         {
             get 
             {
                 try
                 {
                     return _drawComponents[_index];
                 }
                 catch (IndexOutOfRangeException)
                 {
                     throw new InvalidOperationException
                         ("DrawableEum out of Range");
                 }
            
             }
         }
         public void OnChange()
         {
             this._drawComponents = new List<DrawComponent>();

             DrawComponent drawComp;

             foreach (IUpdatableComponent comp in this.Components)
                 if (comp is IDrawableComponent)
                 {
                     IDrawableComponent Dcomp= (IDrawableComponent)comp;
                     drawComp.ID = Dcomp.ID;
                     drawComp.BoundingSpheres = Dcomp.BoundingSpheres;
                     this._drawComponents.Add(drawComp);

                 }

         }
         public override void Update(GameTime gameTime)
         {   
            
             base.Update(gameTime);
         }
    
     }*/

}

