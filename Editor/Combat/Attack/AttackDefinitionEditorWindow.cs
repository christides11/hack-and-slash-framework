using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CAF.Combat
{
    public class AttackDefinitionEditorWindow : EditorWindow
    {
        private AttackDefinition attack;
        private int currentMenu = 0;

        public static void Init(AttackDefinition attack)
        {
            AttackDefinitionEditorWindow window =
                (AttackDefinitionEditorWindow)EditorWindow.GetWindow(typeof(AttackDefinitionEditorWindow));
            window.attack = attack;
            window.Show();
        }

        protected virtual void OnGUI()
        {
            attack = (AttackDefinition)EditorGUILayout.ObjectField("Attack", attack, typeof(AttackDefinition), false);

            DrawMenuBar();
            DrawMenu();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(attack);
            }
        }

        protected virtual void DrawMenuBar()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("General"))
            {
                currentMenu = 0;
            }
            if (GUILayout.Button("Cancels"))
            {
                currentMenu = 1;
            }
            if (GUILayout.Button("Boxes"))
            {
                currentMenu = 2;
            }
            if (GUILayout.Button("Events"))
            {
                currentMenu = 3;
            }
            EditorGUILayout.EndHorizontal();
        }

        protected virtual void DrawMenu()
        {
            if(currentMenu == 0)
            {
                DrawGeneralMenu();
            }
            if (currentMenu == 1)
            {
                DrawCancelWindows();
            }
            if(currentMenu == 2)
            {
                DrawBoxesMenu();
            }
            if(currentMenu == 3)
            {
                DrawEventsWindow();
            }
        }

        protected virtual void DrawGeneralMenu()
        {
            EditorGUI.BeginChangeCheck();

            string attackName = EditorGUILayout.TextField("Name", attack.attackName);
            EditorGUILayout.LabelField("Description");
            string description = EditorGUILayout.TextArea(attack.description, GUILayout.Height(50));

            int stateOverride = EditorGUILayout.IntField("State Override", attack.stateOverride);

            int length = attack.length;
            AnimationClip animationGround = attack.animationGround;
            AnimationClip animationAir = attack.animationAir;
            WrapMode wrapMode = attack.wrapMode;
            float heightRestriction = attack.heightRestriction;
            float gravityScale = attack.gravityScaleAdded;

            if (stateOverride < 0)
            {
                length = EditorGUILayout.IntField("Length", attack.length);

                GUILayout.Space(15);

                animationGround = (AnimationClip)EditorGUILayout.ObjectField("Animation (Ground)", attack.animationGround,
                        typeof(AnimationClip), false);
                animationAir = (AnimationClip)EditorGUILayout.ObjectField("Animation (Air)", attack.animationAir,
                        typeof(AnimationClip), false);
                wrapMode = (WrapMode)EditorGUILayout.EnumPopup("Wrap Mode", attack.wrapMode);

                GUILayout.Space(15);

                heightRestriction = EditorGUILayout.FloatField("Height Restriction", attack.heightRestriction);
                gravityScale = EditorGUILayout.FloatField("Gravity Scale Added", attack.gravityScaleAdded);
            }

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(attack, "Changed General Property.");
                attack.attackName = attackName;
                attack.description = description;
                attack.stateOverride = stateOverride;
                attack.length = length;
                attack.animationGround = animationGround;
                attack.animationAir = animationAir;
                attack.wrapMode = wrapMode;
                attack.heightRestriction = heightRestriction;
                attack.gravityScaleAdded = gravityScale;
            }
        }

        bool jumpCancelWindowsFoldout;
        bool enemyStepWindowsFoldout;
        bool landCancelWindowsFoldout;
        bool commandAttackCancelWindowsFoldout;
        protected virtual void DrawCancelWindows()
        {
            EditorGUI.BeginChangeCheck();

            List<Vector2Int> jumpCancelWindows = new List<Vector2Int>(attack.jumpCancelWindows);
            List<Vector2Int> enemyStepWindows = new List<Vector2Int>(attack.enemyStepWindows);
            List<Vector2Int> landCancelWindows = new List<Vector2Int>(attack.landCancelWindows);
            List<Vector2Int> commandAttackCancelWindows = new List<Vector2Int>(attack.commandAttackCancelWindows);

            DrawCancelWindow("Jump Cancel Windows", ref jumpCancelWindowsFoldout, ref jumpCancelWindows, 180);
            DrawCancelWindow("Enemy Step Windows", ref enemyStepWindowsFoldout, ref enemyStepWindows, 180);
            DrawCancelWindow("Land Cancel Windows", ref landCancelWindowsFoldout, ref landCancelWindows, 180);
            DrawCancelWindow("Command Attack Cancel Windows", ref commandAttackCancelWindowsFoldout, ref commandAttackCancelWindows, 230);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(attack, "Changed Cancel Window.");
                attack.jumpCancelWindows = jumpCancelWindows;
                attack.enemyStepWindows = enemyStepWindows;
                attack.landCancelWindows = landCancelWindows;
                attack.commandAttackCancelWindows = commandAttackCancelWindows;
            }
        }

        protected virtual void DrawCancelWindow(string foldoutName, ref bool foldout, ref List<Vector2Int> windows, float width)
        {
            EditorGUILayout.BeginHorizontal(GUILayout.Width(width));
            foldout = EditorGUILayout.Foldout(foldout, foldoutName, true);
            if (GUILayout.Button("+", GUILayout.Width(20)))
            {
                windows.Add(new Vector2Int());
            }
            EditorGUILayout.EndHorizontal();
            if (foldout)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < windows.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("-", GUILayout.Width(20)))
                    {
                        windows.RemoveAt(i);
                        continue;
                    }
                    windows[i] = EditorGUILayout.Vector2IntField("", windows[i]);
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }
        }

        private void DrawBoxesMenu()
        {

        }

        protected virtual void DrawEventsWindow()
        {

        }
    }
}