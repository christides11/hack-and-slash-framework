using CAF.Combat;
using CAF.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Entities
{
    public class EntityHurtboxManager : MonoBehaviour
    {
        [SerializeField] protected EntityManager manager;

        protected StateHurtboxDefinition currentHurtboxDefinition;

        protected List<Hurtbox> hurtboxPool = new List<Hurtbox>();
        protected Dictionary<int, List<Hurtbox>> hurtboxGroups = new Dictionary<int, List<Hurtbox>>();

        public virtual void Tick()
        {
            CreateHurtboxes(manager.StateManager.CurrentStateFrame);
        }

        List<int> hurtboxGroupsToDelete = new List<int>();
        public virtual void CreateHurtboxes(uint frame)
        {
            if(currentHurtboxDefinition == null)
            {
                return;
            }

            // Create hurtbox groups.
            for(int i = 0; i < currentHurtboxDefinition.hurtboxGroups.Count; i++)
            {
                if (!hurtboxGroups.ContainsKey(i))
                {
                    hurtboxGroups.Add(i, new List<Hurtbox>());
                }
                // Create Hurtboxes.
                for(int w = 0; w < currentHurtboxDefinition.hurtboxGroups[i].boxes.Count; w++)
                {
                    Hurtbox hurtbox;
                    // Group doesn't already have a hurtbox, create one.
                    if(hurtboxGroups[i].Count < w)
                    {
                        hurtbox = CreateHurtbox();
                        hurtboxGroups[i].Add(hurtbox);
                    }
                    else
                    {
                        // Group has a hurtbox here already.
                        hurtbox = hurtboxGroups[i][w];
                    }
                    hurtbox.gameObject.SetActive(true);

                    // Set the hurtbox's position/rotation/etc.
                    SetHurtboxInfo(i, w);
                }
                // Cleanup stray hurtboxes.
                for(int s = currentHurtboxDefinition.hurtboxGroups[i].boxes.Count; s < hurtboxGroups[i].Count; s++)
                {
                    DestroyHurtbox(hurtboxGroups[i][s]);
                    hurtboxGroups[i].RemoveAt(s);
                }
            }

            // Cleanup stray hurtbox groups.
            foreach(int k in hurtboxGroups.Keys)
            {
                if(k >= currentHurtboxDefinition.hurtboxGroups.Count)
                {
                    hurtboxGroupsToDelete.Add(k);
                    CleanupHurtboxGroup(k);
                }
            }
            for(int h = 0; h < hurtboxGroupsToDelete.Count; h++)
            {
                hurtboxGroups.Remove(h);
            }
            hurtboxGroupsToDelete.Clear();
        }

        private void CleanupHurtboxGroup(int groupID)
        {
            for(int i = 0; i < hurtboxGroups[groupID].Count; i++)
            {
                DestroyHurtbox(hurtboxGroups[groupID][i]);
            }
        }

        protected virtual void SetHurtboxInfo(int groupID, int hurtboxIndex)
        {
            throw new NotImplementedException();
        }

        protected virtual Hurtbox CreateHurtbox()
        {
            throw new NotImplementedException();
        }

        protected virtual void DestroyHurtbox(Hurtbox hurtbox)
        {
            hurtboxPool.Add(hurtbox);
            hurtbox.gameObject.SetActive(false);
        }

        public virtual void SetHurtboxDefinition(StateHurtboxDefinition stateHurtboxDefinition)
        {
            currentHurtboxDefinition = stateHurtboxDefinition;
        }
    }
}