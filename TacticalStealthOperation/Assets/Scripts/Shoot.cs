using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private Transform spawnTransform; // TO REMOVE (only used to draw gizmos)
    [SerializeField] private Transform weaponTransform;
    [SerializeField] private float forceMode;
    
    public void ApplyShoot(GameObject o){
        o.GetComponent<Rigidbody>().AddForce(forceMode*weaponTransform.up,ForceMode.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(spawnTransform.position,500*weaponTransform.up);
    }
}
