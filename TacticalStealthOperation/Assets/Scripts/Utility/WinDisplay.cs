using System;
using UnityEngine;

public class WinDisplay : MonoBehaviour
{
    private LevelManager levelManager {get;set;}
    public void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
    }

    public void OnTriggerEnter(Collider coll)
    {
        Debug.Log("collision");
        if (coll.gameObject.CompareTag("Player"))
        {
            Debug.Log("player");
            levelManager.Pause();
            levelManager.WinUICanvas.gameObject.SetActive(true);
        }
    }
}