using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CameraFollow : MonoBehaviour {
    [SerializeField] public Transform target;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private float offsetY = 10.0f;
    [SerializeField] private float offsetZ = -2.72f;
    [SerializeField] private float inclinaison;
    private Vector3 vec;
    void Start(){
        vec = new Vector3(0, offsetY, offsetZ);
        transform.eulerAngles = new Vector3(inclinaison, 0, 0);
    }
    
    void FixedUpdate() {
        Vector3 desiredPosition = target.position+vec;
        transform.position += (desiredPosition-transform.position)*smoothSpeed;
    }
}
