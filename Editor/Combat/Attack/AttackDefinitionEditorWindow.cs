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
                (AttackDefinitionEditorWindow)EditorWindow.GetWindow(typeof(AttackDefinitionEditorWindow));
            window.attack = attack;
            window.Show();
        }

        protected Dictionary<string, Type> attackEventTypes = new Dictionary<string, Type>();
        protected virtual void OnFocus()
        {
            attackEventTypes.Clear();
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var givenType in a.GetTypes())
                {
                    if (givenType.IsSubclassOf(typeof(AttackEvent)))
                    {
                        attackEventTypes.Add(givenType.FullName, givenType);
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

        protected bool jumpCancelWindowsFoldout;
        protected bool enemyStepWindowsFoldout;
        protected bool landCancelWindowsFoldout;
        protected bool commandAttackCancelWindowsFoldout;
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
                AddBoxGroup();
            }
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
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

            currentGroup.styleGain = EditorGUILayout.FloatField("Style Gain", currentGroup.styleGain);
            currentGroup.hitGroupType = (BoxGroupType)EditorGUILayout.EnumPopup("Hit Type", currentGroup.hitGroupType);
            currentGroup.attachToEntity = EditorGUILayout.Toggle("Attach to Entity", currentGroup.attachToEntity);

            EditorGUILayout.BeginHorizontal(GUILayout.Width(300));
            boxesFoldout = EditorGUILayout.Foldout(boxesFoldout, "Boxes", true);
            if (GUILayout.Button("Add"))
            {
                BoxGroupAddBoxDefinition(currentGroup);
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
            EditorGUILayout.Space(10);

            switch (currentGroup.hitGroupType)
            {
                case BoxGroupType.HIT:
                    DrawBoxGroupHitOptions(currentGroup);
                    break;
                case BoxGroupType.GRAB:
                    DrawBoxGroupGrabOptions(currentGroup);
                    break;
            }
        }

        protected virtual void BoxGroupAddBoxDefinition(BoxGroup currentGroup)
        {
            currentGroup.boxes.Add(new BoxDefinition());
        }

        #region Box Group: Hit
        protected virtual void DrawBoxGroupHitOptions(BoxGroup currentGroup)
        {
            EditorGUILayout.LabelField("EFFECT", EditorStyles.boldLabel);
            DrawHitEffectsOptions(currentGroup);
            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("DAMAGE", EditorStyles.boldLabel);
            DrawHitDamageOptions(currentGroup);
            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("FORCES", EditorStyles.boldLabel);
            DrawHitForcesOptions(currentGroup);
            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("STUN", EditorStyles.boldLabel);
            DrawHitStunOptions(currentGroup);
            EditorGUILayout.Space(10);
        }

        protected virtual void DrawHitEffectsOptions(BoxGroup currentGroup)
        {
            currentGroup.hitboxHitInfo.groundOnly = EditorGUILayout.Toggle("Hit Ground Only?", currentGroup.hitboxHitInfo.groundOnly);
            currentGroup.hitboxHitInfo.airOnly = EditorGUILayout.Toggle("Hit Air Only?", currentGroup.hitboxHitInfo.airOnly);
            currentGroup.hitboxHitInfo.unblockable = EditorGUILayout.Toggle("Unblockable?", currentGroup.hitboxHitInfo.unblockable);
            currentGroup.hitboxHitInfo.breakArmor = EditorGUILayout.Toggle("Breaks Armor?", currentGroup.hitboxHitInfo.breakArmor);
            currentGroup.hitboxHitInfo.groundBounces = EditorGUILayout.Toggle("Ground Bounces?", currentGroup.hitboxHitInfo.groundBounces);
            currentGroup.hitboxHitInfo.wallBounces = EditorGUILayout.Toggle("Wall Bounces?", currentGroup.hitboxHitInfo.wallBounces);
            currentGroup.hitboxHitInfo.causesTumble = EditorGUILayout.Toggle("Causes Tumble?", currentGroup.hitboxHitInfo.causesTumble);
            currentGroup.hitboxHitInfo.knockdown = EditorGUILayout.Toggle("Causes Knockdown?", currentGroup.hitboxHitInfo.knockdown);
            currentGroup.hitboxHitInfo.continuousHit = EditorGUILayout.Toggle("Continuous Hit?", currentGroup.hitboxHitInfo.continuousHit);
            if (currentGroup.hitboxHitInfo.continuousHit)
            {
                currentGroup.hitboxHitInfo.spaceBetweenHits = EditorGUILayout.IntField("Space between hits", currentGroup.hitboxHitInfo.spaceBetweenHits);
            }
        }

        protected virtual void DrawHitDamageOptions(BoxGroup currentGroup)
        {
            currentGroup.hitboxHitInfo.damageOnBlock = EditorGUILayout.FloatField("Damage (Block)", currentGroup.hitboxHitInfo.damageOnBlock);
            currentGroup.hitboxHitInfo.damageOnHit = EditorGUILayout.FloatField("Damage (Hit)", currentGroup.hitboxHitInfo.damageOnHit);
            currentGroup.hitboxHitInfo.hitKills = EditorGUILayout.Toggle("Hit Kills", currentGroup.hitboxHitInfo.hitKills);
        }

        protected virtual void DrawHitForcesOptions(BoxGroup currentGroup)
        {
            currentGroup.hitboxHitInfo.opponentResetXForce = EditorGUILayout.Toggle("Reset X Force", currentGroup.hitboxHitInfo.opponentResetXForce);
            currentGroup.hitboxHitInfo.opponentResetYForce = EditorGUILayout.Toggle("Reset Y Force", currentGroup.hitboxHitInfo.opponentResetYForce);
            currentGroup.hitboxHitInfo.forceRelation = (HitboxForceRelation)EditorGUILayout.EnumPopup("Force Relation", currentGroup.hitboxHitInfo.forceRelation);
            currentGroup.hitboxHitInfo.forceType = (HitboxForceType)EditorGUILayout.EnumPopup("Force Type", currentGroup.hitboxHitInfo.forceType);
            switch (currentGroup.hitboxHitInfo.forceType)
            {
                case HitboxForceType.SET:
                    currentGroup.hitboxHitInfo.opponentForceDir = EditorGUILayout.Vector3Field("Force Direction", currentGroup.hitboxHitInfo.opponentForceDir);
                    if (GUILayout.Button("Normalize"))
                    {
                        currentGroup.hitboxHitInfo.opponentForceDir.Normalize();
                    }
                    currentGroup.hitboxHitInfo.opponentForceMagnitude = EditorGUILayout.FloatField("Force Magnitude", currentGroup.hitboxHitInfo.opponentForceMagnitude);
                    break;
                case HitboxForceType.PUSH:
                    currentGroup.hitboxHitInfo.forceIncludeYForce = EditorGUILayout.Toggle("Include Y Force", currentGroup.hitboxHitInfo.forceIncludeYForce);
                    currentGroup.hitboxHitInfo.opponentForceMagnitude
                        = EditorGUILayout.FloatField("Force Multiplier", currentGroup.hitboxHitInfo.opponentForceMagnitude);
                    break;
                case HitboxForceType.PULL:
                    currentGroup.hitboxHitInfo.forceIncludeYForce = EditorGUILayout.Toggle("Include Y Force", currentGroup.hitboxHitInfo.forceIncludeYForce);
                    currentGroup.hitboxHitInfo.opponentForceMagnitude
                        = EditorGUILayout.FloatField("Force Multiplier", currentGroup.hitboxHitInfo.opponentForceMagnitude);
                    currentGroup.hitboxHitInfo.opponentMaxMagnitude
                        = EditorGUILayout.FloatField("Max Magnitude", currentGroup.hitboxHitInfo.opponentMaxMagnitude);
                    break;
            }

            if (currentGroup.hitboxHitInfo.wallBounces)
            {
                currentGroup.hitboxHitInfo.wallBounceForce = EditorGUILayout.FloatField("Wall Bounce Magnitude", currentGroup.hitboxHitInfo.wallBounceForce);
            }
        }

        protected virtual void DrawHitStunOptions(BoxGroup currentGroup)
        {
            currentGroup.hitboxHitInfo.attackerHitstop = (ushort)EditorGUILayout.IntField("Hitstop (Attacker)",
                currentGroup.hitboxHitInfo.attackerHitstop);
            currentGroup.hitboxHitInfo.hitstop = (ushort)EditorGUILayout.IntField("Hitstop", currentGroup.hitboxHitInfo.hitstop);
            currentGroup.hitboxHitInfo.hitstun = (ushort)EditorGUILayout.IntField("Hitstun", currentGroup.hitboxHitInfo.hitstun);
        }
        #endregion

        #region Box Group: Grab
        protected virtual void DrawBoxGroupGrabOptions(BoxGroup currentGroup)
        {
            currentGroup.throwConfirm = (AttackDefinition)EditorGUILayout.ObjectField("Throw Confirm Attack", 
                currentGroup.throwConfirm,
                typeof(AttackDefinition), false);
        }
        #endregion

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
            currentGroup.boxes[index].shape = (BoxShapes)EditorGUILayout.EnumPopup("Shape", currentGroup.boxes[index].shape);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Offset", GUILayout.Width(135));
            currentGroup.boxes[index].offset.x = EditorGUILayout.FloatField(currentGroup.boxes[index].offset.x, GUILayout.Width(40));
            currentGroup.boxes[index].offset.y = EditorGUILayout.FloatField(currentGroup.boxes[index].offset.y, GUILayout.Width(40));
            currentGroup.boxes[index].offset.z = EditorGUILayout.FloatField(currentGroup.boxes[index].offset.z, GUILayout.Width(40));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Rotation", GUILayout.Width(135));
            currentGroup.boxes[index].rotation.x = EditorGUILayout.FloatField(currentGroup.boxes[index].rotation.x, GUILayout.Width(40));
            currentGroup.boxes[index].rotation.y = EditorGUILayout.FloatField(currentGroup.boxes[index].rotation.y, GUILayout.Width(40));
            currentGroup.boxes[index].rotation.z = EditorGUILayout.FloatField(currentGroup.boxes[index].rotation.z, GUILayout.Width(40));
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
                case BoxShapes.Capsule:
                    currentGroup.boxes[index].radius = EditorGUILayout.FloatField("Radius", currentGroup.boxes[index].radius);
                    currentGroup.boxes[index].height = EditorGUILayout.FloatField("Height", currentGroup.boxes[index].height);
                    break;
            }
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
            attack.events[eventSelected].onDetect = EditorGUILayout.Toggle("On Detect?", attack.events[eventSelected].onDetect);
            if (attack.events[eventSelected].onDetect)
            {
                attack.events[eventSelected].onDetectHitboxGroup = EditorGUILayout.IntField("Detect Group",
                    attack.events[eventSelected].onDetectHitboxGroup);
            }
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
                menu.AddSeparator("");

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