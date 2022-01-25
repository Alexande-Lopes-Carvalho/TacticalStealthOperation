using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : Human, IPathComponent {
    [SerializeField] private Path patrolPath; 
    private NavMeshAgentPathFollower nvPathFollower;
    
    public override void Start(){
        base.Start();
        nvPathFollower = GetComponent<NavMeshAgentPathFollower>();
        nvPathFollower.FollowPath(patrolPath);
    }
    
    public void OnPathEnd(){

    }

    public override void FixedUpdate(){
        base.FixedUpdate();
        RefreshTurnAnimation();
    }

    public override void Update(){
        base.Update();
        RefreshMoveAnimation();
    }

    public override void Ear(Transform t){
        Debug.Log(Time.time + " " + name + " eard " + t.gameObject.name);
    }
}
