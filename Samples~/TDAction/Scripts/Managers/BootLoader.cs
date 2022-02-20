using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Sample.TDAction
{
    public class BootLoader : MonoBehaviour
    {
        public GameManager gameManager;
        
        private void Awake()
        {
            gameManager.Initialize();
        }
    }
}