using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Human {
    [SerializeField] private Camera cam;
    private Rigidbody rb;
    private float speed = 3.9f*10000f;

    // Start is called before the first frame update
    public override void Start() {
        base.Start();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public override void FixedUpdate() {
        base.FixedUpdate();
        float vertical = Input.GetAxis("Vertical")*speed;
        float horizontal = Input.GetAxis("Horizontal")*speed;

        rb.AddForce(new Vector3(horizontal, 0, vertical).normalized*Time.deltaTime*speed);
        //transform.position+= new Vector3(horizontal, 0, vertical).normalized*Time.deltaTime*speed;
        RefreshMoveAnimation();
    }

    public override void Update() {
        base.Update();
        SetDirection();
        RefreshTurnAnimation();
    }

    public void SetDirection(){
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        float div = Vector3.Dot(ray.direction, Vector3.up);
        if(div == 0.0f){
            return;
        }
        float k = Vector3.Dot(transform.position-ray.origin, Vector3.up)/div;
        if(k < 0.0f){
            return;
        }
        Vector3 collision = ray.origin+ray.direction*k;
        transform.LookAt(collision);
    }
}

/*
float rayPlane(vec3 rayPos, vec3 rayDir, vec3 planePos, vec3 planeNormal, out vec3 collision, out vec3 normalCollision){
    float div = dot(rayDir, planeNormal);
    if(div == 0.f){
        return -1.f;
    }
    float k = dot(planePos-rayPos, planeNormal)/div;
    if(k < 0.f){
        return -1.f;
    }
    collision = rayPos+rayDir*k;
    normalCollision = planeNormal;
    return distance(rayPos, collision); // if rayDir normalized, dist = k
}
*/