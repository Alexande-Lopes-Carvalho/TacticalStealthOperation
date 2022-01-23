using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    [SerializeField] private List<FireMode> fireMode;
    [SerializeField] private int fireModeIndex = 0;
    [SerializeField] private int burstBullet = 3;
    [SerializeField] private float fireRate = 16; // in ms
    public float FireRate{get => fireRate;}
    [SerializeField] private float reloadRate = 2900; // in ms
    public float ReloadRate{get => reloadRate;}
    [SerializeField] private WeaponCategory category;
    public WeaponCategory Category{get => category;}
    [SerializeField] private float damage;
    [SerializeField] private int MaxBulletsInMagazine;
    [SerializeField] private int bulletsInMagazine; // bullets in magazine plus the one in the chamber
    private Animator weaponAnimator;
    [SerializeField] private GameObject magazine;
    public GameObject Magazine{get => magazine;}
    [SerializeField] private GameObject cartridgeSpawn;
    [SerializeField] private Ammunition ammunitionType;
    private ObjectPooler cartridgePool;
    private ObjectPooler bulletPool {get; set;}
    private ObjectPooler magazinePool;
    public ObjectPooler MagazinePool{get => magazinePool;}
    [SerializeField] private float cartridgeDeltaY = 0.05f, cartridgeDeltaZ = 0.05f, cartridgeMinRotation = 0.05f, cartridgeMaxRotation = 0.05f, cartridgeStrength = 1;
    private int bulletBuffer = 0; // according to fire mode
    protected static readonly int shootSpeedAnimation = Animator.StringToHash("shootSpeed");
    protected static readonly int reloadSpeedAnimation = Animator.StringToHash("reloadSpeed");
    private Rigidbody rbody;
    [SerializeField] private Shoot shoot;
    [SerializeField] private Transform spawnTransform;

    // Start is called before the first frame update
    private void Start() {
        rbody = GetComponent<Rigidbody>();
        weaponAnimator = GetComponent<Animator>();
        foreach (AnimationClip clip in weaponAnimator.runtimeAnimatorController.animationClips) {
            if (clip.name == "Shoot") {
                //Debug.print("Weapon Shoot" + clip.length + " with rate : " + fireRate + " gives " + (fireRate/(clip.length*1000.0f)));
                weaponAnimator.SetFloat(shootSpeedAnimation, (clip.length*1000.0f)/fireRate);
            } else if(clip.name == "Reload"){
                //Debug.print("Weapon Reload" + clip.length + " with rate : " + reloadRate + " gives " + (reloadRate/(clip.length*1000.0f)));
                weaponAnimator.SetFloat(reloadSpeedAnimation, (clip.length*1000.0f)/reloadRate);
            }
        }
        Debug.print(name+"MagazinePool");
        magazinePool = GameObject.Find(name+"MagazinePool").GetComponent<ObjectPooler>();
        cartridgePool = GameObject.Find(Enum.GetName(typeof(Ammunition), ((int)ammunitionType))+"Pool").GetComponent<ObjectPooler>();
        bulletPool = GameObject.Find(Enum.GetName(typeof(Ammunition), ((int)ammunitionType))+"BulletPool").GetComponent<ObjectPooler>();
    }

    // Update is called once per frame
    private void Update() {
    }

    public void SetRigibody(bool b){
        //Debug.print("pass rb " + b);
        if(b && rbody == null){
            gameObject.AddComponent<Rigidbody>();
            rbody = GetComponent<Rigidbody>();
        } else if(!b){
            Destroy(rbody);
            rbody = null;
        }
    }

    /* Trigger handling */
    public void PressTrigger(){
        bulletBuffer = (fireMode[fireModeIndex] == FireMode.SEMI)? 1 : (fireMode[fireModeIndex] == FireMode.BURST)? burstBullet : -1;
        //Debug.print(Time.frameCount + " buffer " + bulletBuffer);
    }

    public void ReleaseTigger(){
        bulletBuffer = 0;
    }

    public bool CanShootBullet(){
        return (bulletBuffer == -1 || bulletBuffer > 0) && bulletsInMagazine > 0;
    }
    
    /* Called by Entity that hold the weapon */
    public void Shoot(){
        weaponAnimator.SetTrigger("doShoot");
        --bulletsInMagazine;
        if (bulletBuffer != -1){
            --bulletBuffer;
        }
        if (CanShootBullet()){
            Quaternion q = Quaternion.FromToRotation(Vector3.forward,transform.forward);
            shoot.ApplyShoot(bulletPool.SpawnAt(spawnTransform.position,q.eulerAngles));
        }
    }
    /*
        Shooting Animation Event function ...
    */
    public void OnStartShoot(){
        GameObject o = cartridgePool.SpawnAt(cartridgeSpawn.transform.position, cartridgeSpawn.transform.eulerAngles);
        Vector3 origin = cartridgeSpawn.transform.position;
        float angA = Mathf.PI+UnityEngine.Random.Range(-cartridgeDeltaY/2, cartridgeDeltaY/2), angB = UnityEngine.Random.Range(-cartridgeDeltaZ/2, cartridgeDeltaZ/2), angC = UnityEngine.Random.Range(cartridgeMinRotation, cartridgeMaxRotation);
        Vector3 m = new Vector3(Mathf.Cos(angA)*Mathf.Cos(angB), Mathf.Sin(angB), Mathf.Sin(angA)*Mathf.Cos(angB));
        m = m*(cartridgeStrength/m.magnitude);
        cartridgeSpawn.transform.Translate(m);
        //Debug.print(cartridgeSpawn.transform.position-origin);
        Rigidbody r = o.GetComponent<Rigidbody>();
        //Debug.print(cartridgeSpawn.transform.position-origin);
        r.AddForce(cartridgeSpawn.transform.position-origin, ForceMode.Impulse);
        r.AddTorque(cartridgeSpawn.transform.up*angC, ForceMode.Impulse);

        cartridgeSpawn.transform.position = origin;
    }


    /* Called by Entity that hold the weapon */
    public void OnReloadAction(){
        weaponAnimator.SetTrigger("doReload");
    }
    public void OnExtractEmptyMagazine(){
        magazine.SetActive(false);
    }
    public void OnPutNewMagazine(){
        magazine.SetActive(true);
        bulletsInMagazine = MaxBulletsInMagazine + ((bulletsInMagazine > 0)? 1 : 0); // when reloading, if the previous magazine wasn't empty, a bullet will stay in the chamber
    }
}
