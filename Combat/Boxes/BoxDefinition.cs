using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HnSF.Combat
{
    /// <summary>
    /// Default implementation of BoxDefinitionBase.
    /// </summary>
    [System.Serializable]
    public class BoxDefinition : BoxDefinitionBase
    {
        public Vector3 offset;
        public Vector3 size;
        public Vector3 rotation;
        public float radius;
        public float height;

        public BoxDefinition() : base()
        {

        }

        public BoxDefinition(BoxDefinition other)
        {
            shape = other.shape;
            offset = other.offset;
            size = other.size;
            rotation = other.rotation;
            radius = other.radius;
            height = other.height;
        }

        public override void DrawInspector()
        {
#if UNITY_EDITOR
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Offset", GUILayout.Width(135));
            offset.x = EditorGUILayout.FloatField(offset.x, GUILayout.Width(60));
            offset.y = EditorGUILayout.FloatField(offset.y, GUILayout.Width(60));
            offset.z = EditorGUILayout.FloatField(offset.z, GUILayout.Width(60));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Rotation", GUILayout.Width(135));
            rotation.x = EditorGUILayout.FloatField(rotation.x, GUILayout.Width(60));
            rotation.y = EditorGUILayout.FloatField(rotation.y, GUILayout.Width(60));
            rotation.z = EditorGUILayout.FloatField(rotation.z, GUILayout.Width(60));
            EditorGUILayout.EndHorizontal();
            switch (shape)
            {
                case BoxShape.Rectangle:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Size", GUILayout.Width(135));
                    size.x = EditorGUILayout.FloatField(size.x, GUILayout.Width(60));
                    size.y = EditorGUILayout.FloatField(size.y, GUILayout.Width(60));
                    size.z = EditorGUILayout.FloatField(size.z, GUILayout.Width(60));
                    EditorGUILayout.EndHorizontal();
                    break;
                case BoxShape.Circle:
                    radius
                        = EditorGUILayout.FloatField("Radius", radius);
                    break;
                case BoxShape.Capsule:
                    radius = EditorGUILayout.FloatField("Radius", radius);
                    height = EditorGUILayout.FloatField("Height", height);
                    break;
            }
#endif
        }
    }
}