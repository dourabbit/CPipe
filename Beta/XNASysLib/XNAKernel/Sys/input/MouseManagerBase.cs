using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VertexPipeline;

namespace XNASysLib.XNAKernel
{


    public class MouseManagerBase : DrawableComponent
    {
        #region Fields
        //protected IGame _game;
        protected MouseState mouseState;
        protected MouseState oldMouseState;

        protected int _mouseX;
        protected int _mouseY;
        protected bool _isMoved;
        //protected bool _isInitialized=false;
        protected MouseData _mouseData;
        #endregion
        protected const double MoveThreshold = 3.0;
        protected const int  MouseHistory = 10;


        Queue<MouseData> _mouseHistory;

        #region Properties

        public bool IsMoved
        {
            get { return this._isMoved; }
        }

        public bool IsLeftBtnDown
        {
            get { return this._isLeftBtnDown; }
        }
        protected bool _isLeftBtnDown;

        public bool IsLeftBtnUp
        {
            get { return this._isLeftBtnUp; }
        }
        protected bool _isLeftBtnUp;

        public bool IsRightBtnDown
        {
            get { return this._isRightBtnDown; }
        }
        protected bool _isRightBtnDown;

        public bool IsRightBtnUp
        {
            get { return this._isRightBtnUp; }
        }
        protected bool _isRightBtnUp;
        public Point MousePos
        {
            get
            {
                return new Point
                    (_mouseX, _mouseY);
            }
        }
        public bool IsClicking
        {
            get
            {
                return (IsLeftBtnDown ^ IsRightBtnDown);
            }
        }
        #endregion
        public static MouseManagerBase singleton;
        public MouseManagerBase(IGame game)
            : base(game)
        {
            this._game = game;
            _mouseData = new MouseData();
            _mouseHistory = new Queue<MouseData>();
        }
        public override void Update(GameTime gameTime)
        {
            CheckMouseState();
            UpdateMouseData(gameTime);
        }
        protected void UpdateMouseData(GameTime gameTime)
        {
            if(gameTime!=null)
            _mouseData.SysDataTime = gameTime.TotalGameTime.TotalMilliseconds;


            _mouseData.IsLeftBtnDown = _isLeftBtnDown;
            _mouseData.IsLeftBtnUp = _isLeftBtnUp;
            _mouseData.IsMoving = _isMoved;
            _mouseData.IsRightBtnUp = _isRightBtnUp;
            _mouseData.IsRightBtnDown = _isRightBtnDown;
            _mouseData.MousePos =
                new Point(_mouseX, _mouseY);
           
            _mouseData.ISysInvoker();
           // MyConsole.WriteLine("InvokeMouse");
            _mouseHistory.Enqueue(_mouseData);
            if (_mouseHistory.Count > MouseHistory)
                _mouseHistory.Dequeue();

        }
        protected void BeginCheckMouseState()
        {

            mouseState = Mouse.GetState();

            this._mouseX = mouseState.X;
            this._mouseY = mouseState.Y;
            this._isMoved = IsMoving();

            #region Check if btn was Pressed
            ////////////////////////////////////////////////
            /////Left///////////////////////////////////////
            ////////////////////////////////////////////////



            _isLeftBtnDown =
                mouseState.LeftButton == ButtonState.Pressed ?
                true : false;

            if (mouseState.LeftButton == ButtonState.Pressed)
            {

                if (oldMouseState.LeftButton == ButtonState.Released)
                    this._isLeftBtnDown = true;
                else
                    this._isLeftBtnDown = false;

            }
           
            if (mouseState.LeftButton == ButtonState.Released)
            {

                if (oldMouseState.LeftButton == ButtonState.Pressed)
                    this._isLeftBtnUp = true;
                else
                    this._isLeftBtnUp = false;

            }
            else
                this._isLeftBtnUp = false;
            /////////////////////////////////////////////////
            /////Right///////////////////////////////////////
            /////////////////////////////////////////////////

            _isRightBtnDown =
                mouseState.RightButton == ButtonState.Pressed ?
                true : false;
            if (mouseState.RightButton == ButtonState.Released)
            {

                _isRightBtnUp =
                    oldMouseState.RightButton == ButtonState.Pressed ?
                        true : false;
            }
            else
                this._isRightBtnUp = false;
            #endregion
        }
        protected void EndCheckMouseState()
        {

            if (oldMouseState == mouseState)
                return;

            oldMouseState = mouseState;

        }
        protected void CheckMouseState()
        {
            BeginCheckMouseState();
            EndCheckMouseState();
        }
        bool IsMoving()
        {
            if (this._mouseHistory.Count!=0&&(
                Math.Abs(mouseState.X - this._mouseHistory.Peek().MousePos.X)>MoveThreshold||
                Math.Abs(mouseState.Y - this._mouseHistory.Peek().MousePos.Y)>MoveThreshold))
                return true;
            return false;
        }

      
    }
}
