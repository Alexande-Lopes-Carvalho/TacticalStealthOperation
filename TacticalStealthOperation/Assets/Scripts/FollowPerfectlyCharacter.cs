using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class FollowPerfectlyCharacter : MonoBehaviour {
    [SerializeField] public Transform target;
    [SerializeField] private float offsetY = 10.0f;
    private Vector3 vec;
    void Start(){
        vec = new Vector3(0, offsetY, 0);
    }
    
    void FixedUpdate() {
        Vector3 desiredPosition = target.position + vec;
        transform.position = desiredPosition;
    }
}