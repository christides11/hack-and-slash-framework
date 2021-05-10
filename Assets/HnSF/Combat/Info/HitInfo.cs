#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using UnityEngine;

namespace HnSF.Combat
{
    [System.Serializable]
    public class HitInfo : HitInfoBase
    {
        public bool breakArmor;
        public bool unblockable;
        public bool knockdown;
        public bool groundBounces;
        public float groundBounceForce;
        public bool wallBounces;
        public float wallBounceForce;
        public float damageOnHit;
        public float damageOnBlock;

        public bool causesTumble;
        // Set ForceType.
        public Vector3 opponentForceDir = Vector3.forward;
        // Push/Pull ForceType.
        public bool forceIncludeYForce = false;
        public float opponentForceMagnitude = 1;
        public float opponentMaxMagnitude = 1;
        public float opponentMinMagnitude = 1;

        public AttackDefinition throwConfirm;

        public HitInfo()
        {

        }

        public HitInfo(HitInfoBase other): base(other)
        {
            if(other.GetType() != typeof(HitInfo)
                && other.GetType().IsSubclassOf(typeof(HitInfo)) == false)
            {
                return;
            }
            HitInfo otherHitInfo = (HitInfo)other;
            breakArmor = otherHitInfo.breakArmor;
            unblockable = otherHitInfo.unblockable;
            knockdown = otherHitInfo.knockdown;

            groundBounces = otherHitInfo.groundBounces;
            groundBounceForce = otherHitInfo.groundBounceForce;
            wallBounces = otherHitInfo.wallBounces;
            wallBounceForce = otherHitInfo.wallBounceForce;

            damageOnHit = otherHitInfo.damageOnHit;
            damageOnBlock = otherHitInfo.damageOnBlock;

            causesTumble = otherHitInfo.causesTumble;

            opponentForceDir = otherHitInfo.opponentForceDir;
            opponentForceMagnitude = otherHitInfo.opponentForceMagnitude;

            forceIncludeYForce = otherHitInfo.forceIncludeYForce;
            opponentMaxMagnitude = otherHitInfo.opponentMaxMagnitude;
        }

        [NonSerialized] bool drawHitEffectsDropdown;
        [NonSerialized] bool drawHitDamageDropdown;
        [NonSerialized] bool drawHitForcesDropdown;
        [NonSerialized] bool drawHitStunDropdown;
        public override void DrawInspectorHitInfo()
        {
#if UNITY_EDITOR
            drawHitEffectsDropdown = EditorGUILayout.Foldout(drawHitEffectsDropdown, "EFFECT", true, EditorStyles.boldLabel);
            if (drawHitEffectsDropdown)
            {
                EditorGUI.indentLevel++;
                DrawHitEffectsOptions();
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space(10);

            drawHitDamageDropdown = EditorGUILayout.Foldout(drawHitDamageDropdown, "DAMAGE", true, EditorStyles.boldLabel);
            if (drawHitDamageDropdown)
            {
                EditorGUI.indentLevel++;
                DrawHitDamageOptions();
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space(10);

            drawHitForcesDropdown = EditorGUILayout.Foldout(drawHitForcesDropdown, "FORCE", true, EditorStyles.boldLabel);
            if (drawHitForcesDropdown)
            {
                EditorGUI.indentLevel++;
                DrawHitForcesOptions();
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space(10);

            drawHitStunDropdown = EditorGUILayout.Foldout(drawHitStunDropdown, "STUN", true, EditorStyles.boldLabel);
            if (drawHitStunDropdown)
            {
                EditorGUI.indentLevel++;
                DrawHitStunOptions();
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space(10);
#endif
        }

        public override void DrawInspectorGrabInfo()
        {
#if UNITY_EDITOR
            throwConfirm = (AttackDefinition)EditorGUILayout.ObjectField("Throw Confirm Attack",
                throwConfirm,
                typeof(AttackDefinition), false);
#endif
        }

        protected virtual void DrawHitEffectsOptions()
        {
#if UNITY_EDITOR
            groundOnly = EditorGUILayout.Toggle("Hit Ground Only?", groundOnly);
            airOnly = EditorGUILayout.Toggle("Hit Air Only?", airOnly);
            unblockable = EditorGUILayout.Toggle("Unblockable?", unblockable);
            breakArmor = EditorGUILayout.Toggle("Breaks Armor?", breakArmor);
            groundBounces = EditorGUILayout.Toggle("Ground Bounces?", groundBounces);
            wallBounces = EditorGUILayout.Toggle("Wall Bounces?", wallBounces);
            causesTumble = EditorGUILayout.Toggle("Causes Tumble?", causesTumble);
            knockdown = EditorGUILayout.Toggle("Causes Knockdown?", knockdown);
            continuousHit = EditorGUILayout.Toggle("Continuous Hit?", continuousHit);
            if (continuousHit)
            {
                spaceBetweenHits = EditorGUILayout.IntField("Space between hits", spaceBetweenHits);
            }
#endif
        }

        protected virtual void DrawHitDamageOptions()
        {
#if UNITY_EDITOR
            damageOnBlock = EditorGUILayout.FloatField("Damage (Block)", damageOnBlock);
            damageOnHit = EditorGUILayout.FloatField("Damage (Hit)", damageOnHit);
            hitKills = EditorGUILayout.Toggle("Hit Kills", hitKills);
#endif
        }

        protected virtual void DrawHitForcesOptions()
        {
#if UNITY_EDITOR
            opponentResetXForce = EditorGUILayout.Toggle("Reset X Force", opponentResetXForce);
            opponentResetYForce = EditorGUILayout.Toggle("Reset Y Force", opponentResetYForce);
            forceRelation = (HitboxForceRelation)EditorGUILayout.EnumPopup("Force Relation", forceRelation);
            forceType = (HitboxForceType)EditorGUILayout.EnumPopup("Force Type", forceType);
            switch (forceType)
            {
                case HitboxForceType.SET:
                    opponentForceDir = EditorGUILayout.Vector3Field("Force Direction", opponentForceDir);
                    break;
                case HitboxForceType.PUSH:
                    forceIncludeYForce = EditorGUILayout.Toggle("Include Y Force", forceIncludeYForce);
                    opponentForceMagnitude
                        = EditorGUILayout.FloatField("Force Multiplier", opponentForceMagnitude);
                    break;
                case HitboxForceType.PULL:
                    forceIncludeYForce = EditorGUILayout.Toggle("Include Y Force", forceIncludeYForce);
                    opponentForceMagnitude
                        = EditorGUILayout.FloatField("Force Multiplier", opponentForceMagnitude);
                    opponentMaxMagnitude
                        = EditorGUILayout.FloatField("Max Magnitude", opponentMaxMagnitude);
                    break;
            }

            if (wallBounces)
            {
                wallBounceForce = EditorGUILayout.FloatField("Wall Bounce Magnitude", wallBounceForce);
            }
#endif
        }

        protected virtual void DrawHitStunOptions()
        {
#if UNITY_EDITOR
            attackerHitstop = (ushort)EditorGUILayout.IntField("Hitstop (Attacker)",
                attackerHitstop);
            hitstop = (ushort)EditorGUILayout.IntField("Hitstop", hitstop);
            hitstun = (ushort)EditorGUILayout.IntField("Hitstun", hitstun);
#endif
        }
    }
}