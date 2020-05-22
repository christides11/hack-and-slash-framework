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

        int currentHitboxGroupIndex;
        Vector2 scrollPos;
        private void DrawBoxesMenu()
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("<", GUILayout.Width(40)))
            {
                currentHitboxGroupIndex--;
                if (currentHitboxGroupIndex < 0)
                {
                    currentHitboxGroupIndex = Mathf.Clamp(attack.boxGroups.Count - 1, 0, attack.boxGroups.Count);
                }
            }
            EditorGUILayout.LabelField($"{currentHitboxGroupIndex + 1}/{attack.boxGroups.Count}", GUILayout.Width(50));
            GUILayout.Width(50);
            if (GUILayout.Button(">", GUILayout.Width(40)))
            {
                currentHitboxGroupIndex++;
                if (currentHitboxGroupIndex >= attack.boxGroups.Count)
                {
                    currentHitboxGroupIndex = 0;
                }
            }

            if (GUILayout.Button("Add", GUILayout.Width(50)))
            {
                attack.boxGroups.Add(new BoxGroup());
            }
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                attack.boxGroups.RemoveAt(currentHitboxGroupIndex);
                currentHitboxGroupIndex--;
            }

            EditorGUILayout.EndHorizontal();
            GUILayout.Space(30);
            if (attack.boxGroups.Count == 0)
            {
                return;
            }
            BoxGroup currentGroup = attack.boxGroups[currentHitboxGroupIndex];

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            DrawHitboxGroup(currentGroup);
            EditorGUILayout.EndScrollView();
        }

        bool boxesFoldout;
        protected virtual void DrawHitboxGroup(BoxGroup currentGroup)
        {
            EditorGUILayout.LabelField("GENERAL");
            currentGroup.ID = EditorGUILayout.IntField("Group ID", currentGroup.ID);

            float activeFramesStart = currentGroup.activeFramesStart;
            float activeFramesEnd = currentGroup.activeFramesEnd;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(currentGroup.activeFramesStart.ToString(), GUILayout.Width(30));
            EditorGUILayout.MinMaxSlider(ref activeFramesStart,
                ref activeFramesEnd, 1, (float)attack.length);
            EditorGUILayout.LabelField(currentGroup.activeFramesEnd.ToString(), GUILayout.Width(30));
            EditorGUILayout.EndHorizontal();
            currentGroup.activeFramesStart = (int)activeFramesStart;
            currentGroup.activeFramesEnd = (int)activeFramesEnd;

            currentGroup.hitGroupType = (BoxGroupType)EditorGUILayout.EnumPopup("Hit Type", currentGroup.hitGroupType);
            currentGroup.attachToEntity = EditorGUILayout.Toggle("Attach to Entity", currentGroup.attachToEntity);

            EditorGUILayout.BeginHorizontal(GUILayout.Width(300));
            boxesFoldout = EditorGUILayout.Foldout(boxesFoldout, "Boxes", true);
            if (GUILayout.Button("Add"))
            {
                currentGroup.boxes.Add(new BoxDefinition());
            }
            EditorGUILayout.EndHorizontal();

            if (boxesFoldout)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < currentGroup.boxes.Count; i++)
                {
                    DrawHitboxOptions(currentGroup, i);
                    GUILayout.Space(5);
                }
                EditorGUI.indentLevel--;
            }
        }

        private void DrawHitboxOptions(BoxGroup currentGroup, int index)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                currentGroup.boxes.RemoveAt(index);
                return;
            }
            GUILayout.Label($"Group {index}");
            EditorGUILayout.EndHorizontal();
            currentGroup.boxes[index].shape = (BoxShapes)EditorGUILayout.EnumPopup("Shape", currentGroup.boxes[index].shape);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Offset", GUILayout.Width(135));
            currentGroup.boxes[index].offset.x = EditorGUILayout.FloatField(currentGroup.boxes[index].offset.x, GUILayout.Width(40));
            currentGroup.boxes[index].offset.y = EditorGUILayout.FloatField(currentGroup.boxes[index].offset.y, GUILayout.Width(40));
            currentGroup.boxes[index].offset.z = EditorGUILayout.FloatField(currentGroup.boxes[index].offset.z, GUILayout.Width(40));
            EditorGUILayout.EndHorizontal();
            switch (currentGroup.boxes[index].shape)
            {
                case BoxShapes.Rectangle:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Size", GUILayout.Width(135));
                    currentGroup.boxes[index].size.x = EditorGUILayout.FloatField(currentGroup.boxes[index].size.x, GUILayout.Width(40));
                    currentGroup.boxes[index].size.y = EditorGUILayout.FloatField(currentGroup.boxes[index].size.y, GUILayout.Width(40));
                    currentGroup.boxes[index].size.z = EditorGUILayout.FloatField(currentGroup.boxes[index].size.z, GUILayout.Width(40));
                    EditorGUILayout.EndHorizontal();
                    break;
                case BoxShapes.Circle:
                    currentGroup.boxes[index].radius
                        = EditorGUILayout.FloatField("Radius", currentGroup.boxes[index].radius);
                    break;
            }
        }

        protected virtual void DrawEventsWindow()
        {

        }
    }
}