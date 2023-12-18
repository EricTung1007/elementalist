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
        currentSelecting = "火矢：造成 4 點火屬性傷害";
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
