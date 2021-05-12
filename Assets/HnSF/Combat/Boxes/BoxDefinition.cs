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
    }
}