using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetChoice
{
    NONE,
    ONE_ENEMY,
    ALL_ENEMIES,
    ONE_ALLY,
    ALL_ALLIES,
    ONE_DEAD_ALLY,
    ALL_DEAD_ALLIES,
    USER,
    ALL
}
public enum Element
{
    NONE,
    PHYSICAL,
    FIRE,
    ICE,
    ELECTRICITY,
    WIND,
    LIGHT,
    DARK,
    ALMIGHTY
}
public enum SkillType
{
    NONE,
    HP_DAMAGE,
    MP_DAMAGE,
    HP_RECOVER,
    HP_RECOVER_PERCENT,
    MP_RECOVER,
    MP_RECOVER_PERCENT,
    HP_DRAIN,
    MP_DRAIN
}
public enum Restriction
{
    NONE,
    CANNOT_MOVE,
    ATTACK_ENEMY,
    ATTACK_ANYONE,
    ATTACK_ALLY
}
public enum ElementRelation
{
    NORMAL,
    WEAK,
    STRONG,
    NULL,
    REPEL,
    ABSORB
}
public enum AutoRemainingTime
{
    NONE,
    ACTION_END,
    TURN_END
}

public enum GameState
{
    START_SIDE,
    START_TURN,
    END_SIDE,
    END_TURN
}