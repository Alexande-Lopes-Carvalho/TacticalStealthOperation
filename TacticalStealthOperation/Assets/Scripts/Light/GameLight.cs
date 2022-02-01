using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLight : MonoBehaviour {
    private Light gameLight;
    private bool isEnabled = true;
    public bool IsEnabled{get => isEnabled;}
    private Collider coll;
    private float activationDist = 30f, activationSqrDist;

    // Start is called before the first frame update
    private void Start() {
        gameLight = GetComponent<Light>();
        coll = GetComponent<Collider>();
        activationSqrDist = activationDist*activationDist;
    }

    // Update is called once per frame
    private void Update() {
        if(isEnabled){
            gameLight.enabled = false;
            foreach(Character k in HumanLinker.Characters){
                if((k.transform.position-transform.position).sqrMagnitude < activationSqrDist){
                    gameLight.enabled = true;
                }
            }
        }
    }

    public void Set(bool b){
        gameLight.enabled = b;
        isEnabled = b;
        coll.enabled = b;
    }

    public void OnTriggerEnter(Collider c){
        Debug.Log(c);
    }
}
