using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private string name;
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
    }
    public void LoadTutorial(){
        SceneManager.LoadScene("Tutorial");
    }
    public void LoadStartMenu(){
        SceneManager.LoadScene("StartMenu");
    }
}