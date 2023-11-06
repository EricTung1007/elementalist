using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Effect
{
    public EffectId effectId;
    public int duration;
    public int hp;

    public Effect(EffectId effectId, int duration, int damage)
    {
        this.effectId = effectId;
        this.duration = duration;
        this.hp = damage;
    }
}

public class Spell
{
    public SpellId spellId;
    public int damage;
    public int duration;
    public int level;
    public string releasedBy;
    public string targetedPlayer;
}

public class Player
{
    public string playerId;
    public string playerName;
    private int hp;
    private int maxhp;
    public Type type;

    public List<Effect> sustainedEffect;


    public Effect IsUnderEffect(EffectId effectId)
    {
        foreach (Effect effect in sustainedEffect)
            if (effect.effectId == effectId) return effect;
        return null;
    }

    public int GetHP() { return hp; }
    public void Damage(int damage)
    {
        if (IsUnderEffect(EffectId.physicalAttackImmunity) != null)
            return;
        Effect effect;
        while (damage > 0 && (effect = IsUnderEffect(EffectId.shield)) != null) // use up shield effect
        {
            int block = Math.Min(damage, effect.hp);
            damage -= block; // reduce damage
            effect.hp -= block;
            if (effect.hp == 0) { sustainedEffect.Remove(effect); }
        }
        hp -= damage;
    }

    public void Regenerate(int hp)
    {
        this.hp += hp;
    }
    public void AddSustainedEffect(Effect effect) { sustainedEffect.Add(effect); }
}

public class BattleController : MonoBehaviour
{
    private List<Player> players = new List<Player>();
    private List<Spell> queuedSpells = new List<Spell>();

    private int fixedUpdateCount = 0;
    private int battleInterval = 100;

    // Start is called before the first frame update
    void Start()
    {
        InitBattle();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if ((fixedUpdateCount % battleInterval == 0))
        {
            BattleRound();
        }

        fixedUpdateCount++;
    }


    public void ReleaseSpellId(SpellId spell)
    {
        UnityEngine.Debug.Log("Spell released");
    }

    public void ReleaseSpell(Spell spell)
    {
        queuedSpells.Add(spell);
    }
    void InitBattle()
    {

    }

    void ProcessSustainedEffects()
    {
        foreach (Player player in players)
        {
            foreach (Effect effect in player.sustainedEffect)
            {
                switch (effect.effectId)
                {
                    case EffectId.burn:
                        goto case EffectId.basicDamage;
                    case EffectId.poison:
                        goto case EffectId.basicDamage;
                    case EffectId.basicDamage:
                        player.Damage(effect.hp);
                        break;
                    case EffectId.regenerate:
                        player.Regenerate(effect.hp);
                        break;
                }
                effect.duration--;
            }
            player.sustainedEffect.RemoveAll(effect => effect.duration <= 0);
        }
    }

    void ProcessSpells()
    {
        foreach (Spell spell in queuedSpells)
        {
            Player releasedBy = players[0], targetedPlayer = players[0];
            foreach (Player player in players)
            {
                if (player.playerId == spell.releasedBy)
                    releasedBy = player;
                if (player.playerId == spell.targetedPlayer)
                    targetedPlayer = player;
            }

            switch (spell.spellId)
            {
                case SpellId.fireArrow:
                    if (targetedPlayer.type == Type.fire || targetedPlayer.type == Type.water)
                        break;
                    targetedPlayer.AddSustainedEffect(new Effect(EffectId.burn, 1, 3));
                    if (targetedPlayer.IsUnderEffect(EffectId.burnThorns) != null)
                        releasedBy.AddSustainedEffect(new Effect(EffectId.burn, 1, 2));
                    break;
                case SpellId.acidBomb:
                    if (targetedPlayer.type == Type.grass)
                        break;
                    targetedPlayer.AddSustainedEffect(new Effect(EffectId.poison, 1, 3));
                    if (targetedPlayer.IsUnderEffect(EffectId.burnThorns) != null)
                        releasedBy.AddSustainedEffect(new Effect(EffectId.burn, 1, 2));
                    break;
                case SpellId.steamExplosion:
                    targetedPlayer.AddSustainedEffect(new Effect(EffectId.basicDamage, 2, 0));
                    targetedPlayer.AddSustainedEffect(new Effect(EffectId.dizziness, 0, 3));
                    if (targetedPlayer.IsUnderEffect(EffectId.burnThorns) != null)
                        releasedBy.AddSustainedEffect(new Effect(EffectId.burn, 1, 2));
                    break;
                case SpellId.vinePull:
                    (players[1], players[players.Count - 1]) = (players[players.Count - 1], players[1]);
                    break;
                case SpellId.transformMud:
                    targetedPlayer.AddSustainedEffect(new Effect(EffectId.physicalAttackImmunity, 0, 5));
                    break;
                case SpellId.burningShield:
                    targetedPlayer.AddSustainedEffect(new Effect(EffectId.shield, 2, 7));
                    targetedPlayer.AddSustainedEffect(new Effect(EffectId.burnThorns, 0, 2));
                    break;
                case SpellId.heal:
                    targetedPlayer.AddSustainedEffect(new Effect(EffectId.regenerate, 1, 4));
                    break;
                case SpellId.elementSurge:
                    for (int i = 1; i < players.Count; i++)
                    {
                        players[i].Damage(spell.damage);
                        players[i].AddSustainedEffect(new Effect(EffectId.burn, 1, 3));
                        players[i].AddSustainedEffect(new Effect(EffectId.poison, 1, 3));
                    }
                    break;
            }
        }
    }

    void BattleRound()
    {
        ProcessSustainedEffects();
        ProcessSpells();

        foreach (Player player in players)
        {
            if (player.GetHP() <= 0)
            {
                if (player == players[0])
                {
                    // player dead
                }
                else
                {
                    // enemy dead
                }
            }
        }
        players.RemoveAll(player => player.GetHP() <= 0);
    }


}
