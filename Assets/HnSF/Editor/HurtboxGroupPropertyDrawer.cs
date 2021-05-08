using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace HnSF.Combat
{
    [CustomPropertyDrawer(typeof(HurtboxGroup), true)]
    public class HurtboxGroupPropertyDrawer : PropertyDrawer
    {
        protected Dictionary<string, Type> boxDefinitionTypes = new Dictionary<string, Type>();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float defaultSize = EditorGUIUtility.singleLineHeight * 5 + 14;
            var boxesProperty = property.FindPropertyRelative("boxes");
            if (boxesProperty.isExpanded)
            {
                return defaultSize + EditorGUI.GetPropertyHeight(boxesProperty) + (EditorGUIUtility.singleLineHeight * boxesProperty.arraySize);
            }
            else
            {
                return defaultSize;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float lineSpacing = 18;

            float yPosition = position.y;
            var idRect = new Rect(position.x, yPosition, position.width, lineHeight);
            yPosition += lineSpacing;
            var activeFramesLabelRect = new Rect(position.x, yPosition, 100, lineHeight);
            var activeFramesStartRect = new Rect(position.x+120, yPosition, 50, lineHeight);
            var activeFramesTildeRect = new Rect(activeFramesStartRect.x + activeFramesStartRect.width, yPosition, 30, lineHeight);
            var activeFramesEndRect = new Rect(activeFramesTildeRect.x + activeFramesTildeRect.width, yPosition, 50, lineHeight);
            yPosition += lineSpacing;
            var attachToEntityRect = new Rect(position.x, yPosition, position.width, lineHeight);
            yPosition += lineSpacing;
            var attachToRect = new Rect(position.x, yPosition, position.width, lineHeight);
            yPosition += lineSpacing;
            var boxesFoldRect = new Rect(position.x, yPosition, 10, lineHeight);
            var addBoxButton = new Rect(position.width-50, yPosition, 60, lineHeight);

            EditorGUI.PropertyField(idRect, property.FindPropertyRelative("ID"), new GUIContent("ID"));

            EditorGUI.LabelField(activeFramesLabelRect, "Active Frames");
            EditorGUI.PropertyField(activeFramesStartRect, property.FindPropertyRelative("activeFramesStart"), GUIContent.none);
            EditorGUI.LabelField(activeFramesTildeRect, "~");
            EditorGUI.PropertyField(activeFramesEndRect, property.FindPropertyRelative("activeFramesEnd"), GUIContent.none);

            EditorGUI.PropertyField(attachToEntityRect, property.FindPropertyRelative("attachToEntity"));
            EditorGUI.PropertyField(attachToRect, property.FindPropertyRelative("attachTo"));

            if(GUI.Button(addBoxButton, "Add Box"))
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
                    SerializedProperty tempProperty = property;
                    menu.AddItem(new GUIContent(destination), true, (data) => { OnBoxDefinitionTypeSelected(tempProperty, data); }, hType);
                }
                menu.ShowAsContext();
            }

            var boxesProperty = property.FindPropertyRelative("boxes");
            boxesProperty.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(boxesFoldRect, boxesProperty.isExpanded, GUIContent.none);
            if (boxesProperty.isExpanded)
            {
                yPosition += lineSpacing;
                //float h = (float)EditorGUI.GetPropertyHeight(boxesProperty) / (float)boxesProperty.arraySize;
                for (int i = 0; i < boxesProperty.arraySize; i++)
                {
                    float tHeight = (float)EditorGUI.GetPropertyHeight(boxesProperty.GetArrayElementAtIndex(i));
                    Rect buttonPosition = new Rect(boxesFoldRect.x, yPosition, 25, lineHeight);
                    Rect labelPosition = new Rect(boxesFoldRect.x+20, yPosition, position.width-20, lineHeight);
                    Rect propertyPosition = new Rect(boxesFoldRect.x, yPosition + lineHeight, position.width, tHeight);
                    if(GUI.Button(buttonPosition, new GUIContent("X")))
                    {
                        boxesProperty.DeleteArrayElementAtIndex(i);
                        break;
                    }
                    EditorGUI.LabelField(labelPosition, $"Box {i}");
                    EditorGUI.PropertyField(propertyPosition, boxesProperty.GetArrayElementAtIndex(i));
                    yPosition += tHeight + lineHeight + lineSpacing;
                }
            }
            EditorGUI.EndFoldoutHeaderGroup();
            EditorGUI.EndProperty();
        }

        protected virtual void OnBoxDefinitionTypeSelected(SerializedProperty property, object t)
        {
            property.serializedObject.Update();
            var boxesProperty = property.FindPropertyRelative("boxes");
            boxesProperty.InsertArrayElementAtIndex(boxesProperty.arraySize);
            boxesProperty.GetArrayElementAtIndex(boxesProperty.arraySize-1).managedReferenceValue = Activator.CreateInstance(boxDefinitionTypes[(string)t]);
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}