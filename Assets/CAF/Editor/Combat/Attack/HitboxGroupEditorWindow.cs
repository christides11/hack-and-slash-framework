using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace CAF.Combat
{
    public class HitboxGroupEditorWindow : EditorWindow
    {
        public static Dictionary<string, Type> boxDefinitionTypes = new Dictionary<string, Type>();
        public static Dictionary<string, Type> hitInfoTypes = new Dictionary<string, Type>();
        public HitboxGroup hitboxGroup;

        public static void Init(HitboxGroup hitboxGroup)
        {
            HitboxGroupEditorWindow window = (HitboxGroupEditorWindow)EditorWindow.GetWindow(typeof(HitboxGroupEditorWindow), true, $"Hitbox {hitboxGroup.activeFramesStart}~{hitboxGroup.activeFramesEnd}");
            window.hitboxGroup = hitboxGroup;
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
            if(hitboxGroup == null)
            {
                Close();
            }
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            DrawHitboxGroupInfo();
            EditorGUILayout.EndScrollView();
        }

        protected bool boxesFoldout;
        protected virtual void DrawHitboxGroupInfo()
        {
            EditorGUILayout.LabelField("GENERAL", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            hitboxGroup.ID = EditorGUILayout.IntField("Group ID", hitboxGroup.ID);

            hitboxGroup.chargeLevelNeeded = EditorGUILayout.IntField("Charge Level Needed", hitboxGroup.chargeLevelNeeded);
            if (hitboxGroup.chargeLevelNeeded >= 0)
            {
                hitboxGroup.chargeLevelMax = EditorGUILayout.IntField("Charge Level Max", hitboxGroup.chargeLevelMax);
            }

            hitboxGroup.hitGroupType = (HitboxType)EditorGUILayout.EnumPopup("Hit Type", hitboxGroup.hitGroupType);
            hitboxGroup.attachToEntity = EditorGUILayout.Toggle("Attach to Entity", hitboxGroup.attachToEntity);

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
                for (int i = 0; i < hitboxGroup.boxes.Count; i++)
                {
                    DrawHitboxOptions(i);
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

            if (hitboxGroup.hitboxHitInfo == null)
            {
                return;
            }
            EditorGUILayout.LabelField($"Type: {hitboxGroup.hitboxHitInfo.GetType().FullName}");

            switch (hitboxGroup.hitGroupType)
            {
                case HitboxType.HIT:
                    hitboxGroup.hitboxHitInfo.DrawInspectorHitInfo();
                    break;
                case HitboxType.GRAB:
                    hitboxGroup.hitboxHitInfo.DrawInspectorGrabInfo();
                    break;
            }
        }

        protected virtual void DrawHitboxOptions(int index)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                hitboxGroup.boxes.RemoveAt(index);
                return;
            }
            GUILayout.Label($"Group {index}");
            EditorGUILayout.EndHorizontal();
            hitboxGroup.boxes[index].DrawInspector();
        }

        protected void OnHitInfoSelected(object t)
        {
            if(hitboxGroup.hitboxHitInfo != null)
            {
                hitboxGroup.hitboxHitInfo = (HitInfoBase)Activator.CreateInstance(hitInfoTypes[(string)t], new object[] { hitboxGroup.hitboxHitInfo });
                return;
            }
            hitboxGroup.hitboxHitInfo = (HitInfoBase)Activator.CreateInstance(hitInfoTypes[(string)t]);
        }

        protected void OnBoxDefinitionSelected(object t)
        {
            hitboxGroup.boxes.Add((BoxDefinitionBase)Activator.CreateInstance(boxDefinitionTypes[(string)t]));
        }
    }
}
