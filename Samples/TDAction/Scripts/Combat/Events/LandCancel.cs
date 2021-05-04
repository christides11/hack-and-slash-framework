﻿using UnityEngine;
using System.Collections.Generic;
using CAF.Combat;
using TDAction.Entities;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TDAction.Combat.Events
{
    public class LandCancel : AttackEvent
    {
        public override string GetName()
        {
            return "Land Cancel";
        }

        public override AttackEventReturnType Evaluate(int frame, int endFrame,
            CAF.Fighters.FighterBase controller, AttackEventVariables variables)
        {
            if ((controller as TDAction.Entities.FighterManager).TryLandCancel())
            {
                return AttackEventReturnType.INTERRUPT;
            }
            return AttackEventReturnType.NONE;
        }
    }
}