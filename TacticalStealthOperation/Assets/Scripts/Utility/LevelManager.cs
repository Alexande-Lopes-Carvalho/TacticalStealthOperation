using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    [SerializeField] private Canvas winUICanvas;
    public Canvas WinUICanvas{
        get => winUICanvas;
    }
    [SerializeField] private Canvas pausedUICanvas;
    [SerializeField] private Transform levelParent;

    private float pausedVal = 0.0f;
    private float playVal = 1.0f;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            PauseOrResume();
        }
    }

    public void SetLevel(int index){
        if (index >= 0 && index < levelPrefabs.Count){
            mainCamera.SetActive(false);
            mainCanvas.gameObject.SetActive(false);
            loseUICanvas.gameObject.SetActive(false);
            winUICanvas.gameObject.SetActive(false);
            pausedUICanvas.gameObject.SetActive(false);
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
        winUICanvas.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(true);
        Time.timeScale = playVal;
    }

    public void DestroyLevels(){
        for(int i=0;i<levelParent.childCount;++i){
            Destroy(levelParent.GetChild(i).gameObject);
        }
    }

    public void PauseOrResume()
    {
        if (Time.timeScale == pausedVal)
        {
            Resume();
        }
        else
        {
            Pause();
        }
        
    }

    public void Pause()
    {
        Time.timeScale = pausedVal;
        pausedUICanvas.gameObject.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = playVal;
        pausedUICanvas.gameObject.SetActive(false);
    }
}
