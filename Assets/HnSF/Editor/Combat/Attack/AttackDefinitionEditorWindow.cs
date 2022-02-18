using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HnSF.Combat
{
    public class AttackDefinitionEditorWindow : EditorWindow
    {
        protected PreviewRenderUtility renderUtils;
        [SerializeField] public AttackDefinition attack;

        protected bool autoplay;

        protected double playInterval;
        protected double nextPlayTime = 0;

        protected bool showHitboxGroups = true;
        protected bool showEvents = true;
        protected bool showCharges = true;
        protected bool showCancels = true;

        public static void Init(AttackDefinition attack)
        {
            AttackDefinitionEditorWindow window = ScriptableObject.CreateInstance<AttackDefinitionEditorWindow>();
            window.attack = attack;
            window.Show();
        }

        protected Dictionary<string, Type> attackEventDefinitionTypes = new Dictionary<string, Type>();
        protected Dictionary<string, Type> hitboxGroupTypes = new Dictionary<string, Type>();
        protected Dictionary<string, Type> chargeGroupTypes = new Dictionary<string, Type>();
        protected Dictionary<string, Type> cancelListDefinitionTypes = new Dictionary<string, Type>();
        protected virtual void OnFocus()
        {
            attackEventDefinitionTypes.Clear();
            hitboxGroupTypes.Clear();
            chargeGroupTypes.Clear();
            cancelListDefinitionTypes.Clear();
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var givenType in a.GetTypes())
                {
                    if (givenType == typeof(AttackEventDefinition) || givenType.IsSubclassOf(typeof(AttackEventDefinition)))
                    {
                        attackEventDefinitionTypes.Add(givenType.FullName, givenType);
                    }
                    if(givenType == typeof(HitboxGroup) || givenType.IsSubclassOf(typeof(HitboxGroup)))
                    {
                        hitboxGroupTypes.Add(givenType.FullName, givenType);
                    }
                    if (givenType == typeof(ChargeDefinition) || givenType.IsSubclassOf(typeof(ChargeDefinition)))
                    {
                        chargeGroupTypes.Add(givenType.FullName, givenType);
                    }
                    if (givenType == typeof(CancelListDefinition) || givenType.IsSubclassOf(typeof(CancelListDefinition)))
                    {
                        cancelListDefinitionTypes.Add(givenType.FullName, givenType);
                    }
                }
            }
        }

        protected virtual void OnEnable()
        {
            playInterval = Time.fixedDeltaTime;
            nextPlayTime = EditorApplication.timeSinceStartup+playInterval;
            if (renderUtils == null)
            {
                renderUtils = new PreviewRenderUtility(true);
            }
            renderUtils.camera.cameraType = CameraType.SceneView;
            renderUtils.camera.transform.position = new Vector3(0, 1, -10);
            renderUtils.camera.transform.LookAt(new Vector3(0, 1, 0));
            renderUtils.camera.farClipPlane = 100;
        }

        protected virtual void OnDisable()
        {
            if (renderUtils != null)
            {
                renderUtils.Cleanup();
            }
        }

        protected virtual void Update()
        {
            Repaint();
        }

        protected Vector2 scroll;
        public int timelineFrame = 0;
        protected virtual void OnGUI()
        {
            if (attack == null)
            {
                Close();
                return;
            }
            var pos = position;
            pos.x = 0;
            pos.y = 0;

            SerializedObject serializedObject = new SerializedObject(attack);
            serializedObject.Update();

            GUILayout.BeginArea(pos);
            scroll = EditorGUILayout.BeginScrollView(scroll);

            DrawGeneralOptions(serializedObject);
            GUILayout.BeginHorizontal();
            MenuBar(serializedObject);
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            DrawPlayControls();
            GUILayout.Space(10);
            if (showHitboxGroups)
            {
                DrawHitboxGroupBars(serializedObject);
            }
            GUILayout.Space(10);
            if (showEvents)
            {
                DrawEventBars(serializedObject);
            }
            GUILayout.Space(10);
            if (showCharges)
            {
                DrawChargeBars(serializedObject);
            }
            GUILayout.Space(10);
            if (showCancels)
            {
                DrawCancelBars(serializedObject);
            }
            EditorGUILayout.EndScrollView();
            GUILayout.EndArea();

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void DrawPlayControls()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(EditorGUIUtility.IconContent("d_Profiler.PrevFrame"), GUILayout.Width(30)))
            {
                timelineFrame = Mathf.Max(0, timelineFrame - 1);
                FastForward();
            }
            if (GUILayout.Button(EditorGUIUtility.IconContent("d_Profiler.NextFrame"), GUILayout.Width(30)))
            {
                IncrementForward();
            }
            if (GUILayout.Button(EditorGUIUtility.IconContent("d_preAudioAutoPlayOff"), GUILayout.Width(30), GUILayout.Height(18)))
            {
                FastForward();
            }
            if (GUILayout.Button(EditorGUIUtility.IconContent("d_Profiler.LastFrame"), GUILayout.Width(30)))
            {
                autoplay = !autoplay;
                if (autoplay)
                {
                    nextPlayTime = EditorApplication.timeSinceStartup + playInterval;
                }
            }
            if (autoplay && EditorApplication.timeSinceStartup >= nextPlayTime)
            {
                if (timelineFrame + 1 > attack.length)
                {
                    timelineFrame = 0;
                    FastForward();
                }
                else
                {
                    IncrementForward();
                }
                nextPlayTime = EditorApplication.timeSinceStartup + playInterval;
            }
            timelineFrame = (int)EditorGUILayout.Slider(timelineFrame, 0, attack.length);
            EditorGUILayout.EndHorizontal();
        }

        protected virtual void FastForward()
        {
            if(timelineFrame == 0)
            {
                return;
            }
            int oldFrame = timelineFrame;
            timelineFrame = 0;

            for (int i = 0; i < oldFrame; i++)
            {
                IncrementForward();
            }
        }

        protected virtual void IncrementForward()
        {
            timelineFrame = Mathf.Min(timelineFrame + 1, attack.length);
        }

        protected virtual void DrawGeneralOptions(SerializedObject serializedObject)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("attackName"), new GUIContent("Name"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("length"));
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("hurtboxDefinition"));
            if (GUILayout.Button("Open"))
            {
                Selection.activeObject = serializedObject.FindProperty("hurtboxDefinition").objectReferenceValue;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("useState"), new GUIContent("State Override"));
            EditorGUI.BeginDisabledGroup(!serializedObject.FindProperty("useState").boolValue);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("stateOverride"), GUIContent.none);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }

        protected virtual void MenuBar(SerializedObject serializedObject)
        {
            showHitboxGroups = GUILayout.Toggle(showHitboxGroups, "Hitbox Grops", "Button");
            showEvents = GUILayout.Toggle(showEvents, "Events", "Button");
            showCharges = GUILayout.Toggle(showCharges, "Charges", "Button");
            showCancels = GUILayout.Toggle(showCancels, "Cancels", "Button");
        }

        #region Timeline Elements
        protected virtual void DrawHitboxGroupBars(SerializedObject serializedObject)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Hitbox Groups", EditorStyles.boldLabel);
            if (GUILayout.Button("+", GUILayout.Width(30), GUILayout.MaxWidth(30)))
            {
                GenericMenu menu = new GenericMenu();

                foreach (string t in hitboxGroupTypes.Keys)
                {
                    string destination = t.Replace('.', '/');
                    menu.AddItem(new GUIContent(destination), true, OnHitboxGroupSelected, t);
                }
                menu.ShowAsContext();
            }
            EditorGUILayout.EndHorizontal();
            DrawUILine(Color.gray);
            var hitboxGroupProperty = serializedObject.FindProperty("hitboxGroups");
            for(int i = 0; i < hitboxGroupProperty.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(25);
                if(GUILayout.Button("-", GUILayout.Width(20)))
                {
                    hitboxGroupProperty.DeleteArrayElementAtIndex(i);
                    return;
                }
                if (GUILayout.Button("↑", GUILayout.Width(20)))
                {
                    if (i != 0)
                    {
                        hitboxGroupProperty.MoveArrayElement(i, i - 1);
                        return;
                    }
                }
                if (GUILayout.Button("↓", GUILayout.Width(20)))
                {
                    if (i != hitboxGroupProperty.arraySize - 1)
                    {
                        hitboxGroupProperty.MoveArrayElement(i, i + 1);
                        return;
                    }
                }
                SerializedProperty arrayElement = hitboxGroupProperty.GetArrayElementAtIndex(i);
                float activeFramesStart = arrayElement.FindPropertyRelative("activeFramesStart").intValue;
                float activeFramesEnd = arrayElement.FindPropertyRelative("activeFramesEnd").intValue;
                EditorGUILayout.LabelField($"{activeFramesStart.ToString("F0")}~{activeFramesEnd.ToString("F0")}", GUILayout.Width(55));
                if(GUILayout.Button("Info", GUILayout.Width(100)))
                {
                    OpenHitboxGroupWindow(attack, attack.hitboxGroups[i], "hitboxGroups", i);
                }
                EditorGUILayout.MinMaxSlider(ref activeFramesStart,
                    ref activeFramesEnd,
                    1,
                    serializedObject.FindProperty("length").intValue);
                arrayElement.FindPropertyRelative("activeFramesStart").intValue = (int)activeFramesStart;
                arrayElement.FindPropertyRelative("activeFramesEnd").intValue = (int)activeFramesEnd;
                EditorGUILayout.EndHorizontal();
            }
        }

        protected virtual void OpenHitboxGroupWindow(AttackDefinition attack, HitboxGroup hitboxGroup, string v, int i)
        {
            HitboxGroupEditorWindow.Init(attack, hitboxGroup, v, i);
        }

        protected virtual void DrawEventBars(SerializedObject serializedObject)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Events", EditorStyles.boldLabel);
            if(GUILayout.Button("+", GUILayout.Width(30), GUILayout.MaxWidth(30)))
            {
                GenericMenu menu = new GenericMenu();

                foreach (string t in attackEventDefinitionTypes.Keys)
                {
                    string destination = t.Replace('.', '/');
                    menu.AddItem(new GUIContent(destination), true, OnAttackConditionSelected, t);
                }
                menu.ShowAsContext();
            }
            EditorGUILayout.EndHorizontal();
            DrawUILine(Color.gray);
            var eventsProperty = serializedObject.FindProperty("events");
            for (int i = 0; i < eventsProperty.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(25);
                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    eventsProperty.DeleteArrayElementAtIndex(i);
                    return;
                }
                if (GUILayout.Button("↑", GUILayout.Width(20)))
                {
                    if (i != 0)
                    {
                        eventsProperty.MoveArrayElement(i, i - 1);
                        return;
                    }
                }
                if (GUILayout.Button("↓", GUILayout.Width(20)))
                {
                    if (i != eventsProperty.arraySize - 1)
                    {
                        eventsProperty.MoveArrayElement(i, i + 1);
                        return;
                    }
                }
                SerializedProperty arrayElement = eventsProperty.GetArrayElementAtIndex(i);
                float activeFramesStart = arrayElement.FindPropertyRelative("startFrame").intValue;
                float activeFramesEnd = arrayElement.FindPropertyRelative("endFrame").intValue;
                EditorGUILayout.LabelField($"{activeFramesStart.ToString("F0")}~{activeFramesEnd.ToString("F0")}", GUILayout.Width(55));
                if (GUILayout.Button(arrayElement.FindPropertyRelative("nickname").stringValue, GUILayout.Width(100)))
                {
                    OpenAttackEventDefinitionWindow(attack, i);
                }
                EditorGUILayout.MinMaxSlider(ref activeFramesStart,
                    ref activeFramesEnd,
                    1,
                    serializedObject.FindProperty("length").intValue);
                arrayElement.FindPropertyRelative("startFrame").intValue = (int)activeFramesStart;
                arrayElement.FindPropertyRelative("endFrame").intValue = (int)activeFramesEnd;
                EditorGUILayout.EndHorizontal();
            }
        }

        protected virtual void OpenAttackEventDefinitionWindow(AttackDefinition attack, int i)
        {
            AttackEventDefinitionEditorWindow.Init(attack, i);
        }

        protected virtual void DrawChargeBars(SerializedObject serializedObject)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Charge Groups", EditorStyles.boldLabel);
            if(GUILayout.Button("+", GUILayout.Width(30), GUILayout.MaxWidth(30)))
            {
                GenericMenu menu = new GenericMenu();

                foreach (string t in chargeGroupTypes.Keys)
                {
                    string destination = t.Replace('.', '/');
                    menu.AddItem(new GUIContent(destination), true, OnChargeGroupSelected, t);
                }
                menu.ShowAsContext();
            }
            EditorGUILayout.EndHorizontal();
            DrawUILine(Color.gray);
            var chargeWindowsProperty = serializedObject.FindProperty("chargeWindows");
            for (int i = 0; i < chargeWindowsProperty.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(25);
                if(GUILayout.Button("-", GUILayout.Width(20)))
                {
                    chargeWindowsProperty.DeleteArrayElementAtIndex(i);
                    return;
                }
                if (GUILayout.Button("↑", GUILayout.Width(20)))
                {
                    if (i != 0)
                    {
                        chargeWindowsProperty.MoveArrayElement(i, i - 1);
                        return;
                    }
                }
                if (GUILayout.Button("↓", GUILayout.Width(20)))
                {
                    if (i != chargeWindowsProperty.arraySize - 1)
                    {
                        chargeWindowsProperty.MoveArrayElement(i, i + 1);
                        return;
                    }
                }
                SerializedProperty arrayElement = chargeWindowsProperty.GetArrayElementAtIndex(i);
                float activeFramesStart = arrayElement.FindPropertyRelative("startFrame").intValue;
                float activeFramesEnd = arrayElement.FindPropertyRelative("endFrame").intValue;
                EditorGUILayout.LabelField($"{activeFramesStart.ToString("F0")}~{activeFramesEnd.ToString("F0")}", GUILayout.Width(55));
                if (GUILayout.Button("Info", GUILayout.Width(100)))
                {
                    OpenChargeGroupEditorWindow(attack, attack.chargeWindows[i]);
                }
                EditorGUILayout.MinMaxSlider(ref activeFramesStart,
                    ref activeFramesEnd,
                    1,
                    serializedObject.FindProperty("length").intValue);
                arrayElement.FindPropertyRelative("startFrame").intValue = (int)activeFramesStart;
                arrayElement.FindPropertyRelative("endFrame").intValue = (int)activeFramesEnd;
                EditorGUILayout.EndHorizontal();
            }
        }

        protected virtual void OpenChargeGroupEditorWindow(AttackDefinition attack, ChargeDefinition chargeDefinition)
        {
            ChargeGroupEditorWindow.Init(attack, chargeDefinition);
        }

        protected virtual void DrawCancelBars(SerializedObject serializedObject)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Cancels", EditorStyles.boldLabel);
            if (GUILayout.Button("+", GUILayout.Width(30), GUILayout.MaxWidth(30)))
            {
                GenericMenu menu = new GenericMenu();

                foreach (string t in cancelListDefinitionTypes.Keys)
                {
                    string destination = t.Replace('.', '/');
                    menu.AddItem(new GUIContent(destination), true, OnCancelListDefinitionSelected, t);
                }
                menu.ShowAsContext();
            }
            EditorGUILayout.EndHorizontal();
            DrawUILine(Color.gray);
            var cancelsProperty = serializedObject.FindProperty("cancels");
            for(int i = 0; i < cancelsProperty.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(25);
                if(GUILayout.Button("-", GUILayout.Width(20)))
                {
                    cancelsProperty.DeleteArrayElementAtIndex(i);
                    return;
                }
                if (GUILayout.Button("↑", GUILayout.Width(20)))
                {
                    if (i != 0)
                    {
                        cancelsProperty.MoveArrayElement(i, i - 1);
                        return;
                    }
                }
                if (GUILayout.Button("↓", GUILayout.Width(20)))
                {
                    if (i != cancelsProperty.arraySize - 1)
                    {
                        cancelsProperty.MoveArrayElement(i, i + 1);
                        return;
                    }
                }
                SerializedProperty arrayElement = cancelsProperty.GetArrayElementAtIndex(i);
                float activeFramesStart = arrayElement.FindPropertyRelative("startFrame").intValue;
                float activeFramesEnd = arrayElement.FindPropertyRelative("endFrame").intValue;
                EditorGUILayout.LabelField($"{activeFramesStart.ToString("F0")}~{activeFramesEnd.ToString("F0")}", GUILayout.Width(55));
                if (GUILayout.Button("Info", GUILayout.Width(100)))
                {
                    OpenCancelListDefinitionEditorWindow(attack, i);
                }
                EditorGUILayout.MinMaxSlider(ref activeFramesStart,
                    ref activeFramesEnd,
                    1,
                    serializedObject.FindProperty("length").intValue);
                arrayElement.FindPropertyRelative("startFrame").intValue = (int)activeFramesStart;
                arrayElement.FindPropertyRelative("endFrame").intValue = (int)activeFramesEnd;
                EditorGUILayout.EndHorizontal();
            }
        }

        protected virtual void OpenCancelListDefinitionEditorWindow(AttackDefinition attack, int i)
        {
            CancelListDefinitionEditorWindow.Init(attack, i);
        }
        #endregion

        protected virtual void OnHitboxGroupSelected(object t)
        {
            SerializedObject attackObject = new SerializedObject(attack);
            attackObject.Update();
            SerializedProperty hitboxGroupsProperty = attackObject.FindProperty("hitboxGroups");
            hitboxGroupsProperty.InsertArrayElementAtIndex(hitboxGroupsProperty.arraySize);
            hitboxGroupsProperty.GetArrayElementAtIndex(hitboxGroupsProperty.arraySize - 1).managedReferenceValue = Activator.CreateInstance(hitboxGroupTypes[(string)t]);
            attackObject.ApplyModifiedProperties();
        }

        protected virtual void OnAttackConditionSelected(object t)
        {
            SerializedObject attackObject = new SerializedObject(attack);
            attackObject.Update();
            SerializedProperty eventProperty = attackObject.FindProperty("events");
            eventProperty.InsertArrayElementAtIndex(eventProperty.arraySize);
            eventProperty.GetArrayElementAtIndex(eventProperty.arraySize - 1).managedReferenceValue = Activator.CreateInstance(attackEventDefinitionTypes[(string)t]);
            attackObject.ApplyModifiedProperties();
        }

        protected virtual void OnChargeGroupSelected(object t)
        {
            SerializedObject attackObject = new SerializedObject(attack);
            attackObject.Update();
            SerializedProperty eventProperty = attackObject.FindProperty("chargeWindows");
            eventProperty.InsertArrayElementAtIndex(eventProperty.arraySize);
            eventProperty.GetArrayElementAtIndex(eventProperty.arraySize - 1).managedReferenceValue = Activator.CreateInstance(chargeGroupTypes[(string)t]);
            attackObject.ApplyModifiedProperties();
        }

        protected virtual void OnCancelListDefinitionSelected(object t)
        {
            SerializedObject attackObject = new SerializedObject(attack);
            attackObject.Update();
            SerializedProperty eventProperty = attackObject.FindProperty("cancels");
            eventProperty.InsertArrayElementAtIndex(eventProperty.arraySize);
            eventProperty.GetArrayElementAtIndex(eventProperty.arraySize - 1).managedReferenceValue = Activator.CreateInstance(cancelListDefinitionTypes[(string)t]);
            attackObject.ApplyModifiedProperties();
        }

        public void DrawUILine(Color color, int thickness = 2, int padding = 10)
        {
            return;
            /*
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2;
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);*/
        }
    }
}
