using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // TODO : fix bug of distance not detecting trigger
    // TODO : multiple shoot mods
    private void OnTriggerEnter(Collider other)
    {
        //Debug.print(other.transform.name);
        if (other.transform.tag == "Human"){
            gameObject.SetActive(false);
            Debug.print("Touched !");
        }
     //   Destroy(gameObject);
        // TODO : pool management
    }


    private void Update()
    {
        CheckHumanCollision();
    }
    public bool CheckHumanCollision(){
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position,gameObject.transform.up,out hit, 1)){
            Debug.print(hit.transform.tag);
            if (hit.transform.tag == "Human"){
                Debug.print("Bullet hit the target.");
            }
            return hit.transform.tag == "Human";
        }
        return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(gameObject.transform.position,gameObject.transform.up);
    }

}
