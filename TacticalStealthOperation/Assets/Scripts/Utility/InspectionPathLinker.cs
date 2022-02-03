using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectionPathLinker : MonoBehaviour {
    private static InspectionPathLinker instance;
    public static InspectionPathLinker Instance{get => instance;}
    private List<InspectionPath> inspectionPathList;
    private List<Path> currentPathList;
    public List<Path> CurrentPathList{get => currentPathList;}

    private Path prefab;
    private Transform t;
    private Collider currentCollider;

    private Vector3 offset = new Vector3(0, 0.1f, 0), position;
    public Vector3 Position{get => position;}
    private void Awake(){
        if(instance != null){
            Destroy(instance.gameObject);
        }
        instance = this;
        inspectionPathList = new List<InspectionPath>();
        t = transform;
        prefab = transform.GetChild(0).GetComponent<Path>();
    }

    // Start is called before the first frame update
    private void Start() {
        
    }

    // Update is called once per frame
    private void Update() {
        
    }

    public void SetPath(Vector3 _position){
        currentCollider = null;
        currentPathList = null;
        position = _position;
        for(int i = 0; i < inspectionPathList.Count; ++i){
            //Debug.Log("is Inside " + inspectionPathList[i].gameObject.transform.position + " " + position);
            if(inspectionPathList[i].IsInside(position+offset)){
                //Debug.Log("yes");
                currentPathList = inspectionPathList[i].PathList;
                currentCollider = inspectionPathList[i].Coll;
                return;
            }
        }
    }

    public Path GeneratePathTo(Vector3 position){
        Path res = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        Path.PathState a = new Path.PathState(position, false, new Vector3(0, -90, 0), 1);
        Path.PathState b = new Path.PathState(position, false, new Vector3(0, 90, 0), 1);
        res.PathStates.Add(a);
        res.PathStates.Add(b);
        return res;
    }

    public Path GenerateStayPath(Vector3 position, float euler){
        Path res = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        res.Type = Path.PathType.Loop;
        Path.PathState a = new Path.PathState(position, false, new Vector3(0, euler, 0), 3600);
        res.PathStates.Add(a);
        return res;
    }

    public Path GenerateDefaultPath(){
        return GeneratePathTo(position);
    }

    public bool IsInsideCurrent(Transform t){
        return currentCollider.bounds.Contains(t.position+offset);
    }

    public void Add(InspectionPath p){
        //Debug.Log("InspectionPathLinker Add " + p);
        inspectionPathList.Add(p);
    }
}
