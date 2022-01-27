using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard : Human, IPathComponent {
    [SerializeField] private Path patrolPath;
    [SerializeField] private float visualAcuity = 1;
    public float VisualAcuity{get => visualAcuity; set => visualAcuity = value;}
    private float sqrVisualAcuity;

    [SerializeField] private float targetAcquiredDist = 1;
    public float TargetAcquiredDist{get => targetAcquiredDist; set => targetAcquiredDist = value;}
    private float sqrTargetAcquiredDist;
    private UnityEngine.AI.NavMeshAgent agent;
    private GuardState currentState;
    private NavMeshAgentPathFollower nvPathFollower;

    private Human target;
    private float lastPathToTarget; // time
    private bool canSeeTarget = false;
    private Vector3 lastGuardPosition;
    private float transitionAttack = 0; // 0 : look where walking | 1 : look to target
    public override void Start(){
        base.Start();
        nvPathFollower = GetComponent<NavMeshAgentPathFollower>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        sqrVisualAcuity = visualAcuity*visualAcuity;
        sqrTargetAcquiredDist = targetAcquiredDist*targetAcquiredDist;
        lastGuardPosition = transform.position;
        Patrol();
    }

    private void SetState(GuardState s){
        currentState = s;
        target = null;
        nvPathFollower.StopFollowingPath();
    }

    private void Patrol(){
        SetState(GuardState.PATROL);
        nvPathFollower.FollowPath(patrolPath);
    }

    private void Inspect(Path inspectPath){
        SetState(GuardState.INSPECT);
        nvPathFollower.FollowPath(inspectPath);
    }

    public void Attack(Human e){
        SetState(GuardState.ATTACK);
        target = e;
        lastPathToTarget = -1;
        canSeeTarget = false;
    }
    
    public void OnPathEnd(){
        Patrol();
    }

    public override void FixedUpdate(){
        base.FixedUpdate();
        if(currentState == GuardState.ATTACK){
            Vector3 toTarget = (target.Eyes.position-Eyes.position);
            Vector3 toWalk = (transform.position-lastGuardPosition);
            transitionAttack = (canSeeTarget)? Mathf.Min(transitionAttack+Time.deltaTime*1, 1) : Mathf.Max(transitionAttack-Time.deltaTime*1, 0);
            Debug.Log(Vector3.SignedAngle(toWalk, toTarget, transform.up) + " " + toWalk);
            transform.LookAt(transform.position+Quaternion.AngleAxis(transitionAttack*Vector3.SignedAngle(toWalk, toTarget, transform.up), transform.up)*toWalk);
        }
        RefreshTurnAnimation();
        if((transform.position-lastGuardPosition).sqrMagnitude > 0.01f){
            lastGuardPosition = transform.position;
        }
    }

    public override void Update(){
        base.Update();
        RefreshMoveAnimation();
        if(currentState == GuardState.ATTACK){
            if(target == null){
                Patrol();
            } else {
                
                canSeeTarget = CanSeeTarget();
                float sqrDistToTarget = (target.Feet.position-Feet.position).sqrMagnitude;

                if(!canSeeTarget || sqrDistToTarget > sqrTargetAcquiredDist){ // Keep moving to target if is nowhere to be seen or is too far
                    //Debug.Log(Time.time + " Keep Moving " + canSeeTarget + " " + (sqrDistToTarget > sqrTargetAcquiredDist));
                    if(lastPathToTarget == -1 || Time.time-lastPathToTarget > sqrDistToTarget*0.00025){
                        //Debug.Log(Time.time + " Refresh Moving");
                        lastPathToTarget = Time.time;
                        NavMeshPath path = new UnityEngine.AI.NavMeshPath();
                        agent.CalculatePath(target.Eyes.position, path);
                        agent.SetPath(path);
                    }
                } else { // stop moving
                    //Debug.Log(Time.time + " Stop Moving");
                    agent.isStopped = true;
                    agent.ResetPath();
                }
                Debug.DrawLine(Eyes.position, target.Eyes.position, (canSeeTarget)? Color.red : Color.blue);
            }
        }
        
    }

    private bool CanSeeTarget(){
        Vector3 direction = target.Eyes.position-Eyes.position;
        RaycastHit hit;
        if(Physics.Raycast(Eyes.position, direction, out hit, visualAcuity)){
            Debug.Log(Time.time + " " + hit.transform.name);
            return hit.transform == target.transform;
        }
        Debug.Log(Time.time + " NOTHING");
        return false;
    }

    public override void Ear(Transform t){
        Debug.Log(Time.time + " " + name + " eard " + t.gameObject.name);
    }

    enum GuardState {
        PATROL = 0,
        INSPECT = 1,
        ATTACK = 2
    }
}
