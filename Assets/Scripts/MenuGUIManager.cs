using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


public class MenuGUIMeneger : MonoBehaviour
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
    public GameObject pausingImage;
    public GameObject BackStartButton;
    public GameObject resultMenu;
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
        BackStartButton.SetActive(true);
        pauseButton.SetActive(false);
        pausingImage.SetActive(true);
    }

    public void Continue(){
        Time.timeScale = 1f;
        pauseBackground.SetActive(false);
        continueButton.SetActive(false);
        restartButton.SetActive(false);
        tutorialButton.SetActive(false);
        BackStartButton.SetActive(false);
        pausingImage.SetActive(false);
        pauseButton.SetActive(true);
    }

    public void Restart(){
        pauseBackground.SetActive(false);
        continueButton.SetActive(false);
        restartButton.SetActive(false);
        tutorialButton.SetActive(false);
        BackStartButton.SetActive(false);
        pausingImage.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level 1");
    }

    public void Tutorial(){
        continueButton.SetActive(false);
        restartButton.SetActive(false);
        BackStartButton.SetActive(false);
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
        pausingImage.SetActive(false);
        pauseButton.SetActive(true);
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
        BackStartButton.SetActive(true);
    }

    public void GameWin()
    {
        Time.timeScale = 0f;
        resultMenu.SetActive(true);
        win.SetActive(true);
        lose.SetActive(false);
        againButton.SetActive(true);
        startMenuButton.SetActive(true);
    }
    public void GameLose()
    {
        Time.timeScale = 0f;
        resultMenu.SetActive(true);
        win.SetActive(false);
        lose.SetActive(true);
        againButton.SetActive(true);
        startMenuButton.SetActive(true);
    }
}