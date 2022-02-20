using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HnSF.Sample.TDAction
{
    public class FighterInputManager : MonoBehaviour
    {
        public static int inputCapacity = 1024;
        public int BufferLimit { get; set; } = 0;

        [SerializeField] private FighterManager manager;
        
        [HideInInspector] public Vector2[] movement = new Vector2[inputCapacity];
        [HideInInspector] public InputButtonData[] lightAtk = new InputButtonData[inputCapacity];
        [HideInInspector] public InputButtonData[] jump = new InputButtonData[inputCapacity];
        [HideInInspector] public InputButtonData[] dash = new InputButtonData[inputCapacity];
        [HideInInspector] public InputButtonData[] lockOn = new InputButtonData[inputCapacity];

        public void FeedInput(int frame, PlayerInputData inputData)
        {
            movement[frame % inputCapacity] = inputData.movement;
            lightAtk[frame % inputCapacity] = new InputButtonData(inputData.lightAtk, lightAtk[(frame-1) % inputCapacity]);
            jump[frame % inputCapacity] = new InputButtonData(inputData.jump, jump[(frame-1) % inputCapacity]);
            dash[frame % inputCapacity] = new InputButtonData(inputData.dash, dash[(frame-1) % inputCapacity]);
            lockOn[frame % inputCapacity] = new InputButtonData(inputData.lockOn, lockOn[(frame-1) % inputCapacity]);
        }
        
        public virtual Vector2 GetMovement(int startOffset = 0)
        {
            return movement[(manager.Manager.Simulation.Tick - startOffset) % inputCapacity];
        }
        
        public virtual InputButtonData GetLight(out int buttonOffset, int startOffset = 0, int bufferFrames = 0)
        {
            return GetButton(ref lightAtk, out buttonOffset, startOffset, bufferFrames);
        }
        
        public virtual InputButtonData GetJump(out int buttonOffset, int startOffset = 0, int bufferFrames = 0)
        {
            return GetButton(ref jump, out buttonOffset, startOffset, bufferFrames);
        }
        
        public virtual InputButtonData GetDash(out int buttonOffset, int startOffset = 0, int bufferFrames = 0)
        {
            return GetButton(ref dash, out buttonOffset, startOffset, bufferFrames);
        }
        
        public virtual InputButtonData GetLockOn(out int buttonOffset, int startOffset = 0, int bufferFrames = 0)
        {
            return GetButton(ref lockOn, out buttonOffset, startOffset, bufferFrames);
        }

        protected virtual InputButtonData GetButton(ref InputButtonData[] buttonArray, out int buttonOffset, int startOffset = 0, int bufferFrames = 0)
        {
            buttonOffset = startOffset;
            int currentTick = (int)manager.Manager.Simulation.Tick;
            for(int i = 0; i < bufferFrames; i++)
            {
                if (BufferLimit >= currentTick - (bufferFrames + i))
                {
                    break;
                }

                if (buttonArray[(currentTick - (bufferFrames + i)) % inputCapacity].firstPress)
                {
                    buttonOffset = startOffset + i;
                    return buttonArray[(currentTick - (bufferFrames + i)) % inputCapacity];
                }
            }
            return buttonArray[(currentTick - startOffset) % inputCapacity];
        }
    }
}