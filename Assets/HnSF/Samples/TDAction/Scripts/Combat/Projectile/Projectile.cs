using HnSF.Combat;
using HnSF.Simulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Combat
{
    public class Projectile : MonoBehaviour, ISimObject
    {
        public HitboxGroup hitbox;

        public ProjectileSubModule subModules;

        public int frame = 0;

        public virtual void Init(HitboxGroup hitboxGroup)
        {
            this.hitbox = hitboxGroup;
            frame = 0;
        }


        public void SimUpdate()
        {
            frame++;
        }

        public void SimLateUpdate()
        {
        }
    }
}