using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour {
    [SerializeField] private GameObject leftHand, rightHand;
    [SerializeField] private Weapon weapon;
    [SerializeField] private RuntimeAnimatorController gunAnimationController, assaultRifleAnimationController;
    private GameObject hiddenMagazine;
    private Animator animator;
    private bool isPressingWeaponTrigger = false;

    protected static readonly int shootSpeedAnimation = Animator.StringToHash("shootSpeed");
    protected static readonly int reloadSpeedAnimation = Animator.StringToHash("reloadSpeed");
    protected static readonly int doShootAnimation = Animator.StringToHash("doShoot");

    // Start is called before the first frame update
    void Start() {
        leftHand.SetActive(false);
        animator = GetComponent<Animator>();
        EquipWeapon(weapon);
    }

    public void EquipWeapon(Weapon _weapon){
        if(weapon != null){
            weapon.transform.parent = null;
            weapon.SetRigibody(true);
            foreach(Transform k in leftHand.transform){
                Destroy(k.gameObject);
            }
        }

        weapon = _weapon;
        weapon.SetRigibody(false);
        weapon.transform.SetParent(rightHand.transform);
        weapon.transform.localPosition = new Vector3(0, 0, 0);
        weapon.transform.localEulerAngles = new Vector3(0, 0, 0);
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
        //Debug.print("Enemy : Press");
        isPressingWeaponTrigger = true;
        weapon.PressTrigger();
        if(weapon.CanShootBullet()){
            animator.SetBool(doShootAnimation, true);
        }
    }
    public void ReleaseWeaponTrigger(){
        //Debug.print("Enemy : Release");
        isPressingWeaponTrigger = false;
        animator.SetBool(doShootAnimation, false);
    }

    private void SetHiddenMagazine(){
        hiddenMagazine = Instantiate(weapon.Magazine);
        hiddenMagazine.transform.parent = leftHand.transform;
        hiddenMagazine.transform.localPosition = new Vector3(0, 0, 0);
        hiddenMagazine.transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)){
            PressWeaponTrigger();
        } else if(Input.GetKeyUp(KeyCode.Space)){
            ReleaseWeaponTrigger();
        } else if(Input.GetKeyDown(KeyCode.R)){
            animator.SetTrigger("doReload");
        } else if(Input.GetKeyDown(KeyCode.T)){
            animator.SetTrigger("gotHit");
        }
    }

    /*
        Shooting Rifle Animation Event function ...
    */
    public void OnStartFiringAnimation(){
        //Debug.print(Time.frameCount + " startFiring");
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
