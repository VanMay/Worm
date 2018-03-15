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
    private Animator Animator;
    private PlayerController playerController;
    private PlayerWeaponController playerWeaponController;

	void Start () {
        Animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        playerWeaponController = GetComponent<PlayerWeaponController>();
        leftFoot = Animator.GetBoneTransform(HumanBodyBones.LeftFoot);
        rightFoot = Animator.GetBoneTransform(HumanBodyBones.RightFoot);
	}

    void OnAnimatorIK()
    {
        FootIK();
        AimingIK();
        ShootIK();
        LookAtIK();
    }

    void FootIK()
    {
        Ray leftFootRay = new Ray(leftFoot.position + Vector3.up * 0.1f, Vector3.down);
        Ray rightFootRay = new Ray(rightFoot.position + Vector3.up * 0.1f, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(leftFootRay, out hit))
        {
            float leftFootIKWeight = Animator.GetFloat("LeftFoot");
            Vector3 leftFootPos = hit.point + Vector3.up * 0.1f;
            Quaternion leftFootRot = Quaternion.FromToRotation(leftFoot.transform.up, hit.normal) * leftFoot.rotation;
            SetIK(AvatarIKGoal.LeftFoot, leftFootIKWeight, leftFootPos, leftFootRot);
        }
        if (Physics.Raycast(rightFootRay, out hit))
        {
            float rightFootIKWeight = Animator.GetFloat("RightFoot");
            Vector3 rightFootPos = hit.point + Vector3.up * 0.1f;
            Quaternion rightFootRot = Quaternion.FromToRotation(rightFoot.transform.up, hit.normal) * rightFoot.rotation;
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
            Animator.SetLookAtWeight(lookAtIKWeight, bodyIKWeight, headIKWeight, eyesIKWeight, clampIKWeight);
            Animator.SetLookAtPosition(playerWeaponController.weapon.lookAtPoint.position);
        }
    }

    public void ShootIK()
    {
        Quaternion rot = Quaternion.Euler(Vector3.zero);
        Animator.SetBoneLocalRotation(HumanBodyBones.LeftFoot, rot);
    }

    void LookAtIK()
    {
        if (lookAtTarget != null)
        {
            Animator.SetLookAtWeight(lookAtIKWeight, bodyIKWeight, headIKWeight, eyesIKWeight, clampIKWeight);
            Animator.SetLookAtPosition(lookAtTarget.transform.position);
        }
    }

    void SetIK(AvatarIKGoal goal, float ikWeight, Vector3 pos, Quaternion rot)
    {
        Animator.SetIKPositionWeight(goal, ikWeight);
        Animator.SetIKRotationWeight(goal, ikWeight);

        Animator.SetIKPosition(goal, pos);
        Animator.SetIKRotation(goal, rot);
    }
}
