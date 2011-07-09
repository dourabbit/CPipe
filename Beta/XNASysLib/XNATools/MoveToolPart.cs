using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using XNASysLib.XNAKernel;
using System.Collections.Generic;
using XNASysLib.Primitives3D;
using XNAPhysicsLib;
using VertexPipeline;

namespace XNASysLib.XNATools
{


  
    public class MoveToolPart : DraggableSphere, IHotSpot
    {
        /*public GeometricPrimitive model
        { get { return this; } }
        */

        ITransformNode _centreTarget;
        Vector3 _offset;
        aCTool _tool;
        public Vector3 Offset
        { get { return _offset; } }


        public OnSelected SelectionHandler
        {
            get { return _selCompData.SelectionHandler; }
            set { _selCompData.SelectionHandler = value; }
        }


        bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
        }

        public MoveToolPart(IGame game, Vector3 offset,
                        ITransformNode refWorld, aCTool tool)
            : base(game, 1f, 8)
        {
            this._tool = tool;
            _centreTarget = refWorld;
            _offset = offset;
            offset.Normalize();

            this._dragHandler = null;

            offset.Normalize();
            if (offset == Vector3.UnitX)
            {
                _ID = "MoveAxisX";
                this._dragHandler += this.DragOnX;
            }
            if (offset == Vector3.UnitY)
            {
                _ID = "MoveAxisY";
                this._dragHandler += this.DragOnY;
            }
            if (offset == Vector3.UnitZ)
            {
                _ID = "MoveAxisZ";
                this._dragHandler += this.DragOnZ;
            }
        }

        protected override void OnSelected(ISelectable obj, bool isSelected)
        {

            if (isSelected)
            {
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
                _rasterizerState = RasterizerState.CullNone;
                this._curState =
                    InteractiveObjState.AwayFrom;
            }



        }


        public override void Initialize()
        {

            /*
            this._world.Translation =
               this._centreTarget.Pivot
                    + _offset;
            this._translation = this._world.Translation;*/

            //this.TransformNode.Translate =
            //   Vector3.Transform(_centreTarget.Pivot.Translation,_centreTarget.World)
            //        + _offset;

            this.TransformNode.Translate =
                    _centreTarget.Translate + _centreTarget.Pivot.Translation + _offset;

            this.TransformNode.AbsoluteTransform = this.TransformNode.World;
            this._selCompData.dataModifitionHandler.Invoke();

            base.Initialize();
        }
    
        public override void Update(GameTime gameTime)
        {
            if (!this._isInitialized)
                this.Initialize();


            base.Update(gameTime);


        }
        protected override void OnDragStart()
        {
            if(this._tool.PreExe!=null)
                this._tool.PreExe.Invoke();
            this._isActive = true;
            base.OnDragStart();
        }
        protected override void OnDragEnd()
        {

            if (this._tool.AfterExe != null)
                this._tool.AfterExe.Invoke();

            this._isActive = false;

            base.OnDragEnd();
        }
        protected override void OnDragExe()
        {
            if (this._tool.Exe != null)
                this._tool.Exe.Invoke();

            base.OnDragExe();
        }
        public void Move(Vector3 centreTargetPivot)
        {
            /*
            this._world.Translation = centreTargetPivot
                    + _offset;
            this._translation = this._world.Translation;*/

            this.TransformNode.Translate = centreTargetPivot
                    + _offset;

            this._selCompData.dataModifitionHandler.Invoke();
        }

        public override void Draw(GameTime gameTime,ICamera cam)
        {

            base.Draw(gameTime,cam);
        }
      
        protected override void Dispose(bool disposing)
        {
            
            base.Dispose(disposing);
        }

    }

  

}