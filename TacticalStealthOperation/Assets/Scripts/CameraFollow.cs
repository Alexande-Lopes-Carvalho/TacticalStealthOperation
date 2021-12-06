using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CameraFollow : MonoBehaviour {
    [SerializeField] public Transform target;
    public float smoothSpeed = 0.125f;
    public float offset;
    void Start(){
        offset = transform.position.y-target.position.y;
    }
    
    void FixedUpdate() {
        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y+offset, target.position.z);
        transform.position += (desiredPosition-transform.position)*0.125f;
    }
}
