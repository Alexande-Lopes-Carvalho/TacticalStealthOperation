using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    [SerializeField] private float damage;
    [SerializeField] private int MaxBulletsInMagazine;
    [SerializeField] private int bulletsInMagazine;

    [SerializeField] private Animator weaponAnimator;
    [SerializeField] private GameObject magazine;
    public GameObject Magazine{get => magazine;}
    protected static readonly int doAction = Animator.StringToHash("doAction");

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void onShoot(){
        weaponAnimator.SetBool(doAction, true);
    }
    public void onStopShoot(){
        weaponAnimator.SetBool(doAction, false);
    }
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
