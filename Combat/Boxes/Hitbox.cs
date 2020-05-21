using CAF.Simulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Combat
{
    public class Hitbox : SimObject
    {
        protected GameObject owner;
        protected bool activated;
        protected Collider coll;
        public HitInfo hitInfo;
    }
}