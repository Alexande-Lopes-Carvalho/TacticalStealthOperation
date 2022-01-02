using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug : MonoBehaviour {
    [SerializeField] private ObjectPooler objPool;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)){
            //Debug.print("pass");
            objPool.SpawnAt(new Vector3(34.5f, 0.8f, 0), new Vector3(-90, 0, 0));
        }
    }
}
