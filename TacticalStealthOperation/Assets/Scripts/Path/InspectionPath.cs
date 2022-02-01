using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectionPath : MonoBehaviour {
    private List<Path> pathList;
    public List<Path> PathList{get => pathList;}
    private Collider coll; 
    public Collider Coll{get => coll;}
    // Start is called before the first frame update
    private void Start() {
        pathList = new List<Path>();
        foreach(Transform k in transform){
            pathList.Add(k.GetComponent<Path>());
        }
        coll = GetComponent<Collider>();
        InspectionPathLinker.Add(this);
        //Debug.Log(coll);
    }

    // Update is called once per frame
    private void Update() {
        
    }

    public bool IsInside(Vector3 position){
        return coll.bounds.Contains(position);
    }
}
