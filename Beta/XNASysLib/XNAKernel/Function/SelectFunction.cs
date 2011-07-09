using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using XNAPhysicsLib;
using XNASysLib.XNATools;
using XNASysLib.Primitives3D;
using VertexPipeline;

namespace XNASysLib.XNAKernel
{
    public delegate void SelectionChanged();
    
    public class SelectFunction:DrawableComponent
    {
        DebugDraw _debugDraw;
        ICamera _cam;

        


        public static SelectionChanged OnSelectionUpdate;

        static List<ISelectable> _oldSelection = new List<ISelectable>();
        static List<ISelectable> _selection
        = new List<ISelectable>();

        static IGame _gameRef;

        public SelectFunction(IGame game)
            : base(game)
        {
            _gameRef = game;
        }

        public static List<ISelectable> Selection
        { get { return _selection; } }
        
        public static void Select(ISelectable obj)
        {
            //Check the list before add
            if (_selection.Contains(obj) && _selection.Count == 1)
                return;

            if (IsSelected(obj))
                return;

            _selection.Clear();
            
            /*
            if (obj is SceneCompileObj)
            {
                aCTool tool = (aCTool)_gameRef.Components.Find(
                    delegate(IUpdatableComponent matcher)
                    {
                        return matcher is aCTool ?
                            true : false;
                    });

                if (tool != null)
                    tool.Dispose();
            }

            */
            _selection.Add(obj);
            newSelEvn(obj);

            //obj.Data.SelectionHandler.Invoke(obj,true);
        }

        static void newSelEvn(ISelectable obj)
        {
            ISceneNod model = obj as ISceneNod;


            Stack<string> nodeFullNm = new Stack<string>();
            if(model ==null)
                return;
 
            nodeFullNm.Push(model.NodeNm);
            INode tmp = model.Parent;
            while(tmp!=null)
            {   
                    nodeFullNm.Push(tmp.NodeNm);
                    tmp = tmp.Parent;
            }
            OBJTYPE objT;
            if ((ISceneNod)model.Parent == null)
                return;


            if (((ISceneNod)model.Parent).Type == typeof(Pipe))
                objT = OBJTYPE.Pipe;
            else if (((ISceneNod)model.Parent).Type == typeof(Valve))
                objT = OBJTYPE.Valve;
            else if (((ISceneNod)model.Parent).Type == typeof(HoleEllipse))
                objT = OBJTYPE.HoleEllipse;

            else if (((ISceneNod)model.Parent).Type == typeof(HoleRect))
                objT = OBJTYPE.HoleRect;
            else if (((ISceneNod)model.Parent).Type == typeof(Chamber))
                objT = OBJTYPE.Chamber;

            else if (((ISceneNod)model.Parent).Type == typeof(Well))
                objT = OBJTYPE.Well;

            else
                objT = OBJTYPE.Building;


            new SysEvn(0, null, objT, SYSEVN.Select, nodeFullNm.ToArray());
            
        }
        /*
        public static bool Select(string ID)
        {
            IUpdatableComponent result =
                   _gameRef.Components.Find(
                delegate(IUpdatableComponent matcher)
                {
                    return (matcher.ID == ID) ?
                        true : false;
                });
            if (result != null&&result is ISelectable)
            {
                _selection.Add((ISelectable)result);
                return true;
            }
            return false;
        }*/

        public static IUpdatableComponent Select(string ID)
        {
            IUpdatableComponent result =
                   _gameRef.Components.Find(
                delegate(IUpdatableComponent matcher)
                {
                    return (matcher.ID == ID) ?
                        true : false;
                });
            if (result!=null&& result is ISelectable)
            {
                _selection.Add((ISelectable)result);
                return result;
            }
            return null;
        }

        public static List<ISelectable> Select(Predicate<IUpdatableComponent> predicate)
        {
            List<IUpdatableComponent> tmp =
                   _gameRef.Components.FindAll(predicate);

            tmp = tmp.FindAll(
                delegate(IUpdatableComponent matcher)
                {
                    return matcher is ISelectable ? true : false;
                });

            List<ISelectable> result = new List<ISelectable>();
            foreach (IUpdatableComponent ucomp in tmp)
            {
                result.Add((ISelectable)ucomp);
            }
            return result;
        }


        static IUpdatableComponent find
            (ref IGame game, string ID)
        { 

            return
           game.Components.Find(
                delegate(IUpdatableComponent matcher)
                {
                    return (matcher.ID == ID) ?
                        true : false;
                });

        }

        public static bool IsSelected(object obj)
        {
            if (!(obj is ISelectable))
                return false;
            ISelectable sel = (ISelectable)obj;
            return _selection.Contains(sel);
        }
        public static void DeSelect(ISelectable obj)
        {
            if (_selection.Exists(
                delegate(ISelectable member)
                {
                    return
                        member == obj ? true : false;
                }))
            {
                _selection.Remove(obj);
              //  obj.Data.SelectionHandler.Invoke(obj,false);
            }
        }
        public override void Initialize()
        {

            _debugDraw = new DebugDraw(_game.GraphicsDevice);
            _cam = (ICamera)_game.Services.GetService(typeof(ICamera));


            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
           

            if (_selection.Count!= _oldSelection.Count||
                (_selection.Count > 0 && _selection[0] != _oldSelection[0]))
            {

               
                //foreach (ISelectable sel in _oldSelection)
                  //  sel.Data.SelectionHandler.Invoke(false);
                foreach (ISelectable sel in _oldSelection)
                {
                    sel.Data.SelectionHandler.Invoke(sel,false);
                }
                _oldSelection.Clear();
                foreach (ISelectable sel in _selection)
                {
                    
                    _oldSelection.Add(sel);
                }
                if(OnSelectionUpdate!=null)
                OnSelectionUpdate.Invoke();
            }
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime,ICamera cam)
        {
            if(_debugDraw==null)
                _debugDraw = new DebugDraw(_game.GraphicsDevice);

            _debugDraw.Begin(
                Matrix.Identity,
                cam.ViewMatrix,
                cam.ProjectionMatrix);
            //_debugDraw.DrawWireGrid(Vector3.UnitX*5,Vector3.UnitZ*5,
             //          new Vector3(-2.5f,0,-2.5f),20,20,Color.Green);

            foreach (ISelectable scomp in _selection)
                if (scomp.Data.BoundingSpheres!=null)
                _debugDraw.DrawWireSphere(scomp.Data.BoundingSpheres[0],
                    Color.Green);


            _debugDraw.End();
            base.Draw(gameTime, cam);
        }
    }
}
