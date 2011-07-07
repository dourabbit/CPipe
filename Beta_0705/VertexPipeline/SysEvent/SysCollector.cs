using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using VertexPipeline;

namespace VertexPipeline
{

    public class SysCollector : IDisposable
    {
        public delegate void GameListener(ISysData gameData);
        public GameListener Listener;


        static SysCollector _singleton;
        public static SysCollector Singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = new SysCollector();
                return _singleton;
            }
        }
        public static List<Scene> Scenes=new List<Scene>();
        
        public SysCollector()
        {
            _singleton = this;
            Scenes.Add(new Scene());
            this.Listener += this.DispatchData;
        }
        //public virtual void Initialize()
        //{
           
        
        //}

        public virtual void DispatchData(ISysData gameData)
        {

            foreach (aC_Reactor item in aC_Reactor.ReactorPool)
            {
                item.Dispatch(gameData);
            }
            
        }
        public void Dispose()
        {
            aC_Reactor.ReactorPool = new List<aC_Reactor>();

        }

    }

    
}
