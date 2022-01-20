using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveRoof : MonoBehaviour
{
    [SerializeField] private GameObject roofs;
    
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            roofs.SetActive(false);
        }
    }
    
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            roofs.SetActive(true);
        }
    }
}
