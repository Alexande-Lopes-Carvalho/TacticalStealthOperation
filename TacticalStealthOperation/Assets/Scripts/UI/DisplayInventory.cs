using UnityEngine;

public class DisplayInventory : MonoBehaviour
{
    [SerializeField] private GameObject keyInventaire1;
    [SerializeField] private GameObject keyInventaire2;
    [SerializeField] private GameObject keyInventaire3;

    public void DisplayNewElement(Item item)
    {
        switch (item)
        {
            case Item.Key1 :
                keyInventaire1.SetActive(true);
                break;
            case Item.Key2 :
                keyInventaire2.SetActive(true);
                break;
            case Item.Key3 :
                keyInventaire3.SetActive(true);
                break;
        }
    }
}