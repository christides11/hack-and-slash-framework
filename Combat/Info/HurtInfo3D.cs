using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Combat
{
    public class HurtInfo3D : HurtInfoBase
    {
        public Vector3 center, forward, right;

        public HurtInfo3D() : base()
        {

        }

        public HurtInfo3D(HitInfoBase hitInfo, Vector3 center, Vector3 forward, Vector3 right)
        {
            this.hitInfo = hitInfo;
            this.center = center;
            this.forward = forward;
            this.right = right;
        }
    }
}