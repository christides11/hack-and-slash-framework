using HnSF.Combat;
using HnSF.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Fighters
{
    public class FighterHurtboxManager : MonoBehaviour
    {
        [SerializeField] protected FighterBase manager;

        protected StateHurtboxDefinition hurtboxDefinition;

        protected List<Hurtbox> hurtboxPool = new List<Hurtbox>();
        protected Dictionary<int, List<Hurtbox>> hurtboxGroups = new Dictionary<int, List<Hurtbox>>();

        public virtual void Tick()
        {
            CreateHurtboxes(hurtboxDefinition, manager.StateManager.CurrentStateFrame);
        }

        List<int> hurtboxGroupsToDelete = new List<int>();
        public virtual void CreateHurtboxes(StateHurtboxDefinition currentHurtboxDefinition, uint frame)
        {
            if(currentHurtboxDefinition == null)
            {
                return;
            }

            for(int i = 0; i < currentHurtboxDefinition.hurtboxGroups.Count; i++)
            {
                if (!hurtboxGroups.ContainsKey(i))
                {
                    hurtboxGroups.Add(i, new List<Hurtbox>());
                }

                // STRAY HURTBOX CLEANUP //
                for (int s = currentHurtboxDefinition.hurtboxGroups[i].boxes.Count; s < hurtboxGroups[i].Count; s++)
                {
                    DestroyHurtbox(hurtboxGroups[i][s]);
                    hurtboxGroups[i].RemoveAt(s);
                }

                // CHECK CLEANUP WINDOW //
                if(CheckWithinHurtboxWindow(currentHurtboxDefinition, i, frame) == false)
                {
                    CleanupHurtboxGroup(i);
                    continue;
                }

                // CREATE HURTBOX GROUPS //
                for(int w = 0; w < currentHurtboxDefinition.hurtboxGroups[i].boxes.Count; w++)
                {
                    Hurtbox hurtbox;
                    // Group doesn't already have a hurtbox, create one.
                    if(hurtboxGroups[i].Count <= w)
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
            }

            // CLEANUP EXTRA HURTBOXES //
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

        protected virtual bool CheckWithinHurtboxWindow(StateHurtboxDefinition currentHurtboxDefinition, int groupIndex, uint frame)
        {
            if (currentHurtboxDefinition.hurtboxGroups[groupIndex].activeFramesEnd == -1)
            {
                return true;
            }
            if (frame > currentHurtboxDefinition.hurtboxGroups[groupIndex].activeFramesEnd
                || frame < currentHurtboxDefinition.hurtboxGroups[groupIndex].activeFramesStart)
            {
                return false;
            }
            return true;
        }

        protected virtual void CleanupHurtboxGroup(int groupID)
        {
            for(int i = 0; i < hurtboxGroups[groupID].Count; i++)
            {
                DestroyHurtbox(hurtboxGroups[groupID][i]);
            }
            hurtboxGroups[groupID].Clear();
        }

        protected virtual void SetHurtboxInfo(int groupID, int hurtboxIndex)
        {
            throw new NotImplementedException();
        }

        protected virtual Hurtbox CreateHurtbox()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deactivates a hurtbox and returns it to the pool.
        /// </summary>
        /// <param name="hurtbox">The hurtbox to deactivate.</param>
        protected virtual void DestroyHurtbox(Hurtbox hurtbox)
        {
            hurtboxPool.Add(hurtbox);
            hurtbox.gameObject.SetActive(false);
        }

        public virtual void SetHurtboxDefinition(StateHurtboxDefinition stateHurtboxDefinition)
        {
            hurtboxDefinition = stateHurtboxDefinition;
        }
    }
}