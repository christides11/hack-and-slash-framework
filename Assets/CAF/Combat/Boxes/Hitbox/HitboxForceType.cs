namespace CAF.Combat
{
    public enum HitboxForceType
    {
        SET = 0, // Force is set and based on a given forward direction
        PUSH = 1, // Force pushes away from a given center. The farther from the center, the less the force.
        PULL = 2 // Force pulls towards a given center. The farther from the center, the more the force.
    }
}