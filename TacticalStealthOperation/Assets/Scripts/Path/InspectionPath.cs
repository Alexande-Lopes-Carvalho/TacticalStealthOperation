using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectionPath : MonoBehaviour {
    [SerializeField] private Path path;
    public Path Path{get => path;}
    [SerializeField] private Collider coll; 
    // Start is called before the first frame update
    private void Start() {
        InspectionPathLinker.Add(this);
    }

    // Update is called once per frame
    private void Update() {
        
    }

    public bool IsInside(Vector3 position){
        return coll.bounds.Contains(position);
    }
}
