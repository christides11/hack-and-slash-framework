using CAF.Simulation;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Combat
{
    public abstract class HitboxBase : SimObject
    {
        public delegate void HurtAction(GameObject hurtableHit, HitInfoBase hitInfo);
        public virtual event HurtAction OnHurt;

        protected GameObject owner;
        protected Transform directionOwner;
        protected bool activated;
        public HitInfoBase hitInfo;

        public List<IHurtable> ignoreList = null;
        public List<GameObject> hitHurtables = new List<GameObject>();

        public abstract void Initialize(GameObject owner, Transform directionOwner, int team,
            BoxShapes shape, HitInfoBase hitInfo, BoxDefinitionBase boxDefinition, List<IHurtable> ignoreList = null);

        public virtual void Activate()
        {
            activated = true;
        }

        /// <summary>
        /// Deactivates the hitbox.
        /// </summary>
        public virtual void Deactivate()
        {
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
                    HurtHurtable(ih);
                    ignoreList.Add(ih);
                    OnHurt?.Invoke(hitHurtables[i], hitInfo);
                }
            }
        }

        protected virtual void HurtHurtable(IHurtable ih)
        {
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
        }
    }
}