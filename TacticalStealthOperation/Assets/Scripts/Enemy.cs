using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] private ObjectPooler magazinePool;
    [SerializeField] private GameObject leftHand, rightHand;
    [SerializeField] private Weapon weapon;
    private GameObject hiddenMagazine;

    // Start is called before the first frame update
    void Start() {
        setHiddenMagazine();
        leftHand.SetActive(false);
    }

    private void setHiddenMagazine(){
        hiddenMagazine = Instantiate(weapon.Magazine);
        hiddenMagazine.transform.parent = leftHand.transform;
        hiddenMagazine.transform.localPosition = new Vector3(0, 0, 0);
        hiddenMagazine.transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update() {
        
    }

    /*
        Shooting Rifle Animation Event function ...
    */
    public void OnStartFiringRifleAnimation(){
        weapon.Shoot();
    }

    /*
        Reloading Rifle Animation Event function ...
    */
    public void OnStartReloadingAnimation(){
        leftHand.SetActive(false);
        weapon.Magazine.SetActive(true);
    }
    public void OnExtractEmptyMagazine(){
        leftHand.SetActive(true);
        weapon.OnExtractEmptyMagazine();
    }
    public void OnDropEmptyMagazine(){
        leftHand.SetActive(false);
        magazinePool.SpawnAt(leftHand.transform.position, leftHand.transform.eulerAngles);
    }
    public void OnGetNewMagazine(){
        leftHand.SetActive(true);
    } 
    public void OnPutNewMagazine(){
        leftHand.SetActive(false);
        weapon.OnPutNewMagazine();
    }
    public void OnReloadActionWeapon(){
        weapon.OnReloadAction();
    }
    public void OnEndReloadingAnimation(){

    }
}
