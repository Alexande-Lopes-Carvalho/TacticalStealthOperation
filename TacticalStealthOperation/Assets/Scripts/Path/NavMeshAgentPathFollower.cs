using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavMeshAgentPathFollower : PathFollower {
    private UnityEngine.AI.NavMeshAgent agent;

    private Vector3 lastPosition;
    private float angularSpeed, currentTime, rotationTime;
    private Quaternion startRotation;

    // Start is called before the first frame update
    public override void Awake() {
        base.Awake();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        lastPosition = transform.position;
        angularSpeed = (float)(agent.angularSpeed);
    }

    // Update is called once per frame
    void Update() {
        
    }
    public override void StopFollowingPath(){
        agent.isStopped = true;
        agent.ResetPath();
        base.StopFollowingPath();
    }

    public override void SetDestination(Vector3 _destination) {
        base.SetDestination(_destination);
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(_destination, path);
        if(path.status == NavMeshPathStatus.PathPartial){
            agent.SetPath(path);
            Debug.Log("Found path");
        }
        Debug.Log("Set Destination " + _destination);
    }

    public override void MoveToDestination(){
        transform.LookAt(transform.position+(transform.position-lastPosition));
        lastPosition = transform.position;
    }

    public override bool IsDestinationAcquired(){
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && (agent.hasPath || agent.velocity.sqrMagnitude == 0f);
    }

    public override void SetFacingRotation(Vector3 _facingRotation) {
        base.SetFacingRotation(_facingRotation);
        float angle = Quaternion.Angle(transform.rotation, FacingRotation);
        rotationTime = angle/angularSpeed;
        currentTime = 0;
        startRotation = transform.rotation;
    }

    public override void RotateToFacing(){
        transform.rotation = Quaternion.Slerp(startRotation, FacingRotation, Mathf.Min(currentTime/rotationTime, 1f));
        currentTime += Time.fixedDeltaTime;
    }
}
