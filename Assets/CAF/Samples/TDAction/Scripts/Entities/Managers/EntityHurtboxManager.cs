using CAF.Combat;
using System.Collections;
using System.Collections.Generic;
using TDAction.Managers;
using UnityEngine;

namespace TDAction.Entities
{
    public class EntityHurtboxManager : CAF.Entities.EntityHurtboxManager
    {
        protected override void SetHurtboxInfo(int groupID, int hurtboxIndex)
        {

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