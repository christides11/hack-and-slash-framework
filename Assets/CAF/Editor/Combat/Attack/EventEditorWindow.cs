using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace CAF.Combat
{
    public class EventEditorWindow : EditorWindow
    {
        protected Dictionary<string, Type> attackEventTypes = new Dictionary<string, Type>();
        public AttackEventDefinition attackEvent;
        public AttackDefinition attack;

        public static void Init(AttackDefinition attack, AttackEventDefinition attackEvent)
        {
            EventEditorWindow window =
                (EventEditorWindow)EditorWindow.GetWindow(typeof(EventEditorWindow),
                true,
                attackEvent.nickname
                );
            window.attack = attack;
            window.attackEvent = attackEvent;
            window.Show();
        }

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

        Vector2 scrollPos;
        protected virtual void OnGUI()
        {
            if (attackEvent == null)
            {
                Close();
            }
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            DrawEventInfo();
            //DrawHurtboxGroupInfo();
            EditorGUILayout.EndScrollView();
        }

        protected bool eventVariablesFoldout;
        private void DrawEventInfo()
        {
            attackEvent.nickname = EditorGUILayout.TextField("Name", attackEvent.nickname);
            attackEvent.active = EditorGUILayout.Toggle("Active", attackEvent.active);
            attackEvent.onHit = EditorGUILayout.Toggle("On Hit?", attackEvent.onHit);
            if (attackEvent.onHit)
            {
                attackEvent.onHitHitboxGroup = EditorGUILayout.IntField("Hitbox Group",
                    attackEvent.onHitHitboxGroup);
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Charge Required", GUILayout.Width(150));
            EditorGUILayout.LabelField("min", GUILayout.Width(25));
            attackEvent.chargeLevelMin = EditorGUILayout.IntField(attackEvent.chargeLevelMin, GUILayout.Width(30));
            EditorGUILayout.LabelField("max", GUILayout.Width(25));
            attackEvent.chargeLevelMax = EditorGUILayout.IntField(attackEvent.chargeLevelMax, GUILayout.Width(30));
            EditorGUILayout.EndHorizontal();
            attackEvent.inputCheckTiming = (AttackEventInputCheckTiming)EditorGUILayout.EnumPopup("Input Requirement",
                attackEvent.inputCheckTiming);

            EditorGUI.indentLevel++;
            if (attackEvent.inputCheckTiming != AttackEventInputCheckTiming.NONE)
            {
                SerializedObject serializedObject = new UnityEditor.SerializedObject(attack);
                serializedObject.Update();
                attackEvent.inputCheckStartFrame = (int)EditorGUILayout.IntField("Start Frame", (int)attackEvent.inputCheckStartFrame);
                attackEvent.inputCheckEndFrame = (int)EditorGUILayout.IntField("End Frame", (int)attackEvent.inputCheckEndFrame);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Input");
                attackEvent.input.DrawInspector();
                serializedObject.ApplyModifiedProperties();
            }
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField(attackEvent.attackEvent == null ? "..."
                : attackEvent.attackEvent.GetName());
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
                if (attackEvent.attackEvent != null)
                {
                    attackEvent.attackEvent.DrawEventVariables(attackEvent);
                }
                EditorGUI.indentLevel--;
            }
        }

        protected void OnAttackEventSelected(object t)
        {
            attackEvent.attackEvent = (AttackEvent)Activator.CreateInstance(attackEventTypes[(string)t]);
        }
    }
}