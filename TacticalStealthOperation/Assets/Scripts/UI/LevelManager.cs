using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> levelPrefabs;
    private int Index {get;set;} = 0;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Transform levelParent;
    [SerializeField] private GameObject mainEventSystem;

    public void SetLevel(int index){
        mainCamera.SetActive(false);
        mainCanvas.gameObject.SetActive(false);
        Destroy(mainEventSystem);//.SetActive(false);
        Instantiate(levelPrefabs[Index],levelParent);//.SetActive(false);
        levelPrefabs[index].SetActive(true);
        Index = index;
    }

    public void BackToMenu(){
        DestroyLevels();
        mainCamera.SetActive(true);
        mainCanvas.gameObject.SetActive(true);
        Instantiate(mainEventSystem);//.SetActive(true);
    }

    public void DestroyLevels(){
        for(int i=0;i<levelParent.childCount;++i){
            Destroy(levelParent.GetChild(i).gameObject);
        }
    }
}
