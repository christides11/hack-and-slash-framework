using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Sample.TDAction.State
{
    public class LocalPlayerManager : MonoBehaviour
    {
        public int maxLocalPlayers = 4;
        public List<LocalPlayerData> localPlayers = new List<LocalPlayerData>();

        public CameraLayoutDefinition[] playerCameraLayouts = new CameraLayoutDefinition[4];
        
        public bool AddPlayer()
        {
            if (localPlayers.Count == maxLocalPlayers) return false;
            
            
            return true;
        }

        public bool RemovePlayer(int playerID)
        {
            return true;
        }
        
        
    }
}