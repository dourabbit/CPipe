using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace SysLib
{

    public class cCamera :ICam
    {
        Vector3 _position;
        Vector3 _targetPos;
        float _targetDis;
        Quaternion _rotation;
        
        Matrix myWorld;
        Matrix myView;
        Matrix myProjection;
        Viewport myViewport;

        public Matrix ICam.WorldMatrix
        {
            get
            {
                return this.myWorld;
            }
        }
        public Matrix ICam.ViewMatrix
        {
            get
            {
                return this.myView;
            }
        }

        public Matrix ICam.ProjectionMatrix
        {
            get
            {
                return this.myProjection;
            }
        }

        public Viewport ICam.Viewport
        {
            get
            {
                return this.myViewport;
            }
        }


        float tranlationSpeed = .001f;
        float turnSpeed = .001f;


        public cCamera()
      
        {
            _position = new Vector3(0, 0, 0);
            _targetPos = new Vector3(0,0,0);
            _rotation = new Quaternion(0, 0, 0, 1);

            myProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 1.777f, 0.1f, 10000f);

        }
        public  void Initialize()
        {


        }

        public  void Update()
        {
            _targetDis = Math.Abs(Vector3.Distance
                (this._position, this._targetPos));


            myWorld = Matrix.Identity;

            myView = Matrix.Invert(Matrix.CreateFromQuaternion(_rotation) *
                                   Matrix.CreateTranslation(_position));

                



            if (Keyboard.GetState().IsKeyDown(Keys.W))
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




        }
        public void Rotate(Vector3 axis, float angle)
        {
            axis = Vector3.Transform(axis, Matrix.CreateFromQuaternion(_rotation));
            _rotation = Quaternion.Normalize(
                                                Quaternion.CreateFromAxisAngle(axis, angle)
                                                *
                                                _rotation
                                                );

            this._position = this._targetPos;
            Translate(new Vector3(0,0,this._targetDis));


        }
        public void Translate(Vector3 distance)
        {
            _position += Vector3.Transform(distance, Matrix.CreateFromQuaternion(_rotation));
            this._targetPos += Vector3.Transform(distance, Matrix.CreateFromQuaternion(_rotation));
            
        }
    }
}