using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class StartMenuController : MonoBehaviour
{
    public GameObject level0;
    public GameObject level1;
    public GameObject level2;
    TextMeshProUGUI level0result;
    TextMeshProUGUI level1result;
    TextMeshProUGUI level2result;
    // Start is called before the first frame update
    void Start()
    {
    level0result = level0.GetComponent<TextMeshProUGUI>();  
    level1result = level1.GetComponent<TextMeshProUGUI>();
    level2result = level2.GetComponent<TextMeshProUGUI>();; ;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerPrefs.HasKey("level0result")){
            if(PlayerPrefs.GetInt("level0result") == 1)
                level0result.text = "成功通關";
        }
        if(PlayerPrefs.HasKey("level1result")){
            if(PlayerPrefs.GetInt("level1result") == 1)
                level1result.text = "成功通關";
        }
        if(PlayerPrefs.HasKey("level2result")){
                level2result.text = PlayerPrefs.GetInt("level2result").ToString() + "回合";
        }
    }
}
