using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CAF.Combat
{
    [System.Serializable]
    public class BoxDefinitionBase
    {

        public BoxShape shape;

        public BoxDefinitionBase()
        {

        }

        public BoxDefinitionBase(BoxDefinitionBase other)
        {

        }

        public virtual void DrawInspector()
        {
#if UNITY_EDITOR
            shape = (BoxShape)EditorGUILayout.EnumPopup("Shape", shape);
#endif
        }
    }
}