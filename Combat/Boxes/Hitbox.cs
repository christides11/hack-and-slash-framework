using CAF.Simulation;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Combat
{
    public class Hitbox : SimObject
    {
        public delegate void HurtAction(GameObject hurtableHit, HitInfo hitInfo);
        public event HurtAction OnHurt;

        protected GameObject owner;
        protected Transform directionOwner;
        protected bool activated;
        protected Collider coll;
        public HitInfo hitInfo;

        public List<IHurtable> ignoreList = null;
        public List<GameObject> hitHurtables = new List<GameObject>();

        public virtual void Initialize(GameObject owner, Transform directionOwner, BoxShapes shape, 
            HitInfo hitInfo, List<IHurtable> ignoreList = null)
        {
            this.owner = owner;
            this.directionOwner = directionOwner;
            this.ignoreList = ignoreList;
        }

        public virtual void Activate()
        {
            coll.enabled = true;
            activated = true;
        }

        /// <summary>
        /// Deactivates the hitbox.
        /// </summary>
        public virtual void Deactivate()
        {
            coll.enabled = false;
            activated = false;
        }

        /// <summary>
        /// Reactivates the hitbox, settings it's parameters back to their defaults.
        /// </summary>
        /// <param name="ignoreList"></param>
        public virtual void ReActivate(List<IHurtable> ignoreList = null)
        {
            this.ignoreList = ignoreList;
            hitHurtables.Clear();
            Activate();
        }

        /// <summary>
        /// Initializes the hitbox as a rectangle type hitbox.
        /// </summary>
        /// <param name="size">The size of the hitbox.</param>
        /// <param name="rotation">The rotation of the hitbox.</param>
        protected virtual void CreateRectangle(Vector3 size, Vector3 rotation)
        {
            transform.rotation = Quaternion.Euler(rotation);

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

        public virtual void CheckHits()
        {
            CheckHurtables();

            hitHurtables.Clear();
        }

        protected virtual void CheckHurtables()
        {
            if (hitHurtables.Count > 0)
            {
                for (int i = 0; i < hitHurtables.Count; i++)
                {
                    IHurtable ih = hitHurtables[i].GetComponent<IHurtable>();
                    if (ignoreList.Contains(ih))
                    {
                        continue;
                    }
                    switch (hitInfo.forceRelation)
                    {
                        case HitboxForceRelation.ATTACKER:
                            ih.Hurt(directionOwner.position, directionOwner.forward, directionOwner.right, hitInfo);
                            break;
                        case HitboxForceRelation.HITBOX:
                            ih.Hurt(transform.position, transform.forward, transform.right, hitInfo);
                            break;
                        case HitboxForceRelation.WORLD:
                            ih.Hurt(transform.position, Vector3.forward, Vector3.right, hitInfo);
                            break;
                    }
                    ignoreList.Add(ih);
                    OnHurt?.Invoke(hitHurtables[i], hitInfo);
                }
            }
        }

        /// <summary>
        /// Called every tick for whatever object's are within this hitbox.
        /// Gets all the hitboxes and checks if they should be hurt next LateUpdate.
        /// </summary>
        /// <param name="other">The collider in our hitbox.</param>
        protected virtual void OnTriggerStay(Collider other)
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
    }
}