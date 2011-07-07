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
using Microsoft.Xna.Framework.Input;
using XNASysLib.Primitives3D;
using VertexPipeline;
#endregion

namespace XNASysLib.XNATools
{

    public class ToolPrimitive : ScenePrimitive, IDraggable,ITool
    {
        protected OnDrag _dragHandler;
        protected IDrawableComponent _toolTarget;
        protected string _toolNm;
        public IDrawableComponent ToolTarget
        {
            get { return this._toolTarget; }
        }
        public virtual Vector3 DragVector
        {
            get 
            {
                if (this._toolTarget is ITransformNode)
                    return ((ITransformNode)_toolTarget).Pivot;

                else
                    return _toolTarget.World.Translation;
            }
        }
        public string ToolNm
        {
            get { return this._toolNm; }
        }
        public OnDrag DragHandler
        {
            set { this._dragHandler = value; }
            get { return _dragHandler; }
        }

        /// <summary>
        /// Test Code
        /// </summary>
        /// <param name="game"></param>
        public ToolPrimitive(IGame game)
            : base(game)
        {
            this._toolTarget = this;
        }
        public ToolPrimitive(IGame game, 
            IDrawableComponent target)
            :base(game)
        {
            this._toolTarget = target;
            
        }
        public override void Initialize()
        {
            //this._dragHandler += DragOnX;
            this._dataReactor = (DataReactor)_game.Services.
                GetService(typeof(DataReactor));
            base.Initialize();
        }

        public void DragOnX()
        {
            Point  scrPEnd;

           scrPEnd = this._dataReactor.MousePos.Value;

           
            Ray end = this._dataReactor.CalculateCursorRay
                (new Vector2(scrPEnd.X, scrPEnd.Y));

            float d = DragVector.Z;
            Plane movePlane = new Plane(Vector3.UnitZ, d);

            
            float? endDis = end.Intersects(movePlane);

            
            Vector3 endVec = end.Position + end.Direction * endDis.Value;

            this.TranslateX = endVec.X;

        }
        public void DragOnY()
        {
            Point  scrPEnd;

            scrPEnd = this._dataReactor.MousePos.Value;

            Ray end =this._dataReactor. CalculateCursorRay
                (new Vector2(scrPEnd.X, scrPEnd.Y));


            float d = DragVector.Z;
            Plane movePlane = new Plane(Vector3.UnitZ, d);

            float? endDis = end.Intersects(movePlane);

            Vector3 endVec = end.Position + end.Direction * endDis.Value;

            this.TranslateY = endVec.Y;
        }
        public void DragOnZ()
        {
            Point  scrPEnd;

            scrPEnd = this._dataReactor.MousePos.Value;

            Ray end = this._dataReactor.CalculateCursorRay
                (new Vector2(scrPEnd.X, scrPEnd.Y));

            float d = DragVector.Y;
            Plane movePlane = new Plane(Vector3.UnitY, d);

            
            float? endDis = end.Intersects(movePlane);

            
            Vector3 endVec = end.Position + end.Direction * endDis.Value;
            this.TranslateZ = endVec.Z;
        }


        public override void Update(GameTime gameTime)
        {
            if (!_isInitialized)
                this.Initialize();

            if (!_dataReactor.MousePos.HasValue)
            {
                base.Update(gameTime);
                return;
            }
            Vector2 mousePos = new Vector2(
                this._dataReactor.MousePos.Value.X,
                _dataReactor.MousePos.Value.Y);

            Ray r = _dataReactor.CalculateCursorRay(mousePos);

            this._distOfObj = _dataReactor.CalculateIntersection
                (r, this.Data);

            InteractiveObjState newState = this._oldstate;

            if (_distOfObj.HasValue &&
                _oldstate != InteractiveObjState.OnDrag &&
                _oldstate != InteractiveObjState.OnDragStart)
            {

                newState = InteractiveObjState.OnRollOver;


            }
            if (!_distOfObj.HasValue && _oldstate != InteractiveObjState.OnDrag)

                newState = InteractiveObjState.AwayFrom;

            //-------------------------------------------------OnRollOver

            if (//_oldState != ToolPartState.OnRollOver&&
                //_oldState != (ToolPartState.OnRollOver|ToolPartState.OnDrag)&&
                newState == InteractiveObjState.OnRollOver)
            {
                if (RollOverHandler != null)
                    RollOverHandler.Invoke();

                //if (_oldstate != InteractiveObjState.OnRollOver)
                  //  MyConsole.WriteLine(_ID +
                    //    "OnRollOver--------------------------");

                this._dataReactor.AddHotReg(this);

            }
            else if (_oldstate == InteractiveObjState.OnRollOver &&
                newState == InteractiveObjState.AwayFrom)
            {
                if (RollOutHandler != null)
                    RollOutHandler.Invoke();


               // MyConsole.WriteLine(_ID +
                   // "OnRollOut--------------------------");

                this._dataReactor.DelHotReg(this);
            }
            else if (newState == InteractiveObjState.AwayFrom)
            {

                this._dataReactor.DelHotReg(this);
            }

            //-----------------------------------------------IsDragging
            if (//_dataR.MouseData.IsMoving &&
                this._dataReactor.MouseData.Value.IsLeftBtnDown &&
                Keyboard.GetState().IsKeyUp(Keys.LeftAlt)//&&
                //_dataR.IsInHotReg(this)
               && newState == InteractiveObjState.OnRollOver &&
                _oldstate != InteractiveObjState.OnDragStart)
            {


                newState = InteractiveObjState.OnDragStart;
                this._selCompData.SelectionHandler.Invoke(this, true);
                //this._dragStart =
                //      DataReactor.GetDataReactor().
                //    MousePos.Value;
                //MyConsole.WriteLine(_ID + "IsDragging --------------------------Start"+
                  //  _oldstate.ToString());


            }
            if (Mouse.GetState().LeftButton == ButtonState.Released &&
                newState == InteractiveObjState.OnDrag &&
                Keyboard.GetState().IsKeyUp(Keys.LeftAlt))
            {

                //_dragStart = null;
                //_dragTransOffset = Vector3.Zero;
                newState = InteractiveObjState.OnDragEnd;
                this._selCompData.SelectionHandler.Invoke(this, false);
               // MyConsole.WriteLine(_ID + "IsDragging --------------------------End");
            }
            if (//_dataR.MouseData.IsMoving &&
                (_oldstate == InteractiveObjState.OnDragStart ||
                 _oldstate == InteractiveObjState.OnDrag) &&
                Mouse.GetState().LeftButton == ButtonState.Pressed)
            {

                newState = InteractiveObjState.OnDrag;
                //MyConsole.WriteLine(_ID + "IsDragging --------------------------Exe");
                this.DragHandler.Invoke();
            }
            this._oldstate = newState; 
           // base.Update(gameTime);
        }

    }
}
