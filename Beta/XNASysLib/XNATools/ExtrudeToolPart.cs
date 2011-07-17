using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexPipeline;
using XNASysLib.Primitives3D;
using XNASysLib.XNAKernel;

namespace XNASysLib.XNATools
{


  
    public class ExtrudeToolPart : DraggableSphere, IHotSpot
    {
        
        PipeBase _centreTarget;
        float _offset;
        //public OnSelected SelectionHandler
        //{
        //    get { return _selCompData.SelectionHandler; }
        //    set { _selCompData.SelectionHandler = value; }
        //}
    
        bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
        }

        public ExtrudeToolPart(IGame game, PipeBase pipe, float offset)
            : base(game, 1f, 16)
        {
            _centreTarget = pipe;
            _offset = offset;

            if (_offset == 1)
                this.ID = "FrontDragger";
            else
                this._ID = "RearDragger";

            this._dragHandler = null;
            this._dragHandler += DragOnLocalZ;
        }
        void DragOnLocalZ()
        {

            Vector2 mousePos = new Vector2
                (_dataReactor.MousePos.Value.X,
                    _dataReactor.MousePos.Value.Y);


            Quaternion rotQuat;
            Vector3 trans;
            Vector3 scale;


            _centreTarget.TransformNode.
                   AbsoluteTransform.Decompose
                   (out scale, out rotQuat, out trans);



            
            Vector3 norX = Vector3.Transform(Vector3.UnitX, rotQuat);
            norX.Normalize();

            Vector3 tmp=this.DragPosition-Vector3.Zero;
            tmp.Normalize();

            float cosTheta= Vector3.Dot(norX,tmp);
            float d= cosTheta*(this.DragPosition.Length());
            Plane planeX = new Plane(norX,d);



            Vector3 norY = Vector3.Transform(Vector3.UnitY, rotQuat);
            norX.Normalize();

            tmp = this.DragPosition - Vector3.Zero;
            tmp.Normalize();

            cosTheta = Vector3.Dot(norY, tmp);
            d = cosTheta * (this.DragPosition.Length());

            Plane planeY = new Plane(norY, d);

            //GetPlane
            Plane interPlane =
                getPlane(this._dataReactor.CalculateCursorRay
                (mousePos),planeX,planeY);
            //Get Original Point on Plane
            Vector3 startVec = castRay(this._mouseIntialPos.Value, interPlane);
            //Get End Point on Plane
            Vector3 endVec = castRay(_dataReactor.MousePos.Value, interPlane);


            //Projecting the mouse Vec into local Z axis
            Vector3 nor = Vector3.Transform(Vector3.UnitZ, rotQuat);
            Vector3 vec = endVec - startVec;
            cosTheta = Vector3.Dot(vec,nor);
            float dis = vec.Length()*cosTheta;

            
            Vector3 deltaPos=new Vector3(
                dis*Vector3.Dot(nor,Vector3.UnitX),
                dis*Vector3.Dot(nor,Vector3.UnitY),
                dis*Vector3.Dot(nor,Vector3.UnitZ));

            //this._translation = _primitiveIntialPos + deltaPos;
            this.TransformNode.Translate = _primitiveIntialPos + deltaPos;
            this._selCompData.dataModifitionHandler.Invoke();
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

            this.TransformNode.Translate =
            Vector3.Transform(
            _centreTarget.SideBoundary, _centreTarget.TransformNode.AbsoluteTransform);

            
            
            this._selCompData.dataModifitionHandler.Invoke();
            base.Initialize();
        }
        protected override void OnModifyBoundingSpheres()
        {
            base.OnModifyBoundingSpheres();
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
            
            base.OnDragExe();
        }
    

        public override void Draw(GameTime gameTime,ICamera cam)
        {

            base.Draw(gameTime,cam);
        }
      
        protected override void Dispose(bool disposing)
        {
            
            base.Dispose(disposing);
        }

    }

  

}