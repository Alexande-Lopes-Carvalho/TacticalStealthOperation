using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    [SerializeField] private float damage;
    [SerializeField] private int MaxBulletsInMagazine;
    [SerializeField] private int bulletsInMagazine;

    [SerializeField] private Animator weaponAnimator;
    [SerializeField] private GameObject magazine;
    [SerializeField] private GameObject cartridgeSpawn;
    [SerializeField] private ObjectPooler cartridgePool;
    [SerializeField] private float cartridgeDeltaY = 0.05f, cartridgeDeltaZ = 0.05f, cartridgeMinRotation = 0.05f, cartridgeMaxRotation = 0.05f, cartridgeStrength = 1;
    public GameObject Magazine{get => magazine;}
    protected static readonly int doAction = Animator.StringToHash("doAction");

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }
    
    /* Called by Entity that hold the weapon */
    public void Shoot(){
        weaponAnimator.SetTrigger("doShoot");
    }
    /*
        Shooting Animation Event function ...
    */
    public void OnStartShoot(){
        GameObject o = cartridgePool.SpawnAt(cartridgeSpawn.transform.position, cartridgeSpawn.transform.eulerAngles);
        Vector3 origin = cartridgeSpawn.transform.position;
        float angA = Mathf.PI+Random.Range(-cartridgeDeltaY/2, cartridgeDeltaY/2), angB = Random.Range(-cartridgeDeltaZ/2, cartridgeDeltaZ/2), angC = Random.Range(cartridgeMinRotation, cartridgeMaxRotation);
        Vector3 m = new Vector3(Mathf.Cos(angA)*Mathf.Cos(angB), Mathf.Sin(angB), Mathf.Sin(angA)*Mathf.Cos(angB));
        m = m*(cartridgeStrength/m.magnitude);
        cartridgeSpawn.transform.Translate(m);
        Debug.print(cartridgeSpawn.transform.position-origin);
        Rigidbody r = o.GetComponent<Rigidbody>();
        r.AddForce(cartridgeSpawn.transform.position-origin, ForceMode.Impulse);
        r.AddTorque(cartridgeSpawn.transform.up*angC, ForceMode.Impulse);

        cartridgeSpawn.transform.position = origin;
    }


    /* Called by Entity that hold the weapon */
    public void OnReloadAction(){
        weaponAnimator.SetTrigger("doReload");
    }
    public void OnExtractEmptyMagazine(){
        Magazine.SetActive(false);
    }
    public void OnPutNewMagazine(){
        Magazine.SetActive(true);
    }
}
