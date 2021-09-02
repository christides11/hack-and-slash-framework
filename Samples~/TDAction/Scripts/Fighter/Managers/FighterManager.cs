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
    public class FighterManager : MonoBehaviour, HnSF.Fighters.IFighterBase, ISimObject
    {
        public FighterInputManager InputManager { get { return inputManager; } }
        public FighterCombatManager CombatManager { get { return combatManager; } }
        public FighterStateManager StateManager { get { return stateManager; } }
        public FighterPhysicsManager PhysicsManager { get { return physicsManager; } }
        public FighterHurtboxManager HurtboxManager { get { return hurtboxManager; } }
        public int FaceDirection { get { return faceDirection; } }

        public FighterInputManager inputManager;
        public FighterStatsManager statManager;
        protected int faceDirection = 1;
        public FighterAnimator entityAnimator;
        public CharacterController2D charController2D;
        public Collider2D coll;
        public FighterDefinition entityDefinition;
        public HealthManager healthManager;
        public LayerMask enemyStepLayerMask;
        public float enemyStepRadius = 2;

        public GameObject visual;
        [SerializeField] protected FighterCombatManager combatManager;
        [SerializeField] protected FighterStateManager stateManager;
        [SerializeField] protected FighterPhysicsManager physicsManager;
        [SerializeField] protected FighterHurtboxManager hurtboxManager;

        public virtual void Initialize(FighterControlType controlType)
        {
            inputManager.SetControlType(controlType);
        }

        public void Start()
        {
            CombatManager.SetMoveset(0);
            statManager.SetStats(entityDefinition.movesets[0].fighterStats);
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

        public virtual void Tick()
        {
            inputManager.Tick();

            // Shake during hitstop (only when you got hit by an attack).
            if(CombatManager.HitStop > 0
                && CombatManager.HitStun > 0)
            {
                Vector3 pos = visual.transform.localPosition;
                pos.x = (Mathf.Sign(pos.x) > 0 ? -1 : 0) * .02f;
                visual.transform.localPosition = pos;
            }

            if (CombatManager.HitStop == 0)
            {
                HandleLockon();
                PhysicsManager.CheckIfGrounded();
                StateManager.Tick();
                PhysicsManager.Tick();
            }
            else
            {
                PhysicsManager.Freeze();
            }
            HurtboxManager.Tick();
        }

        public virtual void LateTick()
        {
            StateManager.LateTick();
            CombatManager.CLateUpdate();
        }

        /// <summary>
        /// Handles finding and locking on to targets.
        /// </summary>
        protected virtual void HandleLockon()
        {

        }

        protected virtual void SetupStates()
        {
            stateManager.AddState(new FighterStateFlinchAir(), (int)FighterStates.FLINCH_AIR);
            stateManager.AddState(new FighterStateFlinchGround(), (int)FighterStates.FLINCH_GROUND);
            stateManager.AddState(new FighterStateTumble(), (int)FighterStates.TUMBLE);
            stateManager.AddState(new FighterStateKnockdown(), (int)FighterStates.KNOCKDOWN);
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

        public virtual bool TryAttack(int attackIdentifier, int attackMoveset = -1, bool resetFrameCounter = true)
        {
            if(attackIdentifier != -1)
            {
                if (attackMoveset != -1)
                {
                    CombatManager.SetAttack(attackIdentifier, attackMoveset);
                }
                else
                {
                    CombatManager.SetAttack(attackIdentifier);
                }
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

        public AnimationClip GetAnimationClip(string animationName, int movesetIdentifier = -1)
        {
            if(movesetIdentifier == -1)
            {
                return entityDefinition.sharedAnimations.GetAnimation(animationName);
            }
            else
            {
                if(entityDefinition.movesets.Count <= movesetIdentifier)
                {
                    return null;
                }
                if (entityDefinition.movesets[movesetIdentifier].animations.TryGetAnimation(animationName, out AnimationClip movesetClip))
                {
                    return movesetClip;
                }
                return entityDefinition.sharedAnimations.GetAnimation(animationName);
            }
        }

        public void SetTargetable(bool value)
        {
            throw new System.NotImplementedException();
        }

        public Vector3 GetMovementVector(float horizontal, float vertical)
        {
            throw new System.NotImplementedException();
        }

        public Vector3 GetVisualBasedDirection(Vector3 direction)
        {
            throw new System.NotImplementedException();
        }

        public void RotateVisual(Vector3 direction, float speed)
        {
            throw new System.NotImplementedException();
        }

        public void SetVisualRotation(Vector3 direction)
        {
            throw new System.NotImplementedException();
        }

        public GameObject GetGameObject()
        {
            throw new System.NotImplementedException();
        }
    }
}