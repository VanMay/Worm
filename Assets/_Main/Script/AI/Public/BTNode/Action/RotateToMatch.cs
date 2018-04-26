using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Main.Public
{
    [TaskCategory("Main/Public")]
    [TaskDescription("Return Running until the rotation of this gameobject matched the rotation of the target gameobject")]
    public class RotateToMatch : Action
    {
        public SharedGameObject MatchTarget;
        public SharedFloat OffsetAngle;
        public SharedFloat RotateSpeed = 180;

        private Quaternion originRotation;
        private Quaternion targetRotation;
        private float duration;
        private float timer;

        public override void OnStart()
        {
            if(MatchTarget == null)
            {
                Debug.LogError("[Custum AI]: No Match Target");
            }
            originRotation = transform.rotation;
            targetRotation = MatchTarget.Value.transform.rotation * Quaternion.AngleAxis(OffsetAngle.Value, Vector3.up);
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