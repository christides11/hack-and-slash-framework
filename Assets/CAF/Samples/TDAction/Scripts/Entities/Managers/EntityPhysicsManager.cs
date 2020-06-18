using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Entities
{
    public class EntityPhysicsManager : CAF.Entities.EntityPhysicsManager
    {

        public override void Tick()
        {
            ((EntityManager)controller).charController2D.move(GetOverallForce());
        }
    }
}