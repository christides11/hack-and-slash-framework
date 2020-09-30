using System.Collections.Generic;
using UnityEngine;

namespace CAF.Combat
{
    [System.Serializable]
    public class BoxGroup
    {
        public int ID;
        public int activeFramesStart = 1;
        public int activeFramesEnd = 1;
        public BoxGroupType hitGroupType;
        [SerializeReference] public List<BoxDefinitionBase> boxes = new List<BoxDefinitionBase>();
        public bool attachToEntity = true;
        public int chargeLevelNeeded = -1;
        public int chargeLevelMax = 1;

        [SerializeReference] public HitInfoBase hitboxHitInfo = new HitInfo();

        public BoxGroup()
        {

        }

        public BoxGroup(BoxGroup other)
        {
            ID = other.ID;
            activeFramesStart = other.activeFramesStart;
            activeFramesEnd = other.activeFramesEnd;
            hitGroupType = other.hitGroupType;
            attachToEntity = other.attachToEntity;
            if (other.hitboxHitInfo.GetType() == typeof(HitInfo))
            {
                hitboxHitInfo = new HitInfo((HitInfo)other.hitboxHitInfo);
            }
            boxes = new List<BoxDefinitionBase>();
        }
    }
}
