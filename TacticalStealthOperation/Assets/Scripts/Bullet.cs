using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // TODO : fix bug of distance not detecting trigger
    // TODO : multiple shoot mods
    private void OnTriggerEnter(Collider other)
    {
        Debug.print(other.transform.tag);
        if (other.transform.tag == "Human"){
            gameObject.SetActive(false);
            Debug.print("Touched !");
        }
        // TODO : Check if it's a correct behaviour or not (or make sure tue bullet stays on the ground / disappear after hit)
    }


    private void Update()
    {
        CheckHumanCollision();
    }
    public bool CheckHumanCollision(){
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position,gameObject.transform.up,out hit, 5)){
            Debug.print($"{hit.transform.tag} / {hit.transform.name}");
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
