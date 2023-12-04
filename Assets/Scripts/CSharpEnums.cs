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
    grassCollide,
    waterCollide,
    fireCollide, // a.k.a. magma eject
    slime,
    slimeCollect, // a.k.a. water collect
    naturalHeal, // a.k.a. grass collect
    magmaCollect, // a.k.a. fire collect
    dodge,
}

// Sustained Effect ID
public enum EffectId
{
    poison,
    dizziness,
    mud,
}