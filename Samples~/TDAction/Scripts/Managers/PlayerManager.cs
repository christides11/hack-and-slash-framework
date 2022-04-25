using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HnSF.Sample.TDAction
{
    public class PlayerManager : SimulationBehaviour
    {
        public PlayerInput playerInput;

        public Vector2 movement;

        public FighterManager player;


        public override void SimUpdate()
        {
            player.InputManager.FeedInput(Manager.Simulation.Tick, new PlayerInputData()
            {
                dash = false,
                jump = false,
                lightAtk = false,
                lockOn = false,
                movement = movement
            });
        }

        private void OnMove(InputValue movementValue)
        {
            movement = movementValue.Get<Vector2>();
        }
    }
}