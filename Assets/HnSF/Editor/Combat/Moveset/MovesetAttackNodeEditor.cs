using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;
using UnityEditor;
using static HnSF.Combat.MovesetAttackNode;
using XNode;
using Malee.List;

namespace HnSF.Combat
{

    [CustomNodeEditor(typeof(MovesetAttackNode))]
    public class MovesetAttackNodeEditor : NodeEditor
    {
        MovesetAttackNode node;

        bool executeInputsDropdown;

        private ReorderableList executeList;
        private ReorderableList inputSequence;

        public override void OnCreate()
        {
            base.OnCreate();
            executeList = new ReorderableList(serializedObject.FindProperty("inputSequence").FindPropertyRelative("executeInputs"));
            executeList.elementNameProperty = "Execute Inputs";

            inputSequence = new ReorderableList(serializedObject.FindProperty("inputSequence").FindPropertyRelative("sequenceInputs"));
            inputSequence.elementNameProperty = "Input Sequence";
        }

        public override void OnBodyGUI()
        {
            if (node == null) node = target as MovesetAttackNode;

            serializedObject.Update();

            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("lastNode"));

            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("attackDefinition"));

            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("inputSequence").FindPropertyRelative("executeWindow"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("inputSequence").FindPropertyRelative("sequenceWindow"));

            executeList.DoLayoutList();

            inputSequence.DoLayoutList();

            // Draw GUI
            NodeEditorGUILayout.DynamicPortList(
                "nextNode", // field name
                typeof(nextNodeDefinition), // field type
                serializedObject, // serializable object
                NodePort.IO.Output); // onCreate override. This is where the magic happens.

            serializedObject.ApplyModifiedProperties();
        }
    }
}