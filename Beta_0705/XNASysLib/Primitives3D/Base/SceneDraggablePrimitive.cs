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
using VertexPipeline;
#endregion

namespace XNASysLib.Primitives3D
{

    public class SceneDraggablePrimitive : ScenePrimitive, ISelectable//, IEnumerator<IDrawableComponent>, IEnumerable<IDrawableComponent>
    {
       // Vector2 _dragOffset;
        protected Point? _mouseIntialPos;
        protected Vector3 _primitiveIntialPos;
        protected OnDrag _OnDragStartHandler;
        protected OnDrag _OnDragEndHandler;
        protected OnDrag _OnDragExeHandler;

        protected SelectionCondition DragStartChecker;
        protected SelectionCondition DragEndChecker;
        protected SelectionCondition DragExeChecker;
        protected OnDrag _dragHandler;

        public InteractiveObjState State
        {
            get { return _curState; }
        }

        protected virtual Vector3 DragPosition
        {
            get
            {
                return this.TransformNode.Translate;
            }
        }

        public OnDrag OnDragStartHandler
        {
            get { return _OnDragStartHandler; }
            set { _OnDragStartHandler = value; }
        }
        public OnDrag OnDragEndHandler
        { get { return _OnDragEndHandler; }
            set { _OnDragEndHandler = value; }
        }
        public OnDrag OnDragExeHandler
        {
            get { return _OnDragExeHandler; }
            set { _OnDragExeHandler = value; }
        }
        public SceneDraggablePrimitive(IGame game)
            : base(game)
           
        {

            this.DragStartChecker +=checkOnDragStart;
            this.DragEndChecker += checkOnDragEnd;
            this.DragExeChecker += checkOnDragExe;

            this.OnDragStartHandler += OnDragStart;
            this.OnDragExeHandler += OnDragExe;
            this.OnDragEndHandler += OnDragEnd;


            this._dragHandler += DragOnScreen;
        }
       
        
        public override void Initialize()
        {

            
            base.Initialize();
        }


        protected override void LoadContent()
        {
            base.LoadContent();
        }



        public override void Update(GameTime gameTime)
        {
            //TempCode 1012
            if (!_isInitialized)
                this.Initialize();

            if (!_dataReactor.MousePos.HasValue)
            {
                base.Update(gameTime);
                return;
            }

            //RollOver Or RollOut
            
            if (RollOverChecker != null && RollOverChecker.Invoke(this))
            {


                    if (RollOverHandler != null)
                        this.RollOverHandler.Invoke();

            }
            else
            {
                    if (RollOutHandler != null)
                        this.RollOutHandler.Invoke();
            }
        
            //Drag Start
            if (DragStartChecker != null && DragStartChecker.Invoke(this))
            {

                this._curState = InteractiveObjState.OnDragStart;

            }

            //Drag End
            else if (DragEndChecker != null && DragEndChecker.Invoke(this))
            {
                this._curState = InteractiveObjState.OnDragEnd;

            }

            //Drag Exe
            else if (DragExeChecker != null && DragExeChecker.Invoke(this))
            {
                this._curState = InteractiveObjState.OnDrag;
            }

            switch (_curState)
            { 
                case InteractiveObjState.OnDrag:

                    if (this.OnDragExeHandler != null)
                        this.OnDragExeHandler.Invoke();
                    break;
                case InteractiveObjState.OnDragStart:

                    //MyConsole.WriteLine("DragStart");
                    if (this.OnDragStartHandler != null)
                        this.OnDragStartHandler.Invoke();
                    break;
                case InteractiveObjState.OnDragEnd:

                    if (this.OnDragEndHandler != null)
                        this.OnDragEndHandler.Invoke();
                    break;

                default:
                    break;
            }


            _oldstate=_curState;
            

        }
        #region DragXYZ
        public Vector3 castRay(Point mouse, Plane intersectPlane)
        {
            Ray end = this._dataReactor.CalculateCursorRay
                (new Vector2(mouse.X, mouse.Y));

            float? endDis = end.Intersects(intersectPlane);
            return  end.Position + end.Direction * endDis.Value;
        
        }
        /*
        public void DragOnX()
        {
            Plane interPlane=new Plane(Vector3.UnitZ, DragPosition.Z);

            Vector3 startVec = castRay(this._mouseIntialPos, interPlane);
            Vector3 endVec = castRay(_dataReactor.MousePos.Value, interPlane);

            float delta = endVec.X - startVec.X;
            this.TranslateX = this._primitiveIntialPos.X+delta;
        }*/

        public Plane getPlane(Ray camRay, Plane A, Plane B)
        {
           float? a= camRay.Intersects(A);
           float? b = camRay.Intersects(B);

           if (!a.HasValue && b.HasValue)
               return B;
           else if (!b.HasValue && a.HasValue)
               return A;
           else if (a.HasValue && b.HasValue)
           {
               Vector3 vaPos = camRay.Position +
                   camRay.Direction * a.Value;
               Vector3 vbPos = camRay.Position +
                   camRay.Direction * b.Value;

               //cam vector
               Vector3 vaCam = camRay.Position - vaPos;
               vaCam.Normalize();

               Vector3 vbCam = camRay.Position - vbPos;
               vbCam.Normalize();

               float theta1 = Vector3.Dot(vaCam, A.Normal);
               float theta2 = Vector3.Dot(vbCam, B.Normal);

               if (theta1 > theta2)
                   return A;
               else
                   return B;

           }


               new NullReferenceException();
               return new Plane();

    
        }
        public void DragOnScreen()
        {
            Vector2 mousePos = new Vector2
                (_dataReactor.MousePos.Value.X,
                    _dataReactor.MousePos.Value.Y);

            Vector3 initialPosInScreen=
            this._dataReactor.ProjectObjInScreenSpace(this._primitiveIntialPos);

            Vector3 camSpacePos= Vector3.Transform(this._primitiveIntialPos,_camera.ViewMatrix);
            
            Plane interPlane = new Plane(Vector3.UnitZ,-camSpacePos.Z);
            interPlane = Plane.Transform(interPlane,Matrix.Invert(_camera.ViewMatrix));


            Vector3 startVec = castRay(this._mouseIntialPos.Value, interPlane);
            Vector3 endVec = castRay(_dataReactor.MousePos.Value, interPlane);

            this.TransformNode.Translate = endVec;
           // MyConsole.WriteLine("Translating" + startVec.ToString() + ";" + -camSpacePos.Z);
        }
        public void DragOnX()
        {

            Vector2 mousePos= new Vector2
                (_dataReactor.MousePos.Value.X,
                    _dataReactor.MousePos.Value.Y);
           
            Plane interPlane =
                getPlane(this._dataReactor.CalculateCursorRay
                (mousePos),
                new Plane(Vector3.UnitZ, DragPosition.Z),
                new Plane(Vector3.UnitY, DragPosition.Y));

            Vector3 startVec = castRay(this._mouseIntialPos.Value, interPlane);
            Vector3 endVec = castRay(_dataReactor.MousePos.Value, interPlane);

            float delta = endVec.X - startVec.X;
            this.TranslateX = this._primitiveIntialPos.X + delta;
        }

        public void DragOnY()
        {
            Vector2 mousePos = new Vector2
                    (_dataReactor.MousePos.Value.X,
                        _dataReactor.MousePos.Value.Y);

            Plane interPlane =
                getPlane(this._dataReactor.CalculateCursorRay
                (mousePos),
                new Plane(Vector3.UnitZ, DragPosition.Z),
                new Plane(Vector3.UnitX, DragPosition.X));

            //Plane interPlane=new Plane(Vector3.UnitZ, DragPosition.Z);
            Vector3 startVec = castRay(this._mouseIntialPos.Value, interPlane);
            Vector3 endVec = castRay(_dataReactor.MousePos.Value, interPlane);

            float delta = endVec.Y - startVec.Y;
            this.TranslateY = this._primitiveIntialPos.Y+delta;
        }

        public void DragOnZ()
        {
            Vector2 mousePos = new Vector2
                (_dataReactor.MousePos.Value.X,
                    _dataReactor.MousePos.Value.Y);

            Plane interPlane =
                getPlane(this._dataReactor.CalculateCursorRay
                (mousePos),
                new Plane(Vector3.UnitX, DragPosition.X),
                new Plane(Vector3.UnitY, DragPosition.Y));

            //Plane interPlane = new Plane(Vector3.UnitY, DragPosition.Y);
            Vector3 startVec = castRay(this._mouseIntialPos.Value, interPlane);
            Vector3 endVec = castRay(_dataReactor.MousePos.Value, interPlane);

            float delta = endVec.Z - startVec.Z;
            this.TranslateZ = this._primitiveIntialPos.Z + delta;
        }

        /*
        public void DragOnY()
        {
            //Get Mouse Position From Screen 2D coordinate
            Point scrPEnd = this._dataReactor.MousePos.Value;

            //Unprojecting the viewport and getting the ray
            //pointing from the camera eye position, through the cursor. 
            Ray end = this._dataReactor.CalculateCursorRay
                (new Vector2(scrPEnd.X, scrPEnd.Y));


            //Get the XY plane of the object position
            Plane movePlane = new Plane(Vector3.UnitZ, DragPosition.Z);


            float? endDis = end.Intersects(movePlane);


            Vector3 endVec = end.Position + end.Direction * endDis.Value;

            this.TranslateY = endVec.Y;
        }
        public void DragOnZ()
        { 
            //Get Mouse Position From Screen 2D coordinate
            Point scrPEnd = this._dataReactor.MousePos.Value;

            //Unprojecting the viewport and getting the ray
            //pointing from the camera eye position, through the cursor. 
            Ray end = this._dataReactor.CalculateCursorRay
                (new Vector2(scrPEnd.X, scrPEnd.Y));


            //Get the XZ plane of the object position
            Plane movePlane = new Plane(Vector3.UnitY, DragPosition.Y);


            float? endDis = end.Intersects(movePlane);


            Vector3 endVec = end.Position + end.Direction * endDis.Value;
            
            this.TranslateZ = endVec.Z;

            MyConsole.WriteLine("DragZ"+endVec.Z);
        }
        */ 
	#endregion
        protected virtual void OnDragStart()
        {
            this._selCompData.SelectionHandler.Invoke(this, true);
            this._curState = InteractiveObjState.OnDragStart;


            //test
            /*
            Vector3 xy= Vector3.Transform(Vector3.Zero,
                Matrix.Multiply(Matrix.Multiply
                        (this.World,this._camera.ViewMatrix),
                            this._camera.ProjectionMatrix));
            */
            this._mouseIntialPos = _dataReactor.MousePos.Value;
            this._primitiveIntialPos = this.TransformNode.Translate;
         //   MyConsole.WriteLine(_ID + "DragStart");
        }
        protected virtual void OnDragEnd()
        {
            this._selCompData.SelectionHandler.Invoke(this, false);
            this._curState = InteractiveObjState.OnDragEnd;
            _mouseIntialPos = null;
            // MyConsole.WriteLine(_ID + "DragEnd");
        
        }
        protected virtual void OnDragExe()
        {
            this._dragHandler.Invoke();
            this._curState = InteractiveObjState.OnDrag;
          //  MyConsole.WriteLine(_ID+"DragExe");
        }

        protected override void OnRollOver()
        {
            base.OnRollOver();
        }

        protected override void OnRollOut()
        {
            
            base.OnRollOut();
        }
        protected override bool CheckOnRoll(ISelectable obj)
        {
           bool result=base.CheckOnRoll(obj);
          

           return result;
        }
        protected bool checkOnDragStart(ISelectable obj)
        {

            return
                 _dataReactor.MouseData.Value.IsLeftBtnDown &&
                 Keyboard.GetState().IsKeyUp(Keys.LeftAlt) &&
                 _curState == InteractiveObjState.OnRollOver &&
                 _oldstate != InteractiveObjState.OnDragStart;
        }
        protected bool checkOnDragEnd(ISelectable obj) 
        {
            return 
                Mouse.GetState().LeftButton == ButtonState.Released &&
                _oldstate == InteractiveObjState.OnDrag &&
                Keyboard.GetState().IsKeyUp(Keys.LeftAlt);
        }
        protected bool checkOnDragExe(ISelectable obj)
        {

            return
                (_oldstate == InteractiveObjState.OnDragStart ||
                 _oldstate == InteractiveObjState.OnDrag) &&
                Mouse.GetState().LeftButton == ButtonState.Pressed;
        }


        public override void Draw(GameTime gameTime,ICamera cam)
        {
            _game.GraphicsDevice.RasterizerState =
                RasterizerState.CullNone;
            //if (_rasterizerState.FillMode == FillMode.WireFrame)
            //{
            //    _color = Color.Yellow;
            //}
            //else
            //    _color = Color.Red;


            base.Draw(gameTime,cam);
       }



        }
    }

