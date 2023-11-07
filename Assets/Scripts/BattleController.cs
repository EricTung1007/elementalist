using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Events;

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
    public int cooldown;
    public int cdRemain;

    public Spell(SpellId spellId, int damage, int duration, int cooldown)
    {
        this.spellId = spellId;
        this.damage = damage;
        this.duration = duration;
        this.cooldown = cooldown;
        this.cdRemain = cooldown;
    }
}

public class Player
{
    public string playerId;
    public int position; // -1: player, 0: first enemy
    private int hp;
    public int maxhp;
    public Type type;
    public int chi;

    public List<Effect> sustainedEffect;
    public List<Spell> skill;

    public Player(string playerId, int maxhp, Type type)
    {
        this.playerId = playerId;
        this.hp = maxhp;
        this.maxhp = maxhp;
        this.type = type;

        sustainedEffect = new List<Effect>();
        skill = new List<Spell>();
    }

    public Effect IsUnderEffect(EffectId effectId)
    {
        foreach (Effect effect in sustainedEffect)
            if (effect.effectId == effectId) return effect;
        return null;
    }

    public int GetHP() { return hp; }

    public void Damage(int damage)
    {
        UnityEngine.Debug.Log("DAMAGE!\n");
        //if (IsUnderEffect(EffectId.physicalAttackImmunity) != null)
        //    return;
        //Effect effect;
        //while (damage > 0 && (effect = IsUnderEffect(EffectId.shield)) != null) // use up shield effect
        //{
        //    int block = Math.Min(damage, effect.hp);
        //    damage -= block; // reduce damage
        //    effect.hp -= block;
        //    if (effect.hp == 0) { sustainedEffect.Remove(effect); }
        //}
        hp -= damage;
    }

    public void Regenerate(int hp)
    {
        this.hp += hp;
    }
    public void AddSustainedEffect(Effect effect)
    {
        UnityEngine.Debug.Log("Effect " + effect.effectId + " applied on " + playerId + "  HP: " + effect.hp + "  Dur: " + effect.duration);
        sustainedEffect.Add(effect);
    }
}

public class BattleController : MonoBehaviour
{
    private List<Player> players = new List<Player>();
    private List<Spell> queuedSpells = new List<Spell>();

    private UnityEvent<SpellId> slimeEvent;

    private int fixedUpdateCount = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        UnityEngine.Debug.Log("Start");
        InitBattle();
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void FixedUpdate()
    {
        if ((fixedUpdateCount % 50 == 0))
        {
            BattleRound();
        }

        fixedUpdateCount++;
    }


    public void ReleaseSpellId(SpellId spell)
    {
        foreach (Spell mySpell in players[0].skill)
        {
            if (mySpell.spellId == spell)
            {
                PerformSpell(mySpell, players[0], players[1]);
                break;
            }
        }
    }

    // return true if performed successfully or false if the spell is held on until criteria meets
    public bool PerformSpell(Spell spell, Player releasedBy, Player targetedPlayer)
    {
        UnityEngine.Debug.Log(spell.spellId + " performed by " + releasedBy.playerId + " at " + targetedPlayer.playerId);
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
                //(players[1], players[players.Count - 1]) = (players[players.Count - 1], players[1]);
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
            case SpellId.collide:
                targetedPlayer.Damage(spell.damage);
                break;
            case SpellId.miniHeal:
                if (releasedBy.GetHP() / releasedBy.maxhp <= 1.0 / 3.0)
                {
                    releasedBy.AddSustainedEffect(new Effect(EffectId.regenerate, 3, 1));
                    // can only release once
                    releasedBy.skill.RemoveAll(spell => spell.spellId == SpellId.miniHeal);
                }
                break;
            case SpellId.jichi:
                releasedBy.chi++;
                break;
            case SpellId.magmaBomb:
                if (releasedBy.chi >= 2)
                {
                    releasedBy.chi--;
                    releasedBy.AddSustainedEffect(new Effect(EffectId.burn, 6, 1));
                    break;
                }
                else return false;
            case SpellId.dodge:
                // todo
                break;
            default:
                UnityEngine.Debug.Log(spell.spellId);
                break;
        }
        return true;
    }

    private void InitBattle()
    {
        UnityEngine.Debug.Log("INIT");

        players.Add(new Player("player", 15, Type.none));
        players.Last().skill.Add(new Spell(SpellId.fireArrow, 1, 4, 0));
        players.Last().skill.Add(new Spell(SpellId.acidBomb, 1, 4, 0));
        players.Last().skill.Add(new Spell(SpellId.steamExplosion, 2, 3, 0));
        players.Last().skill.Add(new Spell(SpellId.vinePull, 0, 0, 0));
        players.Last().skill.Add(new Spell(SpellId.transformMud, 0, 10, 0));
        players.Last().skill.Add(new Spell(SpellId.burningShield, 3, 7, 0));
        players.Last().skill.Add(new Spell(SpellId.heal, 1, 5, 0));
        players.Last().skill.Add(new Spell(SpellId.elementSurge, 0, 20, 0));

        players.Add(new Player("green_slime", 12, Type.grass));
        players.Last().skill.Add(new Spell(SpellId.collide, 2, 0, 10));
        players.Last().skill.Add(new Spell(SpellId.miniHeal, 0, 3, 12));

        players.Add(new Player("blue_slime", 10, Type.water));
        players.Last().skill.Add(new Spell(SpellId.collide, 2, 0, 10));
        players.Last().skill.Add(new Spell(SpellId.slime, 1, 0, 10));

        players.Add(new Player("red_slime", 8, Type.fire));
        players.Last().skill.Add(new Spell(SpellId.collide, 1, 0, 6));
        players.Last().skill.Add(new Spell(SpellId.magmaBomb, 1, 6, 20));
        players.Last().skill.Add(new Spell(SpellId.dodge, 0, 0, 3));

    }

    void ProcessSustainedEffects()
    {
        foreach (Player player in players)
        {
            foreach (Effect effect in player.sustainedEffect)
            {
                UnityEngine.Debug.Log(effect.effectId);
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

    void BattleRound()
    {
        ProcessSustainedEffects();


        foreach (Player player in players)
        {
            foreach (Spell spell in player.skill)
            {
                if (spell.cooldown > 0 && spell.cdRemain == 0)
                {
                    if (player == players[0])
                    {
                        bool performed = PerformSpell(spell, players[0], players[1]);
                        if (!performed) continue;
                    }
                    else
                    {
                        bool performed = PerformSpell(spell, player, players[0]);
                        if (!performed) continue;
                    }
                    spell.cdRemain = spell.cooldown;
                }

                spell.cdRemain--;
            }



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
        //players.RemoveAll(player => player.GetHP() <= 0);

        string s = "";
        foreach (Player player in players)
            s += player.GetHP() + " / ";
        s += '\n';
        foreach (Player player in players)
        {
            s += "Player " + player.playerId + "  health: " + player.GetHP() + "\n";
            s += "Effects:\n";
            foreach (Effect effect in player.sustainedEffect)
            {
                s += "  " + effect.effectId + "  hp: " + effect.hp + "  duration: " + effect.duration + "\n";
            }
            s += "Skills:\n";
            foreach (Spell spell in player.skill)
            {
                s += "  " + spell.spellId + "  CD: " + spell.cdRemain + "/" + spell.cooldown + "\n";
            }
        }
        UnityEngine.Debug.Log(s);

    }


}
