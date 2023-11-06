using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGUIManager : MonoBehaviour
{
    public GameObject pauseButton;
    public GameObject continueButton;
    public GameObject restartButton;
    public GameObject tutorialButton;
    public GameObject pauseBackground;
    public GameObject win;
    public GameObject lose;
    public GameObject againButton;
    public GameObject startMenuButton;
    public GameObject tutorialImage1;
    public GameObject tutorialImage2;
    public GameObject nextButton;
    public GameObject previousButton;
    public GameObject endButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Pause(){
        Time.timeScale = 0f;
        pauseBackground.SetActive(true);
        continueButton.SetActive(true);
        restartButton.SetActive(true);
        tutorialButton.SetActive(true);
    }

    public void Continue(){
        Time.timeScale = 1f;
        pauseBackground.SetActive(false);
        continueButton.SetActive(false);
        restartButton.SetActive(false);
        tutorialButton.SetActive(false);
    }

    public void Restart(){
        pauseBackground.SetActive(false);
        continueButton.SetActive(false);
        restartButton.SetActive(false);
        tutorialButton.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level 1");
    }

    public void Tutorial(){
        continueButton.SetActive(false);
        restartButton.SetActive(false);
        tutorialImage1.SetActive(true);
        nextButton.SetActive(true);
        endButton.SetActive(true);
        tutorialButton.SetActive(false);
    }
    public void PlayAgain(){
        win.SetActive(false);
        lose.SetActive(false);
        againButton.SetActive(false);
        startMenuButton.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level 1");
    }
    public void StartMenu(){
        win.SetActive(false);
        lose.SetActive(false);
        againButton.SetActive(false);
        startMenuButton.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartMenu");
    }

    public void Next(){
        tutorialImage1.SetActive(false);  
        tutorialImage2.SetActive(true);      
        nextButton.SetActive(false); 
        previousButton.SetActive(true);    
    }

     public void Previous(){
        tutorialImage2.SetActive(false);  
        tutorialImage1.SetActive(true);      
        nextButton.SetActive(true); 
        previousButton.SetActive(false);    
    }  
    public void EndTutorial(){
        tutorialImage1.SetActive(false);  
        tutorialImage2.SetActive(false);      
        nextButton.SetActive(false); 
        previousButton.SetActive(false); 
        endButton.SetActive(false);
        pauseBackground.SetActive(true);
        continueButton.SetActive(true);
        restartButton.SetActive(true);
        tutorialButton.SetActive(true);
    }
}