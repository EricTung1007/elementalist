using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject battleGUIManager;
    [SerializeField] private GameObject textBack1;
    [SerializeField] private GameObject textBack2;
    [SerializeField] private GameObject text3;
    [SerializeField] private GameObject okButton;
    [SerializeField] private GameObject coverWhole;
    [SerializeField] private GameObject coverGrass;
    [SerializeField] private GameObject text4;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject[] nextFunctions;
    [SerializeField] private int nextCount = 0;
    [SerializeField] private GameObject elementTile1;
    [SerializeField] private GameObject elementTile2;
    [SerializeField] private GameObject elementTile3;
    private int whichFunction = 0;
    [SerializeField] private GameObject arrowSign1;
    [SerializeField] private GameObject arrowSign2;
    [SerializeField] private GameObject squareSign1;
    [SerializeField] private GameObject squareSign2;
    // Start is called before the first frame update
    void Start()
    {
//battleGUIManager.GetComponent<TutorialElementGridManager>().GetElementType;
    }

    // Update is called once per frame
    void Update()
    {
        if(whichFunction == 0)
            HandleFirstEliminate();
        else if(whichFunction == 1)
            HandleSecondEliminate();
        else if(whichFunction == 2)
            Time.timeScale = 0f;   
    } 


    public void HandleFirstEliminate(){
        if(battleGUIManager.GetComponent<TutorialElementGridManager>().GetElementType(5,1)== 1 && battleGUIManager.GetComponent<TutorialElementGridManager>().CheckAllElementsExist()){
            Time.timeScale = 0f;
            coverWhole.SetActive(true);
            textBack1.SetActive(true);
            textBack2.SetActive(true);
            text3.SetActive(true);
            okButton.SetActive(true);
            whichFunction++;            
        }
    }
    public void HandleSecondEliminate(){
        if(battleGUIManager.GetComponent<TutorialElementGridManager>().GetElementType(4,0)== 0 && battleGUIManager.GetComponent<TutorialElementGridManager>().CheckAllElementsExist()){
            Time.timeScale = 0f;
            coverWhole.SetActive(true);
            textBack1.SetActive(true);
            textBack2.SetActive(true);
            text4.SetActive(true);
            squareSign1.SetActive(true);
            squareSign2.SetActive(true);
            nextButton.SetActive(true);
            whichFunction++;
        }
    }
    public void HandleCastingSpell(){
        whichFunction++;
   //     if(elementGrid[6, 0]){

    //    }
    }
    public void GoToNext(){
        TutorialFunction modifyActive = nextFunctions[nextCount].GetComponent<TutorialFunction>();
        modifyActive.ModifyActive();
        nextCount++;
        Debug.Log(nextCount);
    }

    public void CloseInteractable(){
        if(whichFunction == 0 && battleGUIManager.GetComponent<TutorialElementGridManager>().GetElementType(5,1)== 1){
           Destroy(elementTile1.GetComponent<ElementTile>()); 
           arrowSign1.SetActive(false);
        }
        else if(whichFunction == 1 && battleGUIManager.GetComponent<TutorialElementGridManager>().GetElementType(4,0)== 0){
            Destroy(elementTile2.GetComponent<ElementTile>());
            arrowSign2.SetActive(false);
        }    
        else if(whichFunction == 2)
            Destroy(elementTile3.GetComponent<ElementTile>());
    }
}
