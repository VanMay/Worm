using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour {
    public Transform weaponContainer;
    public Transform rightShoulder;
    public Weapon weapon;
    public Weapon[] weaponList = new Weapon[4];

    private PlayerController playerController;
    private PlayerInfo playerInfo;

	void Start () {
        playerController = GetComponent<PlayerController>();
        playerInfo = GetComponent<PlayerInfo>();
	}

	void Update () {
        SetWeapon(playerController.isAiming);
        if (InputManager.instance.SwitchWeaponBtn > 0)
        {
            LastWeapon();
        }
        else if (InputManager.instance.SwitchWeaponBtn < 0)
        {
            NextWeapon();
        }
	}

    void SetWeapon(bool armed)
    {
        weapon = weaponList[playerInfo.WeaponIDList[playerInfo.WeaponListIndex]];
        weapon.gameObject.SetActive(armed);
        if (armed)
        {
            //控制武器角度
            weaponContainer.transform.localEulerAngles = new Vector3(Camera.main.transform.eulerAngles.x, 0, 0);
            //控制武器位置
            weaponContainer.transform.position = rightShoulder.position + transform.right * 0.1f;
        }
    }

    void LastWeapon()
    {
        SetWeapon(false);
        playerInfo.WeaponListIndex = (playerInfo.WeaponListIndex + 1) % playerInfo.WeaponIDList.Count;
        SetWeapon(true);
    }

    void NextWeapon()
    {
        SetWeapon(false);
        playerInfo.WeaponListIndex = (playerInfo.WeaponListIndex + playerInfo.WeaponIDList.Count - 1) % playerInfo.WeaponIDList.Count;
        SetWeapon(true);
    }
}
