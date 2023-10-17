using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntToStringManager : MonoBehaviour
{
    public TextMeshProUGUI valueText;
    void Update()
    {
        valueText.text = Game.fixedUpdateCount.ToString();
    }
}
