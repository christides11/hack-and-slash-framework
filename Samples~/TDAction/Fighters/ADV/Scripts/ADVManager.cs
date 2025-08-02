namespace HnSF.Sample.TDAction
{
    public class ADVManager : FighterManager
    {
        public override void Start()
        {
            base.Start();
            StateManager.ChangeState((int)BaseStateEnum.FALL);
        }
    }
}