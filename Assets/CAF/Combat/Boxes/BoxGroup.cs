using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Combat
{
    [System.Serializable]
    public class BoxGroup
    {
        public int ID;
        public int activeFramesStart;
        public int activeFramesEnd;
        public BoxGroupType hitGroupType;
        public List<BoxDefinition> hitboxes = new List<BoxDefinition>();
        public bool attachToEntity = true;

        [SerializeField] public HitInfo hitboxHitInfo = new HitInfo();

        public BoxGroup()
        {

        }

        public BoxGroup(BoxGroup other)
        {
            ID = other.ID;
            activeFramesStart = other.activeFramesStart;
            activeFramesEnd = other.activeFramesEnd;
            hitGroupType = other.hitGroupType;
            hitboxes = new List<BoxDefinition>(other.hitboxes);
            attachToEntity = other.attachToEntity;
            hitboxHitInfo = new HitInfo(other.hitboxHitInfo);
        }
    }
}
