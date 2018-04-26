using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Main.Public
{
    [TaskCategory("Main/Public")]
    [TaskDescription("Return running until this gameobject has rotated target angle")]
    public class RotateAngle : Action
    {
        [Range(-180, 180)]
        public SharedFloat Angle;
        public SharedFloat RotateSpeed = 180;

        private Quaternion originRotation;
        private Quaternion targetRotation;
        private float duration;
        private float timer;

        public override void OnStart()
        {
            originRotation = transform.rotation;
            targetRotation = Quaternion.Euler(transform.eulerAngles + Vector3.up * Angle.Value);
            duration = Angle.Value / RotateSpeed.Value;
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