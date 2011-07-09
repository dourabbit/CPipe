using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using VertexPipeline;


namespace XNASysLib.XNAKernel
{
    /// <summary>
    /// Even if I implement the ISelectable interface, it is just dummy function
    /// Some future work needed on Rendering and Selecting on  the dCamera  
    /// </summary>
    public class dCamera :DrawableComponent,ICamera,ISelectable
    {
        Vector3 _position;
        Vector3 _targetPos;
        float _targetDis;
        Quaternion _rotation;
        float? _distOfObj;

        float _rotY;
        float _rotX;
        Matrix _world;

        public SelectableCompData Data
        {
            get { return new SelectableCompData(); }
        }
        Matrix _view;
        Matrix _projection;
        Viewport _viewport;
        OnSelected _selectionHandler;

        OnRoll _rollOver;
        OnRoll _rollOut;
        bool _keepSel;
        bool _lock;
        public bool Lock
        {
            get { return _lock; }
            set { _lock = value; }
        }
        public OnRoll RollOverHandler
        { get { return _rollOver; } }
        public bool KeepSel
        { set { _keepSel = value; }
            get { return _keepSel; }
        }
        public OnRoll RollOutHandler
        { get { return _rollOut; } }
        public float? DistOfObj
        { get { return _distOfObj; } }


        Matrix ICamera.WorldMatrix
        {
            get
            {
                return this._world;
            }
            set
            {
                this._world = value;
            }
        }
        Matrix ICamera.ViewMatrix
        {
            get
            {
                return this._view;
            }
            set
            {
                this._view = value;
            }
        }

        Matrix ICamera.ProjectionMatrix
        {
            get
            {
                return this._projection;
            }
            set
            {
                this._projection = value;
            }
        }
        Quaternion ICamera.RotationQuat
        {
            get
            { return this._rotation; }

            set
            { this._rotation = value; }
        }

        Viewport ICamera.Viewport
        {
            get
            {
                return this._viewport;
            }
            set 
            {
                this._viewport = value;
            }
        }


        float tranlationSpeed = .01f;
        float turnSpeed = .001f;


        public dCamera(IGame game):base(game)
      
        {

            _position = new Vector3(0, 0, -10);
            _targetPos = new Vector3(0, 0, 0);
            _rotation = new Quaternion(0, 0, 0, 1);
            this._rotX = 0;
            this._rotY = 0;
            _game.Components.GetNm("persp", out _ID);
            _game.Components.Add(this);
            //this._ID = "Persp";
        }
        public override void Initialize()
        {
            float aspectRatio = _game.GraphicsDevice.Viewport.AspectRatio;

            _projection = Matrix.CreatePerspectiveFieldOfView
                (MathHelper.PiOver4, aspectRatio, 0.1f, 10000f);

            _viewport =_game.GraphicsDevice.Viewport;

            base.Initialize();


        }

        public override void Update(GameTime gameTime)
        {
            _targetDis = Math.Abs(Vector3.Distance
                (this._position, this._targetPos));


            _world = Matrix.CreateFromQuaternion(_rotation) *
                                   Matrix.CreateTranslation(_position);

            _view = Matrix.Invert(_world);


            base.Update(gameTime);

        }
        /// <summary>
        /// Rotate camera, toward its centretarget
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="angle"></param>
        public void Rotate(Vector3 axis, float angle)
        {
            //Local axis
            //axis = Vector3.Transform(axis, Matrix.CreateFromQuaternion(_rotation));

            //get quaternion from local axis and angle
            //_rotation = Quaternion.Normalize(Quaternion.CreateFromAxisAngle(axis, angle)
            //                                    *_rotation);

            if(axis==Vector3.Up)
                    this._rotY += angle;
            else if(axis==Vector3.Up)
                    this._rotY -= angle;
            else if (axis == Vector3.Right)
                    this._rotX += angle;
            else if (axis == Vector3.Right)
                    this._rotX -= angle;

            _rotation = Quaternion.CreateFromYawPitchRoll(_rotY, _rotX, 0.0f);
            //Move the cam to its target
            this._position = this._targetPos;

            //Move it back in object space
            Translate(new Vector3(0,0,this._targetDis));

            //correctY();
        }

        void correctY()
        {
            //Plane verticalPlane = new Plane(this._position,this._targetPos,this._position+Vector3.Up);
            //Vector3 localY=Vector3.Transform(Vector3.Up,_rotation);
            //float cosTheta= Vector3.Dot(localY, verticalPlane.Normal);
            //if (cosTheta < 0)
            //    cosTheta = -cosTheta;
            ////if (tmp < 0.2)
            ////{
            ////    return;
            ////}
            //float angle = MathHelper.PiOver2 - (float)Math.Acos(cosTheta);
          
            //MyConsole.WriteLine(angle.ToString());//this._position - this._targetPos
            //_rotation = Quaternion.Normalize(_rotation * Quaternion.CreateFromAxisAngle(this._position - this._targetPos, -angle/10f));


            Plane verticalPlane = new Plane(this._position, this._targetPos, this._position + Vector3.Up);

            Vector3 localY=Vector3.Transform(Vector3.Up,_rotation);
            float cosTheta= Vector3.Dot(localY, verticalPlane.Normal);
            //if (Math.Abs(cosTheta) < 0.1f)
            //    return;


            Vector3 camXaxis = Vector3.Transform(Vector3.Right, _rotation);            
            camXaxis = cosTheta<0 ? -verticalPlane.Normal:verticalPlane.Normal;
            Vector3 camZaxis = Vector3.Normalize( this._position - this._targetPos);
            Vector3 camYaxis = Vector3.Cross(camXaxis,camZaxis);

            Matrix rot = Matrix.CreateLookAt(this._position, this._targetPos,camYaxis);
            this._rotation = Quaternion.CreateFromRotationMatrix(rot);
        }
      
        /// <summary>
        /// Transform camera in object space
        /// </summary>
        /// <param name="distance"></param>
        public void Translate(Vector3 distance)
        {
            _position += Vector3.Transform(distance,
                Matrix.CreateFromQuaternion(_rotation));
        }

        /// <summary>
        /// Transform camera in object space
        /// </summary>
        /// <param name="distance"></param>
        public void Translate(Vector3 distance, out Vector3 position)
        {
            position = Vector3.Transform(distance,
                Matrix.CreateFromQuaternion(_rotation));
        }
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