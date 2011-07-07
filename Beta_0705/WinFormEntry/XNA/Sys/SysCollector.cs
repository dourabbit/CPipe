using System;
using System.Collections.Generic;

namespace SysLib
{

    public class SysCollector : IDisposable
    {
        public delegate void GameListener(ISysData gameData);
        public GameListener Listener;

        public static SysCollector singleton;


        public SysCollector()
        {
            //singleton = new EventCollector();
            this.Listener += this.DispatchData;
        }


        void DispatchData(ISysData gameData)
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
