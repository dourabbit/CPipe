using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using XNASysLib.XNAKernel;
using System.Collections.Generic;
using VertexPipeline;
using XNASysLib.Primitives3D;

namespace XNASysLib.XNATools
{
    public abstract class aCTool : IManipulator, IDrawableComponent
    {
        protected IGame _game;
        protected ICamera _cam;
        protected List<IHotSpot> _hotSpots;
        private ObjImage _before;
        protected string _toolNm="aCTool";
        protected ISelectable _toolTarget;
        protected SpotOnSelect _spotSelectionHandler;
        protected bool _isInitialized;
        protected SnapShots _targetSnapShot;

        public ToolHandler PreExe;
        public ToolHandler Exe;
        public ToolHandler AfterExe;

        public TransformNode TransformNode
        { get; set; }

        public SpotOnSelect SpotSelectionHandler
        { get { return _spotSelectionHandler; } }

        public string ID
        { get { return _toolNm; } }
        public List<IHotSpot> HotSpots
        { get { return _hotSpots; } }
        public string ToolNm
        { get { return _toolNm; } }
        
        public ISelectable ToolTarget
        { get { return _toolTarget; } }
        

        public virtual void ToolPreExe()
        {
           // MyConsole.WriteLine("ToolPreExe");
            SceneNodHierachyModel node = (SceneNodHierachyModel)_toolTarget;



            //if (node.ShapeNode != null)
            //    _targetSnapShot = new
            //        SnapShots("aCTool", node.TransformNode.GetCopy(),
            //                 node.ShapeNode.GetCopy(), null);
            //else
            //    _targetSnapShot = new
            //  SnapShots("aCTool", node.TransformNode.GetCopy(),
            //           null, null);

            
            TransformNode trans = node.TransformNode.GetCopy();
            ShapeNode shape = null;
            if(node.ShapeNode != null)
                shape= node.ShapeNode.GetCopy();
            _before = new ObjImage { Trans = trans, Shape = shape };
        }

        public virtual void ToolExe()
        {
            //MyConsole.WriteLine("ToolExe");
        
        }

        public virtual void ToolAfterExe()
        {
           // MyConsole.WriteLine("ToolAfterExe");
            SceneNodHierachyModel target =
             _toolTarget as SceneNodHierachyModel;
            if (target == null)
                new NullReferenceException();
            
            SceneNodHierachyModel node = (SceneNodHierachyModel)_toolTarget;
            TransformNode trans = node.TransformNode.GetCopy();
            ShapeNode shape = null;
            if(node.ShapeNode != null)
                shape= node.ShapeNode.GetCopy();
            ObjImage after = new ObjImage { Trans = trans, Shape = shape };

            _targetSnapShot= new SnapShots("aCTool", _before, after , null);

            new SysEvn(0, this,
                OBJTYPE.Building, SYSEVN.Tool,
                 new object[3] { this._toolNm, target, _targetSnapShot }
                 );

        }
        
        public aCTool(IGame game)
        {


            this.PreExe += ToolPreExe;
            this.Exe += ToolExe;
            this.AfterExe += ToolAfterExe;



            aCTool tool = (aCTool)game.Components.Find(
                delegate(IUpdatableComponent matcher)
                {
                    return matcher is aCTool ?
                        true : false;
                });

            if (tool != null)
                tool.Dispose();

            this.TransformNode = new TransformNode();

            this._game = game;
            this._hotSpots = new List<IHotSpot>();
            this._game.Components.Add(this);
            
        }
        ~aCTool()
        {
            Dispose(false);
        }
        public virtual void Initialize()
        {
            this._cam = (ICamera)_game.Services.
                GetService(typeof(ICamera));
            this._isInitialized = true;
        }
        public virtual void Update(GameTime gameTime)
        {
            //this.TransformNode.UpdateTransform();
        }
        public virtual void Draw(GameTime gameTime,ICamera cam)
        {
        
        }
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._game.Components.Remove(this);
                foreach (IHotSpot hotS in this._hotSpots)
                    if(hotS!=null)
                    hotS.Dispose();
                
            }
        }
    }

}