using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Main.AI.NPC.TypeWorker
{
    [TaskCategory("Main/AI/NPC/Worker")]
    public class GoToWork : Action
    {
        Worker worker;
        NavMeshAgent agent;

        public override void OnStart()
        {
            worker = GetComponent<Worker>();
            agent = GetComponent<NavMeshAgent>();
        }

        public override TaskStatus OnUpdate()
        {
            agent.isStopped = false;
            agent.SetDestination(worker.workLocation.transform.position);
            return TaskStatus.Success;
        }
    }
}
