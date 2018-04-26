using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Main.Public
{
    [TaskCategory("Main/Public")]
    [TaskDescription("Return running until this gameobject faced the target gameobject")]
    public class RotateToFace : Action
    {
        public SharedGameObject FaceTarget;
        public SharedFloat RotateSpeed = 180;

        private Quaternion originRotation;
        private Quaternion targetRotation;
        private float duration;
        private float timer;

        public override void OnStart()
        {
            if (FaceTarget == null)
            {
                Debug.LogError("[Custum AI]: No Face Target");
            }
            originRotation = transform.rotation;
            targetRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(FaceTarget.Value.transform.position - transform.position, Vector3.up));
            duration = Quaternion.Angle(originRotation, targetRotation) / RotateSpeed.Value;
            timer = 0;
        }

        public override TaskStatus OnUpdate()
        {
            if(Quaternion.Angle(transform.rotation, targetRotation) > 0.5f)
            {
                float factor = Mathf.Clamp01(timer / duration);
                transform.rotation = Quaternion.Lerp(originRotation, targetRotation, factor);
                timer += Time.deltaTime;
                return TaskStatus.Running;
            }
            return TaskStatus.Success;
        }
    }
}