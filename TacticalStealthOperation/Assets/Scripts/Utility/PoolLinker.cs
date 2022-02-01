using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolLinker : MonoBehaviour {
    [SerializeField] private Transform pools;
    private static Dictionary<string, ObjectPooler> dictonary;
    [SerializeField] private Transform poolsDestroyer;
    private static Dictionary<string, ObjectPoolDestroyer> dictionaryDestroyer;
    // Start is called before the first frame update
    private void Awake() {
        dictonary = new Dictionary<string, ObjectPooler>();
        foreach(Transform k in pools){
            dictonary[k.name] = k.GetComponent<ObjectPooler>();
        }
        dictionaryDestroyer = new Dictionary<string, ObjectPoolDestroyer>();
        foreach(Transform k in poolsDestroyer){
            dictionaryDestroyer[k.name] = k.GetComponent<ObjectPoolDestroyer>();
        }
    }

    // Update is called once per frame
    private void Update() {
        
    }

    public static ObjectPooler Get(string k){
        return dictonary[k];
    }

    public static ObjectPoolDestroyer GetDestroyer(string k){
        return dictionaryDestroyer[k];
    }
}
