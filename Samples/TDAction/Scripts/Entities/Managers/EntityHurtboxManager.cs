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
            BoxDefinition bd = (BoxDefinition)currentHurtboxDefinition.hurtboxGroups[groupID].boxes[hurtboxIndex];
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
                hurtbox.Owner = gameObject;
            }
            hurtbox.gameObject.SetActive(false);
            return hurtbox;
        }
    }
}