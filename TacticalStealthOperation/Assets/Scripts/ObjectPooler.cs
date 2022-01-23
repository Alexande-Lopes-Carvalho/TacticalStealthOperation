using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {
    [SerializeField] private int numberOfElement = 10;
    private List<GameObject> pool = new List<GameObject>();
    private int index;
    // Start is called before the first frame update
    public void Start() {
        index = -1;
        foreach(Transform k in transform){
            pool.Add(k.gameObject);
        }
        if(pool.Count > 0 && pool.Count < numberOfElement){
            for(int i = 0, nb = numberOfElement-pool.Count; i < nb; ++i){
                pool.Add(Instantiate(pool[0]));
                pool[pool.Count-1].transform.parent = transform;
            }
        }
    }

    public GameObject SpawnAt(Vector3 position, Vector3 rotation, Vector3 scale){
        //Debug.print("pass " + position);
        Rigidbody r;
        index = (++index)%pool.Count;
       // Debug.print(index);
        GameObject obj = pool[index];
        TrailRenderer tr;
        // TODO : fix trail renderer clear
        if (tr = obj.GetComponent<TrailRenderer>()){
            tr.Clear();
        }
        obj.SetActive(true);
        r = obj.GetComponent<Rigidbody>();
        r.velocity = Vector3.zero;
        r.angularVelocity = Vector3.zero;
        obj.transform.position = position;
        obj.transform.eulerAngles = rotation;
        obj.transform.localScale = scale;
        return obj;
    }
    public GameObject SpawnAt(Vector3 position, Vector3 rotation){
        return SpawnAt(position, rotation, new Vector3(1, 1, 1));
    }
}
