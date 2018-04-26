using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Main.AI.Public
{
    [TaskCategory("Main/AI/Public")]
    [TaskDescription("Set [HaveCheckPosition] false")]
    public class LostCheckPosition : Action
    {
        public GameObject targetGameObject;
        private Cognition cognition;

        public override void OnStart()
        {
            cognition = targetGameObject == null ? GetComponent<Cognition>() : targetGameObject.GetComponent<Cognition>();
        }

        public override TaskStatus OnUpdate()
        {
            cognition.HaveCheckPosition = false;
            return TaskStatus.Success;
        }
    }
}