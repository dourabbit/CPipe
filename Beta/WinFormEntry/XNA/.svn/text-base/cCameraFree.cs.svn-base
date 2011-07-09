using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


namespace SysLib
{

    public class cCameraFree 
    {
        Vector3 _position;
        Vector3 _targetPos;
        Quaternion _rotation;
        
        public Matrix myWorld;
        public Matrix myView;
        public Matrix myProjection;
        public Viewport myViewport;



        float tranlationSpeed = .001f;
        float turnSpeed = .001f;


        public cCameraFree()
      
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


        }
        public void Translate(Vector3 distance)
        {
            _position += Vector3.Transform(distance, Matrix.CreateFromQuaternion(_rotation));
            
        }
    }
}