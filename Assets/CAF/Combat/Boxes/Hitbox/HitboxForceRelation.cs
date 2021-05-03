namespace CAF.Combat
{
    public enum HitboxForceRelation
    {
        ATTACKER = 0, // Forces relative to the attacker's forward.
        HITBOX = 1, // Forces relative to the hitbox's forward.
        WORLD = 2 // Forces relative to the world's forward.
    }
}