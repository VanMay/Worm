using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIK : MonoBehaviour {
    [Header("LookAtIK")]
    public GameObject lookAtTarget;
    [SerializeField]
    private float lookAtIKWeight = 1;
    [SerializeField]
    private float bodyIKWeight = 0.8f; 
    [SerializeField]
    private float headIKWeight = 1;
    [SerializeField]
    private float eyesIKWeight = 1;
    [SerializeField]
    private float clampIKWeight = 1;

    private Vector3 marchPos;
    private Quaternion marchRot;
    private Vector3 leftHandMarchPos;
    private Quaternion leftHandMarchRot;
    private Vector3 rightHandMarchPos;
    private Quaternion rightHandMarchRot;

    private Animator animator;
    private PlayerController playerController;
    private PlayerWeaponController playerWeaponController;

	void Start () {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        playerWeaponController = GetComponent<PlayerWeaponController>();
	}

    void Update()
    {
        ClimbMarchTarget();
    }

    void OnAnimatorIK()
    {
        FootIK();
        AimingIK();
        LookAtIK();
        ClimbIK();
    }

    //Foot与地面高度和角度匹配
    void FootIK()
    {
        Vector3 leftFootPos = animator.GetIKPosition(AvatarIKGoal.LeftFoot);
        Vector3 rightFootPos = animator.GetIKPosition(AvatarIKGoal.RightFoot);
        Quaternion leftFootRot = animator.GetIKRotation(AvatarIKGoal.LeftFoot);
        Quaternion rightFootRot = animator.GetIKRotation(AvatarIKGoal.RightFoot);
        Ray leftFootRay = new Ray(leftFootPos + Vector3.up * 0.1f, Vector3.down);
        Ray rightFootRay = new Ray(rightFootPos + Vector3.up * 0.1f, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(leftFootRay, out hit))
        {
            float leftFootIKWeight = animator.GetFloat("LeftFoot");
            leftFootPos = hit.point + Vector3.up * 0.1f;
            leftFootRot = Quaternion.FromToRotation(leftFootRot * transform.up, hit.normal) * leftFootRot;
            SetIK(AvatarIKGoal.LeftFoot, leftFootIKWeight, leftFootPos, leftFootRot);
        }
        if (Physics.Raycast(rightFootRay, out hit))
        {
            float rightFootIKWeight = animator.GetFloat("RightFoot");
            rightFootPos = hit.point + Vector3.up * 0.1f;
            rightFootRot = Quaternion.FromToRotation(rightFootRot * transform.up, hit.normal) * rightFootRot;
            SetIK(AvatarIKGoal.RightFoot, rightFootIKWeight, rightFootPos, rightFootRot);
        }
    }

    //瞄准时使手部、身体和头部与武器匹配
    void AimingIK()
    {
        if (playerController.isAiming)
        {
            //手握武器
            float leftHandIKWeight = 1;// AC.GetFloat("LeftHand");
            Transform leftHandPoint = playerWeaponController.weapon.leftHandPoint;
            Vector3 lefthandPos = leftHandPoint.position;
            Quaternion lefthandRot = leftHandPoint.rotation * Quaternion.Euler(0, 30, 150);
            SetIK(AvatarIKGoal.LeftHand, leftHandIKWeight, lefthandPos, lefthandRot);
            float rightHandIKWeight = 1;// AC.GetFloat("RightHand");
            Transform rightHandPoint = playerWeaponController.weapon.rightHandPoint;
            Vector3 righthandPos = rightHandPoint.position;
            Quaternion righthandRot = rightHandPoint.rotation * Quaternion.Euler(0, 0, -90);
            SetIK(AvatarIKGoal.RightHand, rightHandIKWeight, righthandPos, righthandRot);
            //看向目标
            animator.SetLookAtWeight(lookAtIKWeight, bodyIKWeight, headIKWeight, eyesIKWeight, clampIKWeight);
            animator.SetLookAtPosition(playerWeaponController.weapon.lookAtPoint.position);
        }
    }

    //看向指定物体
    void LookAtIK()
    {
        if (lookAtTarget != null)
        {
            animator.SetLookAtWeight(lookAtIKWeight, bodyIKWeight, headIKWeight, eyesIKWeight, clampIKWeight);
            animator.SetLookAtPosition(lookAtTarget.transform.position);
        }
    }

    public void InitCLimbMarchTarget(GameObject player, GameObject platformInteractionBox, Vector3 hitNormal)
    {
        Collider platformCollider = platformInteractionBox.GetComponent<Collider>();
        Vector3 closestPoint = platformCollider.ClosestPoint(player.transform.position);
        float height = platformCollider.bounds.size.y;
        Vector3 right = Vector3.Cross(hitNormal, Vector3.up).normalized;
        Vector3 forward = Vector3.Cross(right, Vector3.up).normalized;
        marchPos = closestPoint - forward * 0.5f;
        marchRot = Quaternion.LookRotation(forward);

        leftHandMarchPos = platformCollider.ClosestPoint(closestPoint + Vector3.up * height - right * 0.25f);
        leftHandMarchRot = Quaternion.Euler(Vector3.zero);

        rightHandMarchPos = platformCollider.ClosestPoint(closestPoint + Vector3.up * height + right * 0.25f);
        rightHandMarchRot = Quaternion.Euler(Vector3.zero);
    }

    //攀爬时与墙面匹配
    void ClimbMarchTarget()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Climb_2m"))
        {
            MatchTargetWeightMask weightMask = new MatchTargetWeightMask(Vector3.one, 0);
            float startTime = 0;
            float endTime = 0.1f;
            animator.MatchTarget(marchPos, marchRot, AvatarTarget.Root, weightMask, startTime, endTime);

            Debug.DrawRay(leftHandMarchPos, Vector3.up * 10, Color.red);
            Debug.DrawRay(rightHandMarchPos, Vector3.up * 10, Color.red);
        }
    }

    void ClimbIK()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Climb_2m"))
        {
            SetIK(AvatarIKGoal.LeftHand, animator.GetFloat("LeftHand"), leftHandMarchPos, leftHandMarchRot);
            SetIK(AvatarIKGoal.RightHand, animator.GetFloat("RightHand"), rightHandMarchPos, rightHandMarchRot);
        }
    }

    void SetIK(AvatarIKGoal goal, float ikWeight, Vector3 pos, Quaternion rot)
    {
        animator.SetIKPositionWeight(goal, ikWeight);
        animator.SetIKRotationWeight(goal, ikWeight);

        animator.SetIKPosition(goal, pos);
        animator.SetIKRotation(goal, rot);
    }
}
