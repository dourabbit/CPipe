using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using VertexPipeline;


namespace XNASysLib.XNAKernel
{

    public class dCamOrth :DrawableComponent,ICamera,ISelectable
    {
        Vector3 _position= Vector3.Zero;
        Vector3 _targetPos=Vector3.Zero;
        float _targetDis;
        Quaternion _rotation;
        float? _distOfObj;

        Matrix _world;
        Matrix _view;
        Matrix _projection;
        Viewport _viewport;
        OnSelected _selectionHandler;
        public SelectableCompData Data
        {
            get { return new SelectableCompData(); }
        }
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
        public OnRoll RollOutHandler
        { get { return _rollOut; } }
        public float? DistOfObj
        { get { return _distOfObj; } }

        public bool KeepSel
        {
            set { _keepSel = value; }
            get { return _keepSel; }
        }
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

        public Viewport Viewport
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
        public Quaternion RotationQuat
        {
            get
            { return this._rotation; }
            set
            { this._rotation = value; }
        }

        //float tranlationSpeed = .01f;
        //float turnSpeed = .001f;


        dCamOrth(IGame game)
            : base(game)
      
        {
            _rotation = new Quaternion(0, 0, 0, 1);
        }
        public dCamOrth(Vector3 camPos, Vector3 targetPos,IGame game)
            :this(game)
        {
            this._rotation = Quaternion.CreateFromRotationMatrix(Matrix.CreateLookAt(camPos,targetPos,Vector3.Up));
        }



        public override void Initialize()
        {
            float aspectRatio = _game.GraphicsDevice.Viewport.AspectRatio;

            //_projection = Matrix.CreatePerspectiveFieldOfView
              //  (MathHelper.PiOver4, aspectRatio, 0.1f, 10000f);

            _projection = Matrix.CreateOrthographic
                (640f,480f,0.1f,1000f);

            _viewport =_game.GraphicsDevice.Viewport;

            base.Initialize();


        }

        public override void Update(GameTime gameTime)
        {
            _targetDis = Math.Abs(Vector3.Distance
                (this._position, this._targetPos));

            if (_position.Z < 0)
                _position.Z = 0;

            _world = Matrix.Identity;
            _view = Matrix.Invert(Matrix.CreateFromQuaternion(_rotation) *
                                   Matrix.CreateTranslation(_position));

            //Ortho cam can't zoom in, because these is no depth data
            //SO we fake it
            float multiplier = _position.Z/100;

            _projection = Matrix.CreateOrthographic
                   (64f * multiplier, 48f * multiplier, 0.1f, 1000f);

            /*   if (Keyboard.GetState().IsKeyDown(Keys.W))
                this.Translate(Vector3.Forward * tranlationSpeed);
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                this.Translate(Vector3.Backward * tranlationSpeed);
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                this.Translate(Vector3.Left * tranlationSpeed);
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                this.Translate(Vector3.Right * tranlationSpeed);
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                this.Rotate(Vector3.Left, turnSpeed);
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                this.Rotate(Vector3.Right, turnSpeed);
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                this.Rotate(Vector3.Up, turnSpeed);
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                this.Rotate(Vector3.Down, turnSpeed);
            */
            base.Update(gameTime);

        }
        public void Rotate(Vector3 axis, float angle)
        {
            //axis = Vector3.Transform(axis, Matrix.CreateFromQuaternion(_rotation));
            //_rotation = Quaternion.Normalize(
            //                                    Quaternion.CreateFromAxisAngle(axis, angle)
            //                                    *
            //                                    _rotation
            //                                    );

            //this._position = this._targetPos;
            //Translate(new Vector3(0,0,this._targetDis));


        }
        public void Translate(Vector3 distance)
        {
            _position += Vector3.Transform(distance,
                Matrix.CreateFromQuaternion(_rotation));
            MyConsole.WriteLine(_position.Z.ToString());
        }
        public void Translate(Vector3 dis, out Vector3 result)
        {
            result = Vector3.Transform(dis,
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