using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {
    private static ItemSpawner instance;
    public static ItemSpawner Instance{get => instance;}
    [SerializeField] private ObjectPoolDestroyer itemPool;
    private Dictionary<Item, GameObject> itemsPrefab;
    [SerializeField] private GameObject key1, key2, key3;

    private void Awake(){
        if(instance != null){
            Destroy(instance);
        }
        instance = this;
        itemsPrefab = new Dictionary<Item, GameObject>();
        itemsPrefab[Item.Key1] = key1;
        itemsPrefab[Item.Key2] = key2;
        itemsPrefab[Item.Key3] = key3;
    }

    public void SpawnAt(Item item, Vector3 position){
        if(item != Item.None){
            itemPool.Add(Instantiate(itemsPrefab[item], position+new Vector3(0, 1, 0), Quaternion.identity));
        }
    }
}
