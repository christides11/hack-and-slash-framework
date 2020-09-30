using CAF.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Entities
{
    public class EntityHurtboxManager : MonoBehaviour
    {
        [SerializeField] protected EntityManager manager;

        public virtual void Tick()
        {

        }
    }
}