using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HnSF.Combat
{
    public class ChargeGroupEditorWindow : EditorWindow
    {
        public ChargeDefinition chargeGroup;
        public AttackDefinition attack;

        public static void Init(AttackDefinition attack, ChargeDefinition chargeGroup)
        {
            ChargeGroupEditorWindow window =
                (ChargeGroupEditorWindow)EditorWindow.GetWindow(typeof(ChargeGroupEditorWindow),
                true,
                $"Charge Group {chargeGroup.startFrame}~{chargeGroup.endFrame}");
            window.attack = attack;
            window.chargeGroup = chargeGroup;
            window.Show();
        }

        Vector2 scrollPos;
        protected virtual void OnGUI()
        {
            if (chargeGroup == null)
            {
                Close();
            }
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            DrawChargeGroupInfo();
            EditorGUILayout.EndScrollView();
        }

        protected virtual void DrawChargeGroupInfo()
        {
            EditorGUI.indentLevel++;
            chargeGroup.startFrame = EditorGUILayout.IntField("Frame", chargeGroup.startFrame);
            chargeGroup.releaseOnCompletion = EditorGUILayout.Toggle("Auto Release?",
                chargeGroup.releaseOnCompletion);

            using (var cHorizontalScope = new GUILayout.HorizontalScope())
            {
                GUILayout.Space(30f); // horizontal indent size od 20 (pixels?)

                using (var cVerticalScope = new GUILayout.VerticalScope())
                {
                    if (GUILayout.Button("Add Charge Level"))
                    {
                        chargeGroup.chargeLevels.Add(CreateChargeLevelInstance());
                    }

                    for (int w = 0; w < chargeGroup.chargeLevels.Count; w++)
                    {
                        DrawChargeLevel(w);
                        EditorGUILayout.Space();
                    }
                }
            }
            EditorGUI.indentLevel--;
        }

        protected virtual void DrawChargeLevel(int index)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("X", GUILayout.Width(30)))
            {
                chargeGroup.chargeLevels.RemoveAt(index);
                return;
            }
            GUILayout.Label($"Level {index + 1}");
            EditorGUILayout.EndHorizontal();
            chargeGroup.chargeLevels[index].maxChargeFrames =
                EditorGUILayout.IntField("Max Charge Frames", chargeGroup.chargeLevels[index].maxChargeFrames);
        }

        protected virtual ChargeLevel CreateChargeLevelInstance()
        {
            return new ChargeLevel();
        }
    }
}