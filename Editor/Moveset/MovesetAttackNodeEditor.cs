using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;
using UnityEditor;
using Malee.Editor;
using static CAF.Combat.MovesetAttackNode;
using XNode;

namespace CAF.Combat
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
            executeList = new ReorderableList(serializedObject.FindProperty("executeInputs"));
            executeList.elementNameProperty = "Execute Inputs";

            inputSequence = new ReorderableList(serializedObject.FindProperty("inputSequence"));
            inputSequence.elementNameProperty = "Input Sequence";
        }

        public override void OnBodyGUI()
        {
            if (node == null) node = target as MovesetAttackNode;

            serializedObject.Update();

            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("lastNode"));

            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("attackDefinition"));

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