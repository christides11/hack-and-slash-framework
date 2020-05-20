using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Combat
{
    public enum HitReactionType
    {
        Hit = 0, // Hit was successful.
        Avoided = 1, // Hit was avoided.
        Blocked = 2, // Hit was blocked.
        Armored = 3 // Hit was armored.
    }
}