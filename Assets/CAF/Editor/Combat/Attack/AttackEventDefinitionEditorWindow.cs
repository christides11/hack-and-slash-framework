using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace CAF.Combat
{
    public class AttackEventDefinitionEditorWindow : EditorWindow
    {
        protected Dictionary<string, Type> attackEventTypes = new Dictionary<string, Type>();
        public AttackDefinition attack;
        public int eventIndex;

        public static void Init(AttackDefinition attack, int eventIndex)
        {
            AttackEventDefinitionEditorWindow window =
                (AttackEventDefinitionEditorWindow)EditorWindow.GetWindow(typeof(AttackEventDefinitionEditorWindow),
                true,
                attack.events[eventIndex].nickname
                );
            window.attack = attack;
            window.eventIndex = eventIndex;
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
            if (attack == null || attack.events.Count <= eventIndex)
            {
                Close();
                return;
            }
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            DrawEventInfo();
            EditorGUILayout.EndScrollView();
        }

        protected bool eventVariablesFoldout;
        private void DrawEventInfo()
        {
            SerializedObject attackObject = new SerializedObject(attack);
            attackObject.Update();

            SerializedProperty eventProperty = attackObject.FindProperty("events").GetArrayElementAtIndex(eventIndex);

            if(eventProperty == null)
            {
                return;
            }

            EditorGUILayout.PropertyField(eventProperty.FindPropertyRelative("nickname"), new GUIContent("Nickname"));
            EditorGUILayout.PropertyField(eventProperty.FindPropertyRelative("active"), new GUIContent("Active"));
            EditorGUILayout.PropertyField(eventProperty.FindPropertyRelative("onHitCheck"), new GUIContent("On Hit Check"));
            switch ((OnHitType)eventProperty.FindPropertyRelative("onHitCheck").enumValueIndex)
            {
                case OnHitType.ID_GROUP:
                    EditorGUILayout.PropertyField(eventProperty.FindPropertyRelative("onHitIDGroup"), new GUIContent("ID Group"));
                    break;
                case OnHitType.HITBOX_GROUP:
                    EditorGUILayout.PropertyField(eventProperty.FindPropertyRelative("onHitHitboxGroup"), new GUIContent("Hitbox Group"));
                    break;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Charge Required", GUILayout.Width(150));
            EditorGUILayout.LabelField("min", GUILayout.Width(25));
            EditorGUILayout.PropertyField(eventProperty.FindPropertyRelative("chargeLevelMin"), GUIContent.none, GUILayout.Width(30));
            EditorGUILayout.LabelField("max", GUILayout.Width(25));
            EditorGUILayout.PropertyField(eventProperty.FindPropertyRelative("chargeLevelMax"), GUIContent.none, GUILayout.Width(30));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(eventProperty.FindPropertyRelative("inputCheckTiming"));

            EditorGUI.indentLevel++;
            if ((AttackEventInputCheckTiming)eventProperty.FindPropertyRelative("inputCheckTiming").enumValueIndex != AttackEventInputCheckTiming.NONE)
            {
                EditorGUILayout.PropertyField(eventProperty.FindPropertyRelative("inputCheckStartFrame"));
                EditorGUILayout.PropertyField(eventProperty.FindPropertyRelative("inputCheckEndFrame"));
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Input");
                EditorGUILayout.PropertyField(eventProperty.FindPropertyRelative("input"));
                EditorGUILayout.Space();
            }
            EditorGUI.indentLevel--;

            EditorGUILayout.LabelField(attack.events[eventIndex].attackEvent == null ? "..." : attack.events[eventIndex].attackEvent.GetName());
            var attackEventProperty = eventProperty.FindPropertyRelative("attackEvent");
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
                if (attack.events[eventIndex].attackEvent != null)
                {
                    EditorGUILayout.PropertyField(attackEventProperty, true);
                }
                EditorGUI.indentLevel--;
            }

            attackObject.ApplyModifiedProperties();
        }

        protected void OnAttackEventSelected(object t)
        {
            SerializedObject attackObject = new SerializedObject(attack);
            attackObject.Update();
            SerializedProperty eventProperty = attackObject.FindProperty("events").GetArrayElementAtIndex(eventIndex);
            var attackEventProperty = eventProperty.FindPropertyRelative("attackEvent");
            attackEventProperty.managedReferenceValue = Activator.CreateInstance(attackEventTypes[(string)t]);
            attackObject.ApplyModifiedProperties();
        }
    }
}