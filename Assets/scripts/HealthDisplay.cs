using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public GameObject target;
    public TextMeshProUGUI valueText;
    void Update()
    {
        int maxHP = target.GetComponent<Unit>().maxHP;
        int currentHP = target.GetComponent<Unit>().currentHP;
        valueText.text = $"{currentHP}/{maxHP}";
    }
}