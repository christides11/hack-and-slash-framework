using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Combat
{
    [System.Serializable]
    public class HitboxGroup
    {
        public int ID;
        public int activeFramesStart = 1;
        public int activeFramesEnd = 1;
        [SerializeReference] public List<BoxDefinitionBase> boxes = new List<BoxDefinitionBase>();
        public bool attachToEntity = true;
        public string attachTo;
        public int chargeLevelNeeded = -1;
        public int chargeLevelMax = 1;

        [SerializeReference] public HitInfoBase hitboxHitInfo = new HitInfo();

        public HitboxGroup()
        {

        }

        public HitboxGroup(HitboxGroup other)
        {
            ID = other.ID;
            activeFramesStart = other.activeFramesStart;
            activeFramesEnd = other.activeFramesEnd;
            attachToEntity = other.attachToEntity;
            chargeLevelNeeded = other.chargeLevelNeeded;
            chargeLevelMax = other.chargeLevelMax;
            if (other.hitboxHitInfo.GetType() == typeof(HitInfo))
            {
                hitboxHitInfo = new HitInfo((HitInfo)other.hitboxHitInfo);
            }
            boxes = new List<BoxDefinitionBase>();
        }
    }
}
