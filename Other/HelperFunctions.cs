using UnityEngine;

namespace HnSF
{
    /// <summary>
    /// Random functions with different purposes.
    /// </summary>
    public static class HelperFunctions
    {
        /// <summary>
        /// Takes the given stick direction and converts it to an 8-way direction.
        /// </summary>
        /// <param name="stick">The direction of the stick.</param>
        /// <param name="zone">How big the axis deadzones are.</param>
        /// <returns>The converted direction vector.</returns>
        public static Vector2Int GetStickDir(Vector2 stick, float zone)
        {
            Vector2Int o = new Vector2Int(Mathf.Abs(stick.x) > zone ? signum(stick.x) : 0,
                Mathf.Abs(stick.y) > zone ? signum(stick.y) : 0);
            return o;
        }

        /// <summary>
        /// Get the signum of the given value.
        /// </summary>
        /// <param name="v">The value to convert.</param>
        /// <returns>1 if the value is more than zero, -1 otherwise.</returns>
        public static int signum(float v)
        {
            return v > 0 ? 1 : -1;
        }
    }
}