using CAF.Simulation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Combat
{
    public class Hitbox2D : HitboxBase
    {

        protected Collider2D coll;

        public override void Initialize(GameObject owner, Transform directionOwner, int team,
            BoxShapes shape, HitInfoBase hitInfo, BoxDefinitionBase boxDefinitionBase, List<IHurtable> ignoreList = null)
        {
            this.owner = owner;
            this.directionOwner = directionOwner;
            this.ignoreList = ignoreList;
            this.hitInfo = hitInfo;

            BoxDefinition boxDefinition = (BoxDefinition)boxDefinitionBase;
            switch (shape)
            {
                case BoxShapes.Rectangle:
                    CreateRectangle(boxDefinition.size);
                    break;
                case BoxShapes.Circle:
                    CreateCircle(boxDefinition.radius);
                    break;
                case BoxShapes.Capsule:
                    CreateCapsule(boxDefinition.radius, boxDefinition.height);
                    break;
            }
        }

        protected virtual void CreateRectangle(Vector2 size)
        {
            BoxCollider2D bc = gameObject.AddComponent<BoxCollider2D>();
            bc.isTrigger = true;
            coll = bc;
            bc.size = size;
        }

        protected virtual void CreateCircle(float radius)
        {
            CircleCollider2D cc = gameObject.AddComponent<CircleCollider2D>();
            cc.isTrigger = true;
            coll = cc;
            cc.radius = radius;
        }

        protected virtual void CreateCapsule(float radius, float height)
        {
            CapsuleCollider2D cc = gameObject.AddComponent<CapsuleCollider2D>();
            cc.isTrigger = true;
            coll = cc;
            cc.size = new Vector2(radius, height);
        }

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

        public override void ReActivate(List<IHurtable> ignoreList = null)
        {
            this.ignoreList = ignoreList;
            hitHurtables.Clear();
            Activate();
        }

        protected void OnTriggerStay2D(Collider2D other)
        {
            CheckForHurtboxes(other);
        }

        protected virtual void CheckForHurtboxes(Collider2D other)
        {
            if (!activated)
            {
                return;
            }

            Hurtbox otherHurtbox = null;
            if (!other.TryGetComponent<Hurtbox>(out otherHurtbox))
            {
                return;
            }

            if (otherHurtbox != null)
            {
                if (!hitHurtables.Contains(otherHurtbox.Owner)
                    && (ignoreList == null || !ignoreList.Contains(otherHurtbox.Hurtable)))
                {
                    hitHurtables.Add(otherHurtbox.Owner);
                }
            }
        }

        protected override void HurtHurtable(IHurtable ih)
        {
            ih.Hurt(BuildHurtInfo());
        }

        protected override HurtInfoBase BuildHurtInfo()
        {
            HurtInfo2D hurtInfo;
            int faceDirection = GetFaceDirection();
            switch (hitInfo.forceRelation)
            {
                case HitboxForceRelation.ATTACKER:
                    hurtInfo = new HurtInfo2D(hitInfo, directionOwner.position, faceDirection);
                    break;
                case HitboxForceRelation.HITBOX:
                    hurtInfo = new HurtInfo2D(hitInfo, transform.position, faceDirection);
                    break;
                case HitboxForceRelation.WORLD:
                    hurtInfo = new HurtInfo2D(hitInfo, transform.position, faceDirection);
                    break;
                default:
                    hurtInfo = new HurtInfo2D();
                    break;
            }
            return hurtInfo;
        }

        private int GetFaceDirection()
        {
            return directionOwner.localScale.x > 0 ? 1 : -1;
        }
    }
}