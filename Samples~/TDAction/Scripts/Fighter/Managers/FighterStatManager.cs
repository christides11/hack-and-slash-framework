using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Sample.TDAction
{
    public class FighterStatManager : MonoBehaviour
    {
        [Header("Ground")] 
        public float groundTraction;

        [Header("Aerial")] 
        public float aerialTraction;
        public float maxFallSpeed;
        public float gravity;
    }
}