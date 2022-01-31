using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField] private TrailRenderer trail;
    [SerializeField][Min(0)] private float maxDist;
    
    [SerializeField][Min(0)] private float velocity;
    private int damage;
    private Vector3 origin;
    private float currentDist;
    private Transform ignoreTransform;
    // Start is called before the first frame update
    private void Start() {
        
    }

    // Update is called once per frame
    private void Update() {
        if(currentDist > maxDist){
            End();
        }

        Vector3 direction = transform.up*velocity*Time.deltaTime;
        float magnitude = direction.magnitude;
        RaycastHit hit;
        //if(ignoreTransform == GameObject.Find("Character").transform){Debug.Log(Time.time + " " + transform.position + " to " + (transform.position+direction));}
        if(Physics.Raycast(transform.position, direction, out hit, magnitude)){
            if(hit.transform != ignoreTransform){
                Entity e = hit.transform.gameObject.GetComponent<Entity>();
                if(e != null){
                    e.Damage(damage);
                }
                End();
            }
        }
        transform.position += direction;
        currentDist += magnitude;
    }

    private void End(){
        gameObject.SetActive(false);
    }

    public void Set(Transform _ignoreTransform, int _damage){
        currentDist = 0;
        damage = _damage;
        ignoreTransform = _ignoreTransform;
        trail.Clear();
    }
}
