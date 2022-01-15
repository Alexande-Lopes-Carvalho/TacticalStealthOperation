using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolLinker : MonoBehaviour {
    [SerializeField] private Transform pools;
    private static Dictionary<string, ObjectPooler> dictonary;
    // Start is called before the first frame update
    void Start() {
        dictonary = new Dictionary<string, ObjectPooler>();
        foreach(Transform k in pools){
            dictonary[k.name] = k.GetComponent<ObjectPooler>();
        }
    }

    // Update is called once per frame
    void Update() {
        
    }

    public static ObjectPooler Get(string k){
        return dictonary[k];
    }
}
