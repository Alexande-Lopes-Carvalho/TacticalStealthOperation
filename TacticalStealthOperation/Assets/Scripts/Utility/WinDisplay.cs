using System;
using UnityEngine;
using TMPro;

public class WinDisplay : MonoBehaviour
{
    [SerializeField] private string endMessage;
    private LevelManager levelManager {get;set;}
    public void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
    }

    public void OnTriggerEnter(Collider coll)
    {
        //Debug.Log("collision");
        if (coll.gameObject.CompareTag("Player"))
        {
            //Debug.Log("player");
            levelManager.Pause();
            levelManager.WinUICanvas.gameObject.SetActive(true);
            levelManager.WinEndMessage.SetText(endMessage);
        }
    }
}