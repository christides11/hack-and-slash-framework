using HnSF.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Fighters
{
    public class FighterInputManager : MonoBehaviour
    {
        [SerializeField] protected FighterBase manager;

        public FighterControlType ControlType { get; protected set; } = FighterControlType.None;
        public uint inputRecordSize { get; protected set; } = 1024;
        public InputRecordItem[] InputRecord { get; protected set; } = null;
        public uint inputTick { get; protected set; } = 0;
        public uint inputBufferTick { get; protected set; } = 0;

        public virtual void Awake()
        {
            InputRecord = new InputRecordItem[inputRecordSize];
        }

        public virtual void SetControlType(FighterControlType controlType)
        {
            ControlType = controlType;
        }

        public virtual void Tick()
        {
            switch (ControlType) {
                case FighterControlType.Player:
                    GetInputs();
                    ProcessInput();
                    break;
            }
        }

        /// <summary>
        /// Get the controller's inputs this frame and add them to the input record.
        /// </summary>
        protected virtual void GetInputs()
        {
        }

        protected virtual void ProcessInput()
        {
            if(inputTick < 2)
            {
                return;
            }

            foreach (var r in InputRecord[(inputTick - 1) % inputRecordSize].inputs)
            {
                r.Value.Process(InputRecord[(inputTick - 2) % inputRecordSize].inputs[r.Key]);
            }
        }


        /// <summary>
        /// Get the axis at the offset given.
        /// </summary>
        /// <param name="axis">The ID of the axis.</param>
        /// <param name="frameOffset">The frame offset to get the axis at.</param>
        /// <returns>The axis.</returns>
        public virtual float GetAxis(int axis, uint frameOffset = 0)
        {
            if (inputTick == 0 || inputTick < frameOffset)
            {
                return 0;
            }
            return ((InputRecordAxis)InputRecord[(inputTick - 1 - frameOffset) % inputRecordSize].inputs[axis]).axis;
        }

        /// <summary>
        /// Get the 2D axis at the offset given.
        /// </summary>
        /// <param name="axis2DID">The ID of the 2D axis.</param>
        /// <param name="frameOffset">The frame offset to get the 2D axis at.</param>
        /// <returns>The 2D axis.</returns>
        public virtual Vector2 GetAxis2D(int axis2DID, uint frameOffset = 0)
        {
            if (inputTick == 0 || inputTick < frameOffset)
            {
                return Vector2.zero;
            }
            return ((InputRecordAxis2D)InputRecord[(inputTick - 1 - frameOffset) % inputRecordSize].inputs[axis2DID]).axis2D;
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
        public virtual InputRecordButton GetButton(int buttonID, uint frameOffset = 0, bool checkBuffer = false, uint bufferFrames = 3)
        {
            return GetButton(buttonID, out uint go, frameOffset, checkBuffer, bufferFrames);
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
        public virtual InputRecordButton GetButton(int buttonID, out uint gotOffset, uint frameOffset = 0, bool checkBuffer = false, uint bufferFrames = 3)
        {
            gotOffset = frameOffset;
            if (inputTick == 0 || inputTick < frameOffset)
            {
                return new InputRecordButton();
            }
            if (checkBuffer && inputTick - 1 >= (frameOffset + bufferFrames))
            {
                for (uint i = 0; i < bufferFrames; i++)
                {
                    InputRecordButton b = ((InputRecordButton)InputRecord[(inputTick - 1 - (frameOffset + i)) % inputRecordSize].inputs[buttonID]);
                    //Can't go further, already used buffer past here.
                    if (inputBufferTick >= (inputTick - 1 - (frameOffset + i)))
                    {
                        break;
                    }
                    if (b.firstPress)
                    {
                        gotOffset = frameOffset + i;
                        return b;
                    }
                }
            }
            return (InputRecordButton)InputRecord[(inputTick - 1 - frameOffset) % inputRecordSize].inputs[buttonID];
        }

        /// <summary>
        /// Clear a specific buffer.
        /// </summary>
        /// <param name="inputID">The input to clear the buffer for.</param>
        public virtual void ClearBuffer()
        {
            inputBufferTick = inputTick;
        }
    }
}