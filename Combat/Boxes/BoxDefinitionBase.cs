using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HnSF.Combat
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
    }
}