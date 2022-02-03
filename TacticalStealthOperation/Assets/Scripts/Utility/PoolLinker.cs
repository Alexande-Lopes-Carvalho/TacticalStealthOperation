using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolLinker : MonoBehaviour {
    private static PoolLinker instance;
    public static PoolLinker Instance{get => instance;}
    [SerializeField] private Transform pools;
    private Dictionary<string, ObjectPooler> dictonary;
    [SerializeField] private Transform poolsDestroyer;
    private Dictionary<string, ObjectPoolDestroyer> dictionaryDestroyer;
    // Start is called before the first frame update
    private void Awake() {
        if(instance != null){
            Destroy(instance.gameObject);
            Destroy(pools.gameObject);
            Destroy(poolsDestroyer.gameObject);
        }
        instance = this;
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

    public ObjectPooler Get(string k){
        return dictonary[k];
    }

    public ObjectPoolDestroyer GetDestroyer(string k){
        return dictionaryDestroyer[k];
    }
}
