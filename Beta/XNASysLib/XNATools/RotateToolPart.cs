
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


  
    public class RotateToolPart : DraggableRing, IHotSpot
    {
        static ITransformNode _rotationCenter;
        Vector3 _offset;
        public Vector3 Offset
        { get { return _offset; } }

        //Vector3 _initialRot;
        Vector3 _x;
        Vector3 _y;
        Vector3 _z;
        //Quaternion _initialAxis;
        Color _initialCol;
        bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
        }
        public override string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public RotateToolPart(IGame game, Vector3 offset,
                        ITransformNode refWorld, aCTool tool)
            : base(game, 6f,0.05f, 64)
        {
            this._tool = tool;
            _rotationCenter = refWorld;
            _offset = offset;
            offset.Normalize();
           
            this._dragHandler = null;
            if (offset == Vector3.UnitX)
            {
                _ID = "RotAxisX";
                this.TransformNode.NodeNm = _ID + "_trans";
                this.ShapeNode.Name = _ID + "_shape";
                this.RotationY = 90;
                _offset = Vector3.Zero;
                _offset.Y = 90;
                this._dragHandler += this.RotOnX;
                _initialCol = Color.Red;
            }
            else if (offset == Vector3.UnitY)
            {
                _ID = "RotAxisY";
                this.TransformNode.NodeNm = _ID + "_trans";
                this.ShapeNode.Name = _ID + "_shape";
                this.RotationX = 90;
                _offset = Vector3.Zero;
                _offset.X = 90;
                this._dragHandler += this.RotOnY;
                _initialCol = Color.Green;
            }
            else if (offset == Vector3.UnitZ)
            {
                _ID = "RotAxisZ";
                this.TransformNode.NodeNm = _ID + "_trans";
                this.ShapeNode.Name = _ID + "_shape";
                _offset = Vector3.Zero;
                this._dragHandler += this.RotOnZ;
                _initialCol = Color.Blue;
            }
            ShapeNode.PreDrawHandler = null;
            ShapeNode.PreDrawHandler += delegate
            {


                _game.GraphicsDevice.RasterizerState =
                    RasterizerState.CullNone;
                if (_rasterizerState.FillMode == FillMode.WireFrame)
                {
                    _color = Color.Yellow;
                }
                else
                    _color = _initialCol;
            };
        }
        #region RotHelper
        void RotOnX()
        {
            Vector2 a = new Vector2(this._dataReactor.MousePos.Value.X, this._dataReactor.MousePos.Value.Y);
            Vector2 b = new Vector2(_mouseIntialPos.Value.X, _mouseIntialPos.Value.Y);

            float disSquare = (a - b).Length();
            if (a.Y < b.Y)
                disSquare = -disSquare;

            if (Math.Abs(disSquare) == 0)
                return;

            _x = _rotationCenter.World.Right;
            _x.Normalize();

            Quaternion delta = Quaternion.CreateFromAxisAngle(_x, MathHelper.ToRadians(disSquare));

            this.TransformNode.RotQuaternion =
                    delta * this.TransformNode.RotQuaternion;
            this.TransformNode.AbsoluteTransform = this.TransformNode.World;
            _rotationCenter.RotQuaternion = delta * _rotationCenter.RotQuaternion;

            _mouseIntialPos = this._dataReactor.MousePos.Value;

        }
        void RotOnY()
        {
            Vector2 a = new Vector2(this._dataReactor.MousePos.Value.X, this._dataReactor.MousePos.Value.Y);
            Vector2 b = new Vector2(_mouseIntialPos.Value.X, _mouseIntialPos.Value.Y);

            float disSquare = (a - b).Length();
            if (a.Y < b.Y)
                disSquare = -disSquare;

            if (Math.Abs(disSquare) == 0)
                return;

            _y = _rotationCenter.World.Up;
            _y.Normalize();

            Quaternion delta = Quaternion.CreateFromAxisAngle(_y, MathHelper.ToRadians(disSquare));
            //this.TransformNode.Pivot = _rotationCenter.Pivot;
            this.TransformNode.RotQuaternion =
                    delta * this.TransformNode.RotQuaternion;
            this.TransformNode.AbsoluteTransform = this.TransformNode.World;
            _rotationCenter.RotQuaternion = delta * _rotationCenter.RotQuaternion;
            _mouseIntialPos = this._dataReactor.MousePos.Value;
        }
        void RotOnZ()
        {
            Vector2 a = new Vector2(this._dataReactor.MousePos.Value.X, this._dataReactor.MousePos.Value.Y);
            Vector2 b = new Vector2(_mouseIntialPos.Value.X, _mouseIntialPos.Value.Y);

            float disSquare = (a - b).Length();
            if (a.Y < b.Y)
                disSquare = -disSquare;

            _z = _rotationCenter.World.Backward;
            _z.Normalize();

            Quaternion delta = Quaternion.CreateFromAxisAngle(_z, MathHelper.ToRadians(disSquare));

            this.TransformNode.RotQuaternion =
                    delta * this.TransformNode.RotQuaternion;
            this.TransformNode.AbsoluteTransform = this.TransformNode.World;
            _rotationCenter.RotQuaternion = delta * _rotationCenter.RotQuaternion;
            _mouseIntialPos = this._dataReactor.MousePos.Value;

        } 
        #endregion
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

            base.Initialize();

            _x = _rotationCenter.AbsoluteTransform.Right;
            _x.Normalize();

            _y = _rotationCenter.AbsoluteTransform.Up;
            _y.Normalize();

            _z = _rotationCenter.AbsoluteTransform.Backward;
            _z.Normalize();

            this.TransformNode.RotQuaternion = _rotationCenter.RotQuaternion;
            if (_offset.Y == 90)//X axis
            {
                this.TransformNode.RotQuaternion =

                    Quaternion.CreateFromAxisAngle(_y, MathHelper.ToRadians(_offset.Y)) * this.TransformNode.RotQuaternion;
            }
            else if (_offset.X == 90)// Y axis
            {
                this.TransformNode.RotQuaternion =
                    Quaternion.CreateFromAxisAngle(_x, MathHelper.ToRadians(_offset.X)) * this.TransformNode.RotQuaternion;
            }
            else
            {

            }
           
            this._selCompData.dataModifitionHandler.Invoke();
            //TransformNode transNod = this.TransformNode.GetCopy();
            this.ShapeNode.Freeze(TransformNode);
            //this.TransformNode = transNod;
            this.TransformNode.Pivot = _rotationCenter.Pivot;
            this.TransformNode.Translate = _rotationCenter.AbsoluteTransform.Translation + _rotationCenter.Pivot.Translation;//_rotationCenter.Translate;
            this.TransformNode.AbsoluteTransform = this.TransformNode.World;
            
            //((DrawableComponent)base).Initialize();
            //this._isInitialized = true;
            //this._camera = (ICamera)_game.Services.GetService(typeof(ICamera));

        }
    
        public override void Update(GameTime gameTime)
        {
            if (!this._isInitialized)
                this.Initialize();


            base.Update(gameTime);


        }

        protected override void OnModifyBoundingSpheres()
        {
            this._selCompData.BoundingSpheres =
                   new BoundingSphere[1];

            BoundingSphere Bsphere = new BoundingSphere(Vector3.Zero,this.Radius);

            //this.TransformNode.UpdateTransform();

            //ShapeNode.GetBounding(out Bsphere.Center, out Bsphere.Radius,this.TransformNode.World);
            Bsphere.Radius++;
           
            this._selCompData.BoundingSpheres[0] = Bsphere;
            this._selCompData.BoundingSpheres[0].Center =
                _rotationCenter.AbsoluteTransform.Translation + _rotationCenter.Pivot.Translation;
            // TransformNode.AbsoluteTransform.Translation;
            // TransformNode.Translate;
            this._selCompData.shape = ShapeNode;
            this._selCompData.transform = this.TransformNode;    
        }

        protected override void OnDragStart()
        {
         
            this._isActive = true;
            _x = _rotationCenter.World.Right;
            _x.Normalize();

            _y = _rotationCenter.World.Up;
            _y.Normalize();

            _z = _rotationCenter.World.Backward;
            _z.Normalize();

            //_initialRot = this._centreTarget.Rotate;
            MyConsole.WriteLine("Start::::::::::::::::::::");
            base.OnDragStart();
        }
        protected override void OnDragEnd()
        {

            this._isActive = false;
            _x = _rotationCenter.World.Right;
            _x.Normalize();

            _y = _rotationCenter.World.Up;
            _y.Normalize();

            _z = _rotationCenter.World.Backward;
            _z.Normalize();

            MyConsole.WriteLine("End::::::::::::::::::::");
            //_initialRot = this._centreTarget.Rotate;
            base.OnDragEnd();
        }
        protected override void OnDragExe()
        {

            base.OnDragExe();
        }
        public void Move(Quaternion rot)//Vector3 centreTargetRot)
        {
            //this.TransformNode.Rotate = centreTargetRot;
            this.TransformNode.RotQuaternion = rot;
            this.TransformNode.AbsoluteTransform = this.TransformNode.World;
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