using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Main.AI.Public
{
    [TaskCategory("Main/AI/Public")]
    [TaskDescription("Compare cognition state")]
    public class CognitionStateComparison : Conditional
    {
        public GameObject targetGameObject;
        public CognitionState cognitionState;

        private Cognition cognition;

        public override void OnStart()
        {
            cognition = targetGameObject == null ? GetComponent<Cognition>() : targetGameObject.GetComponent<Cognition>();
        }

        public override TaskStatus OnUpdate()
        {
            if (cognition.state == cognitionState)
            {
                return TaskStatus.Success;
            }
            else
            {
                return TaskStatus.Failure;
            }
        }
    }
}
