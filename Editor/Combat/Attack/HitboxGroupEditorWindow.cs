using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace HnSF.Combat
{
    public class HitboxGroupEditorWindow : EditorWindow
    {
        public static Dictionary<string, Type> boxDefinitionTypes = new Dictionary<string, Type>();
        public static Dictionary<string, Type> hitInfoTypes = new Dictionary<string, Type>();

        public HitboxGroup hGroup;
        public UnityEngine.Object hitboxGroupObject;
        public string hitboxGroupPropertyName;
        public int hitboxGroupPropertyIndex = -1;

        public static void Init(UnityEngine.Object obj, HitboxGroup hGroup, string hitboxGroupPropertyName, int propertyIndex = -1)
        {
            HitboxGroupEditorWindow window = (HitboxGroupEditorWindow)EditorWindow.GetWindow(typeof(HitboxGroupEditorWindow), true, $"Hitbox");
            window.hGroup = hGroup;
            window.hitboxGroupObject = obj;
            window.hitboxGroupPropertyName = hitboxGroupPropertyName;
            window.hitboxGroupPropertyIndex = propertyIndex;
            window.Show();
        }

        private void OnEnable()
        {
            boxDefinitionTypes.Clear();
            hitInfoTypes.Clear();
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var givenType in a.GetTypes())
                {
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

        Vector2 scrollPos;
        protected virtual void OnGUI()
        {
            if(hitboxGroupObject == null)
            {
                Close();
            }
            //scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            DrawHitboxGroupInfo();
            //EditorGUILayout.EndScrollView();
        }

        protected bool boxesFoldout;
        protected SerializedObject so;
        protected virtual void DrawHitboxGroupInfo()
        {
            so = new SerializedObject(hitboxGroupObject);
            if (so == null)
            {
                Close();
                return;
            }
            so.Update();
            SerializedProperty hitboxGroupProperty = null;
            hitboxGroupProperty = GetHitboxGroupProperty();
            if (hitboxGroupProperty == null)
            {
                Close();
                return;
            }

            EditorGUILayout.LabelField("GENERAL", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(hitboxGroupProperty.FindPropertyRelative("ID"), new GUIContent("ID"));

            EditorGUILayout.PropertyField(hitboxGroupProperty.FindPropertyRelative("chargeLevelNeeded"), new GUIContent("Charge Level Needed"));
            if (hitboxGroupProperty.FindPropertyRelative("chargeLevelNeeded").intValue >= 0)
            {
                EditorGUILayout.PropertyField(hitboxGroupProperty.FindPropertyRelative("chargeLevelMax"), new GUIContent("Charge Level Max"));
            }
            EditorGUILayout.PropertyField(hitboxGroupProperty.FindPropertyRelative("attachToEntity"), new GUIContent("attachToEntity"));

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
                for (int i = 0; i < hitboxGroupProperty.FindPropertyRelative("boxes").arraySize; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        hitboxGroupProperty.FindPropertyRelative("boxes").DeleteArrayElementAtIndex(i);
                        break;
                    }
                    GUILayout.Label($"Box {i}");
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.PropertyField(hitboxGroupProperty.FindPropertyRelative("boxes").GetArrayElementAtIndex(i), GUIContent.none, true);
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

            EditorGUILayout.LabelField($"Type: {hitboxGroupProperty.FindPropertyRelative("hitboxHitInfo").managedReferenceFullTypename}");
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            EditorGUILayout.PropertyField(hitboxGroupProperty.FindPropertyRelative("hitboxHitInfo"), true);
            EditorGUILayout.EndScrollView();
            so.ApplyModifiedProperties();
        }

        protected virtual SerializedProperty GetHitboxGroupProperty()
        {
            SerializedProperty hitboxGroupProperty;
            if (hitboxGroupPropertyIndex != -1)
            {
                hitboxGroupProperty = so.FindProperty(hitboxGroupPropertyName).GetArrayElementAtIndex(hitboxGroupPropertyIndex);
            }
            else
            {
                hitboxGroupProperty = so.FindProperty(hitboxGroupPropertyName);
            }

            return hitboxGroupProperty;
        }

        protected void OnHitInfoSelected(object t)
        {
            so.Update();
            SerializedProperty chargeWindowsProperty = GetHitboxGroupProperty().FindPropertyRelative("hitboxHitInfo");
            if (hGroup.hitboxHitInfo != null)
            {
                chargeWindowsProperty.managedReferenceValue = (HitInfoBase)Activator.CreateInstance(hitInfoTypes[(string)t], 
                    new object[] { hGroup.hitboxHitInfo });
                return;
            }
            chargeWindowsProperty.managedReferenceValue = (HitInfoBase)Activator.CreateInstance(hitInfoTypes[(string)t]);
            so.ApplyModifiedProperties();
        }

        protected void OnBoxDefinitionSelected(object t)
        {
            so.Update();
            var boxesProperty = GetHitboxGroupProperty().FindPropertyRelative("boxes");
            boxesProperty.InsertArrayElementAtIndex(boxesProperty.arraySize);
            boxesProperty.GetArrayElementAtIndex(boxesProperty.arraySize - 1).managedReferenceValue = Activator.CreateInstance(boxDefinitionTypes[(string)t]);
            so.ApplyModifiedProperties();
        }
    }
}
