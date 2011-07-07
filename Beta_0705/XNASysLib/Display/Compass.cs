using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Content;
using XNABuilder;
using XNASysLib.XNAKernel;
using VertexPipeline;

namespace XNASysLib.Display
{
    public class Compass: DrawableComponent
    {



        #region Fields
        DataReactor _dataReactor;
        //MyContentManager _contentManager;
        ContentBuilder _contentBuilder;
        string _AssetNm = "Content/Compass.fbx";
        //ICamera _camera;
        Model _model;
        int _width;
        int _height;
        RenderTarget2D rt;
        
        SpriteBatch _spriteBatch;
        #endregion

        #region VertexBuffer Drawing Rect

        
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


        #endregion

        #region Constructors
        public Compass
            (IGame game):base(game)
        {
            _world = Matrix.Identity;
            _width = 50;
            _height = 50;
            _ID = "Compass";
        }
        #endregion

        #region Initialize
        public override void Initialize()
        {
            


            _dataReactor = (DataReactor)
                _game.Services.GetService(typeof(DataReactor));

            _contentBuilder = (ContentBuilder)_game.Services.
             GetService(typeof(ContentBuilder));

            _contentManager = (MyContentManager)_game.Services.
                GetService(typeof(MyContentManager));



            
         
            base.Initialize();

        }
        protected override void LoadContent()
        {

            string assetNm = Path.GetFileNameWithoutExtension(_AssetNm);

            this._contentBuilder.Add(this._AssetNm, assetNm, null, "VertexProcessor");
            string error = _contentBuilder.Build();
            if (string.IsNullOrEmpty(error))
                this._model = _contentManager.Load<Model>(assetNm);


            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
            base.LoadContent();
        }
        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {
            if (!_isInitialized)
            {
                this.Initialize();

            }

        }
    

     

        #endregion

        #region Draw
      

        public override void Draw(GameTime gameTime)
        {
            //Texture2D tex = new Texture2D(_game.GraphicsDevice,_width,_height);
            //return;
            Viewport viewport = _game.GraphicsDevice.Viewport;
            int leftTopX = viewport.Width - _width;
            int leftTopY = viewport.Height - _height;
            using (rt = new RenderTarget2D(_game.GraphicsDevice, _width, _height))
            {
                _game.GraphicsDevice.SetRenderTarget(rt);

                // Draw the model.
                foreach (ModelMesh mesh in _model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = _world;

                        /*Vector3 camPos;
                        _camera.Translate(new Vector3(0,0,0), out camPos);

                        Matrix viewMatrix = Matrix.Invert(Matrix.CreateFromQuaternion(_camera.RotationQuat) *
                                   Matrix.CreateTranslation(camPos));
                        effect.View = viewMatrix;*/
                        
                        effect.View = _camera.ViewMatrix;
                        effect.Projection = _camera.ProjectionMatrix;
                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;
                        effect.SpecularPower = 16;
                    }

                    mesh.Draw();
                }

                _game.GraphicsDevice.SetRenderTarget(null);

                // int width=_game.GraphicsDevice.Viewport.Width;
                // int height=_game.GraphicsDevice.Viewport.Height;



                _spriteBatch.Begin(0, BlendState.Opaque, null, null, null, null);
                _spriteBatch.Draw(rt,  new Rectangle(0, 0, _width,_height), Color.White);

                _spriteBatch.End();
            }

            _game.GraphicsDevice.Viewport = viewport;
            base.Draw(gameTime);
        }
        #endregion
    }
}
