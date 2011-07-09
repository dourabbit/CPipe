using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using XNASysLib.XNATools;
using VertexPipeline;

namespace XNASysLib.XNAKernel
{
    public class CamNavigationNotifier : aC_Reactor
    {
        IGame _game;
        ICamera _cam;
        MouseData? _oldData;


        float  _transMultiplier=0.01f;
        int _threshold = 1;


        public CamNavigationNotifier(IGame game)
        {
           
            this._proceedType = new int[1] { 0 };
            _game = game;
        }
        public override void Dispatch(ISysData gameData)
        {
            if(gameData is MouseData)
            base.Dispatch(gameData);
        }
        public override void ProceedEvent(ISysData gameData)
        { 
            MouseData data = (MouseData)gameData;

            if (!_oldData.HasValue)
            {
                _oldData = data;
                return;
            }
           

            _cam = (ICamera)_game.Services.
                    GetService(typeof(ICamera));
           

            
            try
            {
                if (!Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
                    return;
                else if (Mouse.GetState().LeftButton == ButtonState.Pressed )//&& data.IsMoving)
                {

                    //if (!(_cam is dCamera))
                    //    return;
                    /*
                    if (_game.Components.Exists(
                        delegate(IUpdatableComponent matcher)
                        {
                            return matcher is aCTool ?
                                true : false;
                        }))
                        return;
                    */
                    //dCamera cam = (dCamera)_cam;

                    if(data.MousePos.X<_oldData.Value.MousePos.X
                        &&Math.Abs( data.MousePos.X-_oldData.Value.MousePos.X)
                        >_threshold)
                      _cam.Rotate(Vector3.Up, 
                            Math.Abs(data.MousePos.X - _oldData.Value.MousePos.X)*
                            _transMultiplier);
                    else
                      _cam.Rotate(Vector3.Up, 
                            Math.Abs(data.MousePos.X - _oldData.Value.MousePos.X)*-1*
                            _transMultiplier);

                    if (data.MousePos.Y < _oldData.Value.MousePos.Y
                        &&Math.Abs( data.MousePos.Y- _oldData.Value.MousePos.Y)
                        > _threshold)
                        _cam.Rotate(Vector3.Right, 
                            Math.Abs(data.MousePos.Y - _oldData.Value.MousePos.Y) *
                            _transMultiplier);
                    else
                        _cam.Rotate(Vector3.Right,
                            Math.Abs(data.MousePos.Y - _oldData.Value.MousePos.Y) *-1
                            * _transMultiplier);
                }

                if (Mouse.GetState().MiddleButton == ButtonState.Pressed && data.IsMoving)
                {
                     if (data.MousePos.Y < _oldData.Value.MousePos.Y
                        &&Math.Abs( data.MousePos.Y- _oldData.Value.MousePos.Y)
                        > _threshold)
                        _cam.Translate(Vector3.Up * Math.Abs( data.MousePos.Y- _oldData.Value.MousePos.Y)* _transMultiplier);
                    else  if (data.MousePos.Y > _oldData.Value.MousePos.Y
                        &&Math.Abs( data.MousePos.Y- _oldData.Value.MousePos.Y)
                        > _threshold)
                        _cam.Translate(Vector3.Down* Math.Abs( data.MousePos.Y- _oldData.Value.MousePos.Y)* _transMultiplier);
                    
                     
                    else  if(data.MousePos.X<_oldData.Value.MousePos.X
                        &&Math.Abs( data.MousePos.X-_oldData.Value.MousePos.X)
                        >_threshold)
                        _cam.Translate(Vector3.Left * Math.Abs( data.MousePos.X-_oldData.Value.MousePos.X)* _transMultiplier);
                    else if(data.MousePos.X>_oldData.Value.MousePos.X
                        &&Math.Abs( data.MousePos.X-_oldData.Value.MousePos.X)
                        >_threshold)
                        _cam.Translate(Vector3.Right* Math.Abs( data.MousePos.X-_oldData.Value.MousePos.X)* _transMultiplier);
                }
                if (Mouse.GetState().RightButton==  ButtonState.Pressed&& data.IsMoving)
                {
                    if (data.MousePos.X < _oldData.Value.MousePos.X)
                        _cam.Translate(Vector3.Backward * 
                            Math.Abs( data.MousePos.X-_oldData.Value.MousePos.X)* 
                            _transMultiplier);
                    else
                        _cam.Translate(Vector3.Forward *
                            Math.Abs( data.MousePos.X-_oldData.Value.MousePos.X)* 
                            _transMultiplier);
                }

                new CameraData(this,this._cam);
            }
            catch
            { return; }
            base.ProceedEvent(gameData);
            //MyConsole.WriteLine(_oldData.Value.MousePos.X.ToString());
            _oldData = data;

        }
    
    }
}
