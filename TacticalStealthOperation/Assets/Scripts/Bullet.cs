using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // TODO : fix bug of distance not detecting trigger
    // TODO : multiple shoot mods
    private void OnTriggerEnter(Collider other)
    {
        Debug.print(other.transform.name);
        if (other.transform.tag == "Human"){
            Debug.print("Touched !");
        }
     //   Destroy(gameObject);
        // TODO : pool management
    }

}
