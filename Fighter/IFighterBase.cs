using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Fighters
{
    public interface IFighterBase
    {
        void Tick();
        void LateTick();
        void SetTargetable(bool value);
        Vector3 GetMovementVector(float horizontal, float vertical);
        Vector3 GetVisualBasedDirection(Vector3 direction);
        void RotateVisual(Vector3 direction, float speed);
        void SetVisualRotation(Vector3 direction);
        GameObject GetGameObject();
    }
}