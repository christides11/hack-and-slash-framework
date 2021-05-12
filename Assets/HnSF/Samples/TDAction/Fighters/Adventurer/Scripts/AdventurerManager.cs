using HnSF.Fighters;
using HnSF.Input;
using System;
using TDAction.Combat;
using TDAction.Inputs;

namespace TDAction.Fighter
{
    public class AdventurerManager : FighterManager
    {
        private void Awake()
        {
            combatManager.OnMovesetChanged += HandleMovesetChange;
        }

        private void HandleMovesetChange(FighterBase self, int lastMoveset)
        {
            entityAnimator.SetMovesetAnimations((combatManager.CurrentMoveset as MovesetDefinition).animations);
            // Only refresh current animation if it's not an attack.
            if (combatManager.CurrentAttackNode == null)
            {
                entityAnimator.Refresh();
            }
            statManager.SetStats((combatManager.CurrentMoveset as MovesetDefinition).fighterStats);
        }

        public override void SimUpdate()
        {
            base.SimUpdate();

            if (inputManager.GetButton((int)EntityInputs.UNIQUE).firstPress)
            {
                combatManager.SetMoveset(combatManager.CurrentMovesetIdentifier == 0 ? 1 : 0);
            }
        }

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