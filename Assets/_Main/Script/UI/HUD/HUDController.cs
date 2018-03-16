using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour {
    [SerializeField]
    private FrontSight frontSight;
    [SerializeField]
    private LeftAmmo leftAmmo;

    public GameObject player;
    private PlayerController playerController;
    private PlayerWeaponController playerWeaponController;

	void Start () {
        playerController = player.GetComponent<PlayerController>();
        playerWeaponController = player.GetComponent<PlayerWeaponController>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update () {
        frontSight.gameObject.SetActive(playerController.isAiming);
        leftAmmo.gameObject.SetActive(playerController.isAiming);

        frontSight.ShowBallisticDiffusion(playerWeaponController.weapon);
        leftAmmo.ShowLeftAmmo(playerWeaponController.weapon.leftAmmo);
    }
}
