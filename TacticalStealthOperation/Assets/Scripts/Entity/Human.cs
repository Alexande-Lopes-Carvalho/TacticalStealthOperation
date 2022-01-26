using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Entity {
    [SerializeField] private GameObject leftHand, rightHand;
    public GameObject LeftHand{get=>leftHand;}
    public GameObject RightHand{get=>rightHand;}
    [SerializeField] private Weapon weapon;
    [SerializeField] private RuntimeAnimatorController gunAnimationController, assaultRifleAnimationController;
    [SerializeField] private Transform eyes;
    public Transform Eyes {get => eyes;}
    public Transform Feet {get => transform;}
    private GameObject hiddenMagazine;
    protected Animator animator;
    private bool isPressingWeaponTrigger = false;

    protected static readonly int shootSpeedAnimation = Animator.StringToHash("shootSpeed");
    protected static readonly int reloadSpeedAnimation = Animator.StringToHash("reloadSpeed");
    protected static readonly int doShootAnimation = Animator.StringToHash("doShoot");
    protected static readonly int forwardSpeedAnimation = Animator.StringToHash("forwardSpeed");
    protected static readonly int rightSpeedAnimation = Animator.StringToHash("rightSpeed");
    protected static readonly int turnAnimation = Animator.StringToHash("turn");
    protected static readonly int isMovingAnimation = Animator.StringToHash("isMoving");

    private float maxSpeedAnimation = 7f;
    private Vector3 lastPosition, lastDirection;
    private float turnSamplingTime, turnBuffer;

    // Start is called before the first frame update
    public override void Start() {
        base.Start();
        leftHand.SetActive(false);
        animator = GetComponent<Animator>();
        EquipWeapon(weapon);
        lastPosition = transform.position;
        lastDirection = transform.forward;
        turnSamplingTime = Time.time;
        turnBuffer = 0;
        EarTransform = eyes;
    }

    private void UnequipWeapon(){
        weapon.Unequipped();
        foreach(Transform k in leftHand.transform){
            Destroy(k.gameObject);
        }
    }

    public void EquipWeapon(Weapon _weapon){
        if(weapon != null){
            UnequipWeapon();
        }

        weapon = _weapon;
        if(weapon.User != null){
            weapon.User.UnequipWeapon();
        }
        weapon.EquippedBy(this);
        //Debug.print("category " + ((int)weapon.Category));
        string shoot, reload;
        if(weapon.Category == WeaponCategory.AssaultRifle){
            gameObject.GetComponent<Animator>().runtimeAnimatorController = assaultRifleAnimationController;
            //animator.runtimeAnimatorController = assaultRifleAnimationController;
            shoot = "Firing_AssaultRifle";
            reload = "Reloading_AssaultRifle";
        } else {
            gameObject.GetComponent<Animator>().runtimeAnimatorController = gunAnimationController;
            //animator.runtimeAnimatorController = gunAnimationController;
            shoot = "Firing_Gun";
            reload = "Reloading_Gun";
        }
        animator = GetComponent<Animator>();
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips) {
            if (clip.name == shoot) {
                animator.SetFloat(shootSpeedAnimation, (clip.length*1000.0f)/weapon.FireRate);
            } else if(clip.name == reload){
                animator.SetFloat(reloadSpeedAnimation, (clip.length*1000.0f)/weapon.ReloadRate);
            }
        }
        SetHiddenMagazine();
    } 

    public void PressWeaponTrigger(){
        //Debug.Log("Enemy : Press");
        isPressingWeaponTrigger = true;
        weapon.PressTrigger();
        if(weapon.CanShootBullet()){
            animator.SetBool(doShootAnimation, true);
        }
    }
    public void ReleaseWeaponTrigger(){
        //Debug.Log("Enemy : Release");
        isPressingWeaponTrigger = false;
        animator.SetBool(doShootAnimation, false);
    }

    private void SetHiddenMagazine(){
        hiddenMagazine = Instantiate(weapon.Magazine);
        hiddenMagazine.transform.parent = leftHand.transform;
        hiddenMagazine.transform.localPosition = new Vector3(0, 0, 0);
        hiddenMagazine.transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    public override void FixedUpdate(){
        base.FixedUpdate();

        //Debug.Log(Mathf.Acos(Vector3.Dot(lastDirection, transform.forward)));
        
    }

    // Update is called once per frame
    public override void Update() {
        base.Update();
    }

    public override void Damage(int damage){
        base.Damage(damage);
        animator.SetTrigger("gotHit");
    }
    public override void Heal(int heal){
        base.Heal(heal);
    }

    public override void Kill(){
        base.Kill();
    }

    protected void RefreshMoveAnimation(){
        Vector3 move = (transform.position-lastPosition)/Time.deltaTime;

        animator.SetFloat(forwardSpeedAnimation, Mathf.Max(Mathf.Min(Vector3.Dot(move, transform.forward)/maxSpeedAnimation, 1.0f), -1.0f));
        animator.SetFloat(rightSpeedAnimation, Mathf.Max(Mathf.Min(Vector3.Dot(move, transform.right)/maxSpeedAnimation, 0.5f), -0.5f));
        animator.SetBool(isMovingAnimation, move.magnitude > 0.1);
        lastPosition = transform.position;
    }
    protected void RefreshTurnAnimation(){
        turnBuffer += Mathf.Acos(Mathf.Min(Mathf.Max(Vector3.Dot(lastDirection, transform.forward), -1.0f), 1.0f))*(Vector3.Dot(Vector3.Cross(lastDirection, transform.forward), transform.up) > 0? 1 : -1);
        lastDirection = transform.forward;
        if(Time.time >= turnSamplingTime+0.05){
            animator.SetFloat(turnAnimation, Mathf.Min(Mathf.Max( (turnBuffer/(Time.time-turnSamplingTime)), -1.0f), 1.0f));
            turnBuffer = 0;
            turnSamplingTime = Time.time;
        }
    }

    /*
        Shooting Rifle Animation Event function ...
    */
    public void OnStartFiringAnimation(){
        if(isPressingWeaponTrigger && weapon.CanShootBullet()){
            weapon.Shoot();
        }
    }
    public void OnEndFiringAnimation(){ // called 1 ms before end of the animation (for state update to be effective)
        animator.SetBool(doShootAnimation, isPressingWeaponTrigger && weapon.CanShootBullet());
    }

    /*
        Reloading Rifle Animation Event function ...
    */
    public void OnStartReloadingAnimation(){
        leftHand.SetActive(false);
        weapon.Magazine.SetActive(true);
        weapon.OnReloadAction();
    }
    public void OnExtractEmptyMagazine(){
        leftHand.SetActive(true);
        weapon.OnExtractEmptyMagazine();
    }
    public void OnDropEmptyMagazine(){
        leftHand.SetActive(false);
        weapon.MagazinePool.SpawnAt(leftHand.transform.position, leftHand.transform.eulerAngles);
    }
    public void OnGetNewMagazine(){
        leftHand.SetActive(true);
    } 
    public void OnPutNewMagazine(){
        leftHand.SetActive(false);
        weapon.OnPutNewMagazine();
    }
    public void OnReloadActionWeapon(){
        
    }
    public void OnEndReloadingAnimation(){

    }
}
