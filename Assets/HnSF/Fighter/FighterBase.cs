using HnSF.Camera;
using HnSF.Combat;
using HnSF.Simulation;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Fighters
{
    public class FighterBase : MonoBehaviour, ITargetable
    {
        public FighterInputManager InputManager { get { return inputManager; } }
        public FighterStateManager StateManager { get { return stateManager; } }
        public FighterCombatManager CombatManager { get { return combatManager; } }
        public FighterPhysicsManagerBase PhysicsManager { get { return physicsManager; } }
        public FighterHurtboxManager HurtboxManager { get { return hurtboxManager; } }

        public virtual bool Targetable { get { return targetable; } }

        protected bool targetable = true;

        [Header("References")]
        [SerializeField] protected FighterInputManager inputManager;
        [SerializeField] protected FighterStateManager stateManager;
        [SerializeField] protected FighterCombatManager combatManager;
        [SerializeField] protected FighterPhysicsManagerBase physicsManager;
        [SerializeField] protected FighterHurtboxManager hurtboxManager;
        public GameObject visual;
        public LookHandler lookHandler;

        public virtual void Tick()
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
                PhysicsManager.Freeze();
            }
            HurtboxManager.Tick();
        }

        public virtual void LateTick()
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