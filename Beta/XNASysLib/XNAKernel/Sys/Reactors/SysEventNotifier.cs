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

        public ObjImage() { }
        ~ObjImage()
        {
            Trans = null;
            Shape = null;
        }
    }
    public class SnapShots : IDisposable
    {
        public ObjImage Before;
        public ObjImage After;
        public object[] Script;
        public string Name;

        bool _disposed = false;
        public SnapShots(string name,ObjImage before, ObjImage after, object[] script)
        {
            Name = name;
            Before = before;
            After = after;
            Script = script;
        }
        ~SnapShots()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this._disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    Before = null;
                    After = null;
                    Script = null;
                    Name = String.Empty;
                }


                // Note disposing has been done.
                _disposed = true;

            }
        }
    }

    public class HistoryEntry:IDisposable
    {
        public double Time;
        public string ToolNm;
        public SnapShots SnapShot;
        public SceneNodHierachyModel Target;
        bool _disposed = false;
        public HistoryEntry(double time, string toolNm,
            SceneNodHierachyModel target, SnapShots snapShot)
        {
            Time = time;
            ToolNm = toolNm;
            Target = target;
            SnapShot = snapShot;
        }
        ~HistoryEntry()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this._disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    SnapShot.Dispose();
                }


                // Note disposing has been done.
                _disposed = true;

            }
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
                string toolNm;
                SceneNodHierachyModel target;
                SnapShots image;
                switch(sysEvn.Event)
                {

                    case SYSEVN.Import:
                            string assetNm=(string)sysEvn.Params[0];
                            SceneNodHierachyModel obj;
                            switch (objT)
                            {
                                case OBJTYPE.Building:
                                    //FBXModelLoader newScene 
                                    //obj=FBXModelLoader.Import(scene, assetNm);
                                    obj = NodCreator.CreateNode(scene, assetNm, typeof(Building));
                                    break;
                                case OBJTYPE.Pipe:
                                    //NodCreator newScene =
                                    //new NodCreator(scene, "_PipeA", typeof(Pipe));
                                    obj = NodCreator.CreateNode(scene, assetNm, typeof(Pipe));
                                    break;
                                case OBJTYPE.Chamber:
                                    //NodCreator newScene =
                                    //new NodCreator(scene, "_PipeA", typeof(Pipe));
                                    obj = NodCreator.CreateNode(scene, assetNm, typeof(Chamber));
                                    break;
                                case OBJTYPE.HoleEllipse:
                                    //NodCreator newScene =
                                    //new NodCreator(scene, "_PipeA", typeof(Pipe));
                                    obj = NodCreator.CreateNode(scene, assetNm, typeof(HoleEllipse));
                                    break;
                                case OBJTYPE.HoleRect:
                                    //NodCreator newScene =
                                    //new NodCreator(scene, "_PipeA", typeof(Pipe));
                                    obj = NodCreator.CreateNode(scene, assetNm, typeof(HoleRect));
                                    break;
                                case OBJTYPE.Valve:
                                    //NodCreator newScene =
                                    //new NodCreator(scene, "_PipeA", typeof(Pipe));
                                    obj = NodCreator.CreateNode(scene, assetNm, typeof(Valve));
                                    break;
                                case OBJTYPE.Well:
                                    //NodCreator newScene =
                                    //new NodCreator(scene, "_PipeA", typeof(Pipe));
                                    obj = NodCreator.CreateNode(scene, assetNm, typeof(Well));
                                    break;
                                default:
                                    obj = NodCreator.CreateNode(scene, assetNm, typeof(SceneNodHierachyModel));
                                    return;
                            }


                            new SysEvn(0, obj, objT, SYSEVN.Initial, assetNm);
                                    
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
                            toolNm = (string)sysEvn.Params[0];
                            target = (SceneNodHierachyModel)sysEvn.Params[1];
                            image = (SnapShots)sysEvn.Params[2];

                            //TimeMechine.History.CurIndex++;
                            TimeMechine.History.Push(
                                new HistoryEntry
                                    (TimeMechine.Time,toolNm,target,image)
                                );
                           
                            break;
                    case SYSEVN.Panel:
                            //        ToolNm---0
                            //        Target---1
                            //        SnapShot---2
                            toolNm = (string)sysEvn.Params[0];
                            target =
                                (SceneNodHierachyModel)sysEvn.Params[1];
                            image = (SnapShots)sysEvn.Params[2];

                            //TimeMechine.History.CurIndex++;
                            TimeMechine.History.Push(
                                new HistoryEntry
                                    (TimeMechine.Time, toolNm, target, image)
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
