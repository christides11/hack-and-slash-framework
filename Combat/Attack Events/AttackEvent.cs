using System;
using HnSF.Fighters;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HnSF.Combat
{
    [Serializable]
    public class AttackEvent
    {
        /// <summary>
        /// Proceses the event.
        /// </summary>
        /// <param name="frame">The frame the event is on relative to it's start.</param>
        /// <param name="endFrame">The last frame of the event, relative to it's start.</param>
        /// <param name="attackState">The attack state using this event.</param>
        /// <param name="manager">The manager using this event.</param>
        /// <param name="variables"></param>
        /// <returns>True if the attack state should cancel.</returns>
        public virtual AttackEventReturnType Evaluate(int frame, int endFrame, FighterBase manager,
            AttackEventVariables variables)
        {
            return AttackEventReturnType.NONE;
        }

        public virtual void DrawEventVariables(AttackEventDefinition eventDefinition)
        {
#if UNITY_EDITOR
#endif
        }

        public virtual string GetName()
        {
            return "Attack Event";
        }
    }
}