using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HnSF.Sample.TDAction
{
    public class PlayerManager : SimulationBehaviour
    {
        public FighterManager player;

        private PlayerControls playerControls;
        public InputAction movement;

        public bool jump;

        private void Awake()
        {
            playerControls = new PlayerControls();
            movement = playerControls.Gameplay.Move;
            movement.Enable();

            playerControls.Gameplay.Jump.performed += (a) => { jump = true; };
            playerControls.Gameplay.Jump.canceled += (a) => { jump = false; };
            playerControls.Gameplay.Jump.Enable();
        }
        

        public override void SimUpdate()
        {
            player.InputManager.FeedInput(Manager.Simulation.Tick, new PlayerInputData()
            {
                attack = false,
                special = false,
                shoot = false,
                unique = false,
                jump = jump,
                lockOn = false,
                dash = false,
                taunt = false,
                movement = Vector2.zero
            });
        }
    }
}