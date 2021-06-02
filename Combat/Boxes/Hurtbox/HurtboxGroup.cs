using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HnSF.Combat
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
    }
}