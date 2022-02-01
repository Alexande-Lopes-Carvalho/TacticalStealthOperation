using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerListener {
    void Notify(ObservableTrigger.TriggerEvent ev, Collider other);
}

public class ObservableTrigger : MonoBehaviour {
    private ITriggerListener listener;
    // Start is called before the first frame update
    public void Register(ITriggerListener _listener){
        listener = _listener;
    }

    private void OnTriggerEnter(Collider other){
        if(listener != null){
            listener.Notify(TriggerEvent.OnTriggerEnter, other);
        }
    }

    public enum TriggerEvent{
        OnTriggerEnter
    }
}
