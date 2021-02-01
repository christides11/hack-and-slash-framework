using CAF.Input;
using System.Collections;
using System.Collections.Generic;
using TDAction.Inputs;
using UnityEngine;

namespace TDAction.Entities
{
    public class EntityInputManager : CAF.Entities.EntityInputManager
    {

        protected override void GetInputs()
        {
            InputRecordItem recordItem = new InputRecordItem();

            recordItem.AddInput((int)EntityInputs.MOVEMENT,
                new InputRecordAxis2D(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))));
            recordItem.AddInput((int)EntityInputs.JUMP,
                new InputRecordButton(Input.GetKey(KeyCode.K)));
            recordItem.AddInput((int)EntityInputs.DASH,
                new InputRecordButton(Input.GetKey(KeyCode.LeftShift)));
            recordItem.AddInput((int)EntityInputs.ATTACK,
                new InputRecordButton(Input.GetKey(KeyCode.I)));

            InputRecord.Add(recordItem);
        }
    }
}