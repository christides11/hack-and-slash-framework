using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace HnSF.Combat
{
    public class AttackEventDefinitionEditorWindow : EditorWindow
    {
        protected Dictionary<string, Type> attackConditionTypes = new Dictionary<string, Type>();
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
            attackConditionTypes.Clear();
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var givenType in a.GetTypes())
                {
                    if (givenType.IsSubclassOf(typeof(AttackEvent)))
                    {
                        attackEventTypes.Add(givenType.FullName, givenType);
                    }
                    if (givenType.IsSubclassOf(typeof(AttackCondition)))
                    {
                        attackConditionTypes.Add(givenType.FullName, givenType);
                    }
                }
            }
        }

        protected Vector2 scrollPos;
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
        protected bool conditionsFoldout;
        protected virtual void DrawEventInfo()
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

            if (GUILayout.Button("Add Condition"))
            {
                GenericMenu menu = new GenericMenu();

                foreach (string t in attackConditionTypes.Keys)
                {
                    string destination = t.Replace('.', '/');
                    menu.AddItem(new GUIContent(destination), true, OnAttackConditionSelected, t);
                }
                menu.ShowAsContext();
            }

            conditionsFoldout = EditorGUILayout.Foldout(conditionsFoldout, "Conditions");
            if (conditionsFoldout)
            {
                SerializedProperty conditionsProperty = eventProperty.FindPropertyRelative("conditions");
                EditorGUI.indentLevel++;
                for(int i = 0; i < conditionsProperty.arraySize; i++)
                {
                    AttackCondition ac = attack.events[eventIndex].conditions[i];
                    EditorGUILayout.BeginHorizontal();
                    if(GUILayout.Button("-", GUILayout.Width(20)) || ac == null)
                    {
                        conditionsProperty.DeleteArrayElementAtIndex(i);
                        EditorGUILayout.EndHorizontal();
                        break;
                    }
                    EditorGUILayout.PropertyField(conditionsProperty.GetArrayElementAtIndex(i), new GUIContent(ac.GetName()), true);
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }

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
            EditorGUILayout.LabelField(attack.events[eventIndex].attackEvent == null ? "..." : attack.events[eventIndex].attackEvent.GetName());
            GUILayout.Space(5);

            if (attack.events[eventIndex].attackEvent != null)
            {
                EditorGUILayout.PropertyField(attackEventProperty, true);

            }

            attackObject.ApplyModifiedProperties();
        }

        protected virtual void OnAttackEventSelected(object t)
        {
            SerializedObject attackObject = new SerializedObject(attack);
            attackObject.Update();
            SerializedProperty eventProperty = attackObject.FindProperty("events").GetArrayElementAtIndex(eventIndex);
            var attackEventProperty = eventProperty.FindPropertyRelative("attackEvent");
            attackEventProperty.managedReferenceValue = Activator.CreateInstance(attackEventTypes[(string)t]);
            attackObject.ApplyModifiedProperties();
        }

        protected virtual void OnAttackConditionSelected(object t)
        {
            SerializedObject attackObject = new SerializedObject(attack);
            attackObject.Update();
            SerializedProperty eventProperty = attackObject.FindProperty("events").GetArrayElementAtIndex(eventIndex);
            eventProperty.FindPropertyRelative("conditions").InsertArrayElementAtIndex(eventProperty.FindPropertyRelative("conditions").arraySize);
            var conditionProperty = eventProperty.FindPropertyRelative("conditions").GetArrayElementAtIndex(eventProperty.FindPropertyRelative("conditions").arraySize-1);
            conditionProperty.managedReferenceValue = Activator.CreateInstance(attackConditionTypes[(string)t]);
            attackObject.ApplyModifiedProperties();
        }
    }
}