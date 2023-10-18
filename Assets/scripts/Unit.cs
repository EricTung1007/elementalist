using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Unit : MonoBehaviour
{
    public int maxHP;
    public int currentHP;
    public Type type;
    //public List<Status> status = new List<Status>();

    public bool isDead = false;

    private void Update()
    {
        if (currentHP >  maxHP)
        {
            currentHP = maxHP;
        }
        if (currentHP <= 0) 
        {
            currentHP = 0;
            isDead = true;
        }
        if (isDead)
        {
            Destroy(this.gameObject);
        }
    }
    public void heal(int healAmount)
    {
        this.currentHP += healAmount;
    }
    public void TakeDamage(int damageAmount, Type damageType)
    {
        float realDamageAmount = damageAmount;
        switch (damageType)
        {
            case Type.fire:
                switch (this.type)
                {
                    case Type.fire: realDamageAmount *= 1.0f; break;
                    case Type.water: realDamageAmount *= 0.5f; break;
                    case Type.grass: realDamageAmount *= 1.5f; break;
                }
                break;
            case Type.water:
                switch (this.type)
                {
                    case Type.fire: realDamageAmount *= 1.5f; break;
                    case Type.water: realDamageAmount *= 1.0f; break;
                    case Type.grass: realDamageAmount *= 0.5f; break;
                }
                break;
            case Type.grass:
                switch (this.type)
                {
                    case Type.fire: realDamageAmount *= 0.5f; break;
                    case Type.water: realDamageAmount *= 1.5f; break;
                    case Type.grass: realDamageAmount *= 1.0f; break;
                }
                break;
        }
        this.currentHP -= System.Convert.ToInt32(realDamageAmount);
    }
    //void GetStatus(Status status)
    public bool muddy;
}
