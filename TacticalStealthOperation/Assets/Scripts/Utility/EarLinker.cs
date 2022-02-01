using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarLinker : MonoBehaviour {
    private static List<Entity> entities;
    private void Awake(){
        entities = new List<Entity>();
    }

    // Start is called before the first frame update
    private void Start() {
        
    }

    // Update is called once per frame
    private void Update() {
        
    }

    public static void Register(Entity e){
        entities.Add(e);
    }

    public static void Remove(Entity e){
        entities.Remove(e);
    }

    public static void NoiseAt(Vector3 noisePosition, Transform noiseMaker, float sqrRange){
        InspectionPathLinker.SetPath(noiseMaker.position);
        foreach(Entity k in entities){
            if(k.transform != noiseMaker && Vector3.SqrMagnitude(k.EarTransform.position-noisePosition) < sqrRange){
                k.Ear(noiseMaker);
            }
        }
    }
}
