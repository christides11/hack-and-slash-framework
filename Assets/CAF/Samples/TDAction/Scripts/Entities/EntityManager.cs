using CAF.Combat;
using CAF.Input;
using Prime31;
using System.Collections;
using System.Collections.Generic;
using TDAction.Entities.Characters;
using UnityEngine;

namespace TDAction.Entities
{
    public class EntityManager : CAF.Entities.EntityManager
    {
        public int FaceDirection { get { return faceDirection; } }


        protected int faceDirection = 1;
        public CharacterController2D charController2D;
        public Collider2D coll;
        public EntityDefinition entityDefinition;
        public HealthManager healthManager;
        public Hitbox hitboxPrefab;


        public virtual void Initialize(InputControlType controlType)
        {
            InputManager.SetControlType(controlType);
        }

        public override void SimStart()
        {
            base.SimStart();
            SetupStates();
        }

        protected virtual void SetupStates()
        {

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
            MovesetAttackNode man = CombatManager.TryAttack();
            if (man != null)
            {
                CombatManager.SetAttack(man);
                StateManager.ChangeState((int)EntityStates.ATTACK);
                return true;
            }
            return false;
        }
    }
}