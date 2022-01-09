using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : Human, IPathComponent {
    [SerializeField]
    private Path patrolPath; 
    private NavMeshAgentPathFollower nvPathFollower;
    private Vector3 lastPosition;
    private float maxSpeed = 7f;
    public override void Start(){
        base.Start();
        nvPathFollower = GetComponent<NavMeshAgentPathFollower>();
        nvPathFollower.FollowPath(patrolPath);
        lastPosition = transform.position;
    }
    
    public void OnPathEnd(){

    }

    public override void Update(){
        base.Update();
        float speed = (transform.position-lastPosition).magnitude/Time.deltaTime;
        animator.SetFloat(speedAnimation, Mathf.Min(speed/maxSpeed, 1.0f));

        lastPosition = transform.position;
    }
}
