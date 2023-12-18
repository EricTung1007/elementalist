using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoBox : MonoBehaviour
{
    [SerializeField] GameObject infoBox;
    public string currentSelecting = "";
    public string currentHovering = "";

    private void Start()
    {
        currentSelecting = "���ڡG�y�� 4 �I���ݩʶˮ`";
    }
    void Update()
    {
        if(currentHovering == "")
        {
            infoBox.GetComponent<TextMeshProUGUI>().text = currentSelecting;
        }
        else
        {
            infoBox.GetComponent<TextMeshProUGUI>().text = currentHovering;
        }
    }
}
