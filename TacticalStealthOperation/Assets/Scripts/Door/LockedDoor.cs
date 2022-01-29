using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour, ITriggerListener {
    private float opened = -1;
    [SerializeField] private Item key;
    [SerializeField] private bool rotForward = true;
    [SerializeField] private ObservableTrigger obs;
    private float currentRotation;
    private float rotationStart;
    private float speed = 180;
    // Start is called before the first frame update
    private void Start() {
        obs.Register(this);
        rotationStart = transform.localEulerAngles .z;
    }

    // Update is called once per frame
    private void Update() {
        if(opened != -1 && currentRotation != 90){
            currentRotation = Mathf.Min((Time.time-opened)*speed, 90);
            Debug.Log(Time.time + " " + (currentRotation*(rotForward? -1 : 1)));
            transform.localEulerAngles  = new Vector3(0, 0, rotationStart+currentRotation*(rotForward? -1 : 1));
        }
    }

    public void Notify(ObservableTrigger.TriggerEvent ev, Collider other){
        if(opened==-1 && ev == ObservableTrigger.TriggerEvent.OnTriggerEnter){
            ItemHolder item = other.GetComponent<ItemHolder>();
            if(item != null && item.HasItem(key)){
                opened = Time.time;
                Destroy(obs.GetComponent<BoxCollider>());
            }
        }
    }
}
