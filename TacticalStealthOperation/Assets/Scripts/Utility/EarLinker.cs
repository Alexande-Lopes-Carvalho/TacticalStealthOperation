using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarLinker : MonoBehaviour {
    private static EarLinker instance;
    public static EarLinker Instance{get=>instance;}
    private List<Entity> entities;
    private void Awake(){
        if(instance != null){
            Destroy(instance.gameObject);
        }
        instance = this;
        entities = new List<Entity>();
    }

    // Start is called before the first frame update
    private void Start() {
        
    }

    // Update is called once per frame
    private void Update() {
        
    }

    public void Register(Entity e){
        entities.Add(e);
    }

    public void Remove(Entity e){
        entities.Remove(e);
    }

    public void NoiseAt(Vector3 noisePosition, Transform noiseMaker, float sqrRange){
        InspectionPathLinker.Instance.SetPath(noiseMaker.position);
        foreach(Entity k in entities){
            if(k.transform != noiseMaker && Vector3.SqrMagnitude(k.EarTransform.position-noisePosition) < sqrRange){
                k.Ear(noiseMaker);
            }
        }
    }
}
