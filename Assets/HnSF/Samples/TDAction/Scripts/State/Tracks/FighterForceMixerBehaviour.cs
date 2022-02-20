using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace HnSF.Sample.TDAction
{
    [System.Serializable]
    public class FighterForceMixerBehaviour : PlayableBehaviour
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            FighterManager cm = playerData as FighterManager;

            if (cm == null)
                return;

            float finalXForce = 0;
            float finalYForce = 0;

            Vector2 finalSetForce = Vector2.zero;
            Vector2 finalAddForce = Vector2.zero;
            
            Vector2 finalWeights = Vector2.zero;

            int inputCount = playable.GetInputCount();

            float extraWeight = 0;
            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i) + extraWeight;
                ScriptPlayable<ForceSetBehaviour> inputPlayable =
                    (ScriptPlayable<ForceSetBehaviour>)playable.GetInput(i);
                ForceSetBehaviour input = inputPlayable.GetBehaviour();

                /*
                if (input.conditon.IsTrue(cm) == false)
                {
                    extraWeight = inputWeight;
                    continue;
                }*/

                switch (input.forceSetType)
                {
                    case ForceSetType.SET:
                        finalSetForce = (finalSetForce * finalWeights.x) + (input.force * inputWeight);
                        finalWeights.x += inputWeight;
                        break;
                    case ForceSetType.ADD:
                        finalAddForce += input.force * inputWeight;
                        finalWeights.y += inputWeight;
                        break;
                }
            }
            finalWeights.Normalize();

            //assign the result to the bound object
            (cm.PhysicsManager as FighterPhysicsManager).forceMovement = finalSetForce * finalWeights.x;
            (cm.PhysicsManager as FighterPhysicsManager).forceMovement += finalAddForce * finalWeights.y;
        }
    }
}