using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Combat
{
    public class HurtInfoBase
    {
        public HitInfoBase hitInfo;

        public HurtInfoBase()
        {

        }

        public HurtInfoBase(HitInfoBase hitInfo)
        {
            this.hitInfo = hitInfo;
        }
    }
}