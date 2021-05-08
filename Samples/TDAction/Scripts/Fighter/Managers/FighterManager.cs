using HnSF.Combat;
using HnSF.Input;
using HnSF.Simulation;
using Prime31;
using System.Collections;
using System.Collections.Generic;
using TDAction.Inputs;
using UnityEngine;

namespace TDAction.Fighter
{
    public class FighterManager : HnSF.Fighters.FighterBase, ISimObject
    {
        public int FaceDirection { get { return faceDirection; } }

        public FighterStatsManager statManager;
        protected int faceDirection = 1;
        public FighterAnimator entityAnimator;
        public CharacterController2D charController2D;
        public Collider2D coll;
        public FighterDefinition entityDefinition;
        public HealthManager healthManager;
        public LayerMask enemyStepLayerMask;
        public float enemyStepRadius = 2;

        public virtual void Initialize(InputControlType controlType)
        {
            InputManager.SetControlType(controlType);
        }

        public void Start()
        {
            CombatManager.SetMoveset(0);
            statManager.SetStats(entityDefinition.movesets[0].fighterStats);
            entityAnimator.SetAnimations(entityDefinition.movesets[0].animations);
            SetupStates();
            CombatManager.OnExitHitStop += (self) => { visual.transform.localPosition = Vector3.zero; };
            if (healthManager.MaxHealth == 0)
            {
                healthManager.SetMaxHealth(statManager.CurrentStats.health.GetCurrentValue());
                healthManager.SetHealth(statManager.CurrentStats.health.GetCurrentValue());
            }
        }

        public virtual void SimUpdate()
        {
            Tick();
        }

        public virtual void SimLateUpdate()
        {
            LateTick();
        }

        public override void Tick()
        {
            // Shake during hitstop (only when you got hit by an attack).
            if(CombatManager.HitStop > 0
                && CombatManager.HitStun > 0)
            {
                Vector3 pos = visual.transform.localPosition;
                pos.x = (Mathf.Sign(pos.x) > 0 ? -1 : 0) * .02f;
                visual.transform.localPosition = pos;
            }

            base.Tick();
        }

        protected virtual void SetupStates()
        {
            StateManager.AddState(new FighterStateFlinchAir(), (int)FighterStates.FLINCH_AIR);
            StateManager.AddState(new FighterStateFlinchGround(), (int)FighterStates.FLINCH_GROUND);
            StateManager.AddState(new FighterStateTumble(), (int)FighterStates.TUMBLE);
            StateManager.AddState(new FighterStateKnockdown(), (int)FighterStates.KNOCKDOWN);
        }

        public void SetFaceDirection(int faceDirection)
        {
            this.faceDirection = faceDirection;
            Vector3 visualScale = visual.transform.localScale;
            visualScale.x = 1 * faceDirection;
            visual.transform.localScale = visualScale;
        }

        public virtual bool TryAttack()
        {
            int man = CombatManager.TryAttack();
            if (man != -1)
            {
                CombatManager.SetAttack(man);
                StateManager.ChangeState((int)FighterStates.ATTACK);
                return true;
            }
            return false;
        }

        public virtual bool TryAttack(int attackIdentifier, bool resetFrameCounter = true)
        {
            if(attackIdentifier != -1)
            {
                CombatManager.SetAttack(attackIdentifier);
                StateManager.ChangeState((int)FighterStates.ATTACK, resetFrameCounter ? 0 : StateManager.CurrentStateFrame);
                return true;
            }
            return false;
        }

        Collider2D[] enemyStepResults = new Collider2D[2];
        public virtual bool TryEnemyStep(uint bufferFrames = 3)
        {
            if (InputManager.GetButton((int)EntityInputs.JUMP, 0, true, bufferFrames).firstPress)
            {
                Physics2D.OverlapCircleNonAlloc(transform.position, enemyStepRadius, enemyStepResults, enemyStepLayerMask);
                foreach (Collider2D e in enemyStepResults)
                {
                    if(e == null)
                    {
                        continue;
                    }
                    if (e.gameObject != gameObject)
                    {
                        StateManager.ChangeState((int)FighterStates.ENEMY_STEP);
                        return true;
                    }
                }
            }
            return false;
        }

        public virtual bool TryJump()
        {
            if (InputManager.GetButton((int)EntityInputs.JUMP).firstPress)
            {
                StateManager.ChangeState(physicsManager.IsGrounded ? (ushort)FighterStates.JUMP_SQUAT : (ushort)FighterStates.JUMP);
                return true;
            }
            return false;
        }

        public virtual bool TryLandCancel(bool setState = true)
        {
            physicsManager.CheckIfGrounded();
            if (physicsManager.IsGrounded)
            {
                if (setState)
                {
                    StateManager.ChangeState((ushort)FighterStates.IDLE);
                }
                return true;
            }
            return false;
        }

        public FighterPhysicsManager GetPhysicsManager()
        {
            return (FighterPhysicsManager)PhysicsManager;
        }
    }
}