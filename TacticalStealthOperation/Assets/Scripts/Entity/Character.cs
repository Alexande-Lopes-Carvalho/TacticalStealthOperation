using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Human {
    [SerializeField] private Camera cam;
    private Rigidbody rb;
    [SerializeField] private float speed = 3f*100000f;
    public DisplayHealth healthBar;

    // Start is called before the first frame update
    public override void Start() {
        base.Start();
        rb = GetComponent<Rigidbody>();
        healthBar.SetMaxHealth(GetMaxHealth());
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
        ComputeIsInLight();
        if(Input.GetMouseButtonDown(0)){
            PressWeaponTrigger();
        } else if(Input.GetMouseButtonUp(0)){
            ReleaseWeaponTrigger();
        } else if(Input.GetKeyDown(KeyCode.R)){
            animator.SetTrigger("doReload");
        } else if(Input.GetKeyDown(KeyCode.E)){
            PickNearbyWeapon();
        }

        //Debug.DrawLine(transform.position, transform.position+new Vector3(0, 5, 0), (IsInLight)? Color.blue : Color.red, 0);
    }

    public override void Kill(){
        Destroy(rb);
        Destroy(GetComponent<CapsuleCollider>());
        base.Kill();
    }

    public void SetDirection(){
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        float div = Vector3.Dot(ray.direction, Vector3.up);
        if(div == 0.0f){
            return;
        }
        Vector3 offset = new Vector3(0, RightHand.transform.position.y, 0);
        float k = Vector3.Dot((transform.position+offset)-ray.origin, Vector3.up)/div;
        if(k < 0.0f){
            return;
        }
        Vector3 collision = ray.origin+ray.direction*k;
        transform.LookAt(collision-offset);   
    }

    public override void Damage(int damage, Transform origin){
        Damage(damage);
        healthBar.SetCurrentHealth(GetCurrentHealth());
    }

    public override void Heal(int heal, Transform origin){
        Heal(heal);
        healthBar.SetCurrentHealth(GetCurrentHealth());
    }

    //  PERFECT DIRECTION TO SHOOT TOWARD MOUSE (CAUSE JITTER DUE TO ANIMATION)
    /*public void SetDirection(){
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        float div = Vector3.Dot(ray.direction, Vector3.up);
        if(div == 0.0f){
            return;
        }
        Vector3 offset = new Vector3(0, RightHand.transform.position.y, 0);
        float k = Vector3.Dot((transform.position+offset)-ray.origin, Vector3.up)/div;
        if(k < 0.0f){
            return;
        }

        Vector3 toMouse = ray.origin+ray.direction*k-(transform.position+offset);
        Vector3 toHand = RightHand.transform.position-transform.position, handDirection = RightHand.transform.up;

        toMouse.y = 0;
        toHand.y = 0;
        handDirection.y = 0;
        Debug.Log(Time.time + " " + toMouse + " " + toHand + " " + handDirection);
        float toMouseDist = toMouse.magnitude;
        float a = Mathf.Pow(handDirection.x, 2) + Mathf.Pow(handDirection.z, 2);
        float b = 2*(toHand.x*handDirection.x+toHand.z*handDirection.z);
        float c = Mathf.Pow(toHand.x, 2)+Mathf.Pow(toHand.z, 2)-Mathf.Pow(toMouseDist, 2);

        float delta = b*b-4*a*c;
        if(delta < 0){
            return;
        }
        float w = (-b+Mathf.Sqrt(delta))/(2*a);

        Vector3 res = toHand+w*handDirection;
        Debug.Log(Time.time + " " + res.magnitude + " " + toMouseDist + " " + a + " " + b + " " + c + " " + w);
        Vector3 resToHand = Quaternion.AngleAxis(Vector3.SignedAngle(res, toMouse, Vector3.up), Vector3.up)*toHand;

        transform.LookAt(transform.position+Quaternion.AngleAxis(0.125f*Vector3.SignedAngle(res, toMouse, Vector3.up), Vector3.up)*transform.forward);
        rb.angularVelocity = Vector3.zero;
        Debug.DrawLine(Vector3.zero+offset, toMouse+offset, Color.red);
        offset.y = offset.y+0.1f;
        Debug.DrawLine(Vector3.zero+offset, toHand+offset, Color.green);
        Debug.DrawLine(toHand+offset, toHand+handDirection+offset, Color.green);
        offset.y = offset.y+0.1f;
        Debug.DrawLine(Vector3.zero+offset, resToHand+offset, Color.blue);
        Debug.DrawLine(resToHand+offset, Quaternion.AngleAxis(Vector3.SignedAngle(res, toMouse, Vector3.up), Vector3.up)*res+offset, Color.blue);
    }*/
    
}
