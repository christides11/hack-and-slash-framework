using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Sample.TDAction
{
    public class GameManager : MonoBehaviour
    {
        public SimulationManager Simulation { get; } = new SimulationManager();

        public FighterManager fighterPrefab;
        
        public Transform playerSpawn;
        public Transform[] cpuSpawns;

        public FighterManager[] players;
        public FighterManager[] cpus;

        private bool initialized = false;
        
        public void Initialize()
        {
            Simulation.gameManager = this;
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
        }

        private void SetupPlayer()
        {
            players[0] = GameObject.Instantiate(fighterPrefab, playerSpawn.position, Quaternion.identity);
            players[0].name = $"P0.{players[0].name}";
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