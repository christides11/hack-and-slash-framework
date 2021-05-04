using CAF.Combat;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Fighters
{
    /// <summary>
    /// Handles the hitboxes and other boxes used by entities for combat.
    /// </summary>
    public class FighterHitboxManager
    {
        protected class IDGroupCollisionInfo{
            public List<GameObject> hitIHurtables = new List<GameObject>();
            public HashSet<int> hitboxGroups = new HashSet<int>();
        }

        public delegate void HitboxGroupEventAction(HitboxGroup hitboxGroup, int hitboxIndex, Hurtbox hurtbox);
        public event HitboxGroupEventAction OnHitHurtbox;

        // ID Group : Owners Hit
        protected Dictionary<int, IDGroupCollisionInfo> collidedIHurtables = new Dictionary<int, IDGroupCollisionInfo>();

        public FighterCombatManager combatManager;
        public FighterBase manager;

        public FighterHitboxManager(FighterCombatManager combatManager, FighterBase manager)
        {
            this.combatManager = combatManager;
            this.manager = manager;
        }

        public virtual void Reset()
        {
            collidedIHurtables.Clear();
        }

        public virtual GameObject[] GetIDGroupCollisions(int IDGroup)
        {
            if(collidedIHurtables.ContainsKey(IDGroup) == false)
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

        public virtual bool CheckForCollision(int hitboxGroupIndex, HitboxGroup hitboxGroup)
        {
            Hurtbox[] hurtboxes = null;
            for(int i = 0; i < hitboxGroup.boxes.Count; i++)
            {
                hurtboxes = CheckBoxCollision(hitboxGroup, i);

                // This hitbox hit nothing.
                if(hurtboxes == null || hurtboxes.Length == 0)
                {
                    continue;
                }

                // Hit thing(s). Check if we should actually hurt them.
                if (!collidedIHurtables.ContainsKey(hitboxGroup.ID))
                {
                    collidedIHurtables.Add(hitboxGroup.ID, new IDGroupCollisionInfo());
                }
                for(int j = 0; j < hurtboxes.Length; j++)
                {
                    // Owner was already hit by this ID group or is null, ignore it.
                    if (hurtboxes[j] == null || collidedIHurtables[hitboxGroup.ID].hitIHurtables.Contains(hurtboxes[j].Owner))
                    {
                        continue;
                    }
                    // Additional filtering.
                    if (ShouldHurt(hitboxGroup, i, hurtboxes[j]) == false)
                    {
                        continue;
                    }
                    HurtHurtbox(hitboxGroup, i, hurtboxes[j]);
                    collidedIHurtables[hitboxGroup.ID].hitIHurtables.Add(hurtboxes[j].Owner);
                    collidedIHurtables[hitboxGroup.ID].hitboxGroups.Add(hitboxGroupIndex);
                }
            }
            return false;
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

        protected virtual void HurtHurtbox(HitboxGroup hitboxGroup, int hitboxIndex, Hurtbox hurtbox)
        {
            combatManager.SetHitStop(hitboxGroup.hitboxHitInfo.attackerHitstop);
            hurtbox.Hurtable.Hurt(BuildHurtInfo(hitboxGroup, hitboxIndex, hurtbox));
            OnHitHurtbox?.Invoke(hitboxGroup, hitboxIndex, hurtbox);
        }

        protected virtual HurtInfoBase BuildHurtInfo(HitboxGroup hitboxGroup, int hitboxIndex, Hurtbox hurtbox)
        {
            return new HurtInfoBase();
        }

        protected virtual Hurtbox[] CheckBoxCollision(HitboxGroup hitboxGroup, int boxIndex)
        {
            return null;
        }
    }
}