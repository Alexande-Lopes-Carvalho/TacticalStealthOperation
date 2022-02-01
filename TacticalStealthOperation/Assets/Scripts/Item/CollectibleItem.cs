using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : MonoBehaviour {
    [SerializeField] private Item item;
    private Vector3 startPosition, startEuler;
    private float angPos, angEul;
    // Start is called before the first frame update
    private void Start() {
        startPosition = transform.position;
        startEuler = transform.eulerAngles;
        angPos = 0;
        angEul = 0;
    }

    // Update is called once per frame
    private void Update() {
        angPos = (angPos+Time.deltaTime*2)%(2*Mathf.PI);
        angEul = (angEul+Time.deltaTime*0.75f)%(2*Mathf.PI);
        transform.position = new Vector3(transform.position.x, startPosition.y+Mathf.Cos(angPos)*0.075f, transform.position.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, startEuler.z+Mathf.Cos(angEul)*2.5f);
    }

    private void OnTriggerEnter(Collider coll){
        ItemHolder itemHolder = coll.GetComponent<ItemHolder>();
        if(itemHolder != null){
            itemHolder.Add(item);
            Destroy(gameObject);
        }
    }
}
