
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
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization;

#endregion

namespace VertexPipeline
{
    //public delegate void UpdateHandler();

    public interface ISceneModel : INode,ITransObj, IUpdatableComponent
    {
        Type Type
        {
            get;
            set;
        }
        ShapeNode ShapeNode
        {
            get;
            set;
        }
        List<INode> FlattenNods
        {
            get;
        }
    }
    public partial class Scene: IGame
    {
        #region Fields
        protected string ID;
        protected GraphicsDevice _device;
        protected MyContentManager _contentManager;
        protected GameTime _gameTime;
        protected GameComponents<IUpdatableComponent> _components;
        protected bool _visible;
        protected static ServiceContainer _service;
        protected Rectangle _curViewRect;
        protected Viewport _curViewport;
        public static List<Scene> Scenes = new List<Scene>();
        #endregion
        //public SysEvent SysEvnHandler;
        //public event UpdateHandler UpdateScene;
        #region Properties

        public List<ISceneModel> Models
        {
            get 
            {
                List<ISceneModel> result= new List<ISceneModel>();
                foreach (IUpdatableComponent comp in _components.FindAll(
                   delegate(IUpdatableComponent matcher)
                   {
                       return matcher is ISceneModel ? true : false;
                   })) 
                   {
                       result.Add((ISceneModel)comp);
                    }

                return result;
            }
            set
            {
                foreach (ISceneModel model in value)
                    _components.Add(model);
            }
        
        }

        public Rectangle ActiveViewRect
        {
            get { return _curViewRect; }
            set { _curViewRect = value; }
        }
        public Viewport ActiveViewport
        {
            get { return _curViewport; }
            set { _curViewport = value; }
        }
        public ServiceContainer Services
        {
            get { return _service; }
            set { _service = value; }
        }
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }
        public GraphicsDevice GraphicsDevice
        {
            get { return _device; }
            set { _device = value; }
        }
        public MyContentManager MyContentManager
        {
            get { return _contentManager; }
        }
        public GameTime GameTime
        {
            get { return _gameTime; }
        }
        public GameComponents<IUpdatableComponent> Components
        {
            get { return _components; }
        } 
        #endregion
        
        public Scene()
        {
            _components = new GameComponents<IUpdatableComponent>(this);
            Scenes.Add(this);
            this.Initialize();
        }
        //public Scene(SerializationInfo info, StreamingContext ctxt)
        //{
        //    this.Models = (List<ISceneModel>)info.GetValue("Models", typeof(List<ISceneModel>));
        
        //}
        //public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        //{
        //    info.AddValue("Models", this.Models);
        //}
        protected virtual void Initialize()
        {
            _visible = true;
            
        }

      
        public virtual void Update()
        {
            //if (this.UpdateScene != null)
            //    this.UpdateScene.Invoke();

            try
            {
                foreach (IUpdatableComponent updatable in this._components)
                    updatable.Update(this._gameTime);

            }
            catch (Exception e)
            {
                MyConsole.WriteLine(e.Message);
            }
        }

        public virtual void Draw(ICamera cam)
        {
            try
            {


                if (_visible)
                {
                    foreach (IDrawableComponent drawable in this._components)
                    {

                            drawable.Draw(this._gameTime,cam);
                    }
                }
            }
            catch (Exception e)
            {
                MyConsole.WriteLine(e.Message);
            }
        }

        public void Add(IUpdatableComponent component)
        {
            this._components.Add(component);
        }

        

    
    }
}
