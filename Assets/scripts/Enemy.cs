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
}
