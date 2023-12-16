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
    waterBall,
    woodenArrow,
    firePillar,
    poisonBomb,
    steamExplosion,
    vinePull,
    transformMud,
    heal,
    tieUp,  // 200% damage
    elementClear,

    grassCollide,
    waterCollide,
    fireCollide, // a.k.a. magma eject
    slime,
    slimeCollect, // a.k.a. water collect
    naturalHeal, // a.k.a. grass collect
    healAllEnemy, // green
    magmaCollect, // a.k.a. fire collect
    dodge,        // red
}

// Sustained Effect ID
public enum EffectId
{
    poison,
    dizziness,
    mud,
    tiedUp
}