using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HnSF.Combat
{
    /// <summary>
    /// Handles the hitboxes and other boxes used by entities for combat.
    /// </summary>
    public class HitboxManager: MonoBehaviour
    {
        public class IDGroupCollisionInfo
        {
            public List<GameObject> hitIHurtables = new List<GameObject>();
            public HashSet<int> hitboxGroups = new HashSet<int>();
        }

        public delegate void HitboxGroupEventAction(HitboxGroup hitboxGroup, int hitboxIndex, Hurtbox hurtbox, HitReactionBase hitReaction);
        public event HitboxGroupEventAction OnHitHurtbox;

        // ID Group : Owners Hit
        public Dictionary<int, IDGroupCollisionInfo> collidedIHurtables = new Dictionary<int, IDGroupCollisionInfo>();

        public virtual void Reset()
        {
            collidedIHurtables.Clear();
        }

        public virtual GameObject[] GetIDGroupCollisions(int IDGroup)
        {
            if (collidedIHurtables.ContainsKey(IDGroup) == false)
            {
                return null;
            }
            return collidedIHurtables[IDGroup].hitIHurtables.ToArray();
        }

        /// <summary>
        /// If the given ID group has hit anything.
        /// </summary>
        /// <param name="IDGroup">The ID group.</param>
        /// <returns>True if the ID group has hit something.</returns>
        public virtual bool IDGroupHasHurt(int IDGroup)
        {
            if (collidedIHurtables.ContainsKey(IDGroup) && collidedIHurtables[IDGroup].hitIHurtables.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// If the given hitbox group has hit anything.
        /// </summary>
        /// <param name="IDGroup">The ID group the hitbox group belongs to.</param>
        /// <param name="hitboxGroupID">The hitbox group's identifier.</param>
        /// <returns>True if the hitbox group has hit something.</returns>
        public virtual bool HitboxGroupHasHurt(int IDGroup, int hitboxGroupID)
        {
            if (collidedIHurtables.ContainsKey(IDGroup))
            {
                if (collidedIHurtables[IDGroup].hitboxGroups.Count > 0 && collidedIHurtables[IDGroup].hitboxGroups.Contains(hitboxGroupID))
                {
                    return true;
                }
            }
            return false;
        }

        protected List<Hurtbox> hurtboxes = new List<Hurtbox>();
        public virtual bool CheckForCollision(int hitboxGroupIndex, HitboxGroup hitboxGroup, GameObject attacker, List<GameObject> ignoreList = null)
        {
            bool hurtboxHit = false;
            hurtboxes.Clear();
            for (int i = 0; i < hitboxGroup.boxes.Count; i++)
            {
                CheckBoxCollision(hitboxGroup, i);

                // This hitbox hit nothing.
                if (hurtboxes.Count == 0)
                {
                    continue;
                }

                // Hit thing(s). Check if we should actually hurt them.
                if (!collidedIHurtables.ContainsKey(hitboxGroup.ID))
                {
                    collidedIHurtables.Add(hitboxGroup.ID, new IDGroupCollisionInfo());
                }

                SortHitHurtboxes();

                for (int j = 0; j < hurtboxes.Count; j++)
                {
                    if(ignoreList != null && ignoreList.Contains(hurtboxes[j].Owner))
                    {
                        continue;
                    }
                    if(TryHitHurtbox(hitboxGroup, i, j, hitboxGroupIndex, attacker))
                    {
                        hurtboxHit = true;
                    }
                }
            }
            return hurtboxHit;
        }

        protected virtual void SortHitHurtboxes()
        {
            hurtboxes = hurtboxes.OrderBy(x=>x?.HurtboxGroup.ID).ToList();
        }

        protected virtual bool TryHitHurtbox(HitboxGroup hitboxGroup, int hitboxIndex, int hurtboxIndex, int hitboxGroupIndex, GameObject attacker)
        {
            // Owner was already hit by this ID group or is null, ignore it.
            if (hurtboxes[hurtboxIndex] == null || collidedIHurtables[hitboxGroup.ID].hitIHurtables.Contains(hurtboxes[hurtboxIndex].Owner))
            {
                return false;
            }
            // Additional filtering.
            if (ShouldHurt(hitboxGroup, hitboxIndex, hurtboxes[hurtboxIndex]) == false)
            {
                return false;
            }
            collidedIHurtables[hitboxGroup.ID].hitIHurtables.Add(hurtboxes[hurtboxIndex].Owner);
            collidedIHurtables[hitboxGroup.ID].hitboxGroups.Add(hitboxGroupIndex);
            HurtHurtbox(hitboxGroup, hitboxIndex, hurtboxes[hurtboxIndex], attacker);
            return true;
        }

        /// <summary>
        /// Determines if this hurtbox should be hit.
        /// </summary>
        /// <param name="hitboxGroup">The hitbox group of the hitbox.</param>
        /// <param name="hitboxIndex">The index of the hitbox.</param>
        /// <param name="hurtbox">The hurtbox that was hit.</param>
        /// <returns>If the hurtbox should be hurt.</returns>
        protected virtual bool ShouldHurt(HitboxGroup hitboxGroup, int hitboxIndex, Hurtbox hurtbox)
        {
            return true;
        }

        protected virtual void HurtHurtbox(HitboxGroup hitboxGroup, int hitboxIndex, Hurtbox hurtbox, GameObject attacker)
        {
            HitReactionBase reaction = hurtbox.Hurtable.Hurt(BuildHurtInfo(hitboxGroup, hitboxIndex, hurtbox, attacker));
            OnHitHurtbox?.Invoke(hitboxGroup, hitboxIndex, hurtbox, reaction);
        }

        protected virtual HurtInfoBase BuildHurtInfo(HitboxGroup hitboxGroup, int hitboxIndex, Hurtbox hurtbox, GameObject attacker)
        {
            return new HurtInfoBase();
        }

        protected virtual void CheckBoxCollision(HitboxGroup hitboxGroup, int boxIndex)
        {

        }
    }
}