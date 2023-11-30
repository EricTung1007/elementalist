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
    [SerializeField] private GameObject okButton;
    [SerializeField] private GameObject coverWhole;
    [SerializeField] private GameObject coverGrass;
    [SerializeField] private GameObject coverSpell;
    [SerializeField] private GameObject text3;
    [SerializeField] private GameObject text4;
    [SerializeField] private GameObject text9;
    [SerializeField] private GameObject text13;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject[] nextFunctions;
    [SerializeField] private int nextCount = 0;
    [SerializeField] private GameObject elementTile1;
    [SerializeField] private GameObject elementTile2;
    private int whichFunction = 0;
    [SerializeField] private GameObject arrowSign1;
    [SerializeField] private GameObject arrowSign2;
    [SerializeField] private GameObject arrowSign3;
    [SerializeField] private GameObject arrowSign4;
    [SerializeField] private GameObject squareSign1;
    [SerializeField] private GameObject squareSign2;
    public GameObject spellTile6;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(whichFunction == 0){
            if(battleGUIManager.GetComponent<TutorialElementGridManager>().GetElementType(5,1)!= 2){
                Destroy(elementTile1.GetComponent<TutorialElementTile>()); 
                coverGrass.SetActive(true);
                arrowSign1.SetActive(false);
            }
            HandleFirstEliminate();
        }      
        else if(whichFunction == 1){
            if( battleGUIManager.GetComponent<TutorialElementGridManager>().GetElementType(4,0) != 1){
                Destroy(elementTile2.GetComponent<TutorialElementTile>());
                arrowSign2.SetActive(false);
            }
            HandleSecondEliminate();
        }
            
        else if(whichFunction == 2)
            HandleChoosingSpell(); 
        else if(whichFunction == 3){
            if(battleGUIManager.GetComponent<TutorialElementGridManager>().GetElementType(3,0)!= 2){
                arrowSign3.SetActive(false);
            }
            HandleCastingSpell();  
        }

    } 


    public void HandleFirstEliminate(){
        if(battleGUIManager.GetComponent<TutorialElementGridManager>().GetElementType(5,1)!= 2 && battleGUIManager.GetComponent<TutorialElementGridManager>().CheckAllElementsExist()){
            arrowSign1.SetActive(false);
            Time.timeScale = 0f;
            textBack1.SetActive(true);
            textBack2.SetActive(true);
            text3.SetActive(true);
            okButton.SetActive(true);
            whichFunction++;            
        }
    }
    public void HandleSecondEliminate(){
        if(battleGUIManager.GetComponent<TutorialElementGridManager>().GetElementType(4,0)!= 1 && battleGUIManager.GetComponent<TutorialElementGridManager>().CheckAllElementsExist()){
            arrowSign2.SetActive(false);            
            coverWhole.SetActive(true);
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
        if(battleGUIManager.GetComponent<TutorialSpellGridManager>().selectedSpellTileNumber == 6){
            arrowSign4.SetActive(false);            
            coverWhole.SetActive(true);
            coverSpell.SetActive(true);
            Time.timeScale = 0f;
            textBack1.SetActive(true);
            textBack2.SetActive(true);
            text9.SetActive(true);
            nextButton.SetActive(true);
            whichFunction++;
        }
    }
    public void HandleCastingSpell(){
        if(battleGUIManager.GetComponent<TutorialElementGridManager>().GetElementType(3,0)!= 2 && battleGUIManager.GetComponent<TutorialElementGridManager>().CheckAllElementsExist()){
            coverWhole.SetActive(true);
            Time.timeScale = 0f;
            textBack1.SetActive(true);
            textBack2.SetActive(true);
            text13.SetActive(true);
            nextButton.SetActive(true);
            whichFunction++;
        }
    }
    public void GoToNext(){
        TutorialFunction modifyActive = nextFunctions[nextCount].GetComponent<TutorialFunction>();
        modifyActive.ModifyActive();
        nextCount++;
        //Debug.Log(nextCount);
    }
    public void ModifySpellClick(){
        spellTile6.GetComponent<TutorialSpellTileClickable>().active = !spellTile6.GetComponent<TutorialSpellTileClickable>().active;            
    }
}
