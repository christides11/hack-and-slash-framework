using CAF.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Entities
{
    public class EntityInputManager : MonoBehaviour
    {
        [SerializeField] protected EntityController controller;

        public InputControlType ControlType { get; protected set; } = InputControlType.None;
        public List<InputRecordItem> InputRecord { get; protected set; } = new List<InputRecordItem>();
        
        protected int inputRecordMaxSize = 600; //60 = second
        protected int controllerID = -1;

        public virtual void Awake()
        {
        }

        public virtual void SetController(int controllerID)
        {
            this.controllerID = controllerID;
        }

        public virtual void SetControlType(InputControlType controlType)
        {
            ControlType = controlType;
        }

        public virtual void Tick()
        {
            switch (ControlType) {
                case InputControlType.Direct:
                    GetInputs();
                    if (InputRecord.Count > 1)
                    {
                        ProcessInputs();
                    }
                    break;
            }
        }

        /// <summary>
        /// Get the controller's inputs this frame and add them to the input record.
        /// </summary>
        protected virtual void GetInputs()
        {
        }

        /// <summary>
        /// Update the current frame's inputs based on the last frame's inputs.
        /// </summary>
        protected virtual void ProcessInputs()
        {
            foreach (var r in InputRecord[InputRecord.Count - 1].inputs)
            {
                r.Value.Process(InputRecord[InputRecord.Count - 2].inputs[r.Key]);
            }
            if (InputRecord.Count > inputRecordMaxSize)
            {
                InputRecord.RemoveAt(0);
            }
        }

        /// <summary>
        /// Get the axis at the offset given.
        /// </summary>
        /// <param name="axis">The ID of the axis.</param>
        /// <param name="frameOffset">The frame offset to get the axis at.</param>
        /// <returns>The axis.</returns>
        public virtual float GetAxis(int axis, int frameOffset = 0)
        {
            if (InputRecord.Count == 0 || (InputRecord.Count - 1) <= frameOffset)
            {
                return 0;
            }
            return ((InputRecordAxis)InputRecord[(InputRecord.Count - 1) - frameOffset].inputs[axis]).axis;
        }

        /// <summary>
        /// Get the 2D axis at the offset given.
        /// </summary>
        /// <param name="axis2DID">The ID of the 2D axis.</param>
        /// <param name="frameOffset">The frame offset to get the 2D axis at.</param>
        /// <returns>The 2D axis.</returns>
        public virtual Vector2 GetAxis2D(int axis2DID, int frameOffset = 0)
        {
            if (InputRecord.Count == 0 || (InputRecord.Count - 1) <= frameOffset)
            {
                return Vector2.zero;
            }
            return ((InputRecordAxis2D)InputRecord[(InputRecord.Count - 1) - frameOffset].inputs[axis2DID]).axis2D;
        }

        /// <summary>
        /// Get the status of the button given. If the buffer is being checked, 
        /// the button returned will be from when it was first pressed within the window, if it exist.
        /// Otherwise, the button at the offset is given.
        /// </summary>
        /// <param name="buttonID">The ID of the button to get.</param>
        /// <param name="frameOffset">The offset from the current frame to check.</param>
        /// <param name="checkBuffer">If the buffer should be checked.</param>
        /// <param name="bufferFrames">How many frames to check for buffer.</param>
        /// <returns>The button and the information about it on the frame.</returns>
        public virtual InputRecordButton GetButton(int buttonID, int frameOffset = 0, bool checkBuffer = false, int bufferFrames = 3)
        {
            return GetButton(buttonID, out int go, frameOffset, checkBuffer, bufferFrames);
        }

        /// <summary>
        /// Get the status of the button given. If the buffer is being checked, 
        /// the button returned will be from when it was first pressed within the window, if it exist.
        /// Otherwise, the button at the offset is given.
        /// </summary>
        /// <param name="buttonID">The ID of the button to get.</param>
        /// <param name="frameOffset">The offset from the current frame to check.</param>
        /// <param name="checkBuffer">If the buffer should be checked.</param>
        /// <param name="bufferFrames">How many frames to check for buffer.</param>
        /// <returns>The button and the information about it on the frame.</returns>
        public virtual InputRecordButton GetButton(int buttonID, out int gotOffset, int frameOffset = 0, bool checkBuffer = false, int bufferFrames = 3)
        {
            gotOffset = 0;
            if (InputRecord.Count == 0)
            {
                return new InputRecordButton();
            }

            if (checkBuffer && (InputRecord.Count - 1) >= (frameOffset + bufferFrames))
            {
                for (int i = 0; i < bufferFrames; i++)
                {
                    InputRecordButton b = ((InputRecordButton)InputRecord[(InputRecord.Count - 1) - (frameOffset + bufferFrames)].inputs[buttonID]);
                    //Can't go further, already used buffer past here.
                    if (b.usedInBuffer)
                    {
                        break;
                    }
                    if (b.firstPress)
                    {
                        gotOffset = i;
                        return b;
                    }
                }
            }
            return (InputRecordButton)InputRecord[(InputRecord.Count - 1) - frameOffset].inputs[buttonID];
        }

        /// <summary>
        /// Clear every input's buffer.
        /// </summary>
        public virtual void ClearBuffer()
        {
            foreach (var r in InputRecord[InputRecord.Count - 1].inputs)
            {
                r.Value.UsedInBuffer();
            }
        }

        /// <summary>
        /// Clear a specific buffer.
        /// </summary>
        /// <param name="inputID">The input to clear the buffer for.</param>
        public virtual void ClearBuffer(int inputID)
        {
            InputRecord[InputRecord.Count - 1].inputs[inputID].UsedInBuffer();
        }
    }
}