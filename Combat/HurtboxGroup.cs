using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CAF.Combat
{
    [System.Serializable]
    public class HurtboxGroup
    {
        public int ID;
        public int activeFramesStart = 1;
        public int activeFramesEnd = 1;
        [SerializeReference] public List<BoxDefinitionBase> boxes = new List<BoxDefinitionBase>();
        public bool attachToEntity = true;
        public string attachTo;

        public HurtboxGroup()
        {

        }

        public HurtboxGroup(HurtboxGroup other)
        {

        }

        [NonSerialized] protected Dictionary<string, Type> boxDefinitionTypes = new Dictionary<string, Type>();
        [NonSerialized] protected bool boxesDropdown = false;
        public virtual void DrawInspector(float indentLevel)
        {
#if UNITY_EDITOR
            ID = EditorGUILayout.IntField("ID", ID);
            activeFramesStart = EditorGUILayout.IntField("Active Frames (Start)", activeFramesStart);
            activeFramesEnd = EditorGUILayout.IntField("Active Frames (End)",  activeFramesEnd);
            attachToEntity = EditorGUILayout.Toggle("Attached to Entity", attachToEntity);
            attachTo = EditorGUILayout.TextField("Attached To", attachTo);
            GUILayout.BeginHorizontal();
            GUILayout.Space(EditorGUI.indentLevel * indentLevel);
            if (GUILayout.Button("Add Box"))
            {
                GenericMenu menu = new GenericMenu();

                boxDefinitionTypes = new Dictionary<string, Type>();
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

                foreach (string hType in boxDefinitionTypes.Keys)
                {
                    string destination = hType.Replace('.', '/');
                    menu.AddItem(new GUIContent(destination), true, OnBoxDefinitionTypeSelected, hType);
                }
                menu.ShowAsContext();
            }
            GUILayout.EndHorizontal();

            boxesDropdown = EditorGUILayout.Foldout(boxesDropdown, $"Boxes ({boxes.Count})");
            if (boxesDropdown)
            {
                for (int i = 0; i < boxes.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"Box {i}", GUILayout.MaxWidth(100));
                    if (GUILayout.Button("X", GUILayout.Width(40)))
                    {
                        boxes.RemoveAt(i);
                        return;
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUI.indentLevel++;
                    boxes[i].DrawInspector();
                    EditorGUI.indentLevel--;
                }
            }
#endif
        }

        protected virtual void OnBoxDefinitionTypeSelected(object t)
        {
            boxes.Add((BoxDefinitionBase)Activator.CreateInstance(boxDefinitionTypes[(string)t]));
        }
    }
}