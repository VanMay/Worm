using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputDevice
{
    MouseAndKeyboard,
    XBoxJoystick
}

public class InputManager : MonoBehaviour {
    public static InputManager instance;

    [Header("Character Control Input")]
    public Vector3 InputDir;
    public Vector3 ViewDir;
    public bool ClimbBtn;
    public bool InteractBtn;
    public bool CrouchBtn;
    public bool AimingBtn;
    public bool ShootBtn;
    public bool ReloadBtn;
    public float SwitchWeaponBtn;

    [Header("UI Input")]
    public bool MenuBtn;
    public bool MapBtn;

	void Awake () {
        instance = this;
	}

	void Update () {
        InputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        ViewDir = Camera.main.transform.forward.normalized;
        switch (SettingManager.instance.InputDevice)
        {
            case InputDevice.MouseAndKeyboard:
                {
                    ClimbBtn = Input.GetButtonDown("Key Space");
                    InteractBtn = Input.GetButton("Key E");
                    CrouchBtn = Input.GetButtonDown("Key C");
                    AimingBtn = Input.GetMouseButton(1);
                    if (AimingBtn)
                    {
                        ShootBtn = Input.GetMouseButton(0);
                    }
                    else
                    {
                        ShootBtn = false;
                    }
                    ReloadBtn = Input.GetButtonDown("Key R");
                    SwitchWeaponBtn = Input.GetAxis("Mouse ScrollWheel");
                    break;
                }
            case InputDevice.XBoxJoystick:
                {
                    ClimbBtn = Input.GetButtonDown("Joystick A");
                    InteractBtn = Input.GetButton("Joystick Y");
                    CrouchBtn = Input.GetButtonDown("Joystick B");
                    AimingBtn = Input.GetAxis("Joystick LT") > 0;
                    if (AimingBtn)
                    {
                        ShootBtn = Input.GetAxis("Joystick RT") > 0;
                    }
                    else
                    {
                        ShootBtn = false;
                    }
                    ReloadBtn = Input.GetButtonDown("Joystick RB");
                    SwitchWeaponBtn = Input.GetAxis("Joystick Direction X");
                    break;
                }
            default:
                {
                    break;
                }
        }
	}
}
