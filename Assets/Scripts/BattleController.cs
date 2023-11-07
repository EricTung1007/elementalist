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

    public Effect(EffectId effectId, int hp, int duration)
    {
        this.effectId = effectId;
        this.duration = duration;
        this.hp = hp;
    }
}

public class Spell
{
    public SpellId spellId;
    public int hp;
    public int duration;
    public int cooldown;
    public int cdRemain;

    public Spell(SpellId spellId, int hp, int duration, int cooldown)
    {
        this.spellId = spellId;
        this.hp = hp;
        this.duration = duration;
        this.cooldown = cooldown;
    }
}

public class Player
{
    public string playerId;
    public int position; // -1: dead, no display, 0: player, 1,2,3: enemy
    private int hp;
    public int maxhp;
    public Type type;
    public int chi;
    public int intention = -1; // intentionId = skill[intention].spellId
    public int releaseIn = 999; // cooldown remaining seconds, total cooldown is skill[intention].cooldown

    public List<Effect> sustainedEffect;
    public List<Spell> skill;

    public Player(string playerId, int maxhp, Type type, int position)
    {
        this.playerId = playerId;
        this.hp = maxhp;
        this.maxhp = maxhp;
        this.type = type;
        this.position = position;

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
    public void AddSustainedEffect(Effect effect)
    {
        UnityEngine.Debug.Log("Effect " + effect.effectId + " applied on " + playerId + "  HP: " + effect.hp + "  Dur: " + effect.duration);
        sustainedEffect.Add(effect);
    }
}

public class BattleController : MonoBehaviour
{
    public List<Player> players = new List<Player>();
    private List<Spell> queuedSpells = new List<Spell>();

    private UnityEvent<SpellId> slimeEvent;

    private int fixedUpdateCount = 0;

    private int aliveEnemies = 0;

    [Serialize] 

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
                foreach (Player player in players)
                {
                    if (player.position == 1)
                    {
                        PerformSpell(mySpell, players[0], player);
                        break;
                    }
                }

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
                targetedPlayer.AddSustainedEffect(new Effect(EffectId.burn, spell.duration, spell.hp));
                if (targetedPlayer.IsUnderEffect(EffectId.burnThorns) != null)
                    releasedBy.AddSustainedEffect(new Effect(EffectId.burn, 1, 10));
                break;
            case SpellId.acidBomb:
                if (targetedPlayer.type == Type.grass)
                    break;
                targetedPlayer.AddSustainedEffect(new Effect(EffectId.poison, 1, 3));
                if (targetedPlayer.IsUnderEffect(EffectId.burnThorns) != null)
                    releasedBy.AddSustainedEffect(new Effect(EffectId.burn, 1, 2));
                break;
            case SpellId.steamExplosion:
                targetedPlayer.Damage(2);
                //targetedPlayer.AddSustainedEffect(new Effect(EffectId.basicDamage, 2, 0));
                targetedPlayer.AddSustainedEffect(new Effect(EffectId.dizziness, 0, 3));
                if (targetedPlayer.IsUnderEffect(EffectId.burnThorns) != null)
                    releasedBy.AddSustainedEffect(new Effect(EffectId.burn, 1, 2));
                break;
            case SpellId.vinePull:
                for (int i = 1; i < players.Count; i++)
                {
                    if (players[i].position > 0)
                    {
                        if (players[i].position == aliveEnemies)
                            players[i].position = 1; // pull to the front;
                        else
                            players[i].position++;
                    }
                }
                break;
            case SpellId.transformMud:
                releasedBy.AddSustainedEffect(new Effect(EffectId.physicalAttackImmunity, 0, 5));
                break;
            case SpellId.burningShield:
                releasedBy.AddSustainedEffect(new Effect(EffectId.shield, 2, 7));
                releasedBy.AddSustainedEffect(new Effect(EffectId.burnThorns, 0, 2));
                break;
            case SpellId.heal:
                releasedBy.AddSustainedEffect(new Effect(EffectId.regenerate, spell.duration, spell.hp));
                break;
            case SpellId.elementSurge:
                for (int i = 1; i < players.Count; i++)
                {
                    players[i].Damage(spell.hp);
                    players[i].AddSustainedEffect(new Effect(EffectId.burn, 1, 3));
                    players[i].AddSustainedEffect(new Effect(EffectId.poison, 1, 3));
                }
                break;
            case SpellId.collide:
                targetedPlayer.Damage(spell.hp);
                break;
            case SpellId.miniHeal:
                if (releasedBy.GetHP() / releasedBy.maxhp <= 1.0 / 3.0)
                {
                    releasedBy.AddSustainedEffect(new Effect(EffectId.regenerate, 1, 3));
                    // can only release once
                    releasedBy.skill.RemoveAll(spell => spell.spellId == SpellId.miniHeal);
                    return false;
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

        players.Add(new Player("player", 50, Type.none, 0));
        players.Last().skill.Add(new Spell(SpellId.fireArrow, 1, 4, 0));
        players.Last().skill.Add(new Spell(SpellId.acidBomb, 1, 4, 0));
        players.Last().skill.Add(new Spell(SpellId.steamExplosion, 2, 3, 0));
        players.Last().skill.Add(new Spell(SpellId.vinePull, 0, 0, 0));
        players.Last().skill.Add(new Spell(SpellId.transformMud, 0, 10, 0));
        players.Last().skill.Add(new Spell(SpellId.burningShield, 3, 7, 0));
        players.Last().skill.Add(new Spell(SpellId.heal, 1, 5, 0));
        players.Last().skill.Add(new Spell(SpellId.elementSurge, 0, 20, 0));

        players.Add(new Player("green_slime", 12, Type.grass, 1));
        players.Last().skill.Add(new Spell(SpellId.collide, 2, 0, 10));
        players.Last().skill.Add(new Spell(SpellId.miniHeal, 0, 3, 12));

        players.Add(new Player("blue_slime", 10, Type.water, 2));
        players.Last().skill.Add(new Spell(SpellId.collide, 2, 0, 10));
        players.Last().skill.Add(new Spell(SpellId.slime, 1, 0, 10));

        players.Add(new Player("red_slime", 8, Type.fire, 3));
        players.Last().skill.Add(new Spell(SpellId.collide, 1, 0, 6));
        players.Last().skill.Add(new Spell(SpellId.magmaBomb, 1, 6, 20));
        players.Last().skill.Add(new Spell(SpellId.dodge, 0, 0, 3));

        aliveEnemies = 3;
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
            if (player == players[0])
            {

            }
            else
            {
                if (player.position < 0) continue;
                if (player.intention >= 0)
                {
                    player.releaseIn--;

                    if (player.releaseIn <= 0)
                    {
                        bool performed = PerformSpell(player.skill[player.intention], player, players[0]);
                        if (performed)
                        {
                            player.intention = UnityEngine.Random.Range(0, player.skill.Count - 1);
                            player.releaseIn = player.skill[player.intention].cooldown;
                        }
                        else
                        {
                            player.intention = UnityEngine.Random.Range(0, player.skill.Count - 1);
                            player.releaseIn = player.skill[player.intention].cooldown;
                            //player.releaseIn++;
                        }
                    }
                }
                else
                {
                    player.intention = UnityEngine.Random.Range(0, player.skill.Count - 1);
                    player.releaseIn = player.skill[player.intention].cooldown;
                }

            }
        }

        foreach (Player player in players)
        {
            if (player.GetHP() <= 0 && player.position >= 0)
            {
                if (player == players[0])
                {
                    // player dead
                }
                else
                {
                    int deadEnemyPosition = player.position;
                    player.position = -1;
                    for (int i = 1; i < players.Count; i++)
                    {
                        if (players[i].position > deadEnemyPosition)
                            players[i].position--;
                    }
                    aliveEnemies--;

                    if (aliveEnemies == 0)
                    {
                        // win
                    }
                }
            }
        }
        //players.RemoveAll(player => player.playerId != "player" && player.GetHP() <= 0);


        string s = "Brief: HP: ";
        foreach (Player player in players)
            s += player.GetHP() + " / ";
        s += " POS: ";
        foreach (Player player in players)
            s += player.position + " / ";

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
