using UnityEngine;

namespace HnSF.Combat
{
    public class Hurtbox : MonoBehaviour
    {
        public GameObject Owner { get { return owner; } set { owner = value; } }
        public IHurtable Hurtable { get { return owner.GetComponent<IHurtable>(); } }

        [SerializeField] protected GameObject owner;
    }
}