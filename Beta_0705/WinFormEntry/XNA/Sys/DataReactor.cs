using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SysLib
{
    public class DataReactor : aC_Reactor
    {
        #region Fields
        protected Game _game;
        protected
            SysDataList<ISysData>
                                        _sysDataList;
        
        
        
        
        #endregion

        #region Properties

        public SysDataList<ISysData> DataList
        {
            get { return _sysDataList; }
        }


       
        #endregion

        

        public static DataReactor
            GetDataReactor()
        {
            return (DataReactor)
                _reactorPool.Find(
                    delegate(aC_Reactor match)
                    {
                        return match is DataReactor ? true : false;
                    });
        }
        #region Constructor
        public DataReactor(Game game)
        {
            _game = game;
            this._proceedType = new int[3] { -1, -2, -3 };
            this._sysDataList =
                new SysDataList<ISysData>();
        }
        #endregion

        #region Override aC_Reactor: ProceedEvent() Dispatch()
        
        /*void Debug(int index)
        {
            MouseData data = (MouseData)
            this._sysDataList.GetData(-1, 0);

            Console.Write(data.ISysDataTime.ToString() +
                "---" + "Data0: " +
                data.IsLeftBtnDown.ToString() + "  ");


            data = (MouseData)
            this._sysDataList.GetData(-1, 1);

            Console.Write("Data1: " +
                data.IsLeftBtnDown.ToString() + "\n");
        }
        */
        public override void Dispatch
            (ISysData gameData)
        {
            if (gameData.ISysDataType >= 0)
                return;

            base.Dispatch(gameData);
        }
        public override void ProceedEvent
            (ISysData gameData)
        {

            this._sysDataList.Add(gameData);

            base.ProceedEvent(gameData);
        }
        #endregion

        #region Update
        public virtual void
            Update(GameTime gameTime)
        {

        }
        #endregion

        #region HandlerFuntions
      

        #endregion
    }

  
}
