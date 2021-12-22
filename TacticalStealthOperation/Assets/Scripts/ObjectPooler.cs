using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {
    private List<GameObject> pool = new List<GameObject>();
    private int index;
    // Start is called before the first frame update
    public void Start() {
        index = -1;
        foreach(Transform k in transform){
            pool.Add(k.gameObject);
        }
    }

    public void SpawnAt(Vector3 position, Vector3 rotation, Vector3 scale){
        index = (++index)%pool.Count;
        GameObject obj = pool[index];
        obj.SetActive(true);

        obj.transform.position = position;
        obj.transform.eulerAngles = rotation;
        obj.transform.localScale = scale;
    }
    public void SpawnAt(Vector3 position, Vector3 rotation){
        SpawnAt(position, rotation, new Vector3(1, 1, 1));
    }
}
