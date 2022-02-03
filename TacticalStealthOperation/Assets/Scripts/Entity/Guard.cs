using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard : Human, IPathComponent {
    private static int priorityCount; // initialized in HumanLinker
    public static int PriorityCount{get => priorityCount; set => priorityCount = value;}
    [SerializeField] private Path patrolPath;
    [SerializeField] private float visualAcuity = 1;
    public float VisualAcuity{get => visualAcuity; set => visualAcuity = value;}
    private float sqrVisualAcuity;
    [SerializeField] private float visualAcuityInDarkness = 1;
    public float VisualAcuityInDarkness{get => visualAcuityInDarkness; set => visualAcuityInDarkness = value;}
    private float sqrVisualAcuityInDarkness;
    [SerializeField][Min(0)] private float visualAngle;
    private float visualAngleRadians;
    public float VisualAngle{get => visualAngle;}
    [SerializeField][Min(0)] private float surroundingAwarness;
    public float SurroundingAwarness{get => surroundingAwarness; set => surroundingAwarness = value;}
    private float sqrSurroundingAwarness;

    [SerializeField] private float targetAcquiredDist = 1;
    public float TargetAcquiredDist{get => targetAcquiredDist; set => targetAcquiredDist = value;}
    private float sqrTargetAcquiredDist;
    
    private UnityEngine.AI.NavMeshAgent agent;
    private GuardState currentState;
    private NavMeshAgentPathFollower nvPathFollower;
    private PathFollower.PathStateProvider patrolSave;

    private Human target;
    private float lastPathToTarget; // time
    private bool canSeeTarget = false;
    private Vector3 lastGuardPosition, lastGuardForward;
    private float transitionAttack = 0; // 0 : look where walking | 1 : look to target
    private float transitionAttackSpeed = 1;
    private float timeCheck = 0;

    private Path generatedInspectionPath;
    

    [SerializeField] private Item dropOnKill;
    public override void Start(){
        base.Start();
        nvPathFollower = GetComponent<NavMeshAgentPathFollower>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        sqrVisualAcuity = visualAcuity*visualAcuity;
        sqrTargetAcquiredDist = targetAcquiredDist*targetAcquiredDist;
        sqrSurroundingAwarness = surroundingAwarness*surroundingAwarness;
        visualAngleRadians = Mathf.Deg2Rad*visualAngle;
        lastGuardForward = transform.forward;
        lastGuardPosition = transform.position;
        currentState = GuardState.NO_STATE;
        agent.avoidancePriority = priorityCount++;
        if(patrolPath == null){
            patrolPath = InspectionPathLinker.Instance.GenerateStayPath(transform.position, transform.eulerAngles.y);
            patrolPath.transform.parent = transform;
        }
        Patrol();
    }

    private void SetState(GuardState s){
        if(currentState == GuardState.ATTACK){
            ReleaseWeaponTrigger();
        }
        if(currentState == GuardState.PATROL){
            patrolSave = nvPathFollower.StopFollowingPath();;
        } else {
            nvPathFollower.StopFollowingPath();
        }
        currentState = s;
        target = null;
    }

    private void NoState(){
        SetState(GuardState.NO_STATE);
    }

    private void Patrol(){
        if(currentState != GuardState.PATROL){
            SetState(GuardState.PATROL);
            nvPathFollower.FollowPath(patrolPath, patrolSave);
        }
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
        lastGuardForward = transform.forward;
        lastGuardPosition = transform.position;
    }
    
    public void OnPathEnd(){
        Patrol();
    }

    public override void Damage(int damage){
        if(IsDead()){
            return;
        }
        base.Damage(damage);
    }

    public override void Damage(int damage, Transform origin){
        if(IsDead()){
            return;
        }
        if(currentState == GuardState.ATTACK){
            base.Damage(damage);
        } else {
            base.Damage(damage*2);
            if(IsDead()){
                return;
            }
            SetGeneratedPath(InspectionPathLinker.Instance.GeneratePathTo(origin.position));
            if(!IsPathValid(origin.position)){
                SetGeneratedPath(InspectionPathLinker.Instance.GeneratePathTo(transform.position));
            }
            Inspect(generatedInspectionPath);
        }
        
    }

    public override void FixedUpdate(){
        base.FixedUpdate();
        if(currentState == GuardState.ATTACK){
            Vector3 toTarget = (target.transform.position-transform.position);
            Vector3 toWalk = (lastGuardForward);
            transitionAttack = (canSeeTarget)? Mathf.Min(transitionAttack+Time.deltaTime*transitionAttackSpeed, 1) : Mathf.Max(transitionAttack-Time.deltaTime*transitionAttackSpeed, 0);
            //Debug.Log(Vector3.SignedAngle(toWalk, toTarget, transform.up) + " " + toWalk);
            transform.LookAt(transform.position+Quaternion.AngleAxis(transitionAttack*Vector3.SignedAngle(toWalk, toTarget, transform.up), transform.up)*toWalk);
            //Debug.Log(Time.time + " " + toTarget + " " + toWalk + " " + transitionAttack);
        }
        RefreshTurnAnimation();
        Vector3 v = (transform.position-lastGuardPosition);
        if(v.sqrMagnitude > 0.01f){
            //Debug.Log(Time.time + " Set last");
            lastGuardPosition = transform.position;
            lastGuardForward = v.normalized;
        }
        //Debug.Log(Time.time + " " + transitionAttackSpeed + " " + Mathf.Max(1, transitionAttackSpeed-Time.deltaTime*1));
        transitionAttackSpeed = Mathf.Max(1, transitionAttackSpeed-Time.deltaTime*1);
        
    }

    public override void Update(){
        base.Update();
        RefreshMoveAnimation();
        if(currentState == GuardState.ATTACK){
            if(target.IsDead()){
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
                        agent.CalculatePath(target.transform.position, path);
                        agent.SetPath(path);
                    }
                } else { // stop moving
                    //Debug.Log(Time.time + " Stop Moving");
                    agent.isStopped = true;
                    agent.ResetPath();
                }

                if(canSeeTarget){
                    if(Weapon.HasBulletLeft()){
                        PressWeaponTrigger();
                    } else {
                        ReleaseWeaponTrigger();
                        animator.SetTrigger("doReload");
                    }
                } else {
                    ReleaseWeaponTrigger();
                }

                //Debug.DrawLine(Eyes.position, target.Eyes.position, (canSeeTarget)? Color.red : Color.blue);
            }
        } else if((currentState == GuardState.PATROL || currentState == GuardState.INSPECT) && (Time.time-timeCheck >= 0.15)) {
            foreach(Character k in HumanLinker.Instance.Characters){
                canSeeTarget = CanSeeTarget(k);
                if(canSeeTarget){
                    Attack(k);
                    break;
                }
            }
            timeCheck = Time.time;
        }
    }

    public override void Kill(){
        base.Kill();
        Destroy(nvPathFollower);
        Destroy(agent);
        Destroy(GetComponent<CapsuleCollider>());
        ItemSpawner.Instance.SpawnAt(dropOnKill, transform.position);
    }

    private bool CanSeeTarget(Human target){
        if(target.IsDead()){
            return false;
        }
        Vector3 direction = target.Eyes.position-Eyes.position;
        if((transform.position-target.transform.position).sqrMagnitude < sqrSurroundingAwarness){
            transitionAttackSpeed = 3;
        } else if(Mathf.Acos(Vector3.Dot(direction.normalized, transform.forward)) > visualAngleRadians){
            return false;
        }

        RaycastHit hit;
        if(Physics.Raycast(Eyes.position, direction, out hit, (target.IsInLight)? visualAcuity : visualAcuityInDarkness)){
            //Debug.Log(Time.time + " Ray : " + (hit.transform == target.transform));
            return hit.transform == target.transform;
        }
        //Debug.Log(Time.time + " NOTHING");
        return false;
    }

    private bool CanSeeTarget(){
        return CanSeeTarget(target);
    }

    private void ResetGeneratedPath(){
        if(generatedInspectionPath != null){
            Destroy(generatedInspectionPath.gameObject);
            generatedInspectionPath = null;
        }
    }
    private void SetGeneratedPath(Path p){
        ResetGeneratedPath();
        generatedInspectionPath = p;
        generatedInspectionPath.transform.parent = transform;
    }

    private bool IsPathValid(Vector3 destination){
        NavMeshPath path = new UnityEngine.AI.NavMeshPath();
        agent.CalculatePath(destination, path);
        return path.corners.Length > 0 && (destination-path.corners[path.corners.Length-1]).sqrMagnitude < 0.1;
    }

    public override void Ear(Transform t){
        if(!IsAlive() || currentState == GuardState.ATTACK){
            return;
        }
        Guard e = t.GetComponent<Guard>();
        if(e != null && e.currentState == GuardState.ATTACK){
            if(IsPathValid(e.target.transform.position)){
                Attack(e.target);
            }
        } else {
            Path inspect = null;
            if(InspectionPathLinker.Instance.CurrentPathList == null || InspectionPathLinker.Instance.IsInsideCurrent(transform)){
                SetGeneratedPath(InspectionPathLinker.Instance.GenerateDefaultPath());
                if(IsPathValid(InspectionPathLinker.Instance.Position)){
                    inspect = generatedInspectionPath;
                }
            } else {
                //Debug.Log(Time.time + " " + name + " eard " + t.gameObject.name);
                
                float dist = -1;
                foreach(Path k in InspectionPathLinker.Instance.CurrentPathList){
                    NavMeshPath path = new UnityEngine.AI.NavMeshPath();
                    agent.CalculatePath(k.PathStates[0].destination, path);
                    if(path.corners.Length > 0 && (k.PathStates[0].destination-path.corners[path.corners.Length-1]).sqrMagnitude < 0.1){
                        float evDist = EvaluateDistance(path, transform.position);
                        if(dist == -1 || evDist < dist){
                            dist = evDist;
                            inspect = k;
                        }
                    }
                }
            }
            //Debug.Log(Time.time + " " + inspect);
            if(inspect != null){
                Inspect(inspect);
            }
        }
    }
    private float EvaluateSqrDistance(NavMeshPath path, Vector3 position){
        Vector3 p = position;
        float res = 0;
        foreach(Vector3 v in path.corners){
            res += Vector3.SqrMagnitude(p-v);
            p = v;
        }
        return res;
    }
    private float EvaluateDistance(NavMeshPath path, Vector3 position){
        Vector3 p = position;
        float res = 0;
        foreach(Vector3 v in path.corners){
            res += Vector3.Magnitude(p-v);
            p = v;
        }
        return res;
    }

    protected override void OnTriggerEnter(Collider coll){
        // don't call base method (don't need data about guard visibility)
    }
    protected override void OnTriggerExit(Collider coll){
        // don't call base method (don't need data about guard visibility)
    }

    enum GuardState {
        NO_STATE = -1,
        PATROL = 0,
        INSPECT = 1,
        ATTACK = 2
    }
}
