using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HnSF.Combat
{
    public class HurtboxGroupEditorWindow : EditorWindow
    {
        public static Dictionary<string, Type> boxDefinitionTypes = new Dictionary<string, Type>();
        public HurtboxGroup hurtboxGroup;

        public static void Init(HurtboxGroup hurtboxGroup)
        {
            HurtboxGroupEditorWindow window =
                (HurtboxGroupEditorWindow)EditorWindow.GetWindow(typeof(HurtboxGroupEditorWindow),
                true,
                $"Hurtbox {hurtboxGroup.activeFramesStart}~{hurtboxGroup.activeFramesEnd}");
            window.hurtboxGroup = hurtboxGroup;
            window.Show();
        }

        protected virtual void OnEnable()
        {
            boxDefinitionTypes.Clear();
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var givenType in a.GetTypes())
                {
                    if (givenType.IsSubclassOf(typeof(BoxDefinitionBase)))
                    {
                        boxDefinitionTypes.Add(givenType.FullName, givenType);
                    }
                }
            }
        }

        protected Vector2 scrollPos;
        protected virtual void OnGUI()
        {
            if (hurtboxGroup == null)
            {
                Close();
            }
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            DrawHurtboxGroupInfo();
            EditorGUILayout.EndScrollView();
        }

        protected bool boxesFoldout;
        protected virtual void DrawHurtboxGroupInfo()
        {
            EditorGUILayout.LabelField("GENERAL", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            hurtboxGroup.ID = EditorGUILayout.IntField("Group ID", hurtboxGroup.ID);
            hurtboxGroup.attachToEntity = EditorGUILayout.Toggle("Attach to Entity", hurtboxGroup.attachToEntity);
            hurtboxGroup.attachTo = EditorGUILayout.TextField("Attach to", hurtboxGroup.attachTo);

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
                for (int i = 0; i < hurtboxGroup.boxes.Count; i++)
                {
                    DrawHitboxOptions(i);
                    GUILayout.Space(5);
                }
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(10);
        }

        protected virtual void DrawHitboxOptions(int index)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                hurtboxGroup.boxes.RemoveAt(index);
                return;
            }
            GUILayout.Label($"Group {index}");
            EditorGUILayout.EndHorizontal();
            //hurtboxGroup.boxes[index].DrawInspector();
        }

        protected virtual void OnBoxDefinitionSelected(object t)
        {
            hurtboxGroup.boxes.Add((BoxDefinitionBase)Activator.CreateInstance(boxDefinitionTypes[(string)t]));
        }
    }
}
