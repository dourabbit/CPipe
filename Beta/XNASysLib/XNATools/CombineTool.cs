using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using XNASysLib.XNAKernel;
using XNASysLib.Primitives3D;
using XNAPhysicsLib;
using VertexPipeline;
using System.Collections.Generic;

namespace XNASysLib.XNATools
{
    public class CombineTool : aCTool
    {

        DebugDraw _debugDraw;

        CombineToolPart _xaxis;
        //CombineToolPart _yaxis;

        List<CombineToolPart> _toolPartsList = new List<CombineToolPart>();
        List<CombineToolPart> _destPartsList = new List<CombineToolPart>();

        List<SceneNodHierachyModel> _targetPipes;

        Vector3 _centerTrans;
        float _length = 1;
        public float Length
        {
            set { _length = value; }
        }
        public CombineTool(IGame game, PipeBase target)
            : base(game)
        {
            this._toolTarget = (ISelectable)target.Root;
            ((ISelectable)_toolTarget).KeepSel = true;
            this._length = 5f;

            _targetPipes = new List<SceneNodHierachyModel>();
            foreach (SceneNodHierachyModel node in game.Components.FindAll(
            delegate(IUpdatableComponent matcher)
            {
                return (matcher is SceneNodHierachyModel) && target.Root != matcher ?
                    true : false;
            }))

                this._targetPipes.Add(node);

             if (_targetPipes.Count==0)
                Dispose();
            else
                this.Initialize();
        }


        public override void Initialize()
        {
            //Pipe target = (Pipe)_toolTarget;
            //_xaxis = new CombineToolPart(_game, target, 1f, _destPartsList);
            //_yaxis = new CombineToolPart(_game, target, -1f, _destPartsList);

            foreach (INode node in ((SceneNodHierachyModel)_toolTarget).FlattenNods)
            {
                PipeBase pipe = node as PipeBase;
                if (pipe != null)
                {
                    CombineToolPart part = new CombineToolPart(_game, pipe, _destPartsList);
                    //part.OnDragExeHandler = null;

                    this._toolPartsList.Add(part);
                
                
                }
            
            }
            this._hotSpots.Add(_xaxis);
            //this._hotSpots.Add(_yaxis);

            foreach (SceneNodHierachyModel nodes in this._targetPipes)
            {
                foreach (INode node in nodes.FlattenNods)
                {
                    PipeBase pipe = node as PipeBase;
                    if (pipe != null)
                    {
                        CombineToolPart part = new CombineToolPart(_game, pipe, _destPartsList);
                        part.OnDragExeHandler = null;
                        this._destPartsList.Add(part);
                    }
                }
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


            foreach (CombineToolPart part in _destPartsList)
            {
                part.Update(gameTime);
            }

            foreach (CombineToolPart toolPart in this._toolPartsList)
            {
                toolPart.Update(gameTime);

                result &=
                    toolPart.State == InteractiveObjState.AwayFrom ?
                    true : false;

                
                
                if (toolPart.IsActive)// Move others if active
                {
                    //Editing The parameter of Target Object
                    CombineToolPartExe(toolPart);
                    
                    //foreach (IHotSpot spot2 in this._hotSpots)
                    foreach (CombineToolPart unactiveToolPart in this._toolPartsList)
                    {
                        if (toolPart != unactiveToolPart)
                        {
                            
                            unactiveToolPart.Move
                                //(_toolTarget.World.Translation);
                                (((SceneNodHierachyModel)((SceneNodHierachyModel)_toolTarget).Root).TransformNode.Translate);
                        }
                    }
                    break;
                }

            }

            if (result && Mouse.GetState().LeftButton == ButtonState.Pressed &&
                Keyboard.GetState().IsKeyUp(Keys.LeftAlt))
                this.Dispose();

          
           // _centerTrans = _toolTarget.TransformNode.Pivot.Translation;
            //Changed Pivot type, now it is relative type;

            _centerTrans = _toolTarget.TransformNode.Translate + _toolTarget.TransformNode.Pivot.Translation;


            base.Update(gameTime);
        }

        protected override void Dispose(bool disposing)
        {

            if (disposing)
            {
                foreach (CombineToolPart part in _destPartsList)
                    part.Dispose();

            }
            base.Dispose(disposing);
        }

        void CombineToolPartExe(CombineToolPart toolPart)
        {
            /*

            Vector3 delta=
                Vector3.Transform(new Vector3(0,0, toolPart.Offset), 
                toolPart.TransformNode.RotQuaternion);

            ((SceneNodHierachyModel)((SceneNodHierachyModel)_toolTarget).Root).
                TransformNode.Translate = 
                toolPart.TransformNode.Translate - delta;*/
        }
        public override void Draw(GameTime gameTime,ICamera 
            cam)
        {

            //_debugDraw.Begin(Matrix.Multiply(Matrix.Identity, _cam.ViewMatrix),
            //_cam.ProjectionMatrix);

            DepthStencilState _curState = _game.GraphicsDevice.DepthStencilState;

            _game.GraphicsDevice.DepthStencilState = DepthStencilState.None;


            /*
            foreach (IHotSpot spot in this._hotSpots)
                if (spot is IDrawableComponent)
                    ((IDrawableComponent)spot).Draw(gameTime,cam);*/


            foreach (CombineToolPart toolPart in this._toolPartsList)
            {
                toolPart.Draw(gameTime,cam);
            }

            foreach (CombineToolPart part in _destPartsList)
            {
                part.Draw(gameTime,cam);
            }

            //_debugDraw.DrawLine(_centerTrans, this._xaxis.TransformNode.Translate, Color.Red);
            //_debugDraw.DrawLine(_centerTrans, this._yaxis.TransformNode.Translate, Color.Green);

           // _debugDraw.End();


            _game.GraphicsDevice.DepthStencilState = _curState;
            base.Draw(gameTime,cam);
        }

        public override void Dispose()
        {

            ((ISelectable)_toolTarget).KeepSel = false;
            foreach (CombineToolPart part in _destPartsList)
            {
                part.Dispose();
            }

            foreach (CombineToolPart toolPart in this._toolPartsList)
            {
                toolPart.Dispose();
            }
            base.Dispose();
        }


    }

}