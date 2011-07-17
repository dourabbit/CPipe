#region File Description
//-----------------------------------------------------------------------------
// ModelViewerControl.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNASysLib.XNAKernel;
using XNABuilder;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using XNASysLib.Primitives3D;
using XNASysLib.XNATools;
using System.Collections.Generic;
//using XNASysLib.Display;
using VertexPipeline;
#endregion

namespace WinFormsContentLoading
{
    /// <summary>
    /// Example control inherits from GraphicsDeviceControl, and displays
    /// a spinning 3D model. The main form class is responsible for loading
    /// the model: this control just displays it.
    /// </summary>
    public class SceneEntry : GraphicsDeviceControl
    {


        static Scene _scene;
        static ContentBuilder contentBuilder;
        static MyContentManager contentManager;
        static DataReactor _dataReactor;
        System.Drawing.Point _leftTopCorner;

        static bool _isIntialized = false;

        bool _isActivate;
        ICamera _cam;
        public ICamera Cam
        {
            get 
            {
                if (_cam != null)
                    return _cam;

                List<ISelectable> cams =
                    SelectFunction.Select(
                        delegate(IUpdatableComponent matcher)
                        {
                            Type[] types = matcher.GetType().GetInterfaces();
                            bool checker = false;
                            
                            foreach (Type type in types)
                                checker |= 
                                    type == typeof(ICamera) ? true : false;
                            
                            return checker;
                        });
                
                ICamera firstCam = (ICamera)cams[0];
                return firstCam;

            }
            set { _cam = value; } 
        }
        public bool IsActivate
        {
            get { return _isActivate; }
            set {_isActivate=value;}
        }

        public static List<SceneEntry> SceneEntries; 

        public Rectangle ActiveRegion
        {
            get 
            {
 
                _leftTopCorner=
                    this.PointToScreen(new System.Drawing.Point(0,0));

                
                return new Rectangle(_leftTopCorner.X,_leftTopCorner.Y,
                                    this.Width,this.Height);
            }
        
        }
        //public string SceneID;
        
        public static Scene Scene
        {
            get { return _scene; }
        }
        public SceneEntry()
            : base()
        {
            if (SceneEntries == null)
                SceneEntries = new List<SceneEntry>();

            SceneEntries.Add(this);

            if (_scene == null)
            {
                _scene = new Scene();
                _dataReactor = new DataReactor(_scene);
                //New Notifier
                new SysEventNotifier(_scene);
                new CamNavigationNotifier(_scene);
                new ObjDataReactor(_scene);
            }
            this.IsActivate = false;
        }

        /// <summary>
        /// Initializes the control.
        /// </summary>
        protected override void Initialize()
        {

            if (_isIntialized)
                return;

            // Hook the idle event to constantly redraw our animation.
            Application.Idle += delegate { Invalidate(); };

            /* _scene.GraphicsDevice = (GraphicsDevice)this.Services.
                 GetService(typeof(GraphicsDevice));*/



            _scene.GraphicsDevice = ((IGraphicsDeviceService)this.Services.
                GetService(typeof(IGraphicsDeviceService))).GraphicsDevice;

            contentBuilder = new ContentBuilder();

            contentManager = new MyContentManager(this.Services,
                                                contentBuilder.OutputDirectory);

            dCamera cam = new dCamera(_scene);
            dCamOrth front = new dCamOrth(new Vector3(0,0,10),new Vector3(0,0,0),_scene);
            front.ID = "ф╫йсм╪";
            _scene.Components.Add(front);
            //dCamera cam2 = new dCamera(_scene);
            //new SelectFunction(_scene);
            //new Compass(_scene);

            //_scene.Components.Add(cam);
            _scene.Components.Add(new MouseSelectionManager(_scene));
            _scene.Components.Add(new SelectFunction(_scene));
            _scene.Components.Add(_dataReactor);
            
            _scene.Services = this.Services;
            _scene.Services.AddService<ContentBuilder>(contentBuilder);
            _scene.Services.AddService<MyContentManager>(contentManager);
            _scene.Services.AddService<ICamera>(cam);

           
            _scene.Services.AddService<DataReactor>(_dataReactor);


            //
            //this.MouseMove += new MouseEventHandler(RegistMouse);
            //this.MouseLeave += new EventHandler(UnRegistMouse);
           // this.ClientSizeChanged += new EventHandler(OnSizeChanged);
           

            _isIntialized = true;

         
            
        }
        /*
        void OnSizeChanged(object sender, EventArgs e)
        {
            this.IsActive = this._isActive;
        }
        */
        protected override void OnMouseEnter(EventArgs e)
        {

            DataReactor dataR =
           (DataReactor)_scene.Services.
           GetService(typeof(DataReactor));

            dataR.RetrieveCursorHandler.Invoke();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
             DataReactor dataR=
            (DataReactor)_scene.Services.
            GetService(typeof(DataReactor));

            dataR.LostCursorHandler.Invoke();
            base.OnMouseLeave(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!IsActivate)
            {
                foreach (SceneEntry curEntry in SceneEntries)
                {
                    if (curEntry.Name != this.Name)
                    {

                        curEntry.Refresh();

                    }
                }
                return;
            }
            this.Draw(e.Graphics);

            foreach (SceneEntry curEntry in SceneEntries)
            {
                if (curEntry.Name != this.Name)
                {
                    curEntry.Draw(e.Graphics);
                }
            }
            base.OnPaint(e);
        }

        /// <summary>
        /// Draws the control.
        /// </summary>
        protected override void Render()
        {
           string a= this.Name;
            _scene.Update();
            _scene.Draw(Cam);
            /*
            ((MainForm)this.Parent.Parent.Parent.Parent.Parent
               .Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent).test();*/

        }
        /*
        void CheckRegisting(Rectangle rectangle)
        {


            this.IsActive=
            (rectangle.Intersects(new Rectangle(Mouse.GetState().X,
                                Mouse.GetState().Y, 0, 0)));
                
            
        }*/


       


    }
}
