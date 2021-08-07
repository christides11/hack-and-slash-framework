﻿using HnSF.Combat;
using UnityEngine;

namespace TDAction.Fighter
{
    public class FighterHitboxManager : HnSF.Fighters.FighterHitboxManager
    {
        public Vector3 referencePosition;

        public override void Reset()
        {
            base.Reset();
            referencePosition = manager.transform.position;
        }

        protected override bool ShouldHurt(HitboxGroup hitboxGroup, int hitboxIndex, Hurtbox hurtbox)
        {
            if(hurtbox.Owner.TryGetComponent(out IHurtable ih))
            {
                TDAction.Combat.TeamTypes team = (TDAction.Combat.TeamTypes)ih.GetTeam();
                if(team == Combat.TeamTypes.FFA)
                {
                    // Enemy in free for all team, hurt them.
                    return true;
                }else if(team != (Combat.TeamTypes)combatManager.GetTeam())
                {
                    // Enemy is not in our team, hurt them.
                    return true;
                }
                // Enemy is on our team.
                return false;
            }
            // Not hurtable. Ignore.
            return false;
        }

        Collider2D[] raycastHitList = new Collider2D[0];
        protected override void CheckBoxCollision(HitboxGroup hitboxGroup, int boxIndex)
        {
            TDAction.Fighter.FighterManager fm = manager as TDAction.Fighter.FighterManager;

            Vector3 modifiedOffset = (hitboxGroup.boxes[boxIndex] as HnSF.Combat.BoxDefinition).offset;
            modifiedOffset = new Vector3(modifiedOffset.x * fm.FaceDirection, modifiedOffset.y, 0);

            Vector2 position = hitboxGroup.attachToEntity ? (Vector2)manager.transform.position + (Vector2)modifiedOffset
                : (Vector2)referencePosition + (Vector2)modifiedOffset;
            Vector2 size = (Vector2)(hitboxGroup.boxes[boxIndex] as HnSF.Combat.BoxDefinition).size;
            raycastHitList = Physics2D.OverlapBoxAll(position, size, 0, combatManager.hitboxLayerMask);

            if(hurtboxes.Count < raycastHitList.Length)
            {
                hurtboxes.AddRange(new Hurtbox[raycastHitList.Length - hurtboxes.Count]);
            }
            for (int i = 0; i < raycastHitList.Length; i++)
            {
                Hurtbox h = raycastHitList[i].GetComponent<Hurtbox>();
                if(h.Owner != manager.gameObject)
                {
                    hurtboxes[i] = h;
                }
            }
        }

        protected override HurtInfoBase BuildHurtInfo(HitboxGroup hitboxGroup, int hitboxIndex, Hurtbox hurtbox, GameObject attacker)
        {
            HurtInfo2D hi2d;
            TDAction.Fighter.FighterManager fm = manager as TDAction.Fighter.FighterManager;
            switch (hitboxGroup.hitboxHitInfo.forceRelation)
            {
                case HitboxForceRelation.ATTACKER:
                    hi2d = new HurtInfo2D(hitboxGroup.hitboxHitInfo, (manager.PhysicsManager as FighterPhysicsManager).GetOverallForce(), 
                        attacker.transform.position, fm.FaceDirection);
                    break;
                case HitboxForceRelation.HITBOX:
                    Vector2 position = hitboxGroup.attachToEntity ? (Vector2)manager.transform.position + (Vector2)(hitboxGroup.boxes[hitboxIndex] as HnSF.Combat.BoxDefinition).offset
                : (Vector2)referencePosition + (Vector2)(hitboxGroup.boxes[hitboxIndex] as HnSF.Combat.BoxDefinition).offset;
                    hi2d = new HurtInfo2D(hitboxGroup.hitboxHitInfo, (manager.PhysicsManager as FighterPhysicsManager).GetOverallForce(), 
                        position, fm.FaceDirection);
                    break;
                case HitboxForceRelation.WORLD:
                    hi2d = new HurtInfo2D(hitboxGroup.hitboxHitInfo, (manager.PhysicsManager as FighterPhysicsManager).GetOverallForce(), 
                        hurtbox.transform.position, fm.FaceDirection);
                    break;
                default:
                    hi2d = new HurtInfo2D();
                    break;
            }
            return hi2d;
        }
    }
}
