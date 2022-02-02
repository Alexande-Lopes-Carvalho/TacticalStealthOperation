using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RandomizePosition : MonoBehaviour
{
    public List<Vector3> listPosKey1;
    public List<Vector3> listPosKey2;
    public List<Vector3> listPosKey3;
    [SerializeField] public GameObject key1;
    [SerializeField] public GameObject key2;
    [SerializeField] public GameObject key3;
    
    public void Start()
    {
        int indexKey1 = Random.Range(0, listPosKey1.Count);
        key1.transform.position = listPosKey1[indexKey1];
        int indexKey2 = Random.Range(0, listPosKey2.Count);
        key2.transform.position = listPosKey2[indexKey2];
        int indexKey3 = Random.Range(0, listPosKey3.Count);
        key3.transform.position = listPosKey3[indexKey3];
    }
}