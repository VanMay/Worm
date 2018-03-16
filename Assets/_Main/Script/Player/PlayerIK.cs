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

    private Transform leftFoot;
    private Transform rightFoot;
    private Animator animator;
    private PlayerController playerController;
    private PlayerWeaponController playerWeaponController;

	void Start () {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        playerWeaponController = GetComponent<PlayerWeaponController>();
        leftFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
        rightFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);
	}

    void OnAnimatorIK()
    {
        FootIK();
        AimingIK();
        LookAtIK();
    }

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

    public void AimingIK()
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

    void LookAtIK()
    {
        if (lookAtTarget != null)
        {
            animator.SetLookAtWeight(lookAtIKWeight, bodyIKWeight, headIKWeight, eyesIKWeight, clampIKWeight);
            animator.SetLookAtPosition(lookAtTarget.transform.position);
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
