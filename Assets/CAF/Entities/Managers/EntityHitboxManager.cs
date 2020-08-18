using CAF.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Entities
{
    /// <summary>
    /// Handles the hitboxes and other boxes used by entities for combat.
    /// </summary>
    public class EntityHitboxManager
    {
        public delegate void HitboxGroupEventAction(GameObject hurtableHit, int hitboxGroupIndex, MovesetAttackNode attack);
        public event HitboxGroupEventAction OnHitboxHit;

        // Hitbox Group : Hitboxes
        protected Dictionary<int, List<Hitbox>> hitboxGroups = new Dictionary<int, List<Hitbox>>();
        // Hitbox ID : Hit IHurtables
        protected Dictionary<int, List<IHurtable>> hurtablesHit = new Dictionary<int, List<IHurtable>>();

        public EntityCombatManager combatManager;
        public EntityManager manager;

        public EntityHitboxManager(EntityCombatManager combatManager, EntityManager manager)
        {
            this.combatManager = combatManager;
            this.manager = manager;
        }

        public virtual List<IHurtable> GetHitList(int group)
        {
            if (!hurtablesHit.ContainsKey(group))
            {
                return null;
            }
            return hurtablesHit[group];
        }

        /// <summary>
        /// Destroys all boxes and clears variables.
        /// </summary>
        public virtual void Reset()
        {
            CleanupAllHitboxes();
            hurtablesHit.Clear();
        }

        /// <summary>
        /// Destroy all the boxes and clears the dictionary.
        /// </summary>
        protected virtual void CleanupAllHitboxes()
        {
            foreach (int key in hitboxGroups.Keys)
            {
                for (int i = 0; i < hitboxGroups[key].Count; i++)
                {
                    GameObject.Destroy(hitboxGroups[key][i].gameObject);
                }
            }
            hitboxGroups.Clear();
        }

        /// <summary>
        /// Checks the hitboxes to see what they hit this frame.
        /// This should be called in late update, as physics update right after update.
        /// </summary>
        public virtual void TickBoxes()
        {
            foreach(List<Hitbox> hitboxGroup in hitboxGroups.Values)
            {
                for(int i = 0; i < hitboxGroup.Count; i++)
                {
                    hitboxGroup[i].CheckHits();
                }
            }
        }

        /// <summary>
        /// Activate the hitbox group with the given index.
        /// </summary>
        /// <param name="index">The index of the hitbox group.</param>
        public virtual void ActivateHitboxGroup(int index)
        {
            if (!hitboxGroups.ContainsKey(index))
            {
                return;
            }

            for (int i = 0; i < hitboxGroups[index].Count; i++)
            {
                hitboxGroups[index][i].Activate();
            }
        }

        /// <summary>
        /// Deactivate the hitbox group with the given index.
        /// </summary>
        /// <param name="index">The index of the hitbox group.</param>
        public virtual void DeactivateHitboxGroup(int index)
        {
            if (!hitboxGroups.ContainsKey(index))
            {
                return;
            }

            for (int i = 0; i < hitboxGroups[index].Count; i++)
            {
                hitboxGroups[index][i].Deactivate();
            }
        }

        #region Hitboxes
        /// <summary>
        /// Create the hitbox group of the given index.
        /// </summary>
        /// <param name="index">The index of the hitbox group.</param>
        public virtual void CreateHitboxGroup(int index)
        {
            // Group was already created.
            if (hitboxGroups.ContainsKey(index))
            {
                return;
            }

            // Variables.
            BoxGroup currentGroup = combatManager.CurrentAttack.attackDefinition.boxGroups[index];
            List<Hitbox> groupHitboxList = new List<Hitbox>(currentGroup.boxes.Count);

            // Keep track of what the hitbox ID has hit.
            if (!hurtablesHit.ContainsKey(currentGroup.ID))
            {
                hurtablesHit.Add(currentGroup.ID, new List<IHurtable>());
                hurtablesHit[currentGroup.ID].Add(combatManager);
            }

            // Loop through all the hitboxes in the group.
            for (int i = 0; i < currentGroup.boxes.Count; i++)
            {
                // Instantiate the hitbox with the correct position and rotation.
                BoxDefinition hitboxDefinition = currentGroup.boxes[i];
                Hitbox hitbox = InstantiateHitbox(GetHitboxPosition(hitboxDefinition),
                    GetHitboxRotation(hitboxDefinition));

                // Attach the hitbox if neccessary.
                if (currentGroup.attachToEntity)
                {
                    hitbox.transform.SetParent(manager.transform, true);
                }

                hitbox.Initialize(manager.gameObject, manager.visual.transform, currentGroup.boxes[i].shape,
                    currentGroup.hitboxHitInfo, hitboxDefinition, hurtablesHit[currentGroup.ID]);
                int cID = currentGroup.ID;
                int groupIndex = index;
                hitbox.OnHurt += (hurtable, hitInfo) => { OnHitboxHurt(hurtable, hitInfo, cID, groupIndex); };
                hitbox.Activate();
                groupHitboxList.Add(hitbox);
            }
            // Add the hitbox group to our list.
            hitboxGroups.Add(index, groupHitboxList);
        }

        protected virtual Quaternion GetHitboxRotation(BoxDefinition hitboxDefinition)
        {
            return Quaternion.Euler(manager.visual.transform.eulerAngles + hitboxDefinition.rotation);
        }

        /// <summary>
        /// Calculates where the hitbox being created should be positioned.
        /// </summary>
        /// <param name="hitboxDefinition">The definition of the hitbox.</param>
        /// <returns>The position the hitbox should be in.</returns>
        protected virtual Vector3 GetHitboxPosition(BoxDefinition hitboxDefinition)
        {
            return manager.transform.position
                + manager.GetVisualBasedDirection(Vector3.forward) * hitboxDefinition.offset.z
                + manager.GetVisualBasedDirection(Vector3.right) * hitboxDefinition.offset.x
                + manager.GetVisualBasedDirection(Vector3.up) * hitboxDefinition.offset.y;
        }

        protected virtual Hitbox InstantiateHitbox(Vector3 position, Quaternion rotation)
        {
            throw new NotImplementedException("InstantiateHitbox in EntityHitboxManager must be overriden!");
        }

        /// <summary>
        /// Called whenever a hitbox hits a hurtbox successfully.
        /// </summary>
        /// <param name="hurtableHit">The hurtable that was hit.</param>
        /// <param name="hitInfo">The hitInfo of the hitbox.</param>
        /// <param name="hitboxID">The hitbox ID of the hitbox.</param>
        protected virtual void OnHitboxHurt(GameObject hurtableHit, HitInfoBase hitInfo, int hitboxID, int hitboxGroup)
        {
            hurtablesHit[hitboxID].Add(hurtableHit.GetComponent<IHurtable>());
            combatManager.SetHitStop(hitInfo.attackerHitstop);
            UpdateHitboxIDIgnoreList(hitboxID);
            OnHitboxHit?.Invoke(hurtableHit, hitboxGroup, combatManager.CurrentAttack);
        }

        /// <summary>
        /// Updates the ignore/hit list of all hitboxes with the given ID.
        /// </summary>
        /// <param name="hitboxID">The ID to update the hitboxes for.</param>
        protected virtual void UpdateHitboxIDIgnoreList(int hitboxID)
        {
            foreach (int key in hitboxGroups.Keys)
            {
                if(combatManager.CurrentAttack.attackDefinition.boxGroups[key].ID != hitboxID)
                {
                    continue;
                }
                for (int i = 0; i < hitboxGroups[key].Count; i++)
                {
                    hitboxGroups[key][i].ignoreList = hurtablesHit[hitboxID];
                }
            }
        }
        #endregion
    }
}