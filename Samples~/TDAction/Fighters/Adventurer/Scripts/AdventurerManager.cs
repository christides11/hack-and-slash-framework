using CAF.Input;

namespace TDAction.Fighter
{
    public class AdventurerManager : FighterManager
    {
        protected override void SetupStates()
        {
            base.SetupStates();
            StateManager.AddState(new ADVIdle(), (int)FighterStates.IDLE);
            StateManager.AddState(new ADVWalk(), (int)FighterStates.WALK);
            StateManager.AddState(new CJumpSquat(), (int)FighterStates.JUMP_SQUAT);
            StateManager.AddState(new CJump(), (int)FighterStates.JUMP);
            StateManager.AddState(new CJumpAir(), (int)FighterStates.AIR_JUMP);
            StateManager.AddState(new ADVFall(), (int)FighterStates.FALL);
            StateManager.AddState(new CRun(), (int)FighterStates.RUN);
            StateManager.AddState(new ADVAttack(), (int)FighterStates.ATTACK);
            StateManager.AddState(new CEnemyStep(), (int)FighterStates.ENEMY_STEP);

            StateManager.ChangeState((int)FighterStates.FALL);
        }
    }
}