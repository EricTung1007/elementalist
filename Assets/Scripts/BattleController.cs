using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
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
    public int chi; // could be slime points
    public int intention = -1; // intentionId = skill[intention].spellId
    public int preparedFor = 0; // intention prepared for seconds
    public bool isHurt = false; // is hurt in the last second

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

    public void DoDamage(int damage)
    {
        //UnityEngine.Debug.Log("DAMAGE!\n");
        hp -= damage;
        if (hp < 0) hp = 0;
        if (damage > 0) isHurt = true;
    }

    public void Regenerate(int hp)
    {
        this.hp += hp;
        if (this.hp > this.maxhp)
            hp = this.maxhp;
    }
    public void AddSustainedEffect(Effect effect)
    {
        UnityEngine.Debug.Log("Effect " + effect.effectId + " applied on " + playerId + "  HP: " + effect.hp + "  Dur: " + effect.duration);
        sustainedEffect.Add(effect);
    }

    public int GetSpellIndex(SpellId spellId)
    {
        return skill.IndexOf(skill.Where(s => s.spellId == spellId)?.First());
    }
}

public class BattleController : MonoBehaviour
{
    public List<Player> players = new List<Player>();
    private List<Spell> queuedSpells = new List<Spell>();

    private UnityEvent<SpellId> slimeEvent;

    private int fixedUpdateCount = 0;

    private int aliveEnemies = 0;

    [SerializeField] private UnityEvent GameWin;
    [SerializeField] private UnityEvent GameLose;
    [SerializeField] private UnityEvent DropSlime;

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

    private float GetMultiplier(Type byType, Type toType)
    {
        if (byType == Type.none || toType == Type.none)
            return 1.0F;
        if (byType == Type.fire || toType == Type.grass)
            return 1.5F;
        if (byType == Type.grass || toType == Type.water)
            return 1.5F;
        if (byType == Type.water || toType == Type.fire)
            return 1.5F;
        if (byType == Type.grass || toType == Type.fire)
            return 0.5F;
        if (byType == Type.water || toType == Type.grass)
            return 0.5F;
        if (byType == Type.fire || toType == Type.water)
            return 0.5F;
        return 1.0F;
    }

    public void ElementCollect(Type elementType)
    {
        SpellId collectionSpell = SpellId.none;
        switch (elementType)
        {
            case Type.fire:
                collectionSpell = SpellId.magmaCollect;
                break;
            case Type.water:
                collectionSpell = SpellId.slimeCollect;
                break;
            case Type.grass:
                collectionSpell = SpellId.naturalHeal;
                break;
        }
        if (collectionSpell != SpellId.none)
        {
            foreach (Player player in players)
                if (player.type == elementType)
                    PerformSpell(new Spell(collectionSpell, 1, 0, 0), player, players[0], true);
        }
    }


    // return true if performed/can perform successfully or false if the spell is held on until criteria meets
    public bool PerformSpell(Spell spell, Player byPlayer, Player toPlayer, bool perform = true)
    {
        UnityEngine.Debug.Log(spell.spellId + " performed by " + byPlayer.playerId + " at " + toPlayer.playerId);
        int enemyCount = players.Where(p => p.position > 0).Count();
        switch (spell.spellId)
        {
            case SpellId.fireArrow:
                if (perform)
                    toPlayer.DoDamage((int)Math.Ceiling(spell.hp * GetMultiplier(Type.fire, toPlayer.type)));
                break;
            case SpellId.waterBall:
                if (perform)
                    toPlayer.DoDamage((int)Math.Ceiling(spell.hp * GetMultiplier(Type.water, toPlayer.type)));
                break;
            case SpellId.woodenArrow:
                if (perform)
                    toPlayer.DoDamage((int)Math.Ceiling(spell.hp * GetMultiplier(Type.grass, toPlayer.type)));
                break;
            case SpellId.firePillar:
                if (perform)
                {
                    if (byPlayer == players[0])
                    {
                        for (int i = 1; i < players.Count; i++)
                            if (players[i].position > 0)
                                players[i].DoDamage((int)Math.Ceiling(spell.hp * GetMultiplier(Type.fire, players[i].type)));
                    }
                    else
                        players[0].DoDamage((int)Math.Ceiling(spell.hp * GetMultiplier(Type.fire, players[0].type)));
                }
                break;
            case SpellId.heal:
                if (perform)
                    byPlayer.Regenerate(spell.hp);
                break;
            case SpellId.poisonBomb:
                if (perform)
                    toPlayer.AddSustainedEffect(new Effect(EffectId.poison, spell.hp, spell.duration));
                break;
            case SpellId.steamExplosion:
                if (perform)
                    toPlayer.AddSustainedEffect(new Effect(EffectId.dizziness, spell.hp, spell.duration));
                break;
            case SpellId.transformMud:
                if (perform)
                    foreach (Player player in players)
                        if (player != byPlayer)
                            player.AddSustainedEffect(new Effect(EffectId.mud, spell.hp, spell.duration));
                break;
            case SpellId.vinePull:
                if (players.Where(p => p.position > 0).Count() <= 1)
                    return false;
                if (perform)
                {
                    foreach (Player player in players)
                    {
                        if (player.position == enemyCount) player.position = 1;
                        else if (player.position > 0) player.position++;
                    }
                }
                break;
            case SpellId.grassCollide:
                if (perform)
                    toPlayer.DoDamage((int)Math.Ceiling(spell.hp * GetMultiplier(Type.grass, toPlayer.type)));
                break;
            case SpellId.waterCollide:
                if (perform)
                    toPlayer.DoDamage((int)Math.Ceiling(spell.hp * GetMultiplier(Type.water, toPlayer.type)));
                break;
            case SpellId.fireCollide:
                if (perform)
                    toPlayer.DoDamage((int)Math.Ceiling(spell.hp * GetMultiplier(Type.fire, toPlayer.type)));
                break;
            case SpellId.slime:
                if (byPlayer.chi < spell.hp)
                    return false;
                if (perform)
                {
                    byPlayer.chi -= spell.hp;
                    UnityEngine.Debug.Log("Slime triggered");
                    DropSlime?.Invoke();
                }
                break;
            case SpellId.dodge:
                if (byPlayer.position != 1)
                    return false;
                if (enemyCount <= 1)
                    return false;
                if (perform)
                {
                    foreach (Player player in players)
                        if (player.position > 1) player.position--;
                    byPlayer.position = enemyCount;
                }
                break;
            case SpellId.slimeCollect:
                if (perform)
                    byPlayer.chi++;
                break;
            case SpellId.naturalHeal:
                if (byPlayer.GetHP() >= byPlayer.maxhp) return false;
                if (perform)
                    byPlayer.Regenerate(spell.hp);
                break;
            case SpellId.magmaCollect:
                if (!byPlayer.skill.Where(s => s.spellId == SpellId.fireCollide).Any())
                    return false;
                if (byPlayer.skill.Where(s => s.spellId == SpellId.fireCollide).First().cooldown <= 1)
                    return false;
                if (perform)
                    byPlayer.skill.Where(s => s.spellId == SpellId.fireCollide).First().cooldown--;
                break;
        }

        return true;
    }

    private void InitBattle()
    {
        UnityEngine.Debug.Log("INIT");

        players.Add(new Player("player", 50, Type.none, 0));
        players.Last().skill.Add(new Spell(SpellId.fireArrow, 4, 0, 0));
        players.Last().skill.Add(new Spell(SpellId.waterBall, 4, 0, 0));
        players.Last().skill.Add(new Spell(SpellId.woodenArrow, 4, 0, 0));
        players.Last().skill.Add(new Spell(SpellId.firePillar, 4, 0, 0));
        players.Last().skill.Add(new Spell(SpellId.heal, 7, 0, 0));
        players.Last().skill.Add(new Spell(SpellId.poisonBomb, 1, 10, 0));
        players.Last().skill.Add(new Spell(SpellId.steamExplosion, 0, 6, 0));
        players.Last().skill.Add(new Spell(SpellId.transformMud, 0, 10, 0));
        players.Last().skill.Add(new Spell(SpellId.vinePull, 0, 0, 0));

        players.Add(new Player("green_slime", 12, Type.grass, 1));
        players.Last().skill.Add(new Spell(SpellId.grassCollide, 6, 0, 10));
        players.Last().skill.Add(new Spell(SpellId.naturalHeal, 1, 0, 0));

        players.Add(new Player("blue_slime", 10, Type.water, 2));
        players.Last().skill.Add(new Spell(SpellId.waterCollide, 4, 0, 5));
        players.Last().skill.Add(new Spell(SpellId.slime, 3, 0, 5));
        players.Last().skill.Add(new Spell(SpellId.slimeCollect, 1, 0, 0));

        players.Add(new Player("red_slime", 8, Type.fire, 3));
        players.Last().skill.Add(new Spell(SpellId.fireCollide, 20, 0, 30));
        players.Last().skill.Add(new Spell(SpellId.dodge, 0, 0, 5));
        players.Last().skill.Add(new Spell(SpellId.magmaCollect, 1, 0, 0));

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
                    case EffectId.poison:
                        player.DoDamage(effect.hp);
                        break;
                }
                effect.duration--;
            }
            player.sustainedEffect.RemoveAll(effect => effect.duration <= 0);
        }
    }

    private void ProcessEnemyBehaviour()
    {
        foreach (Player player in players)
        {
            if (player == players[0]) continue; // not enemy
            if (player.position < 0) continue; // dead

            if (player.IsUnderEffect(EffectId.dizziness) != null)
            {
                player.intention = -1; // no intention for you
                continue;
            }

            if (player.intention >= 0) // intention is determined
            {
                player.preparedFor++;

                if (player.preparedFor >= player.skill[player.intention].cooldown)
                {
                    if (PerformSpell(player.skill[player.intention], player, players[0], false))
                    {
                        PerformSpell(player.skill[player.intention], player, players[0], true);
                        player.intention = -1;
                    }
                    else
                        player.intention = -1;
                }
            }

            if (player.intention < 0) // intention to be determined
            {
                int random = UnityEngine.Random.Range(0, 100);

                switch (player.playerId)
                {
                    case "green_slime":
                        player.intention = player.skill.IndexOf(player.skill.Where(s => s.spellId == SpellId.grassCollide).First());
                        break;
                    case "blue_slime":
                        if (PerformSpell(player.skill[player.GetSpellIndex(SpellId.slime)], player, players[0], false) && random < 25)
                            player.intention = player.GetSpellIndex(SpellId.slime);
                        else
                            player.intention = player.GetSpellIndex(SpellId.waterCollide);
                        break;
                    case "red_slime":
                        if (player.position == 1)
                            player.intention = player.GetSpellIndex(SpellId.dodge);
                        else
                            player.intention = player.GetSpellIndex(SpellId.fireCollide);
                        break;
                }

                player.preparedFor = 0;
            }

            player.isHurt = false; // also clear the hurt animation flag
        }
    }


    void BattleRound()
    {
        ProcessSustainedEffects();

        ProcessEnemyBehaviour();


        foreach (Player player in players)
        {
            if (player.GetHP() <= 0 && player.position >= 0)
            {
                if (player == players[0])
                {
                    // player dead
                    Debug.Log("Game Lose!");
                    GameLose?.Invoke();
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
                        Debug.Log("Game Win!");
                        GameWin?.Invoke();
                    }
                }
            }
        }

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
