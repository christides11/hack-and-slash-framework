using CAF.Simulation;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Combat
{
    public class Hitbox3D : HitboxBase
    {
        [SerializeField] protected Collider coll;

        #region Initialization
        public override void Initialize(GameObject owner, Transform directionOwner, int team,
            HitInfoBase hitInfo, List<IHurtable> ignoreList = null)
        {
            this.owner = owner;
            this.directionOwner = directionOwner;
            this.ignoreList = ignoreList;
            this.hitInfo = hitInfo;
            this.team = team;
        }

        public override void Initialize(GameObject owner, Transform directionOwner, int team, 
            BoxShapes shape, HitInfoBase hitInfo, BoxDefinitionBase boxDefinitionBase, List<IHurtable> ignoreList = null)
        {
            this.Initialize(owner, directionOwner, team, hitInfo, ignoreList);

            BoxDefinition boxDefinition = (BoxDefinition)boxDefinitionBase;
            switch (shape)
            {
                case BoxShapes.Rectangle:
                    CreateRectangle(boxDefinition.size);
                    break;
                case BoxShapes.Circle:
                    CreateSphere(boxDefinition.radius);
                    break;
                case BoxShapes.Capsule:
                    CreateCapsule(boxDefinition.radius, boxDefinition.height);
                    break;
            }
        }
        #endregion

        #region Create Shape
        /// <summary>
        /// Initializes the hitbox as a rectangle type hitbox.
        /// </summary>
        /// <param name="size">The size of the hitbox.</param>
        /// <param name="rotation">The rotation of the hitbox.</param>
        protected virtual void CreateRectangle(Vector3 size)
        {
            BoxCollider bc = gameObject.AddComponent<BoxCollider>();
            bc.isTrigger = true;
            coll = bc;
            bc.size = size;
        }

        protected virtual void CreateSphere(float radius)
        {
            SphereCollider sc = gameObject.AddComponent<SphereCollider>();
            sc.isTrigger = true;
            coll = sc;
            sc.radius = radius;
        }

        protected virtual void CreateCapsule(float radius, float height)
        {
            CapsuleCollider cc = gameObject.AddComponent<CapsuleCollider>();
            cc.isTrigger = true;
            coll = cc;
            cc.radius = radius;
            cc.height = height;
            cc.direction = 2;
        }
        #endregion

        public override void Activate()
        {
            base.Activate();
            coll.enabled = true;
        }

        public override void Deactivate()
        {
            base.Deactivate();
            coll.enabled = false;
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            CheckForHurtboxes(other);
        }

        /// <summary>
        /// Called in OnTriggerStay to check for any hurtboxes within the hitbox.
        /// If any are found, they are added to a list to be handled next tick.
        /// </summary>
        /// <param name="other">The hurtbox that collided with this hitbox.</param>
        protected virtual void CheckForHurtboxes(Collider other)
        {
            if (!activated)
            {
                return;
            }

            Hurtbox otherHurtbox = null;
            if (other.TryGetComponent<Hurtbox>(out otherHurtbox) == false)
            {
                return;
            }

            if (hitHurtables.Contains(otherHurtbox.Owner))
            {
                return;
            }

            if (ignoreList == null || ignoreList.Contains(otherHurtbox.Hurtable) == false)
            {
                hitHurtables.Add(otherHurtbox.Owner);
            }
        }

        protected override void HurtHurtable(IHurtable ih)
        {
            ih.Hurt(BuildHurtInfo());
        }

        protected override HurtInfoBase BuildHurtInfo()
        {
            HurtInfo3D hurtInfo;
            switch (hitInfo.forceRelation)
            {
                case HitboxForceRelation.ATTACKER:
                    hurtInfo = new HurtInfo3D(hitInfo, directionOwner.position, directionOwner.forward, directionOwner.right);
                    break;
                case HitboxForceRelation.HITBOX:
                    hurtInfo = new HurtInfo3D(hitInfo, transform.position, transform.forward, transform.right);
                    break;
                case HitboxForceRelation.WORLD:
                    hurtInfo = new HurtInfo3D(hitInfo, transform.position, Vector3.forward, Vector3.right);
                    break;
                default:
                    hurtInfo = new HurtInfo3D();
                    break;
            }
            return hurtInfo;
        }
    }
}