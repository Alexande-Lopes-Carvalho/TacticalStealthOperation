using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : Change so it's suitable for every human object and not a global thing for only one human
public class ShootManager : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private ObjectPooler pool;

    [SerializeField] private Transform spawnTransform;
    [SerializeField] private Transform weaponTransform;
    [SerializeField] private Human human;
    [SerializeField] private Rigidbody bullet;
    [SerializeField] private float forceMode;
    [SerializeField] private Transform target;
    
    // TODO : make sure all the instructions are not global but depends on the weapon and used in Weapon
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject o = Instantiate(bullet.gameObject,spawnTransform.position+weaponTransform.up.normalized,Quaternion.identity);
            o.GetComponent<Rigidbody>().AddForce(forceMode*weaponTransform.up,ForceMode.Impulse);
            weapon.OnStartShoot();
        }

        /* TO REMOVE */
           // target.position = spawnTransform.position+weaponTransform.up.normalized-Vector3.up;
        /**/
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawRay(spawnTransform.position,weaponTransform.up);
    // }
}
