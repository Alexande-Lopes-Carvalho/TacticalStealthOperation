using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour {

    [SerializeField] private List<Item> itemList;
    [SerializeField] private DisplayInventory inventory;
    
    // Start is called before the first frame update
    private void Start() {
        
    }

    // Update is called once per frame
    private void Update() {
        
    }

    public bool HasItem(Item item){
        foreach(Item k in itemList){
            if(k == item){
                return true;
            }
        }
        return false;
    }

    public void Add(Item item){
        itemList.Add(item);
        inventory.DisplayNewElement(item);
    }
}
