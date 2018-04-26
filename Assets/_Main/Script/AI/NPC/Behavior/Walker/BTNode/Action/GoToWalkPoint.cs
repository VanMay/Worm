using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Main.AI.NPC.TypeWalker
{
    [TaskCategory("Main/AI/NPC/Walker")]
    public class GoToWalkPoint : Action
    {
        Walker walker;
        NavMeshAgent agent;

        public override void OnStart()
        {
            walker = GetComponent<Walker>();
            agent = GetComponent<NavMeshAgent>();
        }

        public override TaskStatus OnUpdate()
        {
            agent.isStopped = false;
            agent.SetDestination(walker.walkPoint.transform.position);
            return TaskStatus.Success;
        }
    }
}
