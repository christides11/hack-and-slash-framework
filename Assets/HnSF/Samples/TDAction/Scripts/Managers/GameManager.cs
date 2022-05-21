using System;
using System.Collections;
using System.Collections.Generic;
using HnSF.Sample.TDAction.State;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

namespace HnSF.Sample.TDAction
{
    public class GameManager : MonoBehaviour
    {
        public SimulationManager Simulation { get; } = new SimulationManager();

        public PlayerManager playerManagerPrefab;
        public FighterManager fighterPrefab;
        public Camera playerCamera;
        public Vector3 cameraOffset;
        
        public Transform playerSpawn;
        public Transform[] cpuSpawns;

        public PlayerManager[] playerManagers;
        public FighterManager[] players;
        public FighterManager[] cpus;

        public LocalPlayerManager localPlayerManager;
        private bool initialized = false;

        public void Initialize()
        {
            Simulation.gameManager = this;
            playerManagers = new PlayerManager[1];
            players = new FighterManager[1];
            cpus = new FighterManager[cpuSpawns.Length];
            SetupPlayer();
            SetupCPUs();
            initialized = true;
            EnableJoining();
        }

        private void EnableJoining()
        {
            InputUser.onUnpairedDeviceUsed += WhenUnpairedDeviceUsed;
            ++InputUser.listenForUnpairedDeviceActivity;
        }

        private void DisableJoining()
        {
            InputUser.onUnpairedDeviceUsed -= WhenUnpairedDeviceUsed;
            --InputUser.listenForUnpairedDeviceActivity;
        }

        private void WhenUnpairedDeviceUsed(InputControl control, InputEventPtr eventPtr)
        {
            //Debug.Log($"Unpaired device used: {control.displayName}");
            
            // Make sure it's a button that was actuated.
            if (!(control is ButtonControl))
                return;
            
            // Make sure it's a device that is usable by the player's actions. We don't want
            // to join a player who's then stranded and has no way to actually interact with the game.
            //if (!IsDeviceUsableWithPlayerActions(control.device))
            //    return;
        }

        private void FixedUpdate()
        {
            if (!initialized) return;
            Simulation.Increment();
        }

        private void SetupPlayer()
        {
            //playerManagers[0] = GameObject.Instantiate(playerManagerPrefab, Vector3.zero, Quaternion.identity);
            //players[0] = GameObject.Instantiate(fighterPrefab, playerSpawn.position, Quaternion.identity);
            //players[0].name = $"P0.{players[0].name}";
            //playerManagers[0].player = players[0];
            //Simulation.RegisterObject(playerManagers[0]);
            //Simulation.RegisterObject(players[0]);
        }
        
        private void SetupCPUs()
        {
            for (int i = 0; i < cpus.Length; i++)
            {
                cpus[i] = GameObject.Instantiate(fighterPrefab, cpuSpawns[i].position, Quaternion.identity);
                cpus[i].name = $"CPU{i}.{cpus[i].name}";
                Simulation.RegisterObject(cpus[i].GetComponent<SimulationObject>());
            }
        }
    }
}