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
    [SerializeField] private GameObject coverWhole;
    [SerializeField] private GameObject coverGrass;
    [SerializeField] private GameObject coverWater;
    [SerializeField] private GameObject coverSpell;
    [SerializeField] private GameObject text2;
    [SerializeField] private GameObject text3;
    [SerializeField] private GameObject text4;
    [SerializeField] private GameObject text7;
    [SerializeField] private GameObject text8;
    [SerializeField] private GameObject text11;
    [SerializeField] private GameObject text12;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject[] nextFunctions;
    [SerializeField] private int nextCount = 0;
    [SerializeField] private GameObject previousButton;
    [SerializeField] private GameObject[] previousFunctions;
    [SerializeField] private int previousCount;
    [SerializeField] private GameObject elementTile1;
    [SerializeField] private GameObject elementTile2;
    public GameObject spellTile2;
    private int whichFunction = 0;
    [SerializeField] private GameObject arrowSign1;
    [SerializeField] private GameObject arrowSign2;
    [SerializeField] private GameObject arrowSign3;
    [SerializeField] private GameObject arrowSign4;
    [SerializeField] private GameObject squareSign1;
    [SerializeField] private GameObject squareSign2;
    // Start is called before the first frame update
    void Start()
    {
        previousCount = previousFunctions.GetLength(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(whichFunction == 0){
            if(battleGUIManager.GetComponent<TutorialElementGridManager>().GetElementType(5,1)!= 0){
                Destroy(elementTile1.GetComponent<TutorialElementTile>()); 
                coverGrass.SetActive(true);
                arrowSign1.SetActive(false);
                coverWhole.SetActive(true);
                textBack1.SetActive(false);
                textBack2.SetActive(false);
                text2.SetActive(false);
            }
            HandleFirstEliminate();
        }      
        else if(whichFunction == 1){
            if( battleGUIManager.GetComponent<TutorialElementGridManager>().GetElementType(4,0) != 2){
                Destroy(elementTile2.GetComponent<TutorialElementTile>());
                coverWater.SetActive(true);
                textBack1.SetActive(false);
                textBack2.SetActive(false);
                text3.SetActive(false);
                arrowSign2.SetActive(false);
                coverWhole.SetActive(true);
            }
            HandleSecondEliminate();
        }
            
        else if(whichFunction == 2)
            HandleChoosingSpell(); 
        else if(whichFunction == 3){
            if(battleGUIManager.GetComponent<TutorialElementGridManager>().GetElementType(3,0)!= 1){
                textBack1.SetActive(false);
                textBack2.SetActive(false);  
                text11.SetActive(false); 
                arrowSign3.SetActive(false);            
                coverWhole.SetActive(true);
                coverSpell.SetActive(true);                
            }
            HandleCastingSpell();  
        }

    } 


    public void HandleFirstEliminate(){
        if(battleGUIManager.GetComponent<TutorialElementGridManager>().GetElementType(5,1)!= 0 && battleGUIManager.GetComponent<TutorialElementGridManager>().CheckAllElementsExist()){
            textBack1.SetActive(true);
            textBack2.SetActive(true);
            text3.SetActive(true);
            coverWhole.SetActive(false);
            coverWater.SetActive(false);
            arrowSign2.SetActive(true);
            whichFunction++;         
        }
    }
    public void HandleSecondEliminate(){
        if(battleGUIManager.GetComponent<TutorialElementGridManager>().GetElementType(4,0)!= 2 && battleGUIManager.GetComponent<TutorialElementGridManager>().CheckAllElementsExist()){          
            Time.timeScale = 0f;
            textBack1.SetActive(true);
            textBack2.SetActive(true);
            text4.SetActive(true);
            squareSign1.SetActive(true);
            squareSign2.SetActive(true);
            nextButton.SetActive(true);
            whichFunction++;
        }
    }
    public void HandleChoosingSpell(){
        if(battleGUIManager.GetComponent<TutorialSpellGridManager>().selectedSpellTileNumber == 2){
            arrowSign4.SetActive(false);            
            coverWhole.SetActive(true);
            Time.timeScale = 0f;
            textBack1.SetActive(true);
            textBack2.SetActive(true);
            text7.SetActive(false);
            text8.SetActive(true);
            squareSign1.SetActive(true);
            nextButton.SetActive(true);
            whichFunction++;
        }
    }
    public void HandleCastingSpell(){
        if(battleGUIManager.GetComponent<TutorialElementGridManager>().GetElementType(3,0)!= 1 && battleGUIManager.GetComponent<TutorialElementGridManager>().CheckAllElementsExist()){
            Time.timeScale = 0f;
            textBack1.SetActive(true);
            textBack2.SetActive(true);
            text12.SetActive(true);
            nextButton.SetActive(true);
            whichFunction++;
        }
    }
    public void GoToNext(){
        TutorialFunction modify = nextFunctions[nextCount].GetComponent<TutorialFunction>();
        modify.ModifyActive();
        modify.ModifyGameTime();
        nextCount++;
        previousCount--;
    }
    public void GoBack(){
        TutorialFunction modifyActive = previousFunctions[(-1)*(previousCount+1-previousFunctions.GetLength(0))].GetComponent<TutorialFunction>();
        modifyActive.ModifyActive();
        previousCount++;
        nextCount--;
    }
    public void ModifySpellClick(){
        spellTile2.GetComponent<TutorialSpellTileClickable>().active = !spellTile2.GetComponent<TutorialSpellTileClickable>().active; 
    }
}
