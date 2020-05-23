using CAF.Combat;
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
        // Hitbox Group : Hitboxes
        private Dictionary<int, List<Hitbox>> hitboxGroups = new Dictionary<int, List<Hitbox>>();
        // Hitbox ID : Hit IHurtables
        private Dictionary<int, List<IHurtable>> hurtablesHit = new Dictionary<int, List<IHurtable>>();

        private EntityCombatManager combatManager;
        private EntityController controller;

        public EntityHitboxManager(EntityCombatManager combatManager, EntityController controller)
        {
            this.combatManager = combatManager;
            this.controller = controller;
        }

        /// <summary>
        /// Destroys all boxes and clears variables.
        /// </summary>
        public void Reset()
        {
            CleanupAllHitboxes();
            hurtablesHit.Clear();
        }

        /// <summary>
        /// Destroy all the boxes and clears the dictionary.
        /// </summary>
        private void CleanupAllHitboxes()
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
        /// Checks the hitboxes and detectboxes to see what they hit this frame.
        /// This should be called in late update, as physics update right after update.
        /// </summary>
        public void TickBoxes()
        {
        }

        #region Hitboxes
        /// <summary>
        /// Create the hitbox group of the given index.
        /// </summary>
        /// <param name="index">The index of the hitbox group.</param>
        public void CreateHitboxGroup(int index)
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
                Vector3 pos = controller.GetVisualBasedDirection(Vector3.forward) * hitboxDefinition.offset.z
                    + controller.GetVisualBasedDirection(Vector3.right) * hitboxDefinition.offset.x
                    + controller.GetVisualBasedDirection(Vector3.up) * hitboxDefinition.offset.y;

                Hitbox hbox = InstantiateHitbox(controller.transform.position + pos,
                    Quaternion.Euler(controller.transform.eulerAngles + hitboxDefinition.rotation));

                // Attach the hitbox if neccessary.
                if (currentGroup.attachToEntity)
                {
                    hbox.transform.SetParent(controller.transform, true);
                }
            }
            // Add the hitbox group to our list.
            hitboxGroups.Add(index, groupHitboxList);
        }

        protected virtual Hitbox InstantiateHitbox(Vector3 position, Quaternion rotation)
        {
            return null;
        }
        #endregion
    }
}