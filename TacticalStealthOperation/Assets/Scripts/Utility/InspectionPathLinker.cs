using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectionPathLinker : MonoBehaviour {
    private static List<InspectionPath> inspectionPathList;
    private static Path currentPath;
    public static Path CurrentPath{get => currentPath;}

    private static Path prefab;
    private static Transform t;
    private void Awake(){
        inspectionPathList = new List<InspectionPath>();
        t = transform;
    }

    // Start is called before the first frame update
    private void Start() {
        prefab = transform.GetChild(0).GetComponent<Path>();
    }

    // Update is called once per frame
    private void Update() {
        
    }

    public static void SetPath(Vector3 position){
        for(int i = 0; i < inspectionPathList.Count; ++i){
            if(inspectionPathList[i].IsInside(position)){
                currentPath = inspectionPathList[i].Path;
                return;
            }
        }
        if(currentPath != null){
            Destroy(currentPath);
        }

        currentPath = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        Path.PathState a = new Path.PathState(position, false, new Vector3(0, -90, 0), 1);
        Path.PathState b = new Path.PathState(position, false, new Vector3(0, 90, 0), 1);
        currentPath.PathStates.Add(a);
        currentPath.PathStates.Add(b);
        currentPath.transform.parent = t;
    }

    public static void Add(InspectionPath p){
        inspectionPathList.Add(p);
    }
}
