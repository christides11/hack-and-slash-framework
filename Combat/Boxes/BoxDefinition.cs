using UnityEngine;

namespace CAF.Combat
{
    [System.Serializable]
    public class BoxDefinition
    {
        public BoxShapes shape;
        public Vector3 offset;
        public Vector3 size;
        public Vector3 rotation;
        public float radius;
    }
}