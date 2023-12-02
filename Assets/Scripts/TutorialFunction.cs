using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class TutorialFunction : MonoBehaviour
{
    public GameObject[] setThemActive;
    public GameObject[] setThemUnactive;
    public string sceneName;
    public float num;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ModifyActive(){
        for(int i = 0; i < setThemActive.Length; i++){
            setThemActive[i].SetActive(true);
        }
        for(int i = 0; i < setThemUnactive.Length; i++){
            setThemUnactive[i].SetActive(false);
        }
    }

    public void LoadScene(){
        SceneManager.LoadScene(sceneName);
    }

    public void ModifyGameTime(){
        Time.timeScale = num;
    }
}
