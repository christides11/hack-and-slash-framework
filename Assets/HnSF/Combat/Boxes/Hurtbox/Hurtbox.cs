using System;
using UnityEngine;

namespace HnSF.Combat
{
    public class Hurtbox : MonoBehaviour
    {
        public GameObject Owner { get { return owner; } }
        public IHurtable Hurtable { get { return hurtable; } }
        public HurtboxGroup HurtboxGroup { get { return hurtboxGroup; } }

        [NonSerialized] protected HurtboxGroup hurtboxGroup;
        [NonSerialized] protected GameObject owner;
        [NonSerialized] protected IHurtable hurtable;

        public void Initialize(GameObject owner, HurtboxGroup hurtboxGroup)
        {
            this.owner = owner;
            this.hurtable = owner.GetComponent<IHurtable>();
            this.hurtboxGroup = hurtboxGroup;
        }
    }
}