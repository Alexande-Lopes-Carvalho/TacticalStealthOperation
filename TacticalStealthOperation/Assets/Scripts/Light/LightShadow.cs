using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightShadow : MonoBehaviour {
    private List<GameLight> lightList;
    // Start is called before the first frame update
    private void Start() {
        lightList = new List<GameLight>();
        foreach(Transform k in transform){
            GameLight g = k.GetComponent<GameLight>();
            if(g != null){
                lightList.Add(g);
            }
        }
    }

    // Update is called once per frame
    private void Update() {
        
    }

    public bool Contains(List<GameLight> l){
        foreach(GameLight k in l){
            if(lightList.Contains(k)){
                return true;
            }
        }
        return false;
    }
}
