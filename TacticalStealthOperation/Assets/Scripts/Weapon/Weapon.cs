using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    [SerializeField] private string weaponName; // to find magazine pool
    [SerializeField] private List<FireMode> fireMode;
    [SerializeField] private int fireModeIndex = 0;
    [SerializeField] private int burstBullet = 3;
    [SerializeField] private float fireRate = 16; // in ms
    public float FireRate{get => fireRate;}
    [SerializeField] private float reloadRate = 2900; // in ms
    public float ReloadRate{get => reloadRate;}
    [SerializeField] private WeaponCategory category;
    public WeaponCategory Category{get => category;}
    [SerializeField] private int damage;
    [SerializeField] private int MaxBulletsInMagazine;
    [SerializeField] private int bulletsInMagazine; // bullets in magazine plus the one in the chamber
    public int BulletsInMagazine {get => bulletsInMagazine;}

    private Animator weaponAnimator;
    [SerializeField] private GameObject magazine;
    public GameObject Magazine{get => magazine;}
    [SerializeField] private GameObject cartridgeSpawn;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private Ammunition ammunitionType;
    private ObjectPooler cartridgePool, bulletPool;
    private ObjectPooler magazinePool;
    public ObjectPooler MagazinePool{get => magazinePool;}
    [SerializeField] private float cartridgeDeltaY = 0.05f, cartridgeDeltaZ = 0.05f, cartridgeMinRotation = 0.05f, cartridgeMaxRotation = 0.05f, cartridgeStrength = 1;
    

    private int bulletBuffer = 0; // according to fire mode

    protected static readonly int shootSpeedAnimation = Animator.StringToHash("shootSpeed");
    protected static readonly int reloadSpeedAnimation = Animator.StringToHash("reloadSpeed");

    private Rigidbody rbody;
    private Collider coll;

    private Human user;
    public Human User{get => user;}

    [SerializeField] private float earRange = 1f;
    public float EarRange{get=> earRange; set => earRange = value;}
    private float sqrEarRange;

    private void Awake(){
        coll = GetComponent<Collider>();
    }
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
        magazinePool = PoolLinker.Get(weaponName+"MagazinePool");
        cartridgePool = PoolLinker.Get(Enum.GetName(typeof(Ammunition), ((int)ammunitionType))+"CartridgePool");
        bulletPool = PoolLinker.Get(Enum.GetName(typeof(Ammunition), ((int)ammunitionType))+"BulletPool");
        sqrEarRange = earRange*earRange;
    }

    public void EquippedBy(Human _user){
        user = _user;
        SetRigibody(false);
        transform.SetParent(user.RightHand.transform);
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localEulerAngles = new Vector3(0, 0, 0);
    }
    public void Unequipped(){
        user = null;
        transform.parent = null;
        SetRigibody(true);
    }

    // Update is called once per frame
    private void Update() {
    }

    public bool HasBulletLeft(){
        return bulletsInMagazine != 0;
    }

    public void SetRigibody(bool b){
        //Debug.print("pass rb " + b);
        if(b && rbody == null){
            gameObject.AddComponent<Rigidbody>();
            rbody = GetComponent<Rigidbody>();
            coll.enabled = true;
        } else if(!b){
            Destroy(rbody);
            rbody = null;
            coll.enabled = false;
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
        if(bulletBuffer != -1){
            --bulletBuffer;
        }
    }
    /*
        Shooting Animation Event function ...
    */
    public void OnStartShoot(){
        if(user != null){
            SpawnCartridge();
            SpawnBullet();
            EarLinker.NoiseAt(transform.position, user.transform, sqrEarRange);
        }
    }

    private void SpawnCartridge(){
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

    private void SpawnBullet(){
        GameObject o = bulletPool.SpawnAt(bulletSpawn.position, bulletSpawn.transform.eulerAngles);
        //Debug.Log(Time.time + " " + bulletPool + " " + o);
        Projectile p = o.GetComponent<Projectile>();
        p.Set(User.transform, damage);
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
        bulletsInMagazine = MaxBulletsInMagazine + ((bulletsInMagazine > 0)? 1 : 0); // when reloading, if the previous magazine wasn't empty, a bullet will stay in the chamber
    }
}
