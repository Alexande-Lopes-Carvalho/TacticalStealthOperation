using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectionPathLinker : MonoBehaviour {
    private static List<InspectionPath> inspectionPathList;
    private static List<Path> currentPathList;
    public static List<Path> CurrentPathList{get => currentPathList;}

    private static Path prefab;
    private static Transform t;
    private static Collider currentCollider;

    private static Vector3 offset = new Vector3(0, 0.1f, 0), position;
    public static Vector3 Position{get => position;}
    private void Awake(){
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

    public static void SetPath(Vector3 _position){
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

    public static Path GeneratePathTo(Vector3 position){
        Path res = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        Path.PathState a = new Path.PathState(position, false, new Vector3(0, -90, 0), 1);
        Path.PathState b = new Path.PathState(position, false, new Vector3(0, 90, 0), 1);
        res.PathStates.Add(a);
        res.PathStates.Add(b);
        return res;
    }

    public static Path GenerateStayPath(Vector3 position, float euler){
        Path res = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        res.Type = Path.PathType.Loop;
        Path.PathState a = new Path.PathState(position, false, new Vector3(0, euler, 0), 3600);
        res.PathStates.Add(a);
        return res;
    }

    public static Path GenerateDefaultPath(){
        return GeneratePathTo(position);
    }

    public static bool IsInsideCurrent(Transform t){
        return currentCollider.bounds.Contains(t.position+offset);
    }

    public static void Add(InspectionPath p){
        //Debug.Log("InspectionPathLinker Add " + p);
        inspectionPathList.Add(p);
    }
}
