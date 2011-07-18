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
    public class RotateTool : aCTool
    {

        //List<ISelectable> _HotReg = new List<ISelectable>();
        DebugDraw _debugDraw;

        RotateToolPart _xaxis;
        RotateToolPart _yaxis;
        RotateToolPart _zaxis;

       Vector3 _centerPos;
       ISelectable _curSel;

       float _length = 1;
       public float Length
       {
           set { _length = value; }
       }
       public RotateTool(IGame game, ISelectable target)
            : base(game)
        {
          
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
            _xaxis = new RotateToolPart(_game, _length * Vector3.Right, target.TransformNode,this);
            _yaxis = new RotateToolPart(_game, _length * Vector3.Up, target.TransformNode,this);
            _zaxis = new RotateToolPart(_game, _length * Vector3.Backward, target.TransformNode,this);

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
        public override void Update(GameTime gameTime)
        {
            bool result = true;

            foreach (IHotSpot spot in this._hotSpots)
            {
                RotateToolPart toolPart = (RotateToolPart)spot;
                toolPart.Update(gameTime);

                result &=
                    toolPart.State == InteractiveObjState.AwayFrom ? 
                    true : false;
                
                if (toolPart.IsActive)// Move others if active
                {

                    //_toolTarget.TransformNode.RotQuaternion =
                    //        toolPart.TransformNode.RotQuaternion;

                    //MyConsole.WriteLine(_toolTarget.TransformNode.Rotate.ToString() + ":::");
                    
                    foreach (IHotSpot spot2 in this._hotSpots)
                    {
                        RotateToolPart unactiveToolPart = (RotateToolPart)spot2;
                        if (toolPart != unactiveToolPart)
                        {
                            
                            //Move the unactive tool part
                            unactiveToolPart.
                                Move(toolPart.TransformNode.RotQuaternion);//_toolTarget.TransformNode.RotQuaternion);//World.Translation);
                           
                            
                        }
                    }
                    break;
                }
                    
            }


            //Dispose when none tool part is active
            if (result && Mouse.GetState().LeftButton == ButtonState.Pressed&&
                Keyboard.GetState().IsKeyUp(Keys.LeftAlt))
                this.Dispose();

          
            //reserve center pos for drawing the line
            //_centerPos =Vector3.Transform
            //    (_toolTarget.TransformNode.Pivot.Translation,
            //    _toolTarget.TransformNode.World);

            //WorldTrans plus pivot trans offset
            _centerPos = _toolTarget.TransformNode.Translate + _toolTarget.TransformNode.Pivot.Translation;

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime, ICamera cam)
        {

            _debugDraw.Begin(//Matrix.Multiply(Matrix.Identity, _cam.ViewMatrix),
                Matrix.Identity,
                _cam.ViewMatrix,
                _cam.ProjectionMatrix);

            DepthStencilState _curState = _game.GraphicsDevice.DepthStencilState;
           
            _game.GraphicsDevice.DepthStencilState = DepthStencilState.None;

            

            foreach (IHotSpot spot in this._hotSpots)
                if (spot is IDrawableComponent)
                    ((IDrawableComponent)spot).Draw(gameTime,cam);

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