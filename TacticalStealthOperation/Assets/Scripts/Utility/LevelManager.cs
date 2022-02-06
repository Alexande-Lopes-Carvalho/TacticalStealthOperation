using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> levelPrefabs;
    private int Index {get;set;} = 0;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Canvas loseUICanvas;
    public Canvas LoseUICanvas{
        get => loseUICanvas;
    }
    [SerializeField] private Transform levelParent;

    public void SetLevel(int index){
        if (index >= 0 && index < levelPrefabs.Count){
            mainCamera.SetActive(false);
            mainCanvas.gameObject.SetActive(false);
            loseUICanvas.gameObject.SetActive(false);
            Instantiate(levelPrefabs[index],levelParent);
            levelPrefabs[index].SetActive(true);
            Index = index;
        }
    }

    public void ResetLevel(){
        DestroyLevels();
        SetLevel(Index);
    }

    public void BackToMenu(){
        DestroyLevels();
        mainCamera.SetActive(true);
        loseUICanvas.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(true);
    }

    public void DestroyLevels(){
        for(int i=0;i<levelParent.childCount;++i){
            Destroy(levelParent.GetChild(i).gameObject);
        }
    }
}
