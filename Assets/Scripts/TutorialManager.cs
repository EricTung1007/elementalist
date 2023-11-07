using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialImage1;
    public GameObject tutorialImage2;
    public GameObject nextButton;
    public GameObject previousButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
