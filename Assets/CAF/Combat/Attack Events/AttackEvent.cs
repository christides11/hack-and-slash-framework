using System;
using CAF.Entities;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CAF.Combat
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
        /// <param name="controller">The controller using this event.</param>
        /// <param name="variables"></param>
        /// <returns>True if the attack state should cancel.</returns>
        public virtual bool Evaluate(uint frame, uint endFrame, EntityController controller,
            AttackEventVariables variables)
        {
            return false;
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