using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        }

        private void FixedUpdate()
        {
            if (!initialized) return;
            Simulation.Increment();

            if (players.Length > 0)
            {
                playerCamera.transform.position = players[0].transform.position + cameraOffset;
            }
        }

        private void SetupPlayer()
        {
            playerManagers[0] = GameObject.Instantiate(playerManagerPrefab, Vector3.zero, Quaternion.identity);
            players[0] = GameObject.Instantiate(fighterPrefab, playerSpawn.position, Quaternion.identity);
            players[0].name = $"P0.{players[0].name}";
            playerManagers[0].player = players[0];
            Simulation.RegisterObject(playerManagers[0]);
            Simulation.RegisterObject(players[0]);
        }
        
        private void SetupCPUs()
        {
            for (int i = 0; i < cpus.Length; i++)
            {
                cpus[i] = GameObject.Instantiate(fighterPrefab, cpuSpawns[i].position, Quaternion.identity);
                cpus[i].name = $"CPU{i}.{cpus[i].name}";
                Simulation.RegisterObject(cpus[i]);
            }
        }
    }
}