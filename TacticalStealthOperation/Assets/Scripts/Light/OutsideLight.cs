using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideLight : MonoBehaviour {
    private static OutsideLight instance;
    public static OutsideLight Instance{get => instance; set => instance = value;}
    private List<GameLight> lightList;
    [SerializeField] private bool hasSun;
    // Start is called before the first frame update
    private void Awake() {
        if(instance != null){
            Destroy(instance.gameObject);
        }
        instance = this;
        lightList = new List<GameLight>();
        foreach(Transform k in transform){
            lightList.Add(k.GetComponentInChildren<GameLight>());
        }
    }

    // Update is called once per frame
    private void Update() {
        
    }

    public bool Contains(List<GameLight> l){
        if(hasSun){
            return true;
        } else {
            foreach(GameLight k in l){
                if(lightList.Contains(k)){
                    return true;
                }
            }
        }
        return false;
    }
}
