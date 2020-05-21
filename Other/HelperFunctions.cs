using UnityEngine;

namespace CAF
{
    public static class HelperFunctions
    {
        public static Vector2Int GetStickDir(Vector2 stick, float zone)
        {
            Vector2Int o = new Vector2Int(Mathf.Abs(stick.x) > zone ? signum(stick.x) : 0,
                Mathf.Abs(stick.y) > zone ? signum(stick.y) : 0);
            return o;
        }

        public static int signum(float v)
        {
            return v > 0 ? 1 : -1;
        }
    }
}