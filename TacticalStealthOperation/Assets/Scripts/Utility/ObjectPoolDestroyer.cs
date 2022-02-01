using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolDestroyer : MonoBehaviour {
    [SerializeField] private int numberOfElement = 10;
    private List<GameObject> pool = new List<GameObject>();
    public List<GameObject> Pool{get=> pool;}
    
    public void Add(GameObject g){
        pool.Add(g);
        if(pool.Count > numberOfElement){
            Destroy(pool[0]);
            pool.RemoveAt(0);
        }
        g.transform.parent = this.transform;
    }

    public void Remove(GameObject g){
        pool.Remove(g);
    }
}
