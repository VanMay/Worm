using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInfo : MonoBehaviour {
    [SerializeField]
    private Text weaponText;
    [SerializeField]
    private Text leftAmmoText;

    public void ShowWeaponInfo(Weapon weapon)
    {
        leftAmmoText.text = weapon.leftAmmo.ToString();
        weaponText.text = weapon.name;
    }
}
