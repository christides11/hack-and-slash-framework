using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CAF.Combat
{
    public class AttackDefinitionEditorWindow : EditorWindow
    {
        protected AttackDefinition attack;
        protected int currentMenu = 0;

        public static void Init(AttackDefinition attack)
        {
            AttackDefinitionEditorWindow window =
                ScriptableObject.CreateInstance<AttackDefinitionEditorWindow>();
            window.attack = attack;
            window.Show();
        }

        protected Dictionary<string, Type> attackEventTypes = new Dictionary<string, Type>();
        protected Dictionary<string, Type> hitInfoTypes = new Dictionary<string, Type>();
        protected Dictionary<string, Type> boxDefinitionTypes = new Dictionary<string, Type>();
        protected virtual void OnFocus()
        {
            attackEventTypes.Clear();
            hitInfoTypes.Clear();
            boxDefinitionTypes.Clear();
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var givenType in a.GetTypes())
                {
                    if (givenType.IsSubclassOf(typeof(AttackEvent)))
                    {
                        attackEventTypes.Add(givenType.FullName, givenType);
                    }
                    if (givenType.IsSubclassOf(typeof(HitInfoBase)))
                    {
                        hitInfoTypes.Add(givenType.FullName, givenType);
                    }
                    if (givenType.IsSubclassOf(typeof(BoxDefinitionBase)))
                    {
                        boxDefinitionTypes.Add(givenType.FullName, givenType);
                    }
                }
            }
        }

        protected virtual void OnGUI()
        {
            attack = (AttackDefinition)EditorGUILayout.ObjectField("Attack", attack, typeof(AttackDefinition), false);

            EditorGUILayout.BeginHorizontal();
            DrawMenuBar();
            EditorGUILayout.EndHorizontal();
            DrawMenu();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(attack);
            }
        }

        protected virtual void DrawMenuBar()
        {
            if (GUILayout.Button("General"))
            {
                currentMenu = 0;
            }
            if (GUILayout.Button("Windows"))
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
        }

        protected virtual void DrawMenu()
        {
            if(currentMenu == 0)
            {
                DrawGeneralMenu();
            }
            if (currentMenu == 1)
            {
                DrawWindowsMenu();
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

            if (stateOverride < 0)
            {
                length = Mathf.Clamp(EditorGUILayout.IntField("Length", attack.length), 1, int.MaxValue);

                GUILayout.Space(15);

                EditorGUILayout.LabelField("ANIMATION", EditorStyles.boldLabel);
                animationGround = (AnimationClip)EditorGUILayout.ObjectField("Animation (Ground)", attack.animationGround,
                        typeof(AnimationClip), false);
                animationAir = (AnimationClip)EditorGUILayout.ObjectField("Animation (Air)", attack.animationAir,
                        typeof(AnimationClip), false);
                wrapMode = (WrapMode)EditorGUILayout.EnumPopup("Wrap Mode", attack.wrapMode);
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
            }
        }

        protected bool jumpCancelWindowsFoldout;
        protected bool enemyStepWindowsFoldout;
        protected bool landCancelWindowsFoldout;
        protected bool commandAttackCancelWindowsFoldout;
        protected bool chargeWindowsFoldout;
        protected bool cancelWindowsFoldout;

        protected virtual void DrawWindowsMenu()
        {
            chargeWindowsFoldout = EditorGUILayout.Foldout(chargeWindowsFoldout, "CHARGE WINDOWS", true, EditorStyles.boldLabel);
            if (chargeWindowsFoldout)
            {
                EditorGUI.indentLevel++;
                EditorGUI.BeginChangeCheck();
                if (GUILayout.Button("Add Window"))
                {
                    attack.chargeWindows.Add(CreateChargeDefinition());
                }
                for(int i = 0; i < attack.chargeWindows.Count; i++)
                {
                    DrawChargeWindow(i);
                    EditorGUILayout.Space();
                }
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(attack, "Changed Charge Window.");
                }
                EditorGUI.indentLevel--;
            }

            cancelWindowsFoldout = EditorGUILayout.Foldout(cancelWindowsFoldout, "CANCEL WINDOWS", true, EditorStyles.boldLabel);
            List<Vector2Int> jumpCancelWindows = new List<Vector2Int>(attack.jumpCancelWindows);
            List<Vector2Int> enemyStepWindows = new List<Vector2Int>(attack.enemyStepWindows);
            List<Vector2Int> landCancelWindows = new List<Vector2Int>(attack.landCancelWindows);
            List<Vector2Int> commandAttackCancelWindows = new List<Vector2Int>(attack.commandAttackCancelWindows);
            if (cancelWindowsFoldout)
            {
                EditorGUI.indentLevel++;
                EditorGUI.BeginChangeCheck();
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
                EditorGUI.indentLevel--;
            }
        }

        protected virtual void DrawChargeWindow(int i)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("X", GUILayout.Width(30)))
            {
                attack.chargeWindows.RemoveAt(i);
                return;
            }
            GUILayout.Label($"{i}.");
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel++;
            attack.chargeWindows[i].frame = EditorGUILayout.IntField("Frame", attack.chargeWindows[i].frame);
            attack.chargeWindows[i].releaseOnCompletion = EditorGUILayout.Toggle("Auto Release?", 
                attack.chargeWindows[i].releaseOnCompletion);
            if(GUILayout.Button("Add Charge Level"))
            {
                attack.chargeWindows[i].chargeLevels.Add(CreateChargeLevelInstance());
            }
            for(int w = 0; w < attack.chargeWindows[i].chargeLevels.Count; w++)
            {
                EditorGUI.indentLevel++;
                DrawChargeLevel(i, w);
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
            EditorGUI.indentLevel--;
        }

        protected virtual void DrawChargeLevel(int chargeWindowIndex, int index)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("X", GUILayout.Width(30)))
            {
                attack.chargeWindows[chargeWindowIndex].chargeLevels.RemoveAt(index);
                return;
            }
            GUILayout.Label($"Level {index+1}");
            EditorGUILayout.EndHorizontal();
            attack.chargeWindows[chargeWindowIndex].chargeLevels[index].maxChargeFrames =
                EditorGUILayout.IntField("Max Charge Frames", attack.chargeWindows[chargeWindowIndex].chargeLevels[index].maxChargeFrames);
        }

        protected virtual ChargeLevel CreateChargeLevelInstance()
        {
            return new ChargeLevel();
        }

        protected virtual ChargeDefinition CreateChargeDefinition()
        {
            return new ChargeDefinition();
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

        protected int currentHitboxGroupIndex;
        protected Vector2 scrollPos;
        protected virtual void DrawBoxesMenu()
        {
            EditorGUILayout.BeginHorizontal();
            BoxesMenuNavigationBar();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(30);

            if (attack.boxGroups.Count == 0)
            {
                return;
            }
            BoxGroup currentGroup = attack.boxGroups[currentHitboxGroupIndex];

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            DrawBoxGroup(currentGroup);
            EditorGUILayout.EndScrollView();
        }

        protected virtual void BoxesMenuNavigationBar()
        {
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
                Undo.RecordObject(attack, "Added Box Group");
                AddBoxGroup();
            }
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                Undo.RecordObject(attack, "Removed Box Group");
                attack.boxGroups.RemoveAt(currentHitboxGroupIndex);
                currentHitboxGroupIndex--; 
            }
        }

        protected virtual void AddBoxGroup()
        {
            attack.boxGroups.Add(new BoxGroup());
        }

        protected bool boxesFoldout;
        protected virtual void DrawBoxGroup(BoxGroup currentGroup)
        {
            EditorGUILayout.LabelField("GENERAL", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
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

            currentGroup.chargeLevelNeeded = EditorGUILayout.IntField("Charge Level Needed", currentGroup.chargeLevelNeeded);
            if(currentGroup.chargeLevelNeeded >= 0)
            {
                currentGroup.chargeLevelMax = EditorGUILayout.IntField("Charge Level Max", currentGroup.chargeLevelMax);
            }

            currentGroup.hitGroupType = (BoxGroupType)EditorGUILayout.EnumPopup("Hit Type", currentGroup.hitGroupType);
            currentGroup.attachToEntity = EditorGUILayout.Toggle("Attach to Entity", currentGroup.attachToEntity);

            EditorGUILayout.BeginHorizontal(GUILayout.Width(300));
            boxesFoldout = EditorGUILayout.Foldout(boxesFoldout, "Boxes", true);
            if (GUILayout.Button("Add"))
            {
                GenericMenu menu = new GenericMenu();

                foreach (string t in boxDefinitionTypes.Keys)
                {
                    string destination = t.Replace('.', '/');
                    menu.AddItem(new GUIContent(destination), true, OnBoxDefinitionSelected, t);
                }
                menu.ShowAsContext();
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
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(10);

            if (GUILayout.Button("Set HitInfo Type"))
            {
                GenericMenu menu = new GenericMenu();

                foreach (string t in hitInfoTypes.Keys)
                {
                    string destination = t.Replace('.', '/');
                    menu.AddItem(new GUIContent(destination), true, OnHitInfoSelected, t);
                }

                menu.ShowAsContext();
            }

            if(currentGroup.hitboxHitInfo == null)
            {
                return;
            }

            switch (currentGroup.hitGroupType)
            {
                case BoxGroupType.HIT:
                    currentGroup.hitboxHitInfo.DrawInspectorHitInfo();
                    break;
                case BoxGroupType.GRAB:
                    currentGroup.hitboxHitInfo.DrawInspectorGrabInfo();
                    break;
            }
        }

        protected void OnHitInfoSelected(object t)
        {
            attack.boxGroups[currentHitboxGroupIndex].hitboxHitInfo = (HitInfoBase)Activator.CreateInstance(hitInfoTypes[(string)t]);
        }

        protected void OnBoxDefinitionSelected(object t)
        {
            attack.boxGroups[currentHitboxGroupIndex].boxes.Add((BoxDefinitionBase)Activator.CreateInstance(boxDefinitionTypes[(string)t]));
        }

        protected virtual void DrawHitboxOptions(BoxGroup currentGroup, int index)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                currentGroup.boxes.RemoveAt(index);
                return;
            }
            GUILayout.Label($"Group {index}");
            EditorGUILayout.EndHorizontal();
            currentGroup.boxes[index].DrawInspector();
        }

        #region Events
        protected int eventSelected = -1;
        protected virtual void DrawEventsWindow()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add", GUILayout.Width(100)))
            {
                AddAttackEventDefinition();
            }
            GUILayout.Space(20);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical(GUILayout.MaxWidth(110));
            for (int i = 0; i < attack.events.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("X", GUILayout.Width(15)))
                {
                    attack.events.RemoveAt(i);
                    continue;
                }
                if (GUILayout.Button($"{attack.events[i].nickname}",
                    GUILayout.Height(25), GUILayout.Width(95)))
                {
                    eventSelected = eventSelected == i ? -1 : i;
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            if (eventSelected == -1)
            {
                EditorGUILayout.BeginVertical();
                for (int i = 0; i < attack.events.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(attack.events[i].startFrame.ToString(), GUILayout.Width(20));
                    float eventStart = attack.events[i].startFrame;
                    float eventEnd = attack.events[i].endFrame;
                    EditorGUILayout.MinMaxSlider(ref eventStart, ref eventEnd, 1, attack.length, GUILayout.Height(25));
                    attack.events[i].startFrame = (uint)eventStart;
                    attack.events[i].endFrame = (uint)eventEnd;
                    EditorGUILayout.LabelField(attack.events[i].endFrame.ToString(), GUILayout.Width(20));
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }
            else
            {
                EditorGUILayout.BeginVertical();
                ShowEventInfo(eventSelected);
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        protected virtual void AddAttackEventDefinition()
        {
            attack.events.Add(new AttackEventDefinition());
        }

        protected bool eventVariablesFoldout;
        protected virtual void ShowEventInfo(int eventSelected)
        {
            if (attack.events[eventSelected] == null)
            {
                return;
            }
            attack.events[eventSelected].nickname = EditorGUILayout.TextField("Name", attack.events[eventSelected].nickname);
            attack.events[eventSelected].active = EditorGUILayout.Toggle("Active", attack.events[eventSelected].active);
            attack.events[eventSelected].onHit = EditorGUILayout.Toggle("On Hit?", attack.events[eventSelected].onHit);
            if (attack.events[eventSelected].onHit)
            {
                attack.events[eventSelected].onHitHitboxGroup = EditorGUILayout.IntField("Hitbox Group",
                    attack.events[eventSelected].onHitHitboxGroup);
            }
            attack.events[eventSelected].inputCheckTiming = (AttackEventInputCheckTiming)EditorGUILayout.EnumPopup("Input Requirement", 
                attack.events[eventSelected].inputCheckTiming);
            EditorGUI.indentLevel++;
            if(attack.events[eventSelected].inputCheckTiming != AttackEventInputCheckTiming.NONE)
            {
                SerializedObject serializedObject = new UnityEditor.SerializedObject(attack);
                serializedObject.Update();
                attack.events[eventSelected].inputCheckStartFrame = (uint)EditorGUILayout.IntField("Start Frame", (int)attack.events[eventSelected].inputCheckStartFrame);
                attack.events[eventSelected].inputCheckEndFrame = (uint)EditorGUILayout.IntField("End Frame", (int)attack.events[eventSelected].inputCheckEndFrame);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Input");
                attack.events[eventSelected].input.DrawInspector(
                    serializedObject.FindProperty("events").GetArrayElementAtIndex(eventSelected).FindPropertyRelative("input").FindPropertyRelative("executeInputs"),
                    serializedObject.FindProperty("events").GetArrayElementAtIndex(eventSelected).FindPropertyRelative("input").FindPropertyRelative("sequenceInputs")
                    );
                serializedObject.ApplyModifiedProperties();
            }
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();

            EditorGUILayout.LabelField(attack.events[eventSelected].attackEvent == null ? "..." 
                : attack.events[eventSelected].attackEvent.GetName());
            if (GUILayout.Button("Set Event"))
            {
                GenericMenu menu = new GenericMenu();

                foreach (string t in attackEventTypes.Keys)
                {
                    string destination = t.Replace('.', '/');
                    menu.AddItem(new GUIContent(destination), true, OnAttackEventSelected, t);
                }
                menu.ShowAsContext();
            }
            eventVariablesFoldout = EditorGUILayout.Foldout(eventVariablesFoldout, "Variables", true);
            if (eventVariablesFoldout)
            {
                EditorGUI.indentLevel++;
                if (attack.events[eventSelected].attackEvent != null)
                {
                    attack.events[eventSelected].attackEvent.DrawEventVariables(attack.events[eventSelected]);
                }
                EditorGUI.indentLevel--;
            }
        }

        protected void OnAttackEventSelected(object t)
        {
            attack.events[eventSelected].attackEvent = (AttackEvent)Activator.CreateInstance(attackEventTypes[(string)t]);
        }
        #endregion
    }
}