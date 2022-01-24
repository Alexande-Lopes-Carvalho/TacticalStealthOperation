using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CameraFollow : MonoBehaviour {
    [SerializeField] public Transform target;
    [SerializeField] private float smoothSpeed = 0.125f;
    private Vector3 vec;
    private void Start(){
        vec = new Vector3(0, 8.43f, -2.72f);
        transform.eulerAngles = new Vector3(67.41f, 0, 0);
    }
    
    private void FixedUpdate() {
        Vector3 desiredPosition = target.position+vec;
        transform.position += (desiredPosition-transform.position)*0.125f;
    }
}
