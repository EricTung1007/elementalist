using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadGame(){
        SceneManager.LoadScene("Level 1");
        Time.timeScale = 1.0f; 
    }
    public void LoadTutorial(){
        SceneManager.LoadScene("Tutorial");
         Time.timeScale = 0f; 
    }
    public void LoadStartMenu(){
        SceneManager.LoadScene("StartMenu");
    }
}