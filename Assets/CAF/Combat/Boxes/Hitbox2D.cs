using CAF.Simulation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Combat
{
    public class Hitbox2D : Hitbox
    {

        protected Collider2D coll;

        public override void Initialize(GameObject owner, Transform directionOwner, BoxShapes shape, 
            HitInfoBase hitInfo, BoxDefinition boxDefinition, List<IHurtable> ignoreList = null)
        {
            this.owner = owner;
            this.directionOwner = directionOwner;
            this.ignoreList = ignoreList;
            this.hitInfo = hitInfo;

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
            switch (hitInfo.forceRelation)
            {
                case HitboxForceRelation.ATTACKER:
                    ih.Hurt(directionOwner.position, directionOwner.right, directionOwner.right, hitInfo);
                    break;
                case HitboxForceRelation.HITBOX:
                    ih.Hurt(transform.position, transform.right, transform.right, hitInfo);
                    break;
                case HitboxForceRelation.WORLD:
                    ih.Hurt(transform.position, Vector3.right, Vector3.right, hitInfo);
                    break;
            }
        }
    }
}