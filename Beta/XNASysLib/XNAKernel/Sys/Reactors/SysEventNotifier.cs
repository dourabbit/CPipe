using System;
using System.Collections.Generic;
using XNASysLib.Primitives3D;
using VertexPipeline;
using XNASysLib.XNATools;

namespace XNASysLib.XNAKernel
{
    public struct SnapShot
    {
        public TransformNode Trans;
        public ShapeNode Shape;
        public SnapShot(TransformNode trans, ShapeNode shape)
        {
            Trans = trans;
            Shape = shape;
        }
    }

    public class HistoryEntry
    {
        public double Time;
        public string ToolNm;
        public SnapShot SnapShot;
        public SceneNodHierachyModel Target;
        public HistoryEntry(double time, string toolNm,
            SceneNodHierachyModel target, SnapShot snapShot)
        {
            Time = time;
            ToolNm = toolNm;
            Target = target;
            SnapShot = snapShot;
        }
    }


    public class TimeMechine
    {
        static TimeMechine _singleton;
        static MemoryStack<HistoryEntry> _history=new MemoryStack<HistoryEntry>();
        public static MemoryStack<HistoryEntry> History
        {
            get
            {

                return _history;
            }
        }
        public static HistoryEntry Entry
        {
            get
            {

                if (_history.Count == 0)
                    return null;
                return _history[_history.Count-1];
            }
        }
        public static TimeMechine Singleton
        {
            get
            {
                if (_singleton == null)
                    new TimeMechine();

                return _singleton;
            }
        }

        static double _time;
        public static double Time
        {
            get
            {
                return _time;
            }
        }

        public TimeMechine()
        {
            _singleton = this;
        }


    }


    public class SysEventNotifier : aC_Reactor
    {
        IGame _game;
        SysCollector _sys;
        public SysEventNotifier(IGame game)
        {
            _sys = SysCollector.Singleton;
            this._proceedType = new int[1] { 0 };
            _game = game;
        }
        public override void Dispatch(ISysData gameData)
        {
            if(gameData is SysEvn)
            base.Dispatch(gameData);
        }
        public override void ProceedEvent(ISysData gameData)
        {
            try
            {
                Scene scene = (Scene)_game;
                SysEvn sysEvn = (SysEvn)gameData;


                switch(sysEvn.Event)
                {

                    case SYSEVN.Import:
                            string _params=(string)sysEvn.Params[0];
                            FBXModelLoader newScene =
                                new FBXModelLoader(scene, _params);
                            break;
                    case SYSEVN.Tool:
                            //        ToolNm---0
                            //        Target---1
                            //        SnapShot---2
                            string toolNm = (string)sysEvn.Params[0];
                            SceneNodHierachyModel target = 
                                (SceneNodHierachyModel)sysEvn.Params[1];
                            SnapShot image = (SnapShot)sysEvn.Params[2];

                            TimeMechine.History.CurIndex++;
                            TimeMechine.History.Push(
                                new HistoryEntry
                                    (TimeMechine.Time,toolNm,target,image)
                                );
                           
                            break;

                    default:
                            new NullReferenceException();
                            break;
                }
            }
            catch(Exception f)
            {
                MyConsole.WriteLine("Error in Loading"+f.Message);
                return; 
            }
            base.ProceedEvent(gameData);
        }

    }
}
