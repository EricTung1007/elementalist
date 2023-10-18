using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int position = -1;

    public void SetWorldCoordinate()
    {
        float x = position;

        this.transform.position = new Vector3(2.5f * x, 2.5f, 0);
    }

    public int attackCountDown;
    public int attackCountDownOriginal;
    public int atk;

    public void Attack(int damage)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Unit>().TakeDamage(damage, Type.none);
    }
}