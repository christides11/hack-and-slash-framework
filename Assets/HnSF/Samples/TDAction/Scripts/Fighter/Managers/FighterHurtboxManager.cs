using HnSF.Combat;
using HnSF.Fighters;
using System.Collections;
using System.Collections.Generic;
using TDAction.Managers;
using UnityEngine;

namespace TDAction.Fighter
{
    public class FighterHurtboxManager : HnSF.Fighters.FighterHurtboxManager
    {
        public override IFighterBase Manager { get { return manager; } }
        public override IFighterStateManager StateManager { get { return stateManager; } }

        [SerializeField] protected FighterManager manager;
        [SerializeField] protected FighterStateManager stateManager;

        protected override void SetHurtboxInfo(int groupID, int hurtboxIndex)
        {
            BoxDefinition bd = (BoxDefinition)hurtboxDefinition.hurtboxGroups[groupID].boxes[hurtboxIndex];
            BoxCollider2D bc = hurtboxGroups[groupID][hurtboxIndex].GetComponent<BoxCollider2D>();
            bc.size = bd.size;
            bc.transform.localPosition = bd.offset;
        }

        protected override Hurtbox CreateHurtbox()
        {
            Hurtbox hurtbox;
            // Hurtbox in the pool.
            if(hurtboxPool.Count > 0)
            {
                hurtbox = hurtboxPool[0];
                hurtboxPool.RemoveAt(0);
            }
            else
            {
                hurtbox = GameObject.Instantiate(GameManager.instance.hurtboxPrefab, gameObject.transform, false);
            }
            hurtbox.gameObject.SetActive(false);
            return hurtbox;
        }
    }
}