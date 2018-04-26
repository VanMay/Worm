using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Main.AI.Public
{
    [TaskCategory("Main/AI/Public")]
    [TaskDescription("Set [CheckPosition] with nearby position")]
    public class GetNearbyCheckPosition : Action
    {
        public float minRange = 1f;
        public float maxRange = 2f;

        private Vector3 nextCheckPos;
        private Cognition cognition;

        public override void OnStart()
        {
            cognition = GetComponent<Cognition>();
            GetNextCheckPosition();
        }

        void GetNextCheckPosition()
        {
            //获得随机点
            Vector3 randomDir = new Vector3(Random.value - 0.5f, 0, Random.value - 0.5f).normalized;
            float randomDist = Mathf.Lerp(minRange, maxRange, Random.value);
            nextCheckPos = transform.position + randomDir * randomDist;
            //判断随机点能否到达
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            NavMeshPath path = new NavMeshPath();
            if (!agent.CalculatePath(nextCheckPos, path))
            {
                GetNextCheckPosition();
            }
        }

        public override TaskStatus OnUpdate()
        {
            cognition.CheckPosition = nextCheckPos;
            return TaskStatus.Failure;
        }
    }
}