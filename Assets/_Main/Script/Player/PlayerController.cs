using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    //参数
    [Header("Movement Parameter")]
    [SerializeField]
    private float moveSpeed_Normal = 3;
    [SerializeField]
    private float moveSpeed_Crouch = 1;
    [SerializeField]
    private float moveSpeed_Aiming = 2;
    [SerializeField]
    private float rotateSpeed = 360;
    [SerializeField]
    private float slideSpeed = 3;

    [Header("Player State")]
    public bool isCrouching = false;
    public bool isAiming = false;
    public bool isFalling = false;

    [Header("Detector")]
    public bool isGrounded = true;
    private Vector3 groundNormal;

    //方向变量(世界空间)
    private Vector3 viewDir;
    private Vector3 bodyDir;
    private Vector3 moveDir;

    //移动变量
    private Vector3 velocity;
    private Vector3 moveVelocity;
    private Vector3 slideVelocity;
    private float fallTime;

    //引用组件
    private CharacterController characterController;
    private Animator animator;
    private PlayerWeaponController playerWeaponController;
    public ObstacleDetector detector;

    private AnimatorStateInfo animInfo;

    void Start () {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerWeaponController = GetComponent<PlayerWeaponController>();
        Cursor.lockState = CursorLockMode.Locked;
	}

	void Update () {
        LogicLayerUpdate();
        ViewLayerUpdate();
    }

    //逻辑层更新
    void LogicLayerUpdate()
    {
        if (!animInfo.IsTag("RootMotion"))
        {
            characterController.enabled = true;
            viewDir = Vector3.ProjectOnPlane(InputManager.instance.ViewDir, Vector3.up).normalized;
            bodyDir = transform.forward;
            float viewAngle = MathAdd.Angle_XZ_180(viewDir, Vector3.forward);
            moveDir = Quaternion.AngleAxis(-viewAngle, Vector3.up) * InputManager.instance.InputDir;

            DetectGround();
            Rotate();
            Move();

            isAiming = InputManager.instance.AimingBtn;
            if (InputManager.instance.CrouchBtn)
            {
                isCrouching = !isCrouching;
            }
            if (isCrouching)
            {
                characterController.height = 1.4f;
                characterController.center = Vector3.up * 0.7f;
            }
            else
            {
                characterController.height = 2;
                characterController.center = Vector3.up * 1;
            }
            isFalling = !isGrounded;
            if (isFalling)
            {
                fallTime += Time.deltaTime;
            }
            else if (animInfo.IsTag("Move"))
            {
                fallTime = 0;
            }

            //移动角色
            velocity = Vector3.zero;
            velocity += moveVelocity;
            velocity += slideVelocity;
            if (groundNormal.x != 0 && groundNormal.z != 0)
            {
                characterController.Move(velocity * Time.deltaTime);
            }
            else
            {
                characterController.SimpleMove(velocity);
            }
        }
        else
        {
            characterController.enabled = false;
            isAiming = false;
            isFalling = false;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        slideVelocity = Vector3.zero;
        if (!isGrounded)
        {
            Vector3 cross = Vector3.Cross(hit.normal, Vector3.up);
            Vector3 slideDir = Vector3.Cross(hit.normal, cross);
            slideVelocity = slideDir * slideSpeed;
        }
    }

    void DetectGround()
    {
        isGrounded = false;
        groundNormal = Vector3.zero;
        float detectLength = 0.3f;
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, detectLength))
        {
            if (hit.transform.tag != Tags.Player)
            {
                isGrounded = true;
                groundNormal = hit.normal;
            }
        }
        isGrounded = isGrounded || characterController.isGrounded;
    }

    void Move()
    {
        if (isGrounded)
        {
            moveVelocity = Vector3.zero;
            if (animInfo.IsTag("Move"))
            {
                //将移动方向投影到地面平面上
                moveVelocity = Vector3.ProjectOnPlane(moveDir, groundNormal).normalized * InputManager.instance.InputDir.magnitude;
                //更具当前状态获取移动速度
                float moveSpeed;
                if (isAiming)
                {
                    moveSpeed = moveSpeed_Aiming;
                }
                else if (isCrouching)
                {
                    moveSpeed = moveSpeed_Crouch;
                }
                else
                {
                    moveSpeed = moveSpeed_Normal;
                }
                //获得移动速率
                moveVelocity *= moveSpeed;
            }
        }
    }

    void Rotate()
    {
        if (!isAiming)
        {
            float body2MoveAngle = MathAdd.Angle_XZ_180(bodyDir, moveDir);
            if (Mathf.Abs(body2MoveAngle) > rotateSpeed * Time.deltaTime)
            {
                transform.Rotate(Vector3.up, body2MoveAngle / Mathf.Abs(body2MoveAngle) * rotateSpeed * Time.deltaTime);
            }
            else if (moveDir.magnitude > 0)
            {
                transform.rotation = Quaternion.LookRotation(moveDir);
            }
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(viewDir);
        }
    }

    //表现层更新
    void ViewLayerUpdate()
    {
        float inputMagnitude = InputManager.instance.InputDir.magnitude;
        animator.SetFloat("InputMagnitude", inputMagnitude);
        float inputAngle = MathAdd.Angle_XZ_180(viewDir, moveDir);
        animator.SetFloat("InputAngle", inputAngle);
        if (!animInfo.IsTag("RootMotion"))
        {
            animator.SetBool("isAiming", isAiming);
            animator.SetBool("isCrouching", isCrouching);
            animator.SetBool("isFalling", isFalling);
            animator.SetFloat("FallTime", fallTime);
            if (detector.PlatformLow)
            {
                if (InputManager.instance.ClimbBtn)
                {
                    animator.SetTrigger("ClimbLow");
                }
            }
            else if (detector.PlatformHigh)
            {
                if (InputManager.instance.ClimbBtn)
                {
                    animator.SetTrigger("ClimbHigh");
                }
            }
            else if (detector.Roadblock)
            {
                if (InputManager.instance.ClimbBtn)
                {
                    animator.SetTrigger("Vault");
                }
            }
            if (playerWeaponController.weapon.ReadyToShoot)
            {
                if (InputManager.instance.ShootBtn)
                {
                    animator.SetTrigger("Shoot");
                }
            }
            animator.SetFloat("ShootSpeed", playerWeaponController.weapon.firingRate * 2);
        }
    }

    private void SetStep(int step)
    {
        switch (step)
        {
            case 0:
                {
                    animator.SetBool("LU", true);
                    animator.SetBool("RU", false);
                    break;
                }
            case 1:
                {
                    animator.SetBool("LU", false);
                    animator.SetBool("RU", true);
                    break;
                }
            default:
                {
                    animator.SetBool("LU", false);
                    animator.SetBool("RU", false);
                    break;
                }
        }
    }

    void OnAnimatorMove()
    {
        animInfo = animator.GetCurrentAnimatorStateInfo(0);
        //对于个别动画应用root motion
        if (animInfo.IsTag("RootMotion"))
        {
            animator.ApplyBuiltinRootMotion();
        }
    }
}
