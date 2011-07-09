# define _DEBUG
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
using VertexPipeline;
using XNAPhysicsLib;
#endregion

namespace XNASysLib.Primitives3D
{


    public delegate bool SelectionCondition(ISelectable obj);

    public class ScenePrimitive : DrawableComponent, ISelectable
    {
#if _DEBUG

           DebugDraw _debugDraw;
           List<BoundingSphere> _hotRegs;
#endif

        protected float _modelRadius;
        
        //protected GraphicsDevice _device;
        protected InteractiveObjState _curState;
        protected InteractiveObjState _oldstate;
        protected SelectableCompData _selCompData;
        
        //protected GraphicsDevice GraphicsDevice;
        
        //protected Vector3 _modelCenter;
        protected ContentBuilder _contentBuilder;
        protected string _AssetNm;
        

        

        protected float? _distOfObj;
        protected bool _keepSel;
        protected bool _lock;
        protected DataReactor _dataReactor;

        
        public virtual bool KeepSel
        {
            set 
            { 
                this._keepSel = value;
                if (value)
                {
                    SelectFunction.Select(this);
                    if (SelectFunction.OnSelectionUpdate != null)
                        SelectFunction.OnSelectionUpdate.Invoke();
                }
            }
            get { return _keepSel; }
        }
        public virtual bool Lock
        {
            get
            {
                return _lock;
            }
            set
            {
                _lock = value;
            }
        }
        
        public SelectableCompData Data
        {
            get
            {
                return this._selCompData;
            }
        }
        public float? DistOfObj
        {
            get
            {
                return this._distOfObj;
            }
        }
        public OnRoll RollOutHandler
        { get; set; }

        public OnRoll RollOverHandler
        { get; set; }
        
        protected SelectionCondition RollOverChecker;
        protected SelectionCondition SelectionChecker;

        #region showProperties


        /*
        public virtual Matrix World
        {
            get {return base.TransformNode.World; }//return _world; }
            set 
            {
                TransformNode.World = value;
               // this._translation = _world.Translation;
                //this._selCompData.dataModifitionHandler.Invoke();
            }
        }
        public virtual Vector3 Translate
        {
            get { return base.TransformNode.Translate; }
            set 
            {
                base.TransformNode.Translate = value;

                this._selCompData.dataModifitionHandler.Invoke();
            }
        }
        public virtual Vector3 Scale
        {
            get { return base.TransformNode.Scale; }
            set { base.TransformNode.Scale = value; }
        }

        /// <summary>
        /// Pivot in ObjSpace
        /// </summary>
        public virtual Vector3 Pivot
        {
            get 
            { 
                return TransformNode.Pivot; 
            }
            set { base.TransformNode.Pivot = value; }
        }
        public virtual Vector3 Rotate
        {
            get { return base.TransformNode.Rotate; }
            set { base.TransformNode.Rotate = value; }
        }
        public virtual Quaternion Quaternion
        {
            get { return base.TransformNode.RotQuaternion; }
        }
        */

        //Vector3 _translation;
        //Vector3 _rotation;
        [MyShowProperty]
        public virtual float TranslateX
        {
            get 
            {

                return this.TransformNode.Translate.X; 
            }
            set
            {

                //_translation.X = value;
                base.TransformNode.Translate = new Vector3(
                    value,
                    TransformNode.Translate.Y,
                    TransformNode.Translate.Z
                    );//_translation;
                this._selCompData.dataModifitionHandler.Invoke();
            }
        }
        [MyShowProperty]
        public virtual float TranslateY
        {
            get 
            {
                
                return this.TransformNode.Translate.Y; 
            }
            set
            {
                //_translation.Y = value; 
                base.TransformNode.Translate = new Vector3(
                     TransformNode.Translate.X,
                     value,
                     TransformNode.Translate.Z
                     );
                this._selCompData.dataModifitionHandler.Invoke();
            }
        }
        [MyShowProperty]
        public virtual float TranslateZ
        {
            get
            {

                return this.TransformNode.Translate.Z; 
            }
            set
            {
                //_translation.Z = value; 
                base.TransformNode.Translate = new Vector3(
                     TransformNode.Translate.X,
                     TransformNode.Translate.Y,
                     value
                     );
                this._selCompData.dataModifitionHandler.Invoke();
            }
        }
        [MyShowProperty]
        public virtual float RotationX
        {
            get
            {
                return this.TransformNode.Rotate.X;
               //return _rotation.X;
            }
            set
            {
                //Vector3 localXAxis= this.TransformNode.Pivot.Right;
                //this.TransformNode.RotateInArbitaryAxis(localXAxis,value);

                Vector3 rotate = this.TransformNode.Rotate;
                rotate.X = value;
                
                this.TransformNode.RotateInObjSpace(rotate);
                this.TransformNode.Rotate = rotate;
                //this.TransformNode.Rotate = new Vector3
                //(
                //    value,
                //    this.TransformNode.Rotate.Y,
                //    this.TransformNode.Rotate.Z
                //);
                this._selCompData.dataModifitionHandler.Invoke();
            }
        }
        [MyShowProperty]
        public virtual float RotationY
        {
            get
            {
                return this.TransformNode.Rotate.Y;
                //return _rotation.Y;
            }
            set
            {
                //Vector3 localYAxis = this.TransformNode.Pivot.Up;
                //this.TransformNode.RotateInArbitaryAxis(localYAxis, value);
                Vector3 rotate = this.TransformNode.Rotate;
                rotate.Y = value;
                this.TransformNode.Rotate = rotate;
                this.TransformNode.RotateInObjSpace(rotate);
                //float deltaRot = value - this.TransformNode.Rotate.Y;
                //this.TransformNode.Rotate_worldSpace(
                //    Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(deltaRot)));

                //this.TransformNode.Rotate = new Vector3
                //(
                //    this.TransformNode.Rotate.X,
                //    value,
                //    this.TransformNode.Rotate.Z
                //);
                this._selCompData.dataModifitionHandler.Invoke();
            }
        }
        [MyShowProperty]
        public virtual float RotationZ
        {
            get
            {
                return this.TransformNode.Rotate.Z;
                //return _rotation.Z;
            }
            set
            {
                //Vector3 localZAxis = this.TransformNode.Pivot.Backward;
                //this.TransformNode.RotateInArbitaryAxis(localZAxis, value);
                Vector3 rotate = this.TransformNode.Rotate;
                rotate.Z = value;
                this.TransformNode.Rotate = rotate;
                this.TransformNode.RotateInObjSpace(rotate);
                //float deltaRot = value - this.TransformNode.Rotate.Z;
                //this.TransformNode.Rotate_worldSpace(
                //    Quaternion.CreateFromAxisAngle(Vector3.Backward, MathHelper.ToRadians(deltaRot)));

                //this.TransformNode.Rotate = new Vector3
                //(
                //    this.TransformNode.Rotate.X,
                //    this.TransformNode.Rotate.Y,
                //    value
                //);
                this._selCompData.dataModifitionHandler.Invoke();
            }
        }

        #endregion

        

        public ScenePrimitive(IGame game):base(game)
           
        {
            this._selCompData.IsVisible = true;
            //TempCode
            this._selCompData.IsInCam = true;
            this._lock = false;
            _dataReactor =
           (DataReactor)game.Services.
           GetService(typeof(DataReactor));

            this.RollOverHandler += OnRollOver;
            this.RollOutHandler += OnRollOut;

            this.RollOverChecker += CheckOnRoll;
            this.SelectionChecker += CheckSelection;
            
            this._selCompData.SelectionHandler += OnSelected;
            
            this._selCompData.dataModifitionHandler += OnModify;



        }
       
        
        public override void Initialize()
        {

            System.Reflection.MemberInfo method=
            this._selCompData.dataModifitionHandler.Method;

            this.TransformNode.DataModifiedHandler
                =
                this._selCompData.dataModifitionHandler;

            this._selCompData.dataModifitionHandler.Invoke();

            base.Initialize();
            
        }

        protected virtual void OnModify()
        {

            this._selCompData.BoundingSpheres =
                new BoundingSphere[1];

            this._selCompData.BoundingSpheres[0].Center
                = Vector3.Transform(base.TransformNode.Pivot.Translation, base.TransformNode.World);
            this._selCompData.BoundingSpheres[0].Radius
                = this._modelRadius + 0.01f;
            this.TransformNode.AbsoluteTransform = this.TransformNode.World;
            this._selCompData.transform= this.TransformNode;
            
        }
        protected virtual bool CheckSelection(ISelectable obj)
        {
            ////////////////////////////////////////////////////////
            //If LMB
            ////////////////////////////////////////////////////////
            if (this._curState ==///////////////////////////////////
                InteractiveObjState.OnRollOver &&///////////////////
                this._dataReactor.MouseData.Value.IsLeftBtnDown)////
                return true;////////////////////////////////////////
            ////////////////////////////////////////////////////////
            
            

            ////////////////////////////////////////////////////////
            //Continue selecting
            ////////////////////////////////////////////////////////
            if (this._curState ==///////////////////////////////////
                InteractiveObjState.OnRollOver &&///////////////////
                this._oldstate ==///////////////////////////////////
                    (InteractiveObjState.OnRollOver |///////////////
                        InteractiveObjState.OnClick))///////////////
                return true;////////////////////////////////////////
            ////////////////////////////////////////////////////////
            
            
            ////////////////////////////////////////////////////////
            //Continue Selecting, 
            ////////////////////////////////////////////////////////
            if (this._curState ==///////////////////////////////////
                InteractiveObjState.AwayFrom &&/////////////////////
                this._oldstate ==///////////////////////////////////
                    (InteractiveObjState.OnRollOver |///////////////
                       InteractiveObjState.OnClick) &&//////////////
                !this._dataReactor.MouseData.Value.IsLeftBtnDown)///
                return true;////////////////////////////////////////
            ////////////////////////////////////////////////////////

            return false;
        }
        protected virtual void OnSelected(ISelectable obj, bool isSelected)
        {
            if (isSelected)
            {

                SelectFunction.Select(obj);
                _rasterizerState = new RasterizerState()
                {
                    FillMode = FillMode.WireFrame,
                    CullMode = CullMode.None,
                };
                this._curState =
                    InteractiveObjState.OnClick |
                    InteractiveObjState.OnRollOver;
              
            }
            else
            {
                SelectFunction.DeSelect(obj);

                _rasterizerState = RasterizerState.CullNone;
                this._curState =
                    InteractiveObjState.AwayFrom;
            }

        
        }
        protected virtual bool CheckOnRoll(ISelectable obj)
        {
            if (_lock)
                return false;
            
            Vector2 mousePos = 
                new Vector2(this._dataReactor.MousePos.Value.X,
                            this._dataReactor.MousePos.Value.Y);

            bool result=    _dataReactor.CalculateIntersection
                (_dataReactor.CalculateCursorRay(mousePos), 
                this.Data).HasValue;

            return result;
        }
        protected virtual void OnRollOver()
        {
        //    if (!_dataReactor.IsContainHotReg(this))
        //        _dataReactor.AddHotReg(this);
#if _DEBUG
            if (_debugDraw == null)
                _debugDraw = new DebugDraw(this._game.GraphicsDevice);

            if (this._hotRegs == null)
                _hotRegs = new List<BoundingSphere>();
            _hotRegs.Add(this.Data.BoundingSpheres[0]);
#endif

            this._curState=InteractiveObjState.OnRollOver;

                
        }
        protected virtual void OnRollOut()
        {
            //if (_dataReactor.IsContainHotReg(this))
            //    _dataReactor.DelHotReg(this);
#if _DEBUG
            if (this._hotRegs == null)
                _hotRegs = new List<BoundingSphere>();
            _hotRegs.Clear();
#endif
            //SelectFunction.DeSelect(this);
            this._curState = InteractiveObjState.AwayFrom;
        }
        protected override void LoadContent()
        {
           
          //  this._selCompData.dataModifitionHandler.Invoke();
        }
        public override void Update(GameTime gameTime)
        {
            if (!_isInitialized)
            {
                this.Initialize();
            }

            if (!_dataReactor.MousePos.HasValue)
            {
                base.Update(gameTime);
                return;
            }


            if (_keepSel)
            { 
                
            
            }


            //RollOver Or RollOut
            
            if (RollOverChecker != null && RollOverChecker.Invoke(this))
            {
                    if (RollOverHandler != null)
                        this.RollOverHandler.Invoke();

            }
            else
            {
                    if (RollOverHandler != null)
                        this.RollOutHandler.Invoke();
            }


            if (_dataReactor.MouseData.HasValue && _dataReactor.MouseData.Value.IsClicking)
            {
                //Click 
                if (SelectionChecker != null && SelectionChecker.Invoke(this))
                {
                    if (this._selCompData.SelectionHandler != null)
                        this._selCompData.SelectionHandler.Invoke(this, true);

                }
                else
                {
                    if (this._selCompData.SelectionHandler != null)
                        this._selCompData.SelectionHandler.Invoke(this, false);
                }
            }
            _oldstate=_curState;
            
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime,ICamera cam)
        {
            /*
            _game.GraphicsDevice.RasterizerState =
                RasterizerState.CullNone;
            if (_rasterizerState.FillMode == FillMode.WireFrame)
            {
                _color = Color.Yellow;
            }
            else
                _color = Color.Red;*/


#if _DEBUG

            if (_debugDraw == null)
                _debugDraw = new DebugDraw(this._game.GraphicsDevice);

            if(this._hotRegs==null)
                _hotRegs = new List<BoundingSphere>();

            _debugDraw.Begin(//Matrix.Multiply(Matrix.Identity, cam.ViewMatrix),//this._world, cam.ViewMatrix),
                //this.TransformNode.AbsoluteTransform,
                Matrix.Identity,
                cam.ViewMatrix,
                cam.ProjectionMatrix);


            foreach (BoundingSphere bSphere in this._hotRegs)
                    _debugDraw.DrawWireSphere(bSphere, Color.Red);

            _debugDraw.End();

#endif
            base.Draw(gameTime,cam);
       }


        /*
        protected void AbsoluteRotation()
        {

            Vector3 radianAngle = (_rotation / 360) * 2 * MathHelper.Pi;
            _rotQuaternion = Quaternion.Identity;
            _rotQuaternion = Quaternion.Normalize(
                    Quaternion.CreateFromYawPitchRoll
                    (radianAngle.Y, radianAngle.X, radianAngle.Z)
                );
            _world = 
                Matrix.CreateFromQuaternion(_rotQuaternion) *
            //   Matrix.CreateTranslation(_translation) *
               Matrix.CreateScale(_scale);

        }
        protected void RelativeRotation(Vector3 axis, float angle)
        {
            angle = (angle / 360) * 2 * MathHelper.Pi;
            
            axis = Vector3.Transform
                (axis, Matrix.CreateFromQuaternion(_rotQuaternion));

            _rotQuaternion = Quaternion.Normalize
                (Quaternion.CreateFromAxisAngle(axis, angle)*
                    _rotQuaternion);
        }*/
        public int CompareTo(object input)
        {
            ISelectable obj = (ISelectable)input;

            if (this.DistOfObj > obj.DistOfObj)
                return 1;
            if (this.DistOfObj < obj.DistOfObj)
                return -1;

            return 0;
        }
        }
    }

