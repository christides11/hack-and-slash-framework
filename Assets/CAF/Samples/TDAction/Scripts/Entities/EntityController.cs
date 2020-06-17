using CAF.Input;
using Prime31;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Entities
{
    public class EntityController : CAF.Entities.EntityController
    {
        public CharacterController2D charController2D;
        public Collider2D coll;

        public virtual void Initialize(InputControlType controlType)
        {
            InputManager.SetControlType(controlType);
        }
    }
}