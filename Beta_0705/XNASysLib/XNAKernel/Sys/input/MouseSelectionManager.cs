using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Content;
using XNABuilder;
using VertexPipeline;

namespace XNASysLib.XNAKernel
{
    public class MouseSelectionManager:MouseManagerBase
    {



        #region Fields
        DataReactor _dataReactor;
        //protected MyContentManager _contentManager;
        protected ContentBuilder _contentBuilder;
        protected string _AssetNm = "Content/shaders/Tex2DDraw.fx";
        #endregion

        #region VertexBuffer Drawing Rect

        int numOfCorners = 4;
        public const float drawDepth = 0;

        VertexPositionColorTexture[] _points;
        short[] lineStripIndices;
        short[] QuadIndices;

        Texture2D _texture;
        Vector2 textureTopLeft = new Vector2(0.0f, 0.0f);
        Vector2 textureTopRight = new Vector2(1.0f, 0.0f);
        Vector2 textureBottomLeft = new Vector2(0.0f, 1.0f);
        Vector2 textureBottomRight = new Vector2(1.0f, 1.0f);

        Effect _effect;
        VertexDeclaration vertexDeclaration;
        VertexBuffer vertexBuffer;
        IndexBuffer LineIndexBuffer;
        IndexBuffer QuadIndexBuffer;

        //Matrix _viewMatrix;
        //Matrix _projectionMatrix;
        //Matrix _worldMatrix;
        //Viewport _viewPort;
        bool _selectable = true;
        Rectangle _selection;
        #endregion

        #region Properties
        private Vector3 CamPos
        {
            get
            { 
                Matrix viewInv = Matrix.Invert(this._camera.ViewMatrix);
                return new Vector3(viewInv.M41, viewInv.M42, viewInv.M43);
            
            }
        }
        public Rectangle Selection
        {
            get
            {
                return this._selection;
            }
        }
        public bool Selectable
        {
            set { this._selectable = value; }
            get { return this._selectable; }
        }

        #endregion

        #region Constructors
        public MouseSelectionManager
            (IGame game):base(game)
        {
            
        }
        #endregion

        #region Initialize
        public override void Initialize()
        {
            #region Vertex Buffer Initialization For Drawing Selection Rect
            _points = new VertexPositionColorTexture[numOfCorners];
            lineStripIndices = new short[numOfCorners + 1];

            _effect = new BasicEffect(_game.GraphicsDevice);
            this._camera = (ICamera)_game.Services.
               GetService(typeof(ICamera));
            /*_viewMatrix = Matrix.CreateLookAt(
                        new Vector3(0.0f, 0.0f, 1.0f),
                        Vector3.Zero,
                        Vector3.Up
                        );

            _projectionMatrix = Matrix.CreateOrthographicOffCenter(
                                0,
                                (float)_game.GraphicsDevice.Viewport.Width,
                                (float)_game.GraphicsDevice.Viewport.Height,
                                0,
                                1.0f, 1000.0f);
            */
            // Populate the array with references to indices in the vertex buffer.
            for (int i = 0; i < numOfCorners; i++)
            {
                lineStripIndices[i] = (short)(i);
            }
            lineStripIndices[numOfCorners] = 0;

            QuadIndices = new short[] {
                         0,  1,  3,  // front face 
                         1,  2,  3};
            #endregion

            _dataReactor = (DataReactor)
                _game.Services.GetService(typeof(DataReactor));
            this._contentBuilder = (ContentBuilder)_game.Services.
             GetService(typeof(ContentBuilder));
            //_contentManager = game.MyContentManager;
            _contentManager = (MyContentManager)_game.Services.
                GetService(typeof(MyContentManager));
         
            

            base.Initialize();

        }
        protected override void LoadContent()
        {

            string assetNm = Path.GetFileNameWithoutExtension(_AssetNm);

            this._contentBuilder.Add(this._AssetNm, assetNm, null, "EffectProcessor");
            string error = _contentBuilder.Build();
            if (string.IsNullOrEmpty(error))
                this._effect = _contentManager.Load<Effect>(assetNm);

           // _effect = _game.MyContentManager.Load<Effect>("Effects/Tex2DDraw");

            LineIndexBuffer = new IndexBuffer(_game.GraphicsDevice,
                                          IndexElementSize.SixteenBits,
                           sizeof(short) * this.lineStripIndices.Length,
                           BufferUsage.None);

            vertexDeclaration = VertexPositionColorTexture.VertexDeclaration;

            vertexBuffer = new VertexBuffer(_game.GraphicsDevice,
                            vertexDeclaration,
                            _points.Length,
                            BufferUsage.None
                            );


            vertexBuffer.SetData<VertexPositionColorTexture>(_points);
            LineIndexBuffer.SetData<short>(lineStripIndices);

            QuadIndexBuffer = new IndexBuffer(_game.GraphicsDevice,
                          IndexElementSize.SixteenBits,
                          QuadIndices.Length,
                          BufferUsage.None);
            QuadIndexBuffer.SetData<short>(QuadIndices);

            base.LoadContent();
        }
        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {
            if (!_isInitialized)
            {
                this.Initialize();
                _isInitialized = true;
            }
            //_curViewport = _cam.Viewport;

           // if (!this._dataReactor.MousePos.HasValue)
             //   return;

            BeginCheckMouseState();

            //if(this._dataReactor.MousePos.HasValue)
                //UpdateSelection(this._dataReactor.MousePos);

            EndCheckMouseState();

            UpdateMouseData(gameTime);

           
        }
        
        //TempCode
        bool IsSkip(Keys key)
        {
            return
             (Keyboard.GetState().IsKeyDown(Keys.LeftAlt))?
             true:false;

        }
        void UpdateSelection(Point WorldPos)
        {

            if (oldMouseState.LeftButton == ButtonState.Released)
            {
                this._selection = Rectangle.Empty;
                this._mouseData.SelectionRect = this._selection;
            }

            if (IsSkip(Keys.LeftAlt))
                return;

            if (_isLeftBtnDown && oldMouseState.LeftButton == ButtonState.Released)
            {
                this._selection.X = WorldPos.X;
                this._selection.Y = WorldPos.Y;
            }

            if (_isLeftBtnUp)
            {
                if (oldMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (this._selection.Width == 0 || this._selection.Height == 0)
                        this._selection.Width = this._selection.Height = 1;
                    this._mouseData.SelectionRect = this._selection;
                    //= new Rectangle(this._points[0].Position.,);
                    this._mouseData.SelectionWorldPos = new Vector3[4]
                    {
                        _points[0].Position,
                        _points[1].Position,
                        _points[2].Position,
                        _points[3].Position
                    };
                    Vector3 nearSource = new Vector3(this._selection.Center.X,
                                                    this._selection.Center.Y, 0f);
                    Vector3 farSource = new Vector3(nearSource.X, nearSource.Y, 1f);
                    this._mouseData.SelectionCentre = getCorner(nearSource, farSource,CamPos);
                }
            }

            this._selection.Width = WorldPos.X
                 - this._selection.X;
            this._selection.Height = -WorldPos.Y
                 + this._selection.Y;
        }

        #endregion

        #region Draw
        Vector3 getCorner(Vector3 nearSource, Vector3 farSource, Vector3 camPos)
        {
                Vector3 nearPoint = _game.GraphicsDevice.Viewport.Unproject(nearSource,
                    this._camera.ProjectionMatrix, this._camera.ViewMatrix, Matrix.Identity);
                
                Vector3 farPoint = _game.GraphicsDevice.Viewport.Unproject(farSource,
                        this._camera.ProjectionMatrix, this._camera.ViewMatrix, Matrix.Identity);
                
                Vector3 direction = farPoint - nearPoint;
                direction.Normalize();
                return camPos+direction;
                
        }
        void updateRectPos()
        {
            #region Vertex Buffer: Update Selection Rect
            Vector3 nearSource = new Vector3(this._selection.X, this._selection.Y, 0f);
            Vector3 farSource = new Vector3(nearSource.X, nearSource.Y, 1f);

            this._points[0] = new VertexPositionColorTexture
                                (getCorner(nearSource, farSource, CamPos),
                                Color.Red, textureTopLeft);


            nearSource = new Vector3(this._selection.X + this._selection.Width,
                                            this._selection.Y, 0f);
            farSource = new Vector3(nearSource.X, nearSource.Y, 1f);
            this._points[1] = new VertexPositionColorTexture
                                (getCorner(nearSource, farSource, CamPos),
                                Color.Red, textureTopRight);

            nearSource = new Vector3(this._selection.X + this._selection.Width,
                                            this._selection.Y - this._selection.Height, 0f);
            farSource = new Vector3(nearSource.X, nearSource.Y, 1f);
            this._points[2] = new VertexPositionColorTexture
                                (getCorner(nearSource, farSource, CamPos),
                                Color.Red, textureTopRight);

            nearSource = new Vector3(this._selection.X,
                                          this._selection.Y - this._selection.Height, 0f);
            farSource = new Vector3(nearSource.X, nearSource.Y, 1f);
            this._points[3] = new VertexPositionColorTexture
                                (getCorner(nearSource, farSource, CamPos),
                                Color.Red, textureTopRight);

            /*                
            this._points[0] = new VertexPositionColorTexture
                                (new Vector3(this._selection.X, this._selection.Y, drawDepth),
                                Color.Red, textureTopLeft);

            this._points[1] = new VertexPositionColorTexture
                                (new Vector3(this._selection.X + this._selection.Width,
                                            this._selection.Y, drawDepth),
                                Color.Red, textureTopRight);

            this._points[2] = new VertexPositionColorTexture
                                (new Vector3(this._selection.X + this._selection.Width,
                                               this._selection.Y - this._selection.Height,
                                               drawDepth),
                                Color.Red, textureBottomRight);

            this._points[3] = new VertexPositionColorTexture
                                (new Vector3(this._selection.X,
                                            this._selection.Y - this._selection.Height, drawDepth),
                                Color.Red, textureBottomLeft);


            */
            #endregion

        }
        public override void Draw(GameTime gameTime,ICamera cam)
        {
            if (!_selectable)
                return;

            //if (!this._dataReactor.MousePos.HasValue)
              //  return;

            if (IsSkip(Keys.LeftAlt))
                return;

            if (_isLeftBtnDown && this._selection.Height != 0)
            {




                _effect.Parameters["WorldViewProj"].SetValue
                    (Matrix.Multiply(cam.ViewMatrix, cam.ProjectionMatrix));

                _effect.CurrentTechnique = _effect.Techniques["TransformAndColor"];
                _effect.Parameters["Col"].SetValue(new Vector4(1f, 1f, 1f, 0.5f));
                GraphicsDevice device = _effect.GraphicsDevice;
                device.DepthStencilState = DepthStencilState.Default;
                device.BlendState = BlendState.AlphaBlend;   //DrawHelper.BlendAdd;
                
                

               // _game.GraphicsDevice.RenderState.DepthBufferEnable = true;
               // _game.GraphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.CullMode = CullMode.None;
               
                _game.GraphicsDevice.RasterizerState = rasterizerState;

                this.updateRectPos();

                /*
                _game.GraphicsDevice.VertexDeclaration = vertexDeclaration;
                _game.GraphicsDevice.Vertices[0].SetSource(
               vertexBuffer,
               0,
               VertexPositionColorTexture.SizeInBytes);


                GraphicsDevice.RenderState.AlphaBlendEnable = true;
                 GraphicsDevice.Indices = QuadIndexBuffer;*/
                //Rect

              
          
                /*
                #region draw Rect
                

                //_effect.Begin();
                foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
                {
                    //pass.Begin();
                    pass.Apply();

                    _game.GraphicsDevice.DrawUserIndexedPrimitives
                        <VertexPositionColorTexture>(
                        PrimitiveType.TriangleList,
                        this._points,
                        0,   // vertex buffer offset to add to each element of the index buffer
                        this.numOfCorners,   // number of vertices to draw
                        QuadIndices,
                        0,   // first index element to read
                        2    // number of primitives to draw
                        );

                    //pass.End();
                }
                //_effect.End();
                #endregion
                */
                //GraphicsDevice.RenderState.AlphaBlendEnable = false;
                device.BlendState = BlendState.Opaque;
                //Line
                #region draw Line
               // GraphicsDevice.Indices = LineIndexBuffer;



                //_effect.Begin();
                foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
                {
                    //pass.Begin();

                    pass.Apply();
                    _game.GraphicsDevice.DrawUserIndexedPrimitives
                        <VertexPositionColorTexture>(

                        PrimitiveType.LineStrip,
                        this._points,
                        0,   // vertex buffer offset to add to each element of the index buffer
                        this.numOfCorners,   // number of vertices to draw
                        lineStripIndices,
                        0,   // first index element to read
                        this.numOfCorners    // number of primitives to draw
                        );

                    //pass.End();
                }
                //_effect.End();
                #endregion

                #region draw Point
                // GraphicsDevice.Indices = LineIndexBuffer;


                short[] pointIndice = new short[2];
                pointIndice[0] = 0;
                pointIndice[1] = 1;
                VertexPositionColorTexture[] p = new VertexPositionColorTexture[2];
                p[0] = new VertexPositionColorTexture(this.CamPos, Color.Aqua,Vector2.Zero);
                p[1] = new VertexPositionColorTexture(this._mouseData.SelectionCentre, Color.Aqua,Vector2.Zero);
                //_effect.Begin();
                foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
                {
                    //pass.Begin();
                    
                    pass.Apply();
                    _game.GraphicsDevice.DrawUserIndexedPrimitives
                        <VertexPositionColorTexture>(

                        PrimitiveType.LineStrip,
                        p,
                        0,   // vertex buffer offset to add to each element of the index buffer
                        2,   // number of vertices to draw
                        pointIndice,
                        0,   // first index element to read
                        1    // number of primitives to draw
                        );

                    //pass.End();
                }
                //_effect.End();
                #endregion
            }
            base.Draw(gameTime, cam);
        }
        #endregion
    }
}
