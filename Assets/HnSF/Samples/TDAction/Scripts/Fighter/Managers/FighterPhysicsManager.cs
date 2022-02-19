using System.Collections;
using System.Collections.Generic;
using HnSF.Fighters;
using UnityEngine;

namespace HnSF.Sample.TDAction
{
    public class FighterPhysicsManager : MonoBehaviour, IFighterPhysicsManager
    {
        public bool IsGrounded { get; } = false;
        
        public void Tick()
        {
            throw new System.NotImplementedException();
        }

        public void Freeze()
        {
            throw new System.NotImplementedException();
        }

        public void ResetForces()
        {
            throw new System.NotImplementedException();
        }

        public void CheckIfGrounded()
        {
            throw new System.NotImplementedException();
        }

        public void SetGrounded(bool value)
        {
            throw new System.NotImplementedException();
        }
    }
}