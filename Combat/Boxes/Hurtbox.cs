using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Combat
{
    public class Hurtbox : MonoBehaviour
    {
        public GameObject Owner { get; set; } = null;
        public IHurtable Hurtable { get { return Owner.GetComponent<IHurtable>(); } }
    }
}