using UnityEngine;
using UnityEngine.Serialization;

namespace HnSF.Sample.TDAction
{
    public class FighterInputManager : MonoBehaviour {
        public static int inputCapacity = 40;
        public int BufferLimit { get; set; } = 0;

        [SerializeField] private FighterManager manager;
        
        [HideInInspector] public Vector2[] movement = new Vector2[inputCapacity];
        [FormerlySerializedAs("lightAtk")] [HideInInspector] public bool[] attack = new bool[inputCapacity];
        [HideInInspector] public bool[] special = new bool[inputCapacity];
        [HideInInspector] public bool[] shoot = new bool[inputCapacity];
        [HideInInspector] public bool[] unique = new bool[inputCapacity];
        [HideInInspector] public bool[] jump = new bool[inputCapacity];
        [HideInInspector] public bool[] lockOn = new bool[inputCapacity];
        [HideInInspector] public bool[] dash = new bool[inputCapacity];
        [HideInInspector] public bool[] taunt = new bool[inputCapacity];

        public uint attackHeld;
        public uint specialHeld;
        public uint shootHeld;
        public uint uniqueHeld;
        public uint jumpHeld;
        public uint lockOnHeld;
        public uint dashHeld;
        public uint tauntHeld;

        public void FeedInput(int frame, PlayerInputData inputData)
        {
            if(frame == 0) return;
            movement[frame % inputCapacity] = inputData.movement;
            FeedButton(frame, inputData.attack, ref attack, ref attackHeld);
            FeedButton(frame, inputData.special, ref special, ref specialHeld);
            FeedButton(frame, inputData.shoot, ref shoot, ref shootHeld);
            FeedButton(frame, inputData.unique, ref unique, ref uniqueHeld);
            FeedButton(frame, inputData.jump, ref jump, ref jumpHeld);
            FeedButton(frame, inputData.lockOn, ref lockOn, ref lockOnHeld);
            FeedButton(frame, inputData.dash, ref dash, ref dashHeld);
            FeedButton(frame, inputData.taunt, ref taunt, ref tauntHeld);
        }

        protected virtual void FeedButton(int frame, bool result, ref bool[] buttonArray, ref uint heldTime)
        {
            buttonArray[frame % inputCapacity] = result;
            if (!result)
            {
                heldTime = 0;
                return;
            }

            if (heldTime == uint.MaxValue) return;
            heldTime++;
        }
        
        public virtual Vector2 GetMovement(int startOffset = 0)
        {
            return movement[(manager.Manager.Simulation.Tick - startOffset) % inputCapacity];
        }
        
        public virtual InputButtonData GetLight(out int buttonOffset, int startOffset = 0, int bufferFrames = 0)
        {
            return GetButton(ref attack, out buttonOffset, startOffset, bufferFrames);
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
        
        protected virtual InputButtonData GetButton(ref bool[] buttonArray, out int buttonOffset, int startOffset = 0, int bufferFrames = 0)
        {
            buttonOffset = startOffset;
            int inputTick = manager.Manager.Simulation.Tick - startOffset;
            for(int i = 0; i < bufferFrames; i++)
            {
                if (BufferLimit >= inputTick - i)
                {
                    break;
                }
                
                if (!IsFirstPress(ref buttonArray, inputTick - i)) continue;
                buttonOffset = startOffset + i;
                return new InputButtonData(){ firstPress = true, isDown = true, released = false};
            }

            return CreateButtonData(ref buttonArray, inputTick);
        }

        protected virtual InputButtonData CreateButtonData(ref bool[] buttonArray, int frame)
        {
            bool isFirstPress = IsFirstPress(ref buttonArray, frame);
            return new InputButtonData()
            {
                firstPress = isFirstPress, 
                isDown = isFirstPress || IsDown(ref buttonArray, frame),
                released = !isFirstPress && IsRelease(ref buttonArray, frame)
            };
        }

        protected virtual bool IsFirstPress(ref bool[] buttonArray, int frame)
        {
            if (buttonArray[frame % inputCapacity] && !buttonArray[(frame - 1) % inputCapacity]) return true;
            return false;
        }
        
        protected virtual bool IsRelease(ref bool[] buttonArray, int frame)
        {
            if (!buttonArray[frame % inputCapacity] && buttonArray[(frame - 1) % inputCapacity]) return true;
            return false;
        }
        
        protected virtual bool IsDown(ref bool[] buttonArray, int frame)
        {
            if (buttonArray[frame % inputCapacity]) return true;
            return false;
        }
    }
}