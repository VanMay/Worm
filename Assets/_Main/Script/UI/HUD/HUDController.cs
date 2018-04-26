using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour {
    [SerializeField]
    private FrontSight frontSight;
    [SerializeField]
    private WeaponInfo weaponInfo;

    private GameObject player;
    private PlayerController playerController;
    private PlayerWeaponController playerWeaponController;

	void Start () {
        player = Player.instance.gameObject;
        playerController = player.GetComponent<PlayerController>();
        playerWeaponController = player.GetComponent<PlayerWeaponController>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update () {
        frontSight.gameObject.SetActive(playerController.isAiming);
        weaponInfo.gameObject.SetActive(playerController.isAiming);

        frontSight.ShowBallisticDiffusion(playerWeaponController.weapon);
        weaponInfo.ShowWeaponInfo(playerWeaponController.weapon);
    }
}
