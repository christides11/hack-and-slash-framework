using CAF.Camera;
using CAF.Combat;
using CAF.Simulation;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Entities
{
    public class EntityManager : SimObject, ITargetable
    {
        public EntityInputManager InputManager { get { return entityInput; } }
        public EntityStateManager StateManager { get { return entityStateManager; } }
        public EntityCombatManager CombatManager { get { return entityCombatManager; } }
        public EntityPhysicsManagerBase PhysicsManager { get { return entityPhysicsManager; } }
        public EntityHurtboxManager HurtboxManager { get { return entityHurtboxManager; } }

        public virtual bool Targetable { get { return targetable; } }
        public bool IsGrounded { get; set; } = false;

        protected bool targetable = true;

        [Header("References")]
        [SerializeField] protected EntityInputManager entityInput;
        [SerializeField] protected EntityStateManager entityStateManager;
        [SerializeField] protected EntityCombatManager entityCombatManager;
        [SerializeField] protected EntityPhysicsManagerBase entityPhysicsManager;
        [SerializeField] protected EntityHurtboxManager entityHurtboxManager;
        public GameObject visual;
        public LookHandler lookHandler;

        public override void SimUpdate()
        {
            InputManager.Tick();

            if (CombatManager.HitStop == 0)
            {
                HandleLockon();
                PhysicsManager.CheckIfGrounded();
                StateManager.Tick();
                PhysicsManager.Tick();
            }
            else
            {
                PhysicsManager.ResetForces();
            }
            HurtboxManager.Tick();
        }

        public override void SimLateUpdate()
        {
            CombatManager.CLateUpdate();
        }

        public void SetTargetable(bool value)
        {
            targetable = value;
        }

        /// <summary>
        /// Handles finding and locking on to targets.
        /// </summary>
        protected virtual void HandleLockon()
        {
        }

        /// <summary>
        /// Translates the given vector based on the look transform's forward.
        /// </summary>
        /// <param name="horizontal">The horizontal axis of the vector.</param>
        /// <param name="vertical">The vertical axis of the vector.</param>
        /// <returns>A direction vector based on the camera's forward.</returns>
        public virtual Vector3 GetMovementVector(float horizontal, float vertical)
        {
            if (lookHandler == null)
            {
                return Vector3.forward;
            }
            Vector3 forward = lookHandler.LookTransform().forward;
            Vector3 right = lookHandler.LookTransform().right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            return forward * vertical + right * horizontal;
        }

        /// <summary>
        /// Transforms the given direction into one based on the visual's forward.
        /// </summary>
        /// <param name="direction">The direction vector.</param>
        /// <returns>A direction vector based on the visual's forward.</returns>
        public virtual Vector3 GetVisualBasedDirection(Vector3 direction)
        {
            Vector3 vector = visual.transform.TransformDirection(direction);
            return vector;
        }

        /// <summary>
        /// Rotate the visual towards the given direction based on the speed given.
        /// </summary>
        /// <param name="direction">The direction to rotate towards.</param>
        /// <param name="speed">The speed of the rotation.</param>
        public virtual void RotateVisual(Vector3 direction, float speed)
        {
            Vector3 newDirection = Vector3.RotateTowards(visual.transform.forward, direction, speed, 0.0f);
            visual.transform.rotation = Quaternion.LookRotation(newDirection);
        }

        /// <summary>
        /// Set the visual's rotation to the given direction.
        /// </summary>
        /// <param name="direction">The direction to set the rotation to.</param>
        public virtual void SetVisualRotation(Vector3 direction)
        {
            visual.transform.rotation = Quaternion.LookRotation(direction);
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
} 