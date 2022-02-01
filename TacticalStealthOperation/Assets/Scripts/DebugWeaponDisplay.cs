using UnityEngine;
using System.Collections.Generic;

public class DebugWeaponDisplay : MonoBehaviour {
    [SerializeField] private List<Weapon> weapons;
    [SerializeField] private Character player;
    private int weaponIndex;
    private int fireModeIndex;
    
    // Start is called before the first frame update
    void Start() {
        weaponIndex = 0;
        fireModeIndex = 0;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            weaponIndex = ++weaponIndex%weapons.Count;
            player.EquipWeapon(weapons[weaponIndex]);
        }
        else if (Input.GetKeyDown(KeyCode.M)) {
            Weapon weapon = player.GetWeapon();
            fireModeIndex = ++fireModeIndex % weapon.GetFireModeList().Count;
            weapon.SetFireMode(fireModeIndex);
        }
    }
}