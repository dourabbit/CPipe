using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using XNAPhysicsLib;
using Microsoft.Xna.Framework.Input;
using XNASysLib.XNATools;
using VertexPipeline;
using XNASysLib.Primitives3D;
using Microsoft.Xna.Framework.Graphics;
using XNABuilder;
using VertexPipeline.Data;

namespace XNASysLib.XNAKernel
{
    public delegate void CursorEvent(); 
    public class DataReactor : aC_Reactor,IDrawableComponent
    {
        #region Fields
        protected IGame _game;
        protected static SysDataList<ISysData>  _sysDataList;
        protected bool _isIntialized;
        protected Point _mousePos;
        protected ICamera _cam;
        public ICamera Camera
        { set { _cam = value; } }
        protected Rectangle? _mouseSelection;
        public Rectangle? MouseSelection
        { get { return this._mouseSelection; } }
        protected Vector3[] _mouseSelectionWorldPos;
        protected Vector3 _selectionCentre;
        protected List<ISelectable> _hotReg;


        public TransformNode TransformNode
        { get; set; }

        public bool IsContainHotReg(ISelectable obj)
        {
            return _hotReg.Exists(delegate(ISelectable matcher)
            {
                return obj == matcher ? true : false;
            });

        }
        //public void AddHotReg(ISelectable obj)
        //{
        //    this._hotReg.Add(obj);
        //    _hotReg.Sort();
            
        //}
        public void DelHotReg(ISelectable obj)
        {
            for (int i = 0; i < this._hotReg.Count; i++)
            {
                if (_hotReg[i] == obj)
                {
                    _hotReg.RemoveAt(i);
                }
            }
        }
        public bool IsInHotReg(ISelectable obj)
        {
            return _hotReg.Contains(obj);
        }
        public string ID
        { get { return this._ID; } }
        #endregion
        //protected Matrix _world;
        DebugDraw _debugDraw;
        MouseData _mouseData;
        protected SceneHub _hub;
        public CursorEvent LostCursorHandler;
        public CursorEvent RetrieveCursorHandler;
        protected bool _isCursorLost;
        //Point? _start;

        #region Properties
        public MouseData? MouseData
        { get { return _mouseData; } }

        public bool IsCursorLost
        { get { return _isCursorLost; } }
        
        public SysDataList<ISysData> DataList
        {
            get { return _sysDataList; }
        }
        public Point? MousePos
        {
            get
            {
                    if (_isCursorLost)
                    return null;
                    

                   Point mouse= new Point(
                    _mousePos.X+ this._game.ActiveViewRect.X,
                    _mousePos.Y + this._game.ActiveViewRect.Y);

                  

                   return mouse;
             }
        }
        /*
        public List<ISelectable> HotReg
        {
            get
            {
                return _hotReg;
            }
        }*/
        /*public Matrix World
        {
            get { return _world; }
            set { _world = value; }
        }*/
        public static DataReactor
            GetDataReactor()
        {
            return (DataReactor)
                _reactorPool.Find(
                    delegate(aC_Reactor match)
                    {
                        return match is DataReactor ? true : false;
                    });
        }
       
        #endregion
        #region Constructor
        public DataReactor(IGame game):base()
        {
            _game = game;
            this._proceedType = new int[3] { -1, -2, -3 };
            _sysDataList =
                new SysDataList<ISysData>();
            this._hotReg = new List<ISelectable>();
            _isIntialized = false;
            //_world = Matrix.Identity;

            this.LostCursorHandler += OnLostCursor;
            this.RetrieveCursorHandler += OnRetriveCursor;
        }

        public static int GetDataSlot(Type type)
        {
           return _sysDataList.EmptySlot(type);
        }

        ~DataReactor()
        {
            Dispose(false);
        }
        #endregion

        #region Override aC_Reactor: ProceedEvent() Dispatch()
        
        /*void Debug(int index)
        {
            MouseData data = (MouseData)
            this._sysDataList.GetData(-1, 0);

            Console.Write(data.ISysDataTime.ToString() +
                "---" + "Data0: " +
                data.IsLeftBtnDown.ToString() + "  ");


            data = (MouseData)
            this._sysDataList.GetData(-1, 1);

            Console.Write("Data1: " +
                data.IsLeftBtnDown.ToString() + "\n");
        }
        */
        public override void Dispatch
            (ISysData gameData)
        {
            if (gameData.SysDataType >= 0)
                return;

            base.Dispatch(gameData);
        }
        public override void ProceedEvent
            (ISysData gameData)
        {

            _sysDataList.Add(gameData);

            if (gameData is MouseData)
            {
                _mouseData=(MouseData)gameData;
                this._mousePos = _mouseData.MousePos;

                if (_mouseData.SelectionRect.Width == 0 ||
                                    _mouseData.SelectionRect.Height == 0)
                    this._mouseSelection = null;
                else
                {
                    this._mouseSelection = _mouseData.SelectionRect;
                    this._selectionCentre = _mouseData.SelectionCentre;
                    this._mouseSelectionWorldPos = _mouseData.SelectionWorldPos;
                }
                
            }
            if (gameData is CameraData)
                this._cam = ((CameraData)gameData).Camera;
            

            base.ProceedEvent(gameData);
        }
        #endregion
        public virtual void Initialize()
        { 
            
            this._debugDraw = new 
                DebugDraw(_game.GraphicsDevice);

            if (this._cam == null)
                _cam = (ICamera)_game.Services.GetService(typeof(ICamera));
            
            MyContentManager contentManager=
                (MyContentManager)_game.Services.
                GetService(typeof(MyContentManager));
            
            ContentBuilder builder =
              (ContentBuilder)_game.Services.
              GetService(typeof(ContentBuilder));
           // builder.Add("_FontSetup","_FontSetup",);

            //contentManager.Load<NodesGrp>("_PipeA");
            SpriteFont font= contentManager.Load<SpriteFont>("_FontSetup");
            _hub = new SceneHub(_game, font);//, 
                //new Microsoft.Xna.Framework.Graphics.SpriteBatch(_game.GraphicsDevice));
            this.TransformNode = new VertexPipeline.TransformNode();
            _isIntialized = true;

        }
        protected virtual void OnLostCursor()
        {
            this._isCursorLost = true;
        }
        protected virtual void OnRetriveCursor()
        {
            this._isCursorLost = false;
        }
        #region Update
        public virtual void
            Update(GameTime gameTime)
        {
            if (!_isIntialized)
                this.Initialize();

           // this.TransformNode.UpdateTransform();

        }
        #endregion


        #region HandlerFuntions
        /// <summary>
        /// Checks whether a ray intersects a triangle. This uses the algorithm
        /// developed by Tomas Moller and Ben Trumbore, which was published in the
        /// Journal of Graphics Tools, volume 2, "Fast, Minimum Storage Ray-Triangle
        /// Intersection".
        /// 
        /// This method is implemented using the pass-by-reference versions of the
        /// XNA math functions. Using these overloads is generally not recommended,
        /// because they make the code less readable than the normal pass-by-value
        /// versions. This method can be called very frequently in a tight inner loop,
        /// however, so in this particular case the performance benefits from passing
        /// everything by reference outweigh the loss of readability.
        /// </summary>
        static void RayIntersectsTriangle(ref Ray ray,
                                          ref Vector3 vertex1,
                                          ref Vector3 vertex2,
                                          ref Vector3 vertex3, out float? result)
        {
            // Compute vectors along two edges of the triangle.
            Vector3 edge1, edge2;

            Vector3.Subtract(ref vertex2, ref vertex1, out edge1);
            Vector3.Subtract(ref vertex3, ref vertex1, out edge2);

            // Compute the determinant.
            Vector3 directionCrossEdge2;
            Vector3.Cross(ref ray.Direction, ref edge2, out directionCrossEdge2);

            float determinant;
            Vector3.Dot(ref edge1, ref directionCrossEdge2, out determinant);

            // If the ray is parallel to the triangle plane, there is no collision.
            if (determinant > -float.Epsilon && determinant < float.Epsilon)
            {
                result = null;
                return;
            }

            float inverseDeterminant = 1.0f / determinant;

            // Calculate the U parameter of the intersection point.
            Vector3 distanceVector;
            Vector3.Subtract(ref ray.Position, ref vertex1, out distanceVector);

            float triangleU;
            Vector3.Dot(ref distanceVector, ref directionCrossEdge2, out triangleU);
            triangleU *= inverseDeterminant;

            // Make sure it is inside the triangle.
            if (triangleU < 0 || triangleU > 1)
            {
                result = null;
                return;
            }

            // Calculate the V parameter of the intersection point.
            Vector3 distanceCrossEdge1;
            Vector3.Cross(ref distanceVector, ref edge1, out distanceCrossEdge1);

            float triangleV;
            Vector3.Dot(ref ray.Direction, ref distanceCrossEdge1, out triangleV);
            triangleV *= inverseDeterminant;

            // Make sure it is inside the triangle.
            if (triangleV < 0 || triangleU + triangleV > 1)
            {
                result = null;
                return;
            }

            // Compute the distance along the ray to the triangle.
            float rayDistance;
            Vector3.Dot(ref edge2, ref distanceCrossEdge1, out rayDistance);
            rayDistance *= inverseDeterminant;

            // Is the triangle behind the ray origin?
            if (rayDistance < 0)
            {
                result = null;
                return;
            }

            result = rayDistance;
        }
        static float? RayIntersectsModel(Ray ray, ShapeNode shape, TransformNode trans,
                                         BoundingSphere boundingSphere,
                                         out Vector3 vertex1, out Vector3 vertex2,
                                         out Vector3 vertex3)
        {
            vertex1 = vertex2 = vertex3 = Vector3.Zero;

            // The input ray is in world space, but our model data is stored in object
            // space. We would normally have to transform all the model data by the
            // modelTransform matrix, moving it into world space before we test it
            // against the ray. That transform can be slow if there are a lot of
            // triangles in the model, however, so instead we do the opposite.
            // Transforming our ray by the inverse modelTransform moves it into object
            // space, where we can test it directly against our model data. Since there
            // is only one ray but typically many triangles, doing things this way
            // around can be much faster.
            Matrix world = trans.AbsoluteTransform;//trans.World;
            Matrix inverseTransform = Matrix.Invert(world);
            Ray rayObjSpace;
            rayObjSpace.Position = Vector3.Transform(ray.Position, inverseTransform);
            rayObjSpace.Direction = Vector3.TransformNormal(ray.Direction, inverseTransform);

            if (boundingSphere.Intersects(ray) == null)
            {


                return null;
            }
            else
            {

                // Keep track of the closest triangle we found so far,
                // so we can always return the closest one.
                float? closestIntersection = null;

                Vector3[] tmp;
                shape.GetPositions(out tmp);
                //ushort[] indices = new ushort[shape.Indices.Count];
                //shape.IndexBuffer.GetData<ushort>(indices);

                Vector3[] vertices = new Vector3[shape.Indices.Count];
                for (int i = 0; i < shape.Indices.Count; i++)
                {
                    vertices[i] = tmp[shape.Indices[i]];
                }

                for (int i = 0; i < vertices.Length; i += 3)
                {
                    // Perform a ray to triangle intersection test.
                    float? intersection;

                    RayIntersectsTriangle(ref rayObjSpace,
                                          ref vertices[i],
                                          ref vertices[i + 1],
                                          ref vertices[i + 2],
                                          out intersection);

                    // Does the ray intersect this triangle?
                    if (intersection != null)
                    {
                        //// If so, is it closer than any other previous triangle?
                        //if ((closestIntersection == null) ||
                        //    (intersection < closestIntersection))
                        //{
                        //    // Store the distance to this triangle.
                        //    closestIntersection = intersection;


                        //    // Transform the three vertex positions into world space,
                        //    // and store them into the output vertex parameters.
                        //    Vector3.Transform(ref vertices[i],
                        //                      ref world, out vertex1);

                        //    Vector3.Transform(ref vertices[i + 1],
                        //                      ref world, out vertex2);

                        //    Vector3.Transform(ref vertices[i + 2],
                        //                      ref world, out vertex3);
                        //}
                        return intersection;
                    }
                }

                return closestIntersection;
            }
        }
        /// <summary>
        /// Obsolete
        /// </summary>
        /// <param name="frustum"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool CalculateIntersection
                (BoundingFrustum frustum,
                 SelectableCompData data)
        {
            bool isIntersected=false;

            foreach (BoundingSphere sphere in data.BoundingSpheres)
            //    isIntersected |= frustum.Intersects(sphere);
            { 
                
            
            }
      
            return isIntersected;
                
        }
        public float? CalculateIntersection
                (Ray ray,
                 SelectableCompData data)
        {
            if (data.BoundingSpheres == null)
                return null;

            Vector3 ver1;
            Vector3 ver2;
            Vector3 ver3;

            float? result = RayIntersectsModel(ray, data.shape, data.transform, 
                data.BoundingSpheres[0],out ver1, out ver2, out ver3);

            return result;
        }
        //public float? CalculateIntersection
        //        (Ray ray,
        //         SelectableCompData data)
        //{
        //    float? oldDist = null;
        //    bool isIntersected = false;
        //    if (data.BoundingSpheres == null)
        //        return null;
        //    foreach (BoundingSphere sphere in data.BoundingSpheres)
        //    {

        //        float? newDist = sphere.Intersects(ray);
        //        isIntersected |= newDist.HasValue;
        //        if (newDist.HasValue && !oldDist.HasValue)
        //            oldDist = newDist;
        //        if (newDist.HasValue && oldDist.HasValue
        //            && newDist.Value < oldDist.Value)
        //            oldDist = newDist;
        //    }


        //    return oldDist;

        //}
        BoundingFrustum CreateFrustum(Vector3 camPos, 
            Vector3 centrePos,
            float width, float height)
        {
            float depth = Vector3.Distance(camPos, centrePos);

            Matrix proj = Matrix.CreatePerspective(width,
                                height,
                                depth,1000f);

            Matrix view=Matrix.CreateLookAt(camPos,
                    centrePos,Vector3.Up);
            return new BoundingFrustum(Matrix.Multiply(view, proj));
        }
        
        // CalculateCursorRay Calculates a world space ray starting at the camera's
        // "eye" and pointing in the direction of the cursor. Viewport.Unproject is used
        // to accomplish this. see the accompanying documentation for more explanation
        // of the math behind this function.
        public Ray? CalculateCursorRay(ICamera cam)
        {
            if (!this.MousePos.HasValue)
                 return null;

            Vector2 position = new Vector2(this.MousePos.Value.X,
                this.MousePos.Value.Y);

            Matrix projectionMatrix=cam.ProjectionMatrix;
            Matrix viewMatrix=cam.ViewMatrix;

            // create 2 positions in screenspace using the cursor position. 0 is as
            // close as possible to the camera, 1 is as far away as possible.
            Vector3 nearSource = new Vector3(position, 0f);
            Vector3 farSource = new Vector3(position, 1f);

            // use Viewport.Unproject to tell what those two screen space positions
            // would be in world space. we'll need the projection matrix and view
            // matrix, which we have saved as member variables. We also need a world
            // matrix, which can just be identity.
            Vector3 nearPoint = _game.ActiveViewport.Unproject(nearSource,
                projectionMatrix, viewMatrix, Matrix.Identity);

            Vector3 farPoint = _game.ActiveViewport.Unproject(farSource,
                projectionMatrix, viewMatrix, Matrix.Identity);

            // find the direction vector that goes from the nearPoint to the farPoint
            // and normalize it....
            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            // and then create a new ray using nearPoint as the source.
            return new Ray(nearPoint, direction);
        }
        /// <summary>
        /// CalculateCursorRay Calculates a world space ray starting at the camera's
        /// "eye" and pointing in the direction of the cursor. Viewport.Unproject is used
        /// to accomplish this. see the accompanying documentation for more explanation
        /// of the math behind this function.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Ray CalculateCursorRay(Vector2 position)
        {
            Matrix projectionMatrix = _cam.ProjectionMatrix;
            Matrix viewMatrix = _cam.ViewMatrix;

            // create 2 positions in screenspace using the cursor position. 0 is as
            // close as possible to the camera, 1 is as far away as possible.
            Vector3 nearSource = new Vector3(position, 0f);
            Vector3 farSource = new Vector3(position, 1f);

            // use Viewport.Unproject to tell what those two screen space positions
            // would be in world space. we'll need the projection matrix and view
            // matrix, which we have saved as member variables. We also need a world
            // matrix, which can just be identity.
            Vector3 nearPoint = _game.ActiveViewport.Unproject(nearSource,
                projectionMatrix, viewMatrix, Matrix.Identity);

            Vector3 farPoint = _game.ActiveViewport.Unproject(farSource,
                projectionMatrix, viewMatrix, Matrix.Identity);

            // find the direction vector that goes from the nearPoint to the farPoint
            // and normalize it....
            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            // and then create a new ray using nearPoint as the source.
            return new Ray(nearPoint, direction);
        }

        public Vector3 ProjectObjInScreenSpace(Vector3 position)
        {
            return this._game.ActiveViewport.Project(position,_cam.ProjectionMatrix,_cam.ViewMatrix,Matrix.Identity);
        }

        #endregion


        public virtual void Draw(GameTime gameTime,ICamera cam)
        {
            _debugDraw.Begin(//Matrix.Multiply(this.TransformNode.World,cam.ViewMatrix),//this._world, cam.ViewMatrix),
                Matrix.Identity,
                cam.ViewMatrix,
                cam.ProjectionMatrix);
            _debugDraw.DrawWireGrid(Vector3.UnitX * 20, Vector3.UnitZ * 20,
                       new Vector3(-10f, 0, -10f), 20, 20, Color.Green);


            
           
            //foreach (ISelectable scomp in _hotReg)
            //    foreach(BoundingSphere bSphere in scomp.Data.BoundingSpheres)
            //        _debugDraw.DrawWireSphere(bSphere,Color.Red);
            
            _debugDraw.End();



            _hub.Draw(gameTime, "0", Vector3.Zero);
            _hub.Draw(gameTime, "10", new Vector3(10, 0, 10));
            _hub.Draw(gameTime, "-10", new Vector3(-10, 0, 10));

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            { 
            }
        }
    }

  
}
