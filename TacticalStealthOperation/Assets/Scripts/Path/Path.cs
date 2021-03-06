using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Path : MonoBehaviour {
    
    [SerializeField] private PathType type;

    public PathType Type {get=> type; set => type = value;}
    [SerializeField] private int lap = 0;
    public int Lap{get => lap; set => lap = value;}
    [SerializeField] private List<PathState> pathStates; // should contains time and facing
    public List<PathState> PathStates{get=> pathStates;}
    [SerializeField] private Vector3 offset = new Vector3(0, 0.1f, 0); // Offset in order to make pathfinding work
    // Start is called before the first frame update
    private void Start() {
        if(type == PathType.DoOnce){
           lap = 0;
        }
        //Debug.Log(transform.right + " " + transform.forward + " " + transform.parent.gameObject.name);
        for(int i = 0; i < pathStates.Count; ++i){
            pathStates[i].destination = offset+new Vector3(transform.position.x, 0,transform.position.z)+pathStates[i].destination.x*transform.right + pathStates[i].destination.z*transform.forward;
            if(!pathStates[i].NoRotation){
                pathStates[i].facingRotation = new Vector3(pathStates[i].facingRotation.x, pathStates[i].facingRotation.y+transform.eulerAngles.y, pathStates[i].facingRotation.z);
            }
        }

        /*Vector3 v = offset+((addTransform)? new Vector3(transform.position.x, 0,transform.position.z) : Vector3.zero);
        for(int i = 0; i < pathStates.Count; ++i){
            pathStates[i].destination += v;
            //Debug.Log(" i : " + i + " " + pathStates[i].Destination + " " + pathStates[i].timeToWait + " " + pathStates[i].facingRotation);
        }*/
    }

    // Update is called once per frame
    private void Update() {
        
    }

    public enum PathType {
        DoOnce = 0,
        BackAndForth = 1,
        Loop = 2
    }

    [System.Serializable]
    public class PathState {
        [SerializeField] public Vector3 destination;
        public Vector3 Destination {get => destination;}
        [SerializeField] public bool noRotation;
        public bool NoRotation {get => noRotation;}
        [SerializeField] public Vector3 facingRotation;
        public Vector3 FacingRotation {get => facingRotation;}
        [SerializeField] public float timeToWait;
        public float TimeToWait {get => timeToWait;}

        public PathState(){
        }
        public PathState(Vector3 _destination, bool _noRotation, Vector3 _facingRotation, float _timeToWait){
            destination = _destination;
            noRotation = _noRotation;
            facingRotation = _facingRotation;
            timeToWait = _timeToWait;
        }
    }
}