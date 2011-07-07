using VertexPipeline;
using XNASysLib.XNAKernel;

namespace WinFormsContentLoading
{
    public class ObjDataReactor : aC_Reactor
    {
        IGame _game;


        public ObjDataReactor(IGame game)
        {
            this._proceedType = new int[1] { 0 };
            _game = game;
        }
        public override void Dispatch(ISysData gameData)
        {
            if (gameData is ObjData)
                base.Dispatch(gameData);
        }
        public override void ProceedEvent(ISysData gameData)
        {
            ObjData data = (ObjData)gameData;
            //if (!(data.OBJ is ISelectable))
            //    return;
            //ISelectable sel = (ISelectable)data.OBJ;
            PropertyPanal.UpdateHandler.Invoke(data);

           
        }

    }
}
