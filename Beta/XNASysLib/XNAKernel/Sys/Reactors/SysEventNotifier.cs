using System;
using System.Collections.Generic;
using XNASysLib.Primitives3D;
using VertexPipeline;
using XNASysLib.XNATools;

namespace XNASysLib.XNAKernel
{
    public class ObjImage
    {
        public TransformNode Trans;
        public ShapeNode Shape;
    }
    public class SnapShots
    {
        public ObjImage Before;
        public ObjImage After;
        public object[] Script;
        public string Name;
        public SnapShots(string name,ObjImage before, ObjImage after, object[] script)
        {
            Name = name;
            Before = before;
            After = after;
            Script = script;
        }
    }

    public class HistoryEntry
    {
        public double Time;
        public string ToolNm;
        public SnapShots SnapShot;
        public SceneNodHierachyModel Target;
        public HistoryEntry(double time, string toolNm,
            SceneNodHierachyModel target, SnapShots snapShot)
        {
            Time = time;
            ToolNm = toolNm;
            Target = target;
            SnapShot = snapShot;
        }
    }


    public class TimeMechine
    {
        //static TimeMechine _singleton;
        static MemoryStack<HistoryEntry> _history=new MemoryStack<HistoryEntry>();
        public static MemoryStack<HistoryEntry> History
        {
            get
            {

                return _history;
            }
        }
        public static HistoryEntry LastEntry
        {
            get
            {

                if (_history.Count == 0)
                    return null;
                return _history[_history.Count-1];
            }
        }
        
        //public static TimeMechine Singleton
        //{
        //    get
        //    {
        //        if (_singleton == null)
        //        {
        //            new TimeMechine();
        //            TimeMechine.History.Push(
        //                       new HistoryEntry
        //                           (TimeMechine.Time, "FirstEntry", null, null)
        //                       );
        //        }
        //        return _singleton;
        //    }
        //}

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
           // _singleton = this;
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
                OBJTYPE objT = sysEvn.ObjType; 

                switch(sysEvn.Event)
                {

                    case SYSEVN.Import:
                            string assetNm=(string)sysEvn.Params[0];
                            SceneNodHierachyModel obj;
                            switch (objT)
                            {
                                case OBJTYPE.Building:
                                    //FBXModelLoader newScene 
                                    obj=FBXModelLoader.Import(scene, assetNm);
                                    break;
                                case OBJTYPE.Pipe:
                                    //NodCreator newScene =
                                    //new NodCreator(scene, "_PipeA", typeof(Pipe));
                                    obj = NodCreator.CreateNode(scene, assetNm, typeof(Pipe));
                                    break;
                                default:
                                    obj = NodCreator.CreateNode(scene, assetNm, typeof(Pipe));
                                    break;
                            }
                            ObjImage before = null;
                            ObjImage after = null;

                            TimeMechine.History.Push(
                                new HistoryEntry(TimeMechine.Time, "New", obj,
                                                new SnapShots("New", before, after,
                                                    new object[]{objT, SYSEVN.Import,assetNm,}))
                                );
                                break;
                    case SYSEVN.Tool:
                            //        ToolNm---0
                            //        Target---1
                            //        SnapShot---2
                            string toolNm = (string)sysEvn.Params[0];
                            SceneNodHierachyModel target = 
                                (SceneNodHierachyModel)sysEvn.Params[1];
                            SnapShots image = (SnapShots)sysEvn.Params[2];

                            //TimeMechine.History.CurIndex++;
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
