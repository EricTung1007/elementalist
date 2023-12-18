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
        if (this.hp > maxhp)
            this.hp = maxhp;
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
    private int battleCount = 0;

    private int aliveEnemies = 0;

    [SerializeField] private UnityEvent GameWin;
    [SerializeField] private UnityEvent GameLose;
    [SerializeField] private UnityEvent DropSlime;
    [SerializeField] private UnityEvent ElementClear;
    [SerializeField] private UnityEvent NewWave;
    [SerializeField] private GameObject fireArrowVFX;
    [SerializeField] private GameObject waterBallVFX;
    [SerializeField] private GameObject woodenArrowVFX;
    [SerializeField] private GameObject firePillarVFX;
    [SerializeField] private GameObject healVFX;
    [SerializeField] private GameObject steamExplosionVFX;
    [SerializeField] private GameObject transformMudVFX;
    [SerializeField] private GameObject tieUpVFX;
    [SerializeField] private GameObject vinePullVFX;
    [SerializeField] private GameObject grassCollideVFX;
    [SerializeField] private GameObject waterCollideVFX;
    [SerializeField] private GameObject fireCollideVFX;
    [SerializeField] private GameObject healAllEnemyVFX;

    public int level = 0;

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

    private float GetTypeMultiplier(Type byType, Type toType)
    {
        if (byType == Type.none || toType == Type.none)
            return 1.0F;
        if (byType == Type.fire && toType == Type.grass)
            return 1.5F;
        if (byType == Type.grass && toType == Type.water)
            return 1.5F;
        if (byType == Type.water && toType == Type.fire)
            return 1.5F;
        if (byType == Type.grass && toType == Type.fire)
            return 0.5F;
        if (byType == Type.water && toType == Type.grass)
            return 0.5F;
        if (byType == Type.fire && toType == Type.water)
            return 0.5F;
        return 1.0F;
    }

    private float GetMultiplier(Player byPlayer, Type attackType, Player toPlayer)
    {
        float x = GetTypeMultiplier(attackType, toPlayer.type);
        if (toPlayer.IsUnderEffect(EffectId.tiedUp) != null)
            x *= 2.0F;
        if (byPlayer.IsUnderEffect(EffectId.mud) != null)
            x *= 0.25F;
        return x;
    }

    public void ElementCollect(Type elementType)
    {
        foreach (Player player in players)
        {
            if (player.type == elementType)
                player.chi++;
        }

        //switch (elementType)
        //{
        //    case Type.grass:
        //        greenChi++;
        //        break;
        //    case Type.water:
        //        blueChi++;
        //        break;
        //    case Type.fire:
        //        redChi++;
        //        break;
        //}
        //SpellId collectionSpell = SpellId.none;
        //switch (elementType)
        //{
        //    case Type.fire:
        //        collectionSpell = SpellId.magmaCollect;
        //        break;
        //    case Type.water:
        //        collectionSpell = SpellId.slimeCollect;
        //        break;
        //    case Type.grass:
        //        collectionSpell = SpellId.naturalHeal;
        //        break;
        //}
        //if (collectionSpell != SpellId.none)
        //{
        //    foreach (Player player in players)
        //        if (player.type == elementType)
        //            PerformSpell(player.skill.Where(s => s.spellId == collectionSpell).First(), player, players[0], true);
        //}
    }


    // return true if performed/can perform successfully or false if the spell is held on until criteria meets
    public bool PerformSpell(Spell spell, Player byPlayer, Player toPlayer, bool perform = true)
    {
        UnityEngine.Debug.Log(spell.spellId + (perform ? " performed by " : " tested by ") + byPlayer.playerId + " at " + toPlayer.playerId);
        int enemyCount = players.Where(p => p.position > 0).Count();
        switch (spell.spellId)
        {
            case SpellId.fireArrow:
                if (perform)
                {
                    Vector3 position = new Vector3(-2.88f, 2.5f, 0f);
                    Quaternion rotation = Quaternion.Euler(0f, 90f, -90f);
                    GameObject newfireArrowVFX = Instantiate(fireArrowVFX, position, rotation);
                    newfireArrowVFX.transform.localScale = new Vector3(1f, 1f, 1f);
                    newfireArrowVFX.layer = LayerMask.NameToLayer("VFX");
                    toPlayer.DoDamage((int)Math.Ceiling(spell.hp * GetMultiplier(byPlayer, Type.fire, toPlayer)));
                }

                break;
            case SpellId.waterBall:
                if (perform)
                {
                    Vector3 position = new Vector3(-3f, 2.52f, 0f);
                    Quaternion rotation = Quaternion.Euler(0f, 0f, -90f);
                    GameObject newWaterBallVFX = Instantiate(waterBallVFX, position, rotation);
                    newWaterBallVFX.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                    newWaterBallVFX.layer = LayerMask.NameToLayer("VFX");
                    toPlayer.DoDamage((int)Math.Ceiling(spell.hp * GetMultiplier(byPlayer, Type.water, toPlayer)));
                }
                    
                break;
            case SpellId.woodenArrow:
                if (perform)
                {
                    Vector3 position = new Vector3(-3.12f, 2.53f, 0f);
                    Quaternion rotation = Quaternion.Euler(0f, 90f, -90f);
                    GameObject newWoodenArrowVFX = Instantiate(woodenArrowVFX, position, rotation);
                    newWoodenArrowVFX.transform.localScale = new Vector3(1f, 1f, 1f);
                    newWoodenArrowVFX.layer = LayerMask.NameToLayer("VFX");
                    toPlayer.DoDamage((int)Math.Ceiling(spell.hp * GetMultiplier(byPlayer, Type.grass, toPlayer)));
                }
                break;
            case SpellId.firePillar:
                if (perform)
                {
                    Vector3 position = new Vector3(-2.42f, 2.000149f, 0f);
                    Quaternion rotation = Quaternion.Euler(-90f, 0f, 0f);
                    if (byPlayer == players[0])
                    {
                        for (int i = 1; i < players.Count; i++)
                            if (players[i].position > 0)
                            {
                                GameObject newFirePillarVFX = Instantiate(firePillarVFX, position + new Vector3(players[i].position*2.42f, 0f, 0f) , rotation);
                                newFirePillarVFX.transform.localScale = new Vector3(1f, 1f, 1f);
                                newFirePillarVFX.layer = LayerMask.NameToLayer("VFX");
                                players[i].DoDamage((int)Math.Ceiling(spell.hp * GetMultiplier(byPlayer, Type.fire, players[i])));
                            }
                    }
                    else
                    {
                            GameObject newFirePillarVFX = Instantiate(firePillarVFX, transform.position, rotation);
                            newFirePillarVFX.transform.localScale = new Vector3(1f, 1f, 1f);
                            newFirePillarVFX.layer = LayerMask.NameToLayer("VFX");
                            players[0].DoDamage((int)Math.Ceiling(spell.hp * GetMultiplier(byPlayer, Type.fire, players[0])));
                    }
                }
                break;
            case SpellId.heal:
                if (perform)
                {
                    Vector3 position = new Vector3(-4.522f, 2f, 0f);
                    Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
                    GameObject newHealArrowVFX = Instantiate(healVFX, position, rotation);
                    newHealArrowVFX.transform.localScale = new Vector3(0.15f, 0.2f, 1f);
                    newHealArrowVFX.layer = LayerMask.NameToLayer("VFX");
                    byPlayer.Regenerate(spell.hp);
                }
                    
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
                {
                    Vector3 position = new Vector3(-2.42f, 2f, 0f);
                    Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);
                    for (int i = 1; i < players.Count; i++){
                            if (players[i].position > 0)
                            {
                                GameObject newTransformMudVFX = Instantiate(transformMudVFX, position + new Vector3(players[i].position*2.42f, 0f, 0f) , rotation);
                                newTransformMudVFX.transform.localScale = new Vector3(1f, 1f, 1f);
                                newTransformMudVFX.layer = LayerMask.NameToLayer("VFX");
                            }
                    }
                    foreach (Player player in players)
                        if (player != byPlayer)
                            player.AddSustainedEffect(new Effect(EffectId.mud, spell.hp, spell.duration));
                }
                break;
            case SpellId.tieUp:
                if (perform){
                    Vector3 position = new Vector3(-2.42f, 2.5f, 0f);
                    Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
                    for (int i = 1; i < players.Count; i++){
                        if (players[i].position > 0)
                        {
                            GameObject newTieUpVFX = Instantiate(tieUpVFX, position + new Vector3(players[i].position*2.42f, 0f, 0f) , rotation);
                            newTieUpVFX.transform.localScale = new Vector3(0.075f, 0.1f, 1f);
                            newTieUpVFX.layer = LayerMask.NameToLayer("VFX");
                        }
                    }
                    foreach (Player player in players)
                        if (player != byPlayer)
                            player.AddSustainedEffect(new Effect(EffectId.tiedUp, spell.hp, spell.duration));
                }
                break;
            case SpellId.vinePull:
                if (players.Where(p => p.position > 0).Count() <= 1){
                    Vector3 position = new Vector3(0f, 2.5f, 0f);
                    Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
                    GameObject newVinePullVFX = Instantiate(vinePullVFX, position, rotation);
                    newVinePullVFX.transform.localScale = new Vector3(1f, 1f, 1f);
                    newVinePullVFX.layer = LayerMask.NameToLayer("VFX");
                    return false;
                }  
                if (perform)
                {
                    Vector3 position = new Vector3(-2.42f, 2.5f, 0f);
                    Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
                    for (int i = 1; i < players.Count; i++){
                        if (players[i].position > 0)
                        {
                        GameObject newVinePullVFX = Instantiate(vinePullVFX, position + new Vector3(players[i].position*2.42f, 0f, 0f) , rotation);
                        newVinePullVFX.transform.localScale = new Vector3(1f, 1f, 1f);
                        newVinePullVFX.layer = LayerMask.NameToLayer("VFX");
                        }
                    }
                    foreach (Player player in players)
                    {
                        if (player.position == enemyCount) player.position = 1;
                        else if (player.position > 0) player.position++;
                    }
                }
                break;
            case SpellId.grassCollide:
                if (perform)
                {
                    Vector3 position = new Vector3(-4.522f, 2f, 0f);
                    Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
                    GameObject newGrassCollideVFX = Instantiate(grassCollideVFX, position, rotation);
                    newGrassCollideVFX.transform.localScale = new Vector3(1f, 1f, 1f);
                    newGrassCollideVFX.layer = LayerMask.NameToLayer("VFX");
                    toPlayer.DoDamage((int)Math.Ceiling(spell.hp * GetMultiplier(byPlayer, Type.grass, toPlayer)));
                }
                break;
            case SpellId.waterCollide:
                if (perform)
                {
                    Vector3 position = new Vector3(-4.522f, 2f, 0f);
                    Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
                    GameObject newWaterCollideVFX = Instantiate(waterCollideVFX, position, rotation);
                    newWaterCollideVFX .transform.localScale = new Vector3(1f, 1f, 1f);
                    newWaterCollideVFX.layer = LayerMask.NameToLayer("VFX");
                    toPlayer.DoDamage((int)Math.Ceiling(spell.hp * GetMultiplier(byPlayer, Type.water, toPlayer)));
                }
                break;
            case SpellId.fireCollide:
                if (perform)
                {
                    Vector3 position = new Vector3(-4.522f, 2f, 0f);
                    Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
                    GameObject newFireCollideVFX = Instantiate(fireCollideVFX, position, rotation);
                    newFireCollideVFX .transform.localScale = new Vector3(1f, 1f, 1f);
                    newFireCollideVFX.layer = LayerMask.NameToLayer("VFX");
                    toPlayer.DoDamage((int)Math.Ceiling((spell.hp + byPlayer.chi) * GetMultiplier(byPlayer, Type.fire, toPlayer)));
                }
                break;    
            case SpellId.slime:
                if (byPlayer.chi < spell.hp)
                    return false; // can not afford
                if (perform)
                {
                    byPlayer.chi -= spell.hp;
                    DropSlime?.Invoke();
                }
                break;
            case SpellId.dodge:
                if (!perform) // dodge check
                {
                    if (byPlayer.position != 1)
                        return false;
                    if (enemyCount <= 1)
                        return false;
                }
                if (perform)
                {
                    foreach (Player player in players)
                        if (player.position > 1) player.position--;
                    byPlayer.position = enemyCount;
                    Vector3 position = new Vector3(-4.522f, 1.9f, 0f);
                    Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
                    GameObject newFireCollideVFX = Instantiate(fireCollideVFX, position, rotation);
                    newFireCollideVFX .transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    newFireCollideVFX.layer = LayerMask.NameToLayer("VFX");

                    toPlayer.DoDamage((int)Math.Ceiling(spell.hp * GetMultiplier(byPlayer, byPlayer.type, toPlayer)));
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
            case SpellId.healAllEnemy:
                if (perform)
                {
                    Vector3 position = new Vector3(-2.42f, 1.8f, 0f);
                    Quaternion rotation = Quaternion.Euler(-90f, 0f, 0f);
                    foreach (Player player in players)
                        if (player.position >= 1)
                        {
                            GameObject newHealAllEnemyVFX = Instantiate(healAllEnemyVFX, position + new Vector3(player.position*2.42f, 0f, 0f) , rotation);
                            newHealAllEnemyVFX.transform.localScale = new Vector3(0.15f, 0.6f, 0.5f);
                            newHealAllEnemyVFX.layer = LayerMask.NameToLayer("VFX");
                            player.Regenerate(spell.hp + byPlayer.chi);
                        }
                    byPlayer.chi = 0;
                }
                break;
            case SpellId.magmaCollect:
                if (!byPlayer.skill.Where(s => s.spellId == SpellId.fireCollide).Any())
                    return false;
                if (byPlayer.skill.Where(s => s.spellId == SpellId.fireCollide).First().cooldown - spell.hp < 1)
                    return false;
                if (perform)
                    byPlayer.skill.Where(s => s.spellId == SpellId.fireCollide).First().cooldown -= spell.hp;
                break;
            case SpellId.elementClear:
                if (perform)
                {
                    foreach (Player player in players)
                        player.chi = 0;
                    Vector3 position = new Vector3(-2.42f, 2.48f, 0f);
                    Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
                    for (int i = 1; i < players.Count; i++){
                        if (players[i].position > 0)
                        {
                            GameObject newSteamExplosionVFX = Instantiate(steamExplosionVFX, position + new Vector3(players[i].position*2.42f, 0f, 0f) , rotation);
                            newSteamExplosionVFX.transform.localScale = new Vector3(1f, 1f, 1f);
                            newSteamExplosionVFX.layer = LayerMask.NameToLayer("VFX");
                        }
                    }
                    position = new Vector3(3.2f, -3.47f, 0f);
                    GameObject newSteamExplosionVFX1 = Instantiate(steamExplosionVFX, position, rotation);
                    newSteamExplosionVFX1.transform.localScale = new Vector3(1f, 1f, 1f);
                    newSteamExplosionVFX1.layer = LayerMask.NameToLayer("VFX");
                    ElementClear?.Invoke();
                }
                break;
        }

        return true;
    }

    public int round = 1;
    public int totalRounds = 3;
    private int nextRoundWait = 0;

    private void insertPlayer()
    {
        players.Add(new Player("player", 120, Type.none, 0));
        players.Last().skill.Add(new Spell(SpellId.fireArrow, 4, 0, 0));
        players.Last().skill.Add(new Spell(SpellId.waterBall, 4, 0, 0));
        players.Last().skill.Add(new Spell(SpellId.woodenArrow, 4, 0, 0));
        players.Last().skill.Add(new Spell(SpellId.firePillar, 6, 0, 0));
        players.Last().skill.Add(new Spell(SpellId.heal, 15, 0, 0));
        players.Last().skill.Add(new Spell(SpellId.tieUp, 200, 15, 0));
        players.Last().skill.Add(new Spell(SpellId.elementClear, 0, 0, 0));
        players.Last().skill.Add(new Spell(SpellId.transformMud, 25, 10, 0));
        players.Last().skill.Add(new Spell(SpellId.vinePull, 0, 0, 0));
    }

    private void insertGrass()
    {
        players.Add(new Player("green_slime", 45, Type.grass, players.Count));
        //                                                  baseHp, duration, cooldown 
        players.Last().skill.Add(new Spell(SpellId.grassCollide, 3, 0, 10));
        players.Last().skill.Add(new Spell(SpellId.healAllEnemy, 5, 0, 10));
    }

    private void insertWater()
    {
        players.Add(new Player("blue_slime", 40, Type.water, players.Count));
        players.Last().skill.Add(new Spell(SpellId.waterCollide, 3, 0, 5));
        players.Last().skill.Add(new Spell(SpellId.slime, 3, 0, 10));
    }

    private void insertFire()
    {
        players.Add(new Player("red_slime", 35, Type.fire, players.Count));
        players.Last().skill.Add(new Spell(SpellId.fireCollide, 10, 0, 20));
        players.Last().skill.Add(new Spell(SpellId.dodge, 4, 0, 20));
    }

    private void insertRandomEnemy()
    {
        switch (UnityEngine.Random.Range(0, 2))
        {
            case 0:
                insertGrass();
                return;
            case 1:
                insertWater();
                return;
            case 2:
                insertFire();
                return;
        }
    }

    private void InitBattle(int round)
    {
        if (level == 0)
        {
            switch (round)
            {
                case 1:
                    insertGrass();
                    nextRoundWait = 3;
                    break;

                case 2:
                    insertWater();
                    insertFire();
                    nextRoundWait = 5;
                    break;

                case 3:
                    insertGrass();
                    insertWater();
                    insertFire();
                    nextRoundWait = 0;
                    break;
            }
        }
        if (level == 1)
        {
            switch (round)
            {
                case 1:
                    insertGrass();
                    insertGrass();
                    insertGrass();
                    nextRoundWait = 3;
                    break;

                case 2:
                    insertWater();
                    insertWater();
                    insertWater();
                    nextRoundWait = 5;
                    break;

                case 3:
                    insertFire();
                    insertFire();
                    insertFire();
                    nextRoundWait = 0;
                    break;
            }
        }
        if (level == 2)
        {
            if (round <= 2)
            {
                insertRandomEnemy();
                nextRoundWait = 3;
            }
            else if (round <= 5)
            {
                insertRandomEnemy();
                insertRandomEnemy();
                nextRoundWait = 3;
            }
            else if (round <= 9)
            {
                insertRandomEnemy();
                insertRandomEnemy();
                insertRandomEnemy();
                nextRoundWait = 4;
            }
            else
            {
                insertRandomEnemy();
                insertRandomEnemy();
                insertRandomEnemy();
                insertRandomEnemy();
                nextRoundWait = 5;
            }
            for (int i = 1; i < players.Count; i++)
                players[i].chi = round;
        }
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
            UnityEngine.Debug.Log(player.playerId + ": " + player.intention);
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
                    PerformSpell(player.skill[player.intention], player, players[0], true);
                    player.intention = -1;
                }
            }

            UnityEngine.Debug.Log(player.playerId + " mid: " + player.intention);

            if (player.intention < 0) // intention to be determined
            {
                int random = UnityEngine.Random.Range(0, 100);

                switch (player.type)
                {
                    case Type.grass:
                        if (random < 50)
                            player.intention = player.skill.IndexOf(player.skill.Where(s => s.spellId == SpellId.grassCollide).First());
                        else
                            player.intention = player.skill.IndexOf(player.skill.Where(s => s.spellId == SpellId.healAllEnemy).First());
                        break;
                    case Type.water:
                        if (PerformSpell(player.skill[player.GetSpellIndex(SpellId.slime)], player, players[0], false) && random < 100)
                            player.intention = player.GetSpellIndex(SpellId.slime);
                        else
                            player.intention = player.GetSpellIndex(SpellId.waterCollide);
                        break;
                    case Type.fire:
                        player.intention = player.GetSpellIndex(SpellId.fireCollide);
                        break;
                }

                player.preparedFor = 0;
            }

            if (player.type == Type.fire) // dodge
            {
                if (PerformSpell(player.skill[player.GetSpellIndex(SpellId.dodge)], player, players[0], false))
                {
                    player.intention = player.GetSpellIndex(SpellId.dodge);
                }
            }
        }
    }



    void PlayersManagement()
    {
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
                        reservedNextWave = battleCount + nextRoundWait;
                        return; // the player list should not be iterated through after modification
                    }
                }
            }
        }
    }

    private int reservedNextWave = -1;

    void NextWave()
    {
        UnityEngine.Debug.Log("next wave " + round + " / " + totalRounds);
        if (round <= totalRounds)
        {
            UnityEngine.Debug.Log("next wave " + round);
            players.RemoveRange(1, players.Count - 1);
            InitBattle(round);
            aliveEnemies = players.Count - 1;
            NewWave?.Invoke();
            round++;
        }
        else
        {
            // win
            Debug.Log("Game Win!");
            GameWin?.Invoke();
        }
        reservedNextWave = -1;
    }

    void BattleUpdate()
    {
        foreach (Player player in players)
            player.isHurt = false; // also clear the hurt animation flag

        ProcessSustainedEffects();

        ProcessEnemyBehaviour();
    }

    // Start is called before the first frame update
    private void Awake()
    {
        UnityEngine.Debug.Log("Start");
        insertPlayer(); // init player
        nextRoundWait = 0;
        NextWave();
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void FixedUpdate()
    {
        if ((fixedUpdateCount % 50 == 0))
        {
            BattleUpdate();
            battleCount++;

            if (battleCount % 5 == 0 && players.Count() >= 0 && players[0].position == 0 && players[0].GetHP() < players[0].maxhp)
                players[0].maxhp--;

            if (battleCount >= reservedNextWave && reservedNextWave > 0)
                NextWave();
        }

        PlayersManagement();

        fixedUpdateCount++;
    }

}
