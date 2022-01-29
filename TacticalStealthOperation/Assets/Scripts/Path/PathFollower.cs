using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PathFollower : MonoBehaviour {
    private float closeToDestination = 5f*5f, closeToRotation = 0.01f;
    private Vector3 destination;
    public Vector3 Destination {get => destination;}
    private Quaternion facingRotation;
    public Quaternion FacingRotation {get => facingRotation;}
    private float timeToWait;
    public float TimeToWait {get => timeToWait;}

    private IEnumerator moveToDestination, rotateToFacing, waitUntil;
    private PathStateProvider pathStateProvider;

    private IPathComponent pathComponent;

    // Start is called before the first frame update
    public virtual void Awake() {
        pathComponent = GetComponent<IPathComponent>();
    }

    public IEnumerator MoveToDestinationCoroutine(){
        SetDestination(pathStateProvider.GetDestination());
        while(!IsDestinationAcquired()){
            MoveToDestination();
            yield return new WaitForFixedUpdate();
        }
        if(!pathStateProvider.GetNoRotation()){
            rotateToFacing = RotateToFacingCoroutine();
            StartCoroutine(rotateToFacing);
        } else if(pathStateProvider.GetTimeToWait() != 0){
            waitUntil = WaitUntil();
            StartCoroutine(waitUntil);
        } else {
            ComputeNextPathState();
        }
    }
    public IEnumerator RotateToFacingCoroutine(){
        SetFacingRotation(pathStateProvider.GetFacingRotation());
        while(!IsFacingAcquired()){
            RotateToFacing();
            yield return new WaitForFixedUpdate();
        }
        if(pathStateProvider.GetTimeToWait() != 0){
            waitUntil = WaitUntil();
            StartCoroutine(waitUntil);
        } else {
            ComputeNextPathState();
        }
    }

    public IEnumerator WaitUntil(){
        SetTimeToWait(pathStateProvider.GetTimeToWait());
        yield return new WaitForSeconds(timeToWait);
        ComputeNextPathState();
    }

    public void ComputeNextPathState(){
        if(pathStateProvider.ComputeNextPosition()){
            moveToDestination = MoveToDestinationCoroutine();
            StartCoroutine(moveToDestination);
            return ;
        }
        endPath();
    }

    protected void endPath(){
        StopFollowingPath();
        pathComponent.OnPathEnd();
    }

    public void FollowPath(Path path){
        StopFollowingPath();
        pathStateProvider = new PathStateProvider(path);
        ComputeNextPathState();
    }

    public virtual void StopFollowingPath(){
        if(moveToDestination != null){
            StopCoroutine(moveToDestination);
        }
        if(rotateToFacing != null){
            StopCoroutine(rotateToFacing);
        }
        if(waitUntil != null){
            StopCoroutine(waitUntil);
        }
    }

    // Update is called once per frame
    void Update() {
        
    }

    void FixedUpdate(){
        //Debug.Log(Time.time + " FixedUpdate");
    }

    public virtual void SetDestination(Vector3 _destination) {
        destination = _destination;
    }
    public abstract void MoveToDestination();

    public virtual bool IsDestinationAcquired(){
        Vector3 v = transform.position-destination;
        return v.sqrMagnitude < closeToDestination;
    }


    public virtual void SetFacingRotation(Vector3 _facingRotation) {
        facingRotation = Quaternion.Euler(_facingRotation);
    }
    public abstract void RotateToFacing();
    public bool IsFacingAcquired(){
        return Quaternion.Angle(transform.rotation, facingRotation) <= closeToRotation;
    }

    public virtual void SetTimeToWait(float _timeToWait){
        timeToWait = _timeToWait;
    }

    private class PathStateProvider {
        private Path path;
        private int currentLap = 0;
        private int currentIndex = -1, gain = 1;
        private bool isFinished = false;
        public PathStateProvider(Path _path){
            path = _path;
        }

        public bool ComputeNextPosition(){
            if(isFinished || path.PathStates.Count == 0 || (currentIndex == path.PathStates.Count-1 && path.Type == Path.PathType.DoOnce)){
                currentIndex = -1;
                isFinished = true;
                return false;
            }

            if(path.Type == Path.PathType.BackAndForth){
                gain = (currentIndex == path.PathStates.Count-1)? -1 : (currentIndex == 0)? 1 : gain; 
                currentIndex = currentIndex+gain;
            } else {
                currentIndex = ++currentIndex%path.PathStates.Count;
            }
            
            if(currentIndex == 1 && gain == 1){
                ++currentLap;
                if(path.Lap != 0 && currentLap > path.Lap){
                    currentIndex = -1;
                    isFinished = true;
                    return false;
                }
            }
            return true;
        }

        public Vector3 GetDestination(){
            return path.PathStates[currentIndex].Destination;
        }
        public bool GetNoRotation(){
            return path.PathStates[currentIndex].NoRotation;
        }
        public Vector3 GetFacingRotation(){
            return path.PathStates[currentIndex].FacingRotation;
        }
        public float GetTimeToWait(){
            return path.PathStates[currentIndex].TimeToWait;
        }
    }
}

public interface IPathComponent {
    void OnPathEnd();
}
