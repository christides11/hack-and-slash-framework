using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace CAF.Combat
{
    public class AttackDefinitionEditorWindow : EditorWindow
    {
        PreviewRenderUtility renderUtils;
        protected AttackDefinition attack;
        protected Fighters.FighterBase visualFighterPrefab;

        protected GameObject visualFighterSceneReference;

        bool autoplay;

        double playInterval;
        double nextPlayTime = 0;

        public static void Init(AttackDefinition attack)
        {
            AttackDefinitionEditorWindow window = ScriptableObject.CreateInstance<AttackDefinitionEditorWindow>();
            window.attack = attack;
            window.Show();
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
        protected int timelineFrame = 0;
        protected bool rotationMode = false;
        protected bool moveMode = false;
        protected Vector2 mousePos = new Vector2(0, 0);
        protected Vector2 diff = Vector2.zero;
        protected float rotSpeed = 1;
        protected float scrollWheel = 0;
        protected float scrollSpeed = 0.5f;
        protected float moveSpeed = 0.1f;

        protected virtual void OnGUI()
        {
            if(attack == null)
            {
                Close();
                return;
            }
            var pos = position;
            pos.x = 0;
            pos.y = 0;
            pos.height /= 2.75f;
            pos.height = Mathf.Min(250, pos.height);

            Event e = Event.current;
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (pos.Contains(Event.current.mousePosition))
                    {
                        if (Event.current.button == 0)
                        {
                            mousePos = Event.current.mousePosition;
                            moveMode = true;
                        } else if (Event.current.button == 1)
                        {
                            mousePos = Event.current.mousePosition;
                            rotationMode = true;
                        }
                    }
                    break;
                case EventType.MouseUp:
                    if(Event.current.button == 0)
                    {
                        moveMode = false;
                    }
                    if (Event.current.button == 1)
                    {
                        rotationMode = false;
                    }
                    break;
                case EventType.MouseDrag:
                    if (rotationMode || moveMode)
                    {
                        diff = Event.current.mousePosition - mousePos;
                        mousePos = Event.current.mousePosition;
                    }
                    break;
                case EventType.ScrollWheel:
                    if (rotationMode)
                    {
                        scrollWheel = Event.current.delta.y;
                    }
                    break;
            }

            renderUtils.BeginPreview(pos, EditorStyles.helpBox);
            DrawGround();
            DrawHurtboxes();
            DrawHitboxes();
            renderUtils.Render(false, false);
            renderUtils.EndAndDrawPreview(pos);

            if (scrollWheel != 0)
            {
                renderUtils.camera.transform.position += renderUtils.camera.transform.forward * scrollWheel * scrollSpeed;
                scrollWheel = 0;
            }

            if (diff.magnitude > 0)
            {
                if (moveMode)
                {
                    renderUtils.camera.transform.position += new Vector3(0, diff.y * moveSpeed * Time.deltaTime, 0);
                }
                if (rotationMode)
                {
                    renderUtils.camera.transform.RotateAround(new Vector3(0, renderUtils.camera.transform.position.y, 0), Vector3.up, diff.x * rotSpeed);
                }
                diff = Vector2.zero;
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("fr:", GUILayout.Width(15));
            GUILayout.Label(timelineFrame.ToString(), GUILayout.Width(20));
            GUILayout.Label("/", GUILayout.Width(10));
            GUILayout.Label(attack.length.ToString(), GUILayout.Width(55));
            EditorGUILayout.EndHorizontal();
            if (visualFighterSceneReference)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("x:", GUILayout.Width(13));
                GUILayout.Label(visualFighterSceneReference.transform.position.x.ToString("F1"), GUILayout.Width(25));
                GUILayout.Label("y:", GUILayout.Width(13));
                GUILayout.Label(visualFighterSceneReference.transform.position.y.ToString("F1"), GUILayout.Width(25));
                GUILayout.Label("z:", GUILayout.Width(13));
                GUILayout.Label(visualFighterSceneReference.transform.position.z.ToString("F1"), GUILayout.Width(25));
                EditorGUILayout.EndHorizontal();
            }

            if (attack == null)
            {
                return;
            }

            SerializedObject serializedObject = new SerializedObject(attack);
            serializedObject.Update();
            GUILayout.BeginArea(new Rect(pos.x, pos.y + pos.height, pos.width, position.height - pos.height));

            DrawGeneralOptions(serializedObject);
            if (attack.useState)
            {
                GUILayout.EndArea();
                serializedObject.ApplyModifiedProperties();
                return;
            }
            MenuBar(serializedObject);
            GUILayout.Space(10);
            if (visualFighterSceneReference)
            {
                if(GUILayout.Button("Open Fighter Inspector"))
                {
                    Selection.activeObject = visualFighterSceneReference.gameObject;
                }
            }
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
            if (autoplay && visualFighterSceneReference != null && EditorApplication.timeSinceStartup >= nextPlayTime)
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
            GUILayout.Space(10);
            scroll = EditorGUILayout.BeginScrollView(scroll);
            if (showHitboxGroups)
            {
                DrawHitboxGroupBars(serializedObject);
            }
            GUILayout.Space(10);
            if (showHurtboxGroups)
            {
                DrawHurtboxGroupBars(serializedObject);
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
            EditorGUILayout.EndScrollView();
            GUILayout.EndArea();
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void DrawHitboxes()
        {

        }

        protected virtual void DrawHurtboxes()
        {

        }

        protected virtual void DrawGround()
        {
            Handles.SetCamera(renderUtils.camera);
            Handles.color = Color.grey;
            for (int i = -10; i <= 10; i++)
            {
                Handles.color = i == 0 ? Color.red : Color.grey;
                Handles.DrawLine(new Vector3(i, 0, -10), new Vector3(i, 0, 10));
                Handles.color = i == 0 ? Color.green : Color.grey;
                Handles.DrawLine(new Vector3(-10, 0, i), new Vector3(10, 0, i));
            }
            Handles.color = Color.cyan;
            Handles.DrawLine(Vector3.zero, new Vector3(0, 10, 0));
        }

        protected virtual void FastForward()
        {
            if (visualFighterSceneReference == null)
            {
                return;
            }

            ResetFighterVariables();

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

            if (visualFighterSceneReference == null)
            {
                return;
            }
            for(int i = 0; i < attack.hitboxGroups.Count; i++)
            {
                HandleHitboxGroup(i, attack.hitboxGroups[i]);
            }
            for(int i = 0; i < attack.hurtboxGroups.Count; i++)
            {
                HandleHurtboxGroup(i, attack.hurtboxGroups[i]);
            }
            for (int i = 0; i < attack.events.Count; i++)
            {
                HandleEvent(attack.events[i]);
            }

            MoveEntity();
        }

        protected virtual void HandleHurtboxGroup(int index, HurtboxGroup hurtboxGroup)
        {

        }

        protected virtual void HandleHitboxGroup(int index, HitboxGroup hitboxGroup)
        {

        }

        protected virtual AttackEventReturnType HandleEvent(AttackEventDefinition attackEventDefinition)
        {
            if (!attackEventDefinition.active)
            {
                return AttackEventReturnType.NONE;
            }
            Fighters.FighterBase e = visualFighterSceneReference.GetComponent<Fighters.FighterBase>();

            if (timelineFrame >= attackEventDefinition.startFrame
                && timelineFrame <= attackEventDefinition.endFrame)
            {
                return attackEventDefinition.attackEvent.Evaluate((int)(timelineFrame - attackEventDefinition.startFrame),
                    attackEventDefinition.endFrame - attackEventDefinition.startFrame,
                    e,
                    attackEventDefinition.variables);
            }
            return AttackEventReturnType.NONE;
        }

        protected virtual void MoveEntity()
        {
            Vector2 mov = visualFighterSceneReference.GetComponent<Fighters.FighterPhysicsManager2D>().GetOverallForce();
            Vector3 finalMove = new Vector3(mov.x, mov.y, 0);
            finalMove *= Time.fixedDeltaTime;

            visualFighterSceneReference.transform.position += finalMove;
        }

        Fighters.FighterBase tempFighter;
        protected virtual void DrawGeneralOptions(SerializedObject serializedObject)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("attackName"), new GUIContent("Name"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("length"));
            EditorGUILayout.BeginHorizontal();
            tempFighter = (Fighters.FighterBase)EditorGUILayout.ObjectField("Character", tempFighter, typeof(Fighters.FighterBase), false);
            if (GUILayout.Button("Apply"))
            {
                CreateFighter();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("useState"), new GUIContent("State Override"));
            EditorGUI.BeginDisabledGroup(!serializedObject.FindProperty("useState").boolValue);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("stateOverride"), GUIContent.none);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }

        protected virtual void CreateFighter()
        {
            if(visualFighterSceneReference != null)
            {
                DestroyImmediate(visualFighterSceneReference);
                visualFighterSceneReference = null;
            }
            visualFighterPrefab = tempFighter;
            visualFighterSceneReference = renderUtils.InstantiatePrefabInScene(visualFighterPrefab.gameObject);
            ResetFighterVariables();
        }

        protected virtual void ResetFighterVariables()
        {
            visualFighterSceneReference.transform.position = new Vector3(0, 0, 0);
            visualFighterSceneReference.GetComponent<Fighters.FighterPhysicsManager2D>().forceMovement = Vector2.zero;
        }

        protected bool showHitboxGroups = true;
        protected bool showHurtboxGroups = true;
        protected bool showEvents = true;
        protected bool showCharges = true;
        protected virtual void MenuBar(SerializedObject serializedObject)
        {
            GUILayout.BeginHorizontal();
            showHitboxGroups = GUILayout.Toggle(showHitboxGroups, "Hitbox Grops", "Button");
            showHurtboxGroups = GUILayout.Toggle(showHurtboxGroups, "Hurtbox Groups", "Button");
            showEvents = GUILayout.Toggle(showEvents, "Events", "Button");
            showCharges = GUILayout.Toggle(showCharges, "Charges", "Button");
            GUILayout.EndHorizontal();
        }

        #region Timeline Elements
        protected virtual void DrawHitboxGroupBars(SerializedObject serializedObject)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Hitbox Groups", EditorStyles.boldLabel);
            if (GUILayout.Button("+", GUILayout.Width(30), GUILayout.MaxWidth(30)))
            {
                AddHitboxGroup(serializedObject);
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
                SerializedProperty arrayElement = hitboxGroupProperty.GetArrayElementAtIndex(i);
                float activeFramesStart = arrayElement.FindPropertyRelative("activeFramesStart").intValue;
                float activeFramesEnd = arrayElement.FindPropertyRelative("activeFramesEnd").intValue;
                EditorGUILayout.LabelField($"{activeFramesStart.ToString("F0")}~{activeFramesEnd.ToString("F0")}", GUILayout.Width(55));
                if(GUILayout.Button("Info", GUILayout.Width(100)))
                {
                    HitboxGroupEditorWindow.Init(attack.hitboxGroups[i]);
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

        protected virtual void DrawHurtboxGroupBars(SerializedObject serializedObject)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Hurtbox Groups", EditorStyles.boldLabel);
            if (GUILayout.Button("+", GUILayout.Width(30), GUILayout.MaxWidth(30)))
            {
                AddHurtboxGroup(serializedObject);
            }
            EditorGUILayout.EndHorizontal();
            DrawUILine(Color.gray);
            var hurtboxGroupProperty = serializedObject.FindProperty("hurtboxGroups");
            for (int i = 0; i < hurtboxGroupProperty.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(25);
                if(GUILayout.Button("-", GUILayout.Width(20)))
                {
                    hurtboxGroupProperty.DeleteArrayElementAtIndex(i);
                    return;
                }
                SerializedProperty arrayElement = hurtboxGroupProperty.GetArrayElementAtIndex(i);
                float activeFramesStart = arrayElement.FindPropertyRelative("activeFramesStart").intValue;
                float activeFramesEnd = arrayElement.FindPropertyRelative("activeFramesEnd").intValue;
                EditorGUILayout.LabelField($"{activeFramesStart.ToString("F0")}~{activeFramesEnd.ToString("F0")}", GUILayout.Width(55));
                if (GUILayout.Button("Info", GUILayout.Width(100)))
                {
                    HurtboxGroupEditorWindow.Init(attack.hurtboxGroups[i]);
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

        protected virtual void DrawEventBars(SerializedObject serializedObject)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Events", EditorStyles.boldLabel);
            if(GUILayout.Button("+", GUILayout.Width(30), GUILayout.MaxWidth(30)))
            {
                AddEventDefinition(serializedObject);
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
                SerializedProperty arrayElement = eventsProperty.GetArrayElementAtIndex(i);
                float activeFramesStart = arrayElement.FindPropertyRelative("startFrame").intValue;
                float activeFramesEnd = arrayElement.FindPropertyRelative("endFrame").intValue;
                EditorGUILayout.LabelField($"{activeFramesStart.ToString("F0")}~{activeFramesEnd.ToString("F0")}", GUILayout.Width(55));
                if (GUILayout.Button(arrayElement.FindPropertyRelative("nickname").stringValue, GUILayout.Width(100)))
                {
                    AttackEventDefinitionEditorWindow.Init(attack, i);
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

        protected virtual void DrawChargeBars(SerializedObject serializedObject)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Charge Groups", EditorStyles.boldLabel);
            if(GUILayout.Button("+", GUILayout.Width(30), GUILayout.MaxWidth(30)))
            {
                AddChargeGroup(serializedObject);
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
                SerializedProperty arrayElement = chargeWindowsProperty.GetArrayElementAtIndex(i);
                float activeFramesStart = arrayElement.FindPropertyRelative("startFrame").intValue;
                float activeFramesEnd = arrayElement.FindPropertyRelative("endFrame").intValue;
                EditorGUILayout.LabelField($"{activeFramesStart.ToString("F0")}~{activeFramesEnd.ToString("F0")}", GUILayout.Width(55));
                if (GUILayout.Button("Info", GUILayout.Width(100)))
                {
                    AttackEventDefinitionEditorWindow.Init(attack, i);
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
        #endregion

        protected virtual void AddHitboxGroup(SerializedObject serializedObject)
        {
            serializedObject.FindProperty("hitboxGroups").InsertArrayElementAtIndex(serializedObject.FindProperty("hitboxGroups").arraySize);
            serializedObject.FindProperty("hitboxGroups").GetArrayElementAtIndex(serializedObject.FindProperty("hitboxGroups").arraySize - 1)
                .managedReferenceValue = new HitboxGroup();
        }

        protected virtual void AddHurtboxGroup(SerializedObject serializedObject)
        {
            SerializedProperty hurtboxGroupsProperty = serializedObject.FindProperty("hurtboxGroups");
            hurtboxGroupsProperty.InsertArrayElementAtIndex(hurtboxGroupsProperty.arraySize);
            hurtboxGroupsProperty.GetArrayElementAtIndex(hurtboxGroupsProperty.arraySize - 1)
                .managedReferenceValue = new HurtboxGroup();
        }

        protected virtual void AddEventDefinition(SerializedObject serializedObject)
        {
            SerializedProperty eventProperty = serializedObject.FindProperty("events");
            eventProperty.InsertArrayElementAtIndex(eventProperty.arraySize);
            eventProperty.GetArrayElementAtIndex(eventProperty.arraySize - 1).managedReferenceValue = new AttackEventDefinition();
        }

        protected virtual void AddChargeGroup(SerializedObject serializedObject)
        {
            SerializedProperty chargeWindowsProperty = serializedObject.FindProperty("chargeWindows");
            chargeWindowsProperty.InsertArrayElementAtIndex(chargeWindowsProperty.arraySize);
            chargeWindowsProperty.GetArrayElementAtIndex(chargeWindowsProperty.arraySize - 1).managedReferenceValue = new AttackEventDefinition();
        }

        public static void DrawUILine(Color color, int thickness = 2, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2;
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);
        }
    }
}
