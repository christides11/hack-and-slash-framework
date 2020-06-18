using CAF.Input;
using Prime31;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Entities
{
    public class EntityManager : CAF.Entities.EntityManager
    {
        public CharacterController2D charController2D;
        public Collider2D coll;
        public EntityDefinition entityDefinition;

        public virtual void Initialize(InputControlType controlType)
        {
            InputManager.SetControlType(controlType);
        }
    }
}