using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Element type ID for entities and skills
public enum Type
{
    none,
    fire,
    water,
    grass,
    goo
}

// Spell ID
public enum SpellId
{
    none,
    fireArrow,
    acidBomb,
    steamExplosion,
    vinePull,
    transformMud,
    burningShield,
    heal,
    elementSurge,
    collide,
    miniHeal,
    slime,
    jichi,
    magmaBomb,
    dodge
}

// Sustained Effect ID
public enum EffectId
{
    burn,
    poison,
    basicDamage,
    dizziness,
    physicalAttackImmunity,
    shield,
    burnThorns,
    regenerate,
}