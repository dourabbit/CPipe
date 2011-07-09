using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexPipeline;
using XNASysLib.Primitives3D;
using XNASysLib.XNAKernel;
using System.Collections.Generic;

namespace XNASysLib.XNATools
{


  
    public class CombineToolPart : DraggableSphere, IHotSpot
    {

        /*
        public GeometricPrimitive model
        { get { return this; } }
        */
        List<CombineToolPart> _parts;
        PipeBase _centreTarget;
        //float _offset;
        public OnSelected SelectionHandler
        {
            get { return _selCompData.SelectionHandler; }
            set { _selCompData.SelectionHandler = value; }
        }
        /*
        public override Quaternion Quaternion
        {
            get
            {
                return _centreTarget.Quaternion;
            }
        }
         
        */

        Vector3 _offset;
        public Vector3 Offset
        {
            get
            {

                if (_offset != Vector3.Zero)
                    return _offset;
                _offset = new Vector3(0, 0, _centreTarget.SideBoundary.Z);
                _offset = Vector3.Transform(_offset, _centreTarget.TransformNode.AbsoluteTransform);

                _offset -= ((SceneNodHierachyModel)((SceneNodHierachyModel)_centreTarget).Root).
                    TransformNode.Translate;

                return _offset;

            }
        }
        bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
        }

        public CombineToolPart(IGame game, PipeBase pipe,List<CombineToolPart> parts)
            : base(game, 1f, 16)
        {
            _centreTarget = pipe;
            this.ID = "FrontDragger";

            this._dragHandler = null;
            this._dragHandler += this.DragOnScreen;
            _parts = parts;
        }


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
            /*

            if (this.ID == "FrontDragger")*/
            this.TransformNode.Translate = 
                Vector3.Transform(
                _centreTarget.SideBoundary, _centreTarget.TransformNode.AbsoluteTransform);
            /*
            else
                this.TransformNode.Translate = _centreTarget.SideBCenter;*/
            
            this._selCompData.dataModifitionHandler.Invoke();
            base.Initialize();
        }
        protected override void OnModify()
        {
            base.OnModify();
        }

        public override void Update(GameTime gameTime)
        {
            if (!this._isInitialized)
                this.Initialize();


            base.Update(gameTime);


        }
        protected override void OnDragStart()
        {
            this._isActive = true;
           
            base.OnDragStart();
        }
        protected override void OnDragEnd()
        {
            this._isActive = false;

            base.OnDragEnd();
        }
        protected override void OnDragExe()
        {

            /*  Quaternion rot;
              Vector3 scale;
              Vector3 trans;

              _centreTarget.TransformNode.AbsoluteTransform.Decompose(out scale,out rot,out trans);
            */
            ((SceneNodHierachyModel)((SceneNodHierachyModel)_centreTarget).Root).
                TransformNode.Translate =
                this.TransformNode.Translate - Offset;



            MyConsole.WriteLine(_centreTarget.NodeNm + _offset.ToString());



            bool result=false;

            //fake initialized the OnRollPart, 
            //Because it needed, I don't know why,
            //stupid compiler
            CombineToolPart OnRollPart=_parts[0];
            foreach (CombineToolPart part in _parts)
            {
                result |=
                part._curState == InteractiveObjState.OnRollOver;
                if (part._curState == InteractiveObjState.OnRollOver)
                    OnRollPart = part;
            }


            //If not detecting the rollover
            if (!result)
                base.OnDragExe();
            else//If detecting, then snapping
            {

                this.TransformNode.Translate = OnRollPart.TransformNode.Translate;
            
            }

        }


        public override void Draw(GameTime gameTime,ICamera cam)
        {

            base.Draw(gameTime,cam);
        }
      
        protected override void Dispose(bool disposing)
        {
            
            base.Dispose(disposing);
        }
        public void Move(Vector3 centreTargetPivot)
        {


           // Vector3 deltaWorldSpace = Vector3.
                // Transform
                 //(new Vector3(0, 0, Offset * _centreTarget.Length),
                // Quaternion);
           // this._translation = centreTargetPivot + deltaWorldSpace;
            this.TransformNode.Translate = centreTargetPivot + Offset;
            this._selCompData.dataModifitionHandler.Invoke();
        }

    }

  

}