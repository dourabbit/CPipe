using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VertexPipeline;
using XNAPhysicsLib;
using XNASysLib.Primitives3D;
using XNASysLib.XNAKernel;

namespace XNASysLib.XNATools
{
    public class MoveTool : aCTool
    {

        //List<ISelectable> _HotReg = new List<ISelectable>();
        DebugDraw _debugDraw;

       MoveToolPart _xaxis;
       MoveToolPart _yaxis;
       MoveToolPart _zaxis;

       Vector3 _centerPos;
       //ISelectable _curSel;

       
       float _length = 1;
       public float Length
       {
           set { _length = value; }
       }
       public MoveTool(IGame game, ISelectable target)
            : base(game)
      {
            this._toolNm = "MoveTool";
            //_cetreTrans = target.World.Translation;
            this._toolTarget = target;
            ((ISelectable)_toolTarget).KeepSel = true;
            this._length = 5f;
           this.Initialize();
            
      }


      


       public override void ToolAfterExe()
       {
       

           base.ToolAfterExe();
       }

       public override void ToolExe()
       {

           base.ToolExe();
       }

       public override void ToolPreExe()
       {

         

           base.ToolPreExe();
       }

        public override void Initialize()
        {
            ISelectable target = (ISelectable)_toolTarget;
            _xaxis = new MoveToolPart(_game, _length*Vector3.Right, target.TransformNode,this);
            _yaxis = new MoveToolPart(_game, _length * Vector3.Up, target.TransformNode,this);
            _zaxis = new MoveToolPart(_game, _length * Vector3.Backward, target.TransformNode,this);

            this._hotSpots.Add(_xaxis);
            this._hotSpots.Add(_yaxis);
            this._hotSpots.Add(_zaxis);

            foreach (IHotSpot spot in this._hotSpots)
            {
               // spot.SelectionHandler += toolPartSel;
            }

            this._debugDraw = new
                DebugDraw(_game.GraphicsDevice);

            if (this._cam == null)
                _cam = (ICamera)_game.Services.
                    GetService(typeof(ICamera));

            base.Initialize();
        }
        void move()
        { 
            
        }
        public override void Update(GameTime gameTime)
        {
            bool result = true;
            Vector3 translate = Vector3.Zero;
                
            foreach (IHotSpot spot in this._hotSpots)
            {
                MoveToolPart toolPart =(MoveToolPart)spot;
                toolPart.Update(gameTime);

                result &=
                    toolPart.State == InteractiveObjState.AwayFrom ? 
                    true : false;

               if (toolPart.IsActive)// Move others if active
                {
                    
                   
                    //Move the center target
                    if (!_toolTarget.IsMuteTransform)
                        _toolTarget.TransformNode.Translate =
                        toolPart.TransformNode.Translate - toolPart.Offset;

                    translate = toolPart.TransformNode.Translate - toolPart.Offset;
               
                    //MyConsole.WriteLine(_toolTarget.TransformNode.Translate.ToString() + ":::");
                   
                    
                    foreach (IHotSpot spot2 in this._hotSpots)
                    { 
                        MoveToolPart unactiveToolPart=(MoveToolPart)spot2;
                        if (toolPart != unactiveToolPart)
                        {

                           
                            //Move the unactive tool part
                            unactiveToolPart.
                                Move(translate);//_toolTarget.TransformNode.World.Translation);
                           
                            
                        }
                    }
                    break;
                }
                    
            }


            //Dispose when none tool part is active
            if (result && Mouse.GetState().LeftButton == ButtonState.Pressed&&
                Keyboard.GetState().IsKeyUp(Keys.LeftAlt))
                this.Dispose();
            if (_toolTarget.IsMuteTransform)
                this.Dispose();

            _centerPos = _toolTarget.TransformNode.Translate + _toolTarget.TransformNode.Pivot.Translation;

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime, ICamera cam)
        {

            _debugDraw.Begin(
                //Matrix.Multiply(Matrix.Identity, _cam.ViewMatrix),
                Matrix.Identity,
                _cam.ViewMatrix,
                _cam.ProjectionMatrix);

            DepthStencilState _curState = _game.GraphicsDevice.DepthStencilState;
           
            _game.GraphicsDevice.DepthStencilState = DepthStencilState.None;

            

            foreach (IHotSpot spot in this._hotSpots)
                if (spot is IDrawableComponent)
                    ((IDrawableComponent)spot).Draw(gameTime,cam);

            
            


            _debugDraw.DrawLine(_centerPos,
                new Vector3(_centerPos.X + _length, _centerPos.Y, _centerPos.Z), Color.Red);

            _debugDraw.DrawLine(_centerPos,
                new Vector3(_centerPos.X, _centerPos.Y + _length, _centerPos.Z), Color.Green);

            _debugDraw.DrawLine(_centerPos,
                new Vector3(_centerPos.X, _centerPos.Y, _centerPos.Z + _length), Color.Blue);
            _debugDraw.End();


            _game.GraphicsDevice.DepthStencilState = _curState;
            base.Draw(gameTime,cam);
        }

        public override void Dispose()
        {

            ((ISelectable)_toolTarget).KeepSel = false;
            foreach (IHotSpot toolPart in this._hotSpots)
                toolPart.Dispose();

            base.Dispose();
        }


    }

}