using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayWeapon : MonoBehaviour
{
    [SerializeField] private Character player;
    public TextMeshProUGUI nbBallText;
    public TextMeshProUGUI fireModeText;
    public RawImage weaponImage;
    public RawImage ammunitionImage;
    public List<Texture2D> weaponTextureList;
    public List<Texture2D> ammunitionTextureList;

    public void Update()
    {
        Weapon weapon = player.GetWeapon();
        if(weapon != null){
            nbBallText.SetText(weapon.GetNbBulletsInMagazine().ToString());
            fireModeText.SetText(weapon.GetFireMode());
            
            String weaponName = weapon.GetWeaponName();
            if (weaponName.Equals("FAL"))
            {
                weaponImage.texture = weaponTextureList[0];
                ammunitionImage.texture = ammunitionTextureList[0];
            }
            else if (weaponName.Equals("M1911"))
            {
                weaponImage.texture = weaponTextureList[1];
                ammunitionImage.texture = ammunitionTextureList[1];
            }
        }
    }
}