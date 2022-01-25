using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Path : MonoBehaviour {
    
    [SerializeField] private PathType type;
    public PathType Type {get=> type;}
    [SerializeField] private int lap = 0;
    public int Lap{get => lap;}
    [SerializeField] private List<PathState> pathStates; // should contains time and facing
    public List<PathState> PathStates{get=> pathStates;}
    [SerializeField] private Vector3 offset = new Vector3(0, 0.1f, 0); // Offset in order to make pathfinding work
    // Start is called before the first frame update
    void Start() {
        if(type == PathType.DoOnce){
           lap = 0;
        }
        for(int i = 0; i < pathStates.Count; ++i){
            pathStates[i].destination += offset;
            //Debug.Log(" i : " + i + " " + pathStates[i].Destination + " " + pathStates[i].timeToWait + " " + pathStates[i].facingRotation);
        }
    }

    // Update is called once per frame
    void Update() {
        
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

    }
}