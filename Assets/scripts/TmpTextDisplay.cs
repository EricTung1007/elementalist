using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TmpTextDisplay : MonoBehaviour
{
    public GameObject target;
    public TextMeshProUGUI valueText;
    void Update()
    {
        int atk = target.GetComponent<Enemy>().atk;
        int attackCoolDown = target.GetComponent<Enemy>().attackCountDown;
        valueText.text = $"ATK{atk}({attackCoolDown})";
    }
}
