using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Combat
{
    public class HurtInfo2D : HurtInfoBase
    {
        public Vector2 center;
        public int faceDirection = 1;

        public HurtInfo2D() : base()
        {

        }

        public HurtInfo2D(HitInfoBase hitInfo, Vector2 center, int faceDirection)
        {
            this.hitInfo = hitInfo;
            this.center = center;
            this.faceDirection = faceDirection;
        }
    }
}