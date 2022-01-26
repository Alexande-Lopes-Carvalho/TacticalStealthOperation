using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DebugScript : MonoBehaviour {
    [SerializeField] private ObjectPooler objPool;
    [SerializeField] private Human character;
    [SerializeField] private Guard guard;
    [SerializeField] private List<Weapon> weapons;
    [SerializeField] private Path path;
    private int weaponIndex;
    // Start is called before the first frame update
    private void Start() {
        weaponIndex = 0;
        for(int i = 0; i < 7; ++i){
            //UnityEngine.Debug.Log(i + " : " + ((path.ComputeNextPosition())? ""+path.GetDestination() : "none"));
        }
    }

    // Update is called once per frame
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)){
            //Debug.print("pass");
            objPool.SpawnAt(new Vector3(34.5f, 0.8f, 0), new Vector3(-90, 0, 0));
        } else if(Input.GetKeyDown(KeyCode.P)){
            weaponIndex = ++weaponIndex%weapons.Count;
            character.EquipWeapon(weapons[weaponIndex]);
        } else if(Input.GetKeyDown(KeyCode.O)){
            guard.Attack(character);
        }
    }
}
