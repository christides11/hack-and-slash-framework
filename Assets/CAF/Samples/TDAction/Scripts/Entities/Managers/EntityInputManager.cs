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

            InputRecord.Add(recordItem);
        }
    }
}