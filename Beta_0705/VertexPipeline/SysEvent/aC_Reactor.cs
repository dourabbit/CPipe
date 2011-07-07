using System;
using System.Collections.Generic;
using VertexPipeline;

namespace VertexPipeline
{

    public abstract class aC_Reactor
    {
        protected string _ID;
        protected Type _type;
        protected int[] _proceedType;
       
        protected static
            List<aC_Reactor> _reactorPool =
            new List<aC_Reactor>();
        
        public static List<aC_Reactor> ReactorPool
        {
            get { return _reactorPool; }
            set { _reactorPool = value; }
        }
        public Type ReactorType
        {
            get { return _type; }
        }
        public int[] ProceedType
        {
            get { return _proceedType; }
        }

        public aC_Reactor()
        {
            _reactorPool.Add(this);
            //_history = new ReactorHistory();

            this._ID = "LocalReactor: Jimmy";
            this._type = this.GetType();

        }
        /// <summary>
        /// Checking Reciever, if corresponds with Reactor type, then 
        /// executing EventReactor.ProceedEvent
        /// </summary>
        /// <param name="gameEvent"></param>
        public virtual void Dispatch(ISysData gameData)
        {
#if Debug
            Console.WriteLine("Debug: "+gameData.ISysDataType);
#endif
            this.ProceedEvent(gameData);
            // Console.WriteLine(gameData.ISysDataType+": "+gameData.ISysDataTime.TotalRealTime.Milliseconds);
        }
        public virtual void ProceedEvent(ISysData gameData)
        {

        }
    }
}
