using UnityEngine;

namespace CAF.Input
{
    [System.Serializable]
    public class InputDefinition
    {
        public int buttonID;

        //Stick
        public Vector2 stickDirection;
        public float directionDeviation = 1.0f; // Directions are compared using dot product.
    }
}