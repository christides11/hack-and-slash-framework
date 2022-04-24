namespace HnSF.Sample.TDAction
{
    public class BaseStateReference : FighterStateReferenceBase
    {
        public BaseStateEnum state;
        public override int GetState()
        {
            return (int)state;
        }
    }
}