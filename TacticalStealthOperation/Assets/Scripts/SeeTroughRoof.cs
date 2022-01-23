using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeTroughRoof : MonoBehaviour {
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] int layerMask = 6;

    private Transform obstruction;
    private List<MeshRenderer> obstructionMesh = new List<MeshRenderer>();
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        RaycastHit hit;
        Vector3 direction = (target.position+offset)-transform.position;
        if(Physics.Raycast(transform.position, direction, out hit, direction.magnitude, 1 << layerMask)){
            //Debug.Log(Time.time + " collide with " + hit.transform.gameObject.name + " " + hit.transform.gameObject.layer);
            if(obstruction != hit.transform){
                RemoveLastObstruction();
            }
            obstruction = hit.transform;
            MeshRenderer m = obstruction.gameObject.GetComponent<MeshRenderer>();
            if(m != null){
                obstructionMesh.Add(m);
            }
            foreach(Transform k in obstruction.transform){
                m = k.gameObject.GetComponent<MeshRenderer>();
                if(m != null){
                    obstructionMesh.Add(m);
                }
            }

            foreach(MeshRenderer k in obstructionMesh){
                k.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            }
        } else {
            //Debug.Log("noCollision");
            RemoveLastObstruction();
        }
    }

    private void RemoveLastObstruction(){
        if(obstruction != null){
            obstruction = null;
            foreach(MeshRenderer k in obstructionMesh){
                k.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            }
            obstructionMesh.Clear();
        }
    }
}
