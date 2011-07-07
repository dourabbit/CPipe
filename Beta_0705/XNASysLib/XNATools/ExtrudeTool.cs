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
    public class ExtrudeTool : aCTool
    {

        DebugDraw _debugDraw;

        ExtrudeToolPart _xaxis;
        //ExtrudeToolPart _yaxis;

        Vector3 _centerTrans;
        //ISelectable _curSel;

        float _length = 1;
        public float Length
        {
            set { _length = value; }
        }
        public ExtrudeTool(IGame game, PipeBase target)
            : base(game)
        {

            //_cetreTrans = target.World.Translation;
            this._toolTarget = target;
            ((ISelectable)_toolTarget).KeepSel = true;
            this._length = 5f;
            this.Initialize();

        }


        public override void Initialize()
        {
            PipeBase target = (PipeBase)_toolTarget;
            _xaxis = new ExtrudeToolPart(_game, target, 1f);
            //_yaxis = new ExtrudeToolPart(_game, target, -1f);

            this._hotSpots.Add(_xaxis);
            //this._hotSpots.Add(_yaxis);

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
                ExtrudeToolPart toolPart = (ExtrudeToolPart)spot;
                toolPart.Update(gameTime);

                result &=
                    toolPart.State == InteractiveObjState.AwayFrom ?
                    true : false;

                
                if (toolPart.IsActive)// Move others if active
                {
                    //Editing The parameter of Target Object
                    extrudeToolPartExe(toolPart);

                    foreach (IHotSpot spot2 in this._hotSpots)
                    {
                        ExtrudeToolPart unactiveToolPart = (ExtrudeToolPart)spot2;
                        if (toolPart != unactiveToolPart)
                        {
                            //unactiveToolPart.Move
                            //    (_toolTarget.World.Translation);

                        }
                    }
                    break;
                }

            }

            if (result && Mouse.GetState().LeftButton == ButtonState.Pressed &&
                Keyboard.GetState().IsKeyUp(Keys.LeftAlt))
                this.Dispose();

            //Drawing the axis line 
            _centerTrans = this._toolTarget.TransformNode.AbsoluteTransform.Translation + _toolTarget.TransformNode.Pivot.Translation;
            //_centerTrans =
            //    Vector3.Transform(
            //    _toolTarget.TransformNode.Pivot.Translation,
            //    _toolTarget.TransformNode.World);

            base.Update(gameTime);
        }

        void extrudeToolPartExe(ExtrudeToolPart toolPart)
        {

            Matrix inver = Matrix.Invert(((PipeBase)_toolTarget).TransformNode.AbsoluteTransform);
            Vector3 objSpace = Vector3.Transform
                (toolPart.TransformNode.Translate, inver);
            float delta = objSpace.Z;//objSpace.Z - tooltarget.Z;
            float length = (toolPart.TransformNode.Translate -
                         ((PipeBase)_toolTarget).TransformNode.Translate).Length();

            //if (toolPart.ID == "FrontDragger")
                ((PipeBase)_toolTarget).SideBoundary = 
                    new Vector3(0,0,
                    delta > 0 ? length : -length);

                MyConsole.WriteLine(toolPart.ID + " Is Active"
                   + ((PipeBase)_toolTarget).SideBoundary.ToString());

        }
        public override void Draw(GameTime gameTime,ICamera cam)
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

            _debugDraw.DrawLine(_centerTrans, this._xaxis.TransformNode.Translate, Color.Red);
            //_debugDraw.DrawLine(_centerTrans, this._yaxis.TransformNode.Translate, Color.Green);

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