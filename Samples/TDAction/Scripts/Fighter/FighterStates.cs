using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Fighter
{
    public enum FighterStates
    {
        IDLE = 0,
        WALK = 1,
        DASH = 2,
        RUN = 3,
        JUMP_SQUAT = 4,
        JUMP = 5,
        AIR_JUMP = 6,
        AIR_DASH = 7,
        FALL = 8,
        ATTACK = 9,
        FLINCH_GROUND = 10,
        FLINCH_AIR = 11,
        TUMBLE = 12,
        KNOCKDOWN = 13,
        ENEMY_STEP = 14
    }
}