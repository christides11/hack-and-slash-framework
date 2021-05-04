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

            GUILayout.BeginArea(new Rect(pos.x, pos.y + pos.height, pos.width, position.height - pos.height));
            DrawGeneralOptions();
            if (attack.useState)
            {
                GUILayout.EndArea();
                return;
            }
            MenuBar();
            GUILayout.Space(10);
            if (visualFighterSceneReference)
            {
                if (GUILayout.Button("Open Fighter Inspector"))
                {
                    Selection.activeGameObject = visualFighterSceneReference.gameObject;
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
                DrawHitboxGroupBars();
            }
            GUILayout.Space(10);
            if (showHurtboxGroups)
            {
                DrawHurtboxGroupBars();
            }
            GUILayout.Space(10);
            if (showEvents)
            {
                DrawEventBars();
            }
            GUILayout.Space(10);
            if (showCharges)
            {
                DrawChargeBars();
            }
            EditorGUILayout.EndScrollView();
            GUILayout.EndArea();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(attack);
            }
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

        int tempLength = 0;
        Fighters.FighterBase tempFighter;
        protected virtual void DrawGeneralOptions()
        {
            attack.name = EditorGUILayout.TextField("Name", attack.name);
            EditorGUILayout.BeginHorizontal();
            tempLength = Mathf.Clamp(EditorGUILayout.IntField("Length", tempLength), 1, int.MaxValue);
            if (GUILayout.Button("Apply"))
            {
                attack.length = tempLength;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            tempFighter = (Fighters.FighterBase)EditorGUILayout.ObjectField("Character", tempFighter, typeof(Fighters.FighterBase), false);
            if (GUILayout.Button("Apply"))
            {
                CreateFighter();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("State Override", GUILayout.Width(150));
            attack.useState = EditorGUILayout.Toggle(attack.useState, GUILayout.Width(20));
            EditorGUI.BeginDisabledGroup(!attack.useState);
            attack.stateOverride = (ushort)EditorGUILayout.IntField(attack.stateOverride);
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
        protected virtual void MenuBar()
        {
            GUILayout.BeginHorizontal();
            showHitboxGroups = GUILayout.Toggle(showHitboxGroups, "Hitbox Grops", "Button");
            showHurtboxGroups = GUILayout.Toggle(showHurtboxGroups, "Hurtbox Groups", "Button");
            showEvents = GUILayout.Toggle(showEvents, "Events", "Button");
            showCharges = GUILayout.Toggle(showCharges, "Charges", "Button");
            GUILayout.EndHorizontal();
        }

        #region Timeline Elements
        protected virtual void DrawHitboxGroupBars()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Hitbox Groups", EditorStyles.boldLabel);
            if(GUILayout.Button("+", GUILayout.Width(30), GUILayout.MaxWidth(30)))
            {
                AddHitboxGroup();
            }
            EditorGUILayout.EndHorizontal();
            DrawUILine(Color.gray);
            for(int i = 0; i < attack.hitboxGroups.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(25);
                if(GUILayout.Button("-", GUILayout.Width(20)))
                {
                    attack.hitboxGroups.RemoveAt(i);
                    return;
                }
                float activeFrameStart = attack.hitboxGroups[i].activeFramesStart;
                float activeFrameEnd = attack.hitboxGroups[i].activeFramesEnd;
                EditorGUILayout.LabelField($"{activeFrameStart.ToString("F0")}~{activeFrameEnd.ToString("F0")}", GUILayout.Width(55));
                if (GUILayout.Button("Info", GUILayout.Width(100)))
                {
                    HitboxGroupEditorWindow.Init(attack.hitboxGroups[i]);
                }
                EditorGUILayout.MinMaxSlider(ref activeFrameStart,
                    ref activeFrameEnd,
                    1,
                    attack.length);
                attack.hitboxGroups[i].activeFramesStart = (int)activeFrameStart;
                attack.hitboxGroups[i].activeFramesEnd = (int)activeFrameEnd;
                EditorGUILayout.EndHorizontal();
            }
        }

        protected virtual void DrawHurtboxGroupBars()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Hurtbox Groups", EditorStyles.boldLabel);
            if (GUILayout.Button("+", GUILayout.Width(30), GUILayout.MaxWidth(30)))
            {
                AddHurtboxGroup();
            }
            EditorGUILayout.EndHorizontal();
            DrawUILine(Color.gray);
            for (int i = 0; i < attack.hurtboxGroups.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(25);
                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    attack.hurtboxGroups.RemoveAt(i);
                    return;
                }
                float activeFrameStart = attack.hurtboxGroups[i].activeFramesStart;
                float activeFrameEnd = attack.hurtboxGroups[i].activeFramesEnd;
                EditorGUILayout.LabelField($"{activeFrameStart.ToString("F0")}~{activeFrameEnd.ToString("F0")}", GUILayout.Width(55));
                if (GUILayout.Button("Info", GUILayout.Width(100)))
                {
                    HurtboxGroupEditorWindow.Init(attack.hurtboxGroups[i]);
                }
                EditorGUILayout.MinMaxSlider(ref activeFrameStart,
                    ref activeFrameEnd,
                    1,
                    attack.length);
                attack.hurtboxGroups[i].activeFramesStart = (int)activeFrameStart;
                attack.hurtboxGroups[i].activeFramesEnd = (int)activeFrameEnd;
                EditorGUILayout.EndHorizontal();
            }
        }

        protected virtual void DrawEventBars()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Events", EditorStyles.boldLabel);
            if (GUILayout.Button("+", GUILayout.Width(30), GUILayout.MaxWidth(30)))
            {
                attack.events.Add(new AttackEventDefinition());
            }
            EditorGUILayout.EndHorizontal();
            DrawUILine(Color.gray);
            for (int i = 0; i < attack.events.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(25);
                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    attack.events.RemoveAt(i);
                    return;
                }
                float activeFrameStart = attack.events[i].startFrame;
                float activeFrameEnd = attack.events[i].endFrame;
                EditorGUILayout.LabelField($"{activeFrameStart.ToString("F0")}~{activeFrameEnd.ToString("F0")}", GUILayout.Width(55));
                if (GUILayout.Button(attack.events[i].nickname, GUILayout.Width(100)))
                {
                    EventEditorWindow.Init(attack, attack.events[i]);
                }
                EditorGUILayout.MinMaxSlider(ref activeFrameStart,
                    ref activeFrameEnd,
                    1,
                    attack.length);
                attack.events[i].startFrame = (int)activeFrameStart;
                attack.events[i].endFrame = (int)activeFrameEnd;
                EditorGUILayout.EndHorizontal();
            }
        }

        protected virtual void DrawChargeBars()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Charge Groups", EditorStyles.boldLabel);
            if (GUILayout.Button("+", GUILayout.Width(30), GUILayout.MaxWidth(30)))
            {
                attack.chargeWindows.Add(new ChargeDefinition());
            }
            EditorGUILayout.EndHorizontal();
            DrawUILine(Color.gray);
            for(int i = 0; i < attack.chargeWindows.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(25);
                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    attack.chargeWindows.RemoveAt(i);
                    return;
                }
                float frameStart = attack.chargeWindows[i].startFrame;
                float frameEnd = attack.chargeWindows[i].endFrame;
                EditorGUILayout.LabelField($"{frameStart.ToString("F0")}~{frameEnd.ToString("F0")}", GUILayout.Width(55));
                if (GUILayout.Button("Info", GUILayout.Width(100)))
                {
                    ChargeGroupEditorWindow.Init(attack, attack.chargeWindows[i]);
                }
                EditorGUILayout.MinMaxSlider(ref frameStart,
                    ref frameEnd,
                    1,
                    attack.length);
                attack.chargeWindows[i].startFrame = (int)frameStart;
                attack.chargeWindows[i].endFrame = (int)frameEnd;
                EditorGUILayout.EndHorizontal();
            }
        }
        #endregion

        protected virtual void AddHitboxGroup()
        {
            attack.hitboxGroups.Add(new HitboxGroup());
        }

        protected virtual void AddHurtboxGroup()
        {
            attack.hurtboxGroups.Add(new HurtboxGroup());
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
