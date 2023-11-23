using UnityEditor;
using UnityEngine;

namespace HnSF.Combat
{
    [CustomPropertyDrawer(typeof(HitInfo), true)]
    public class HitInfoPropertyDrawer : HitInfoBasePropertyDrawer
    {

        protected bool damageFoldoutGroup;
        
        public override void DrawProperty(ref Rect position, SerializedProperty property)
        {
            base.DrawProperty(ref position, property);

            // FORCES //
            forcesFoldoutGroup = EditorGUI.BeginFoldoutHeaderGroup(new Rect(position.x, GetLineY(), position.width, lineHeight),
                forcesFoldoutGroup, new GUIContent("Forces"));
            if (forcesFoldoutGroup)
            {
                EditorGUI.indentLevel++;
                DrawForcesGroup(ref position, property);
                EditorGUI.indentLevel--;
            }
            EditorGUI.EndFoldoutHeaderGroup();

            // STUN //
            stunFoldoutGroup = EditorGUI.BeginFoldoutHeaderGroup(new Rect(position.x, GetLineY(), position.width, lineHeight),
                stunFoldoutGroup, new GUIContent("Stun"));
            if (stunFoldoutGroup)
            {
                EditorGUI.indentLevel++;
                DrawStunGroup(position, property);
                EditorGUI.indentLevel--;
            }
            EditorGUI.EndFoldoutHeaderGroup();
            
            // DAMAGE //
            damageFoldoutGroup = EditorGUI.BeginFoldoutHeaderGroup(new Rect(position.x, GetLineY(), position.width, lineHeight),
                damageFoldoutGroup, new GUIContent("Damage"));
            if (damageFoldoutGroup)
            {
                EditorGUI.indentLevel++;
                DrawDamageGroup(position, property);
                EditorGUI.indentLevel--;
            }
            EditorGUI.EndFoldoutHeaderGroup();
        }

        protected virtual void DrawDamageGroup(Rect position, SerializedProperty property)
        {
            EditorGUI.PropertyField(new Rect(position.x, GetLineY(), position.width, lineHeight), property.FindPropertyRelative("damageOnHit"), true);
        }

        protected override void DrawGeneralGroup(ref Rect position, SerializedProperty property)
        {
            base.DrawGeneralGroup(ref position, property);
            EditorGUI.PropertyField(new Rect(position.x, GetLineY(), position.width, lineHeight), property.FindPropertyRelative("causesTumble"), true);
            EditorGUI.PropertyField(new Rect(position.x, GetLineY(), position.width, lineHeight), property.FindPropertyRelative("knockdown"), true);
        }

        protected virtual void DrawStunGroup(Rect position, SerializedProperty property)
        {
        }

        protected virtual void DrawForcesGroup(ref Rect position, SerializedProperty property)
        {
            switch((HitboxForceType)property.FindPropertyRelative("forceType").enumValueIndex)
            {
                case HitboxForceType.SET:
                    EditorGUI.LabelField(new Rect(position.x, GetLineY(), 100, lineHeight), new GUIContent("Force"));
                    EditorGUI.PropertyField(new Rect(position.x+140, GetLineY(), position.width-140, lineHeight), property.FindPropertyRelative("opponentForce"),
                        GUIContent.none, true);
                    break;
                default:
                    EditorGUI.PropertyField(new Rect(position.x, GetLineY(), position.width, lineHeight), property.FindPropertyRelative("forceIncludeYForce"), true);
                    EditorGUI.PropertyField(new Rect(position.x, GetLineY(), position.width, lineHeight), property.FindPropertyRelative("opponentForceMultiplier"), true);
                    EditorGUI.PropertyField(new Rect(position.x, GetLineY(), position.width, lineHeight), property.FindPropertyRelative("opponentMinMagnitude"), true);
                    EditorGUI.PropertyField(new Rect(position.x, GetLineY(), position.width, lineHeight), property.FindPropertyRelative("opponentMaxMagnitude"), true);
                    break;
            }
            EditorGUI.PropertyField(new Rect(position.x, GetLineY(), position.width, lineHeight), property.FindPropertyRelative("groundBounces"), true);
            if (property.FindPropertyRelative("groundBounces").boolValue)
            {
                EditorGUI.PropertyField(new Rect(position.x, GetLineY(), position.width, lineHeight), property.FindPropertyRelative("groundBounceForce"), true);
            }

            EditorGUI.PropertyField(new Rect(position.x, GetLineY(), position.width, lineHeight), property.FindPropertyRelative("wallBounces"), true);
            if (property.FindPropertyRelative("wallBounces").boolValue)
            {
                EditorGUI.PropertyField(new Rect(position.x, GetLineY(), position.width, lineHeight), property.FindPropertyRelative("wallBounceForce"), true);
            }
        }
    }
}