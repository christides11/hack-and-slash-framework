using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Fighters
{
    public interface IFighterPhysicsManager
    {
        bool IsGrounded { get; }

        void Tick();
        void Freeze();
        void ResetForces();
        void CheckIfGrounded();
        void SetGrounded(bool value);
    }
}