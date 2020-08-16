using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Entities.Characters.Boxer
{
    public class BIdle : CIdle
    {
        public override bool CheckInterrupt()
        {
            EntityManager entityManager = GetEntityController();
            if (entityManager.TryAttack())
            {
                return true;
            }
            return base.CheckInterrupt();
        }
    }
}