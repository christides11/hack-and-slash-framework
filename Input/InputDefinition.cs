using UnityEngine;

namespace CAF.Input
{
    [System.Serializable]
    public class InputDefinition
    {
        public InputDefinitionType inputType;

        public int buttonID;
        //Stick
        public Vector2 stickDirection;
        public float directionDeviation = 0.9f; // Directions are compared using dot product.
    }

    public enum InputDefinitionType
    {
        Button = 0,
        Stick = 1
    }
}